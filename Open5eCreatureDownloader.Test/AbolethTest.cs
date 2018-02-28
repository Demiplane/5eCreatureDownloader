using Microsoft.VisualStudio.TestTools.UnitTesting;
using Open5ECreatureDownloader;
using System;
using System.Diagnostics;
using System.Linq;

namespace Open5eCreatureDownloader.Test
{
    [TestClass]
    public class AbolethTest
    {
        private static Lazy<Creature> LazyAboleth = new Lazy<Creature>(() => new CreatureDownloader().DownloadCreatures().Skip(3).First());
        private static Creature Aboleth => LazyAboleth.Value;

        [TestMethod]
        public void Creature_Aboleth_Name()
        {
            Assert.IsTrue(Aboleth.Name.Contains("Aboleth"));
        }
        [TestMethod]
        public void Creature_Aboleth_Type()
        {
            Assert.IsTrue(Aboleth.Type.Contains("aberration"));
        }
        [TestMethod]
        public void Creature_Aboleth_ArmorClass()
        {
            Assert.IsTrue(Aboleth.ArmorClass.Contains("17"));
        }
        [TestMethod]
        public void Creature_Aboleth_HitPoints()
        {
            Assert.IsTrue(Aboleth.HitPoints.Contains("135"));
        }
        [TestMethod]
        public void Creature_Aboleth_Speed()
        {
            Assert.IsTrue(Aboleth.Speed.Contains("swim 40"));
        }

        [TestMethod]
        public void Creature_Aboleth_Strength()
        {
            Assert.IsTrue(Aboleth.Strength.Contains("21"));
        }
        [TestMethod]
        public void Creature_Aboleth_Dexterity()
        {
            Assert.IsTrue(Aboleth.Dexterity.Contains("9"));
        }
        [TestMethod]
        public void Creature_Aboleth_Constitution()
        {
            Assert.IsTrue(Aboleth.Constitution.Contains("15"));
        }
        [TestMethod]
        public void Creature_Aboleth_Intelligence()
        {
            Assert.IsTrue(Aboleth.Intelligence.Contains("18"));
        }
        [TestMethod]
        public void Creature_Aboleth_Wisdom()
        {
            Assert.IsTrue(Aboleth.Wisdom.Contains("15"));
        }
        [TestMethod]
        public void Creature_Aboleth_Charisma()
        {
            Assert.IsTrue(Aboleth.Charisma.Contains("18"));
        }

        [TestMethod]
        public void Creature_Aboleth_SavingThrows()
        {
            Assert.IsTrue(Aboleth.SavingThrows.Contains("Int +8"));
        }
        [TestMethod]
        public void Creature_Aboleth_Skills()
        {
            Assert.IsTrue(Aboleth.Skills.Contains("Perception +10"));
        }
        [TestMethod]
        public void Creature_Aboleth_DamageResistance()
        {
        }
        [TestMethod]
        public void Creature_Aboleth_DamageImmunity()
        {
        }
        [TestMethod]
        public void Creature_Aboleth_ConditionImmunity()
        {
        }

        [TestMethod]
        public void Creature_Aboleth_Senses()
        {
            Assert.IsTrue(Aboleth.Senses.Contains("darkvision 120"));
        }
        [TestMethod]
        public void Creature_Aboleth_Languages()
        {
            Assert.IsTrue(Aboleth.Languages.Contains("telepathy"));
        }
        [TestMethod]
        public void Creature_Aboleth_Challenge()
        {
            Assert.IsTrue(Aboleth.Challenge.Contains("5,900"));
        }

        [TestMethod]
        public void Creature_Aboleth_InnateSpellcasting()
        {
        }
        [TestMethod]
        public void Creature_Aboleth_Spellcasting()
        {
        }

        [TestMethod]
        public void Creature_Aboleth_Traits()
        {
            Assert.IsTrue(Aboleth.Traits.Contains("Amphibious: The aboleth can breathe air and water."));
        }
        [TestMethod]
        public void Creature_Aboleth_Actions()
        {
            Assert.IsTrue(Aboleth.Actions.Contains("Tail: Melee Weapon Attack: +9 to hit, reach 10 ft. one target. Hit: 15 (3d6 + 5) bludgeoning damage."));
        }
        [TestMethod]
        public void Creature_Aboleth_Reactions()
        {
        }
        [TestMethod]
        public void Creature_Aboleth_LegendaryActions()
        {
            Assert.IsTrue(Aboleth.LegendaryActions.Contains("Detect: The aboleth makes a Wisdom (Perception) check."));
        }

        [TestMethod]
        public void Creature_Aboleth_Uri()
        {
            Assert.IsTrue(string.Equals(Aboleth.Uri, "https://open5e.com/monsters/monsters_a-z/a/aboleth.html", StringComparison.OrdinalIgnoreCase));
        }
    }
}
