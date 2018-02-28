using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Open5ECreatureDownloader
{
    public sealed class CreatureDownloader : ICreatureDownloader
    {
        internal static string Clean(string stringObject) => 
            stringObject.Replace(Environment.NewLine, string.Empty).Replace("\n", " ").Replace("  ", " ").Trim();
        internal static string GetPart(string name, HtmlNode article) => string.Join("", article
            .Descendants("p")
            .FirstOrDefault(p => p.InnerText.StartsWith(name))
            ?.ChildNodes
            ?.Skip(1)
            ?.Select(f => f.InnerText)
            ?? Enumerable.Empty<string>())
            .Replace(Environment.NewLine, string.Empty)
            .Trim();
        internal static IEnumerable<HtmlNode> ElementsBeyondChallenge(HtmlNode article) => article
            .ChildNodes
            .First(c => c.Name == "div")
            .ChildNodes
            .Where(c => c.Name == "p")
            .SkipWhile(c => !c.InnerText.StartsWith("Challenge"))
            .Skip(1);
        internal static string[] GetTraits(HtmlNode article) =>
            ElementsBeyondChallenge(article)
            .Where(f => !f.InnerText.StartsWith("Spellcasting"))
            .Where(f => !f.InnerText.StartsWith("*"))
            .Where(f => !f.InnerText.StartsWith("Innate"))
            .Where(f => !f.InnerText.StartsWith("At will"))
            .Where(f => !f.InnerText.Substring(1)
            .StartsWith("/day each"))
            .Select(t => t.InnerText)
            .Select(Clean)
            .ToArray();
        internal static HtmlNode GetSpellcastingElement(HtmlNode article) =>
            ElementsBeyondChallenge(article)
            .FirstOrDefault(f => f.InnerText.StartsWith("Spellcasting"));
        internal static IEnumerable<HtmlNode> GetSpellcastingDetailsElement(HtmlNode article) => article
            .ChildNodes
            .First(c => c.Name == "div")
            .ChildNodes
            .Where(c => c.Name == "p" || c.Name == "blockquote")
            .SkipWhile(f => !f.InnerText.StartsWith("Spellcasting"))
            .Skip(1)
            .Take(1)
            .FirstOrDefault()
            ?.Descendants("p")
            ?? Enumerable.Empty<HtmlNode>();
        internal static IEnumerable<HtmlNode> GetInnateSpellcastingNodes(HtmlNode article) =>
            ElementsBeyondChallenge(article)
            .SkipWhile(f => !f.InnerText.StartsWith("Innate"))
            .TakeWhile(f => f.InnerText.StartsWith("Innate") || f.InnerText.StartsWith("At will") || f.InnerText.Substring(1)
            .StartsWith("/day each"));
        internal static string[] GetActions(HtmlNode article, string actionTypeElementId) => article
            .Descendants("div")
            .FirstOrDefault(d => d.Id == actionTypeElementId)
            ?.Descendants("p")
            .Where(f => f.InnerHtml.Trim()
            .StartsWith("<"))
            .Select(t => t.InnerText)
            .Select(Clean)
            .ToArray()
            ?? new string[] { };
        internal static IEnumerable<HtmlNode> GetStatRaw(HtmlNode article) => 
            article.Descendants("table")
            .FirstOrDefault(f => f.Descendants("th")
            .Any(t => t.InnerText == "STR"))?.Descendants("td") ?? Enumerable.Empty<HtmlNode>();
        internal static IEnumerable<string> GetStatRows(HtmlNode article) => GetStatRaw(article)
            .Count() == 6 ? GetStatRaw(article)
            .Select(f => f.InnerText) : GetStatRaw(article)
            .First()
            .InnerText.Split('|')
            .Concat(GetStatRaw(article)
            .Skip(1)
            .Select(f => f.InnerText));
        internal static bool DocumentIsMonster(HtmlNode article) => GetStatRaw(article)
            .Any();
        internal static HtmlNode GetMonsterArticle(string monsterUri) => new HtmlWeb()
            .Load(monsterUri)
            .DocumentNode.Descendants("div")
            .Where(d => d.Attributes["itemprop"]?.Value == "articleBody")
            .First();
        internal static string GetSpellcasting(HtmlNode article) => string.Join(Environment.NewLine, (new[] { GetSpellcastingElement(article)?.InnerText ?? string.Empty })
            .Concat(GetSpellcastingDetailsElement(article)
            .Select(f => f.InnerText)
            .Select(Clean)));
        internal static string GetInnateSpellcasting(HtmlNode article) => string.Join(Environment.NewLine, GetInnateSpellcastingNodes(article)
            .Select(f => f.InnerText)
            .Select(Clean));
        internal static IEnumerable<string> GetRootUris(string source, string articleElementId) =>
            new HtmlWeb()
                .Load(source)
                .DocumentNode
                .Descendants("div")
                .Where(d => d.Id == articleElementId)
                .SelectMany(d => d.Descendants("a"))
                .Select(a => a.Attributes["href"]?.Value)
                .Where(at => !string.IsNullOrWhiteSpace(at))
                .Select(at => new Uri(new Uri(new Uri(source), @"."), at)
                .AbsoluteUri);
        internal static Creature CreateCreatureFromArticle(HtmlNode article, string uri) =>
            new Creature
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
                ConditionImmunity = Clean(GetPart("Condition Immunities", article)),
                Senses = Clean(GetPart("Senses", article)),
                Languages = Clean(GetPart("Languages", article)),
                Challenge = Clean(GetPart("Challenge", article)),

                InnateSpellcasting = GetInnateSpellcasting(article),
                Spellcasting = GetSpellcasting(article),

                Traits = GetTraits(article),
                Actions = GetActions(article, "actions"),
                Reactions = GetActions(article, "reactions"),
                LegendaryActions = GetActions(article, "legendary-actions"),

                Uri = Clean(uri),
            };
        internal static IEnumerable<string> GetMonsterUris() =>
            GetRootUris(@"https://open5e.com/monsters/monsters_a-z/index.html", "e-core-monsters")
            .Concat(GetRootUris(@"https://open5e.com/monsters/tome-of-beasts/index.html", "tome-of-beasts-kobold-press"))
            .Where(at => !at.EndsWith("index.html"))
            .Where(at => !at.Contains("#"))
            .Where(f => f.IndexOf("template", StringComparison.OrdinalIgnoreCase) < 0);

        public IDictionary<string,string> ListMonsters() =>
            GetMonsterUris()
            .ToDictionary(uri => uri,
                uri =>
                Clean(GetMonsterArticle(uri).Descendants("h1")
                .First()
                .FirstChild.InnerText));

        public IEnumerable<Creature> DownloadCreatures(params string[] monsterUris) =>
            monsterUris
            .Select(monsterUri => new { Article = GetMonsterArticle(monsterUri), Uri = monsterUri })
            .Where(f => DocumentIsMonster(f.Article))
            .Select(ao => CreateCreatureFromArticle(ao.Article, ao.Uri));
    }
}
