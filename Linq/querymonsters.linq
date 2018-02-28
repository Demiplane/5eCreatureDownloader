<Query Kind="Program">
  <NuGetReference>Newtonsoft.Json</NuGetReference>
  <Namespace>Newtonsoft.Json</Namespace>
</Query>

void Main()
{
	var targetFilePath = Util.ReadLine();

	var monsters = JsonConvert.DeserializeObject<Monster[]>(File.ReadAllText(targetFilePath));

	monsters

		.Select(f => f.Actions.FirstOrDefault(r => r.Contains("Multiattack")))
		//.Select(f => f.ToMetadata())
		.Where(f => f != null)
		
		.Dump();
}

public static class MonsterExtensions
{
	public static bool IsFromTomeOfBeasts(this MonsterMetadata monster) => monster.Uri.Contains("tome-of-beasts");
	public static double ToChallengeRating(this MonsterMetadata monster) => Convert.ToDouble(FractionToDouble(monster.Challenge.Split(' ').First()));
	private static double FractionToDouble(string fraction) => fraction.Contains('/') ? (1f / Convert.ToDouble(fraction.Split('/')[1])) : Convert.ToDouble(fraction);
	public static bool IsNpc(this MonsterMetadata monster) => monster.Uri.Contains("nonplayer-characters");
	public static MonsterMetadata ToMetadata(this Monster monster) => MonsterMetadata.Create(monster);
}

public class MonsterMetadata
{
	public static MonsterMetadata Create(Monster monster)
	{
		return new MonsterMetadata()
		{
			Name = monster.Name,
			Type = monster.Type,
			Challenge = monster.Challenge,
			Uri = monster.Uri,
		};
	}

	public string Name { get; set; }
	public string Type { get; set; }
	public string Challenge { get; set; }
	public string Uri { get; set; }
}

public class Monster : MonsterMetadata
{
	public string Strength { get; set; }
	public string Dexterity { get; set; }
	public string Constitution { get; set; }
	public string Intelligence { get; set; }
	public string Wisdom { get; set; }
	public string Charisma { get; set; }
	public string ArmorClass { get; set; }
	public string HitPoints { get; set; }
	public string Speed { get; set; }
	public string SavingThrows { get; set; }
	public string Skills { get; set; }
	public string DamageResistance { get; set; }
	public string DamageImmunity { get; set; }
	public string Senses { get; set; }
	public string Languages { get; set; }
	public string[] Traits { get; set; }
	public string InnateSpellcasting { get; set; }
	public string Spellcasting { get; set; }
	public string[] Actions { get; set; }
	public string[] Reactions { get; set; }
	public string[] LegendaryActions { get; set; }
}