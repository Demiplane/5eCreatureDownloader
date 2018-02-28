using Microsoft.VisualStudio.TestTools.UnitTesting;
using Open5ECreatureDownloader;
using System;
using System.Diagnostics;
using System.Linq;

namespace Open5eCreatureDownloader.Test
{
    [TestClass]
    public class DevaTest
    {
        private static Lazy<Creature> LazyDeva = new Lazy<Creature>(() => new CreatureDownloader().DownloadCreatures().First());
        private static Creature Deva => LazyDeva.Value;

        [TestMethod]
        public void Creature_Deva_Name()
        {
            Assert.IsTrue(Deva.Name.Contains("Deva"));
        }
        [TestMethod]
        public void Creature_Deva_Type()
        {
            Assert.IsTrue(Deva.Type.Contains("celestial"));
        }
        [TestMethod]
        public void Creature_Deva_ArmorClass()
        {
            Assert.IsTrue(Deva.ArmorClass.Contains("natural armor"));
        }
        [TestMethod]
        public void Creature_Deva_HitPoints()
        {
            Assert.IsTrue(Deva.HitPoints.Contains("136"));
        }
        [TestMethod]
        public void Creature_Deva_Speed()
        {
            Assert.IsTrue(Deva.Speed.Contains("fly 90"));
        }

        [TestMethod]
        public void Creature_Deva_Strength()
        {
            Assert.IsTrue(Deva.Strength.Contains("18"));
        }
        [TestMethod]
        public void Creature_Deva_Dexterity()
        {
            Assert.IsTrue(Deva.Dexterity.Contains("18"));
        }
        [TestMethod]
        public void Creature_Deva_Constitution()
        {
            Assert.IsTrue(Deva.Constitution.Contains("18"));
        }
        [TestMethod]
        public void Creature_Deva_Intelligence()
        {
            Assert.IsTrue(Deva.Intelligence.Contains("17"));
        }
        [TestMethod]
        public void Creature_Deva_Wisdom()
        {
            Assert.IsTrue(Deva.Wisdom.Contains("20"));
        }
        [TestMethod]
        public void Creature_Deva_Charisma()
        {
            Assert.IsTrue(Deva.Charisma.Contains("20"));
        }

        [TestMethod]
        public void Creature_Deva_SavingThrows()
        {
            Assert.IsTrue(Deva.SavingThrows.Contains("+9"));
        }
        [TestMethod]
        public void Creature_Deva_Skills()
        {
            Assert.IsTrue(Deva.Skills.Contains("Insight"));
        }
        [TestMethod]
        public void Creature_Deva_DamageResistance()
        {
            Assert.IsTrue(Deva.DamageResistance.Contains("radiant"));
        }
        [TestMethod]
        public void Creature_Deva_DamageImmunity()
        {
            Assert.IsTrue(Deva.DamageImmunity.Contains(""));
        }
        [TestMethod]
        public void Creature_Deva_ConditionImmunity()
        {
            Assert.IsTrue(Deva.ConditionImmunity.Contains("Charmed"));
        }

        [TestMethod]
        public void Creature_Deva_Senses()
        {
            Assert.IsTrue(Deva.Senses.Contains("passive Perception"));
        }
        [TestMethod]
        public void Creature_Deva_Languages()
        {
            Assert.IsTrue(Deva.Languages.Contains("telepathy"));
        }
        [TestMethod]
        public void Creature_Deva_Challenge()
        {
            Assert.IsTrue(Deva.Challenge.Contains("5,900"));
        }

        [TestMethod]
        public void Creature_Deva_InnateSpellcasting()
        {
            Assert.IsTrue(Deva.InnateSpellcasting.Contains("Raise Dead"));
        }
        [TestMethod]
        public void Creature_Deva_Spellcasting()
        {
            Assert.IsTrue(Deva.Spellcasting.Contains(""));
        }

        [TestMethod]
        public void Creature_Deva_Traits()
        {
            Assert.IsTrue(Deva.Traits.Contains("Magic Resistance: The deva has advantage on saving throws against spells and other magical effects."));
        }
        [TestMethod]
        public void Creature_Deva_Actions()
        {
            Assert.IsTrue(Deva.Actions.Contains("Multiattack: The deva makes two melee attacks."));
        }
        [TestMethod]
        public void Creature_Deva_Reactions()
        {
        }
        [TestMethod]
        public void Creature_Deva_LegendaryActions()
        {
        }

        [TestMethod]
        public void Creature_Deva_Uri()
        {
            Assert.IsTrue(string.Equals(Deva.Uri, "https://open5e.com/monsters/monsters_a-z/a/angels/deva.html", StringComparison.OrdinalIgnoreCase));
        }
    }
}
