<Query Kind="Statements">
  <NuGetReference>HtmlAgilityPack</NuGetReference>
  <NuGetReference>Newtonsoft.Json</NuGetReference>
  <Namespace>HtmlAgilityPack</Namespace>
  <Namespace>Newtonsoft.Json</Namespace>
</Query>

var targetFilePath = Util.ReadLine();

var Clean = new Func<string, string>(f => f.Replace(Environment.NewLine, string.Empty).Trim());
var GetPart = new Func<string, HtmlNode, string>((name, article) => string.Join("", article
	.Descendants("p")
	.FirstOrDefault(p => p.InnerText.StartsWith(name))
	?.ChildNodes
	?.Skip(1)
	?.Select(f => f.InnerText) 
	?? Enumerable.Empty<string>())
	.Replace(Environment.NewLine, string.Empty)
	.Trim());
var GetElementsAfterChallenge = new Func<HtmlNode, IEnumerable<HtmlNode>>(article => article
	.ChildNodes
	.First(c => c.Name == "div")
	.ChildNodes
	.Where(c => c.Name == "p")
	.SkipWhile(c => !c.InnerText.StartsWith("Challenge"))
	.Skip(1)
	.Where(f => !f.InnerText.StartsWith("Spellcasting"))
	.Where(f => !f.InnerText.StartsWith("*"))
	.Where(f => !f.InnerText.StartsWith("Innate"))
	.Where(f => !f.InnerText.StartsWith("At will"))
	.Where(f => !f.InnerText.Substring(1).StartsWith("/day each")));
var GetSpellcastingElement = new Func<HtmlNode, HtmlNode>(article => article
	.ChildNodes
	.First(c => c.Name == "div")
	.ChildNodes
	.Where(c => c.Name == "p")
	.SkipWhile(c => !c.InnerText.StartsWith("Challenge"))
	.Skip(1)
	.FirstOrDefault(f => f.InnerText.StartsWith("Spellcasting")));
var GetSpellcastingDetailsElement = new Func<HtmlNode, IEnumerable<HtmlNode>>(article => article
	.ChildNodes
	.First(c => c.Name == "div")
	.ChildNodes
	.Where(c => c.Name == "p" || c.Name == "blockquote")
	.SkipWhile(f => !f.InnerText.StartsWith("Spellcasting"))
	.Skip(1)
	.Take(1)
	.FirstOrDefault()
	?.Descendants("p")
	?? Enumerable.Empty<HtmlNode>());
var GetInnateSpellcastingNodes = new Func<HtmlNode, IEnumerable<HtmlNode>>(article => article
	.ChildNodes
	.First(c => c.Name == "div")
	.ChildNodes
	.Where(c => c.Name == "p")
	.SkipWhile(c => !c.InnerText.StartsWith("Challenge"))
	.Skip(1)
	.SkipWhile(f => !f.InnerText.StartsWith("Innate"))
	.TakeWhile(f => f.InnerText.StartsWith("Innate") || f.InnerText.StartsWith("At will") || f.InnerText.Substring(1).StartsWith("/day each")));
var GetActionElements = new Func<HtmlNode, IEnumerable<HtmlNode>>(article => article
	.Descendants("div")
	.FirstOrDefault(d => d.Id == "actions")
	?.Descendants("p")
	.Where(f => f.InnerHtml.Trim().StartsWith("<"))
	?? Enumerable.Empty<HtmlNode>());
var GetReactionElements = new Func<HtmlNode, IEnumerable<HtmlNode>>(article => article
	.Descendants("div")
	.FirstOrDefault(d => d.Id == "reactions")
	?.Descendants("p")
	.Where(f => f.InnerHtml.Trim().StartsWith("<"))
	?? Enumerable.Empty<HtmlNode>());
var GetLegendaryActionElements = new Func<HtmlNode, IEnumerable<HtmlNode>>(article => article
	.Descendants("div")
	.FirstOrDefault(d => d.Id == "legendary-actions")
	?.Descendants("p")
	?.Skip(1) 
	?? Enumerable.Empty<HtmlNode>());
var GetStatRaw = new Func<HtmlNode, IEnumerable<HtmlNode>>(article => article.Descendants("table").FirstOrDefault(f => f.Descendants("th").Any(t => t.InnerText == "STR"))?.Descendants("td") ?? Enumerable.Empty<HtmlNode>());
var GetStatRows = new Func<HtmlNode, IEnumerable<string>>(article => GetStatRaw(article).Count() == 6 ? GetStatRaw(article).Select(f => f.InnerText) : GetStatRaw(article).First().InnerText.Split('|').Concat(GetStatRaw(article).Skip(1).Select(f => f.InnerText)));

var DocumentIsMonster = new Func<HtmlNode, bool>(article => GetStatRaw(article).Any());

var monsters = new HtmlWeb()
	.Load(@"https://open5e.com/monsters/monsters_a-z/index.html")
	.DocumentNode
	.Descendants("div")
	.Where(d => d.Id == "e-core-monsters")
	.SelectMany(d => d.Descendants("a"))
	.Select(a => a.Attributes["href"]?.Value)
	.Where(at => !string.IsNullOrWhiteSpace(at))
	.Select(at => new Uri(new Uri(@"https://open5e.com/monsters/monsters_a-z/"), at).AbsoluteUri)
	.Concat(new HtmlWeb()
		.Load(@"https://open5e.com/monsters/tome-of-beasts/index.html")
		.DocumentNode
		.Descendants("div")
		.Where(d => d.Id == "tome-of-beasts-kobold-press")
		.SelectMany(d => d.Descendants("a"))
		.Select(a => a.Attributes["href"]?.Value)
		.Where(at => !string.IsNullOrWhiteSpace(at))
		.Select(at => new Uri(new Uri(@"https://open5e.com/monsters/tome-of-beasts/"), at).AbsoluteUri))
	.Where(at => !at.EndsWith("index.html"))
	.Where(at => !at.Contains("#"))
	.Where(f => f.IndexOf("template", StringComparison.OrdinalIgnoreCase) < 0)
	
	
	
	.Select(monsterUri => new { Article = new HtmlWeb().Load(monsterUri.Dump()).DocumentNode.Descendants("div").Where(d => d.Attributes["itemprop"]?.Value == "articleBody").First(), Uri = monsterUri })
	.Where(f => DocumentIsMonster(f.Article))
	.Select(ao => 
	{
		var article = ao.Article;
		
		return new
		{
			Name = Clean(article.Descendants("h1").First().FirstChild.InnerText),
			Type = Clean(article.Descendants("p").First(f => !f.ParentNode.HasClass("figure")).InnerText),
			ArmorClass = Clean(GetPart("Armor Class", article)),
			HitPoints = Clean(GetPart("Hit Points", article)),
			Speed = Clean(GetPart("Speed", article)),

			Strength = Clean(GetStatRows(article).First()),
			Dexterity = Clean(GetStatRows(article).Skip(1).First()),
			Constitution = Clean(GetStatRows(article).Skip(2).First()),
			Intelligence = Clean(GetStatRows(article).Skip(3).First()),
			Wisdom = Clean(GetStatRows(article).Skip(4).First()),
			Charisma = Clean(GetStatRows(article).Skip(5).First()),

			SavingThrows = Clean(GetPart("Saving Throws", article)),
			Skills = Clean(GetPart("Skills", article)),
			DamageResistance = Clean(GetPart("Damage Resistance", article)),
			DamageImmunity = Clean(GetPart("Damage Immunities", article)),
			Senses = Clean(GetPart("Senses", article)),
			Languages = Clean(GetPart("Languages", article)),
			Challenge = Clean(GetPart("Challenge", article)),

			InnateSpellcasting = string.Join(Environment.NewLine, GetInnateSpellcastingNodes(article).Select(f => f.InnerText).Select(Clean)),
			Spellcasting = string.Join(Environment.NewLine, new[] { GetSpellcastingElement(article)?.InnerText ?? string.Empty }.Concat(GetSpellcastingDetailsElement(article).Select(f => f.InnerText).Select(Clean))),

			Traits = GetElementsAfterChallenge(article).Select(t => t.InnerText).ToArray().Select(Clean),
			Actions = GetActionElements(article).Select(t => t.InnerText).ToArray().Select(Clean),
			Reactions = GetReactionElements(article).Select(t => t.InnerText).ToArray().Select(Clean),
			LegendaryActions = GetLegendaryActionElements(article).Select(t => t.InnerText).ToArray().Select(Clean),

			Uri = Clean(ao.Uri),
		};
	})
	//.Dump()
	;

File.WriteAllText(targetFilePath, JsonConvert.SerializeObject(monsters));