using System;
using System.Collections.Generic;
using System.Text;

namespace Open5ECreatureDownloader
{
    public sealed class Creature
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string ArmorClass { get; set; }
        public string HitPoints { get; set; }
        public string Speed { get; set; }

        public string Strength { get; set; }
        public string Dexterity { get; set; }
        public string Constitution { get; set; }
        public string Intelligence { get; set; }
        public string Wisdom { get; set; }
        public string Charisma { get; set; }

        public string SavingThrows { get; set; }
        public string Skills { get; set; }
        public string DamageResistance { get; set; }
        public string DamageImmunity { get; set; }
        public string ConditionImmunity { get; set; }
        public string Senses { get; set; }
        public string Languages { get; set; }
        public string Challenge { get; set; }

        public string InnateSpellcasting { get; set; }
        public string Spellcasting { get; set; }

        public string[] Traits { get; set; }
        public string[] Actions { get; set; }
        public string[] Reactions { get; set; }
        public string[] LegendaryActions { get; set; }

        public string Uri { get; set; }

        public override string ToString()
        {
            return $"Name:{Name}" + Environment.NewLine +
           $"Type:{Type}" + Environment.NewLine +
           $"ArmorClass:{ArmorClass}" + Environment.NewLine +
           $"HitPoints:{HitPoints}" + Environment.NewLine +
           $"Speed:{Speed}" + Environment.NewLine +

           $"Strength:{Strength}" + Environment.NewLine +
           $"Dexterity:{Dexterity}" + Environment.NewLine +
           $"Constitution:{Constitution}" + Environment.NewLine +
           $"Intelligence:{Intelligence}" + Environment.NewLine +
           $"Wisdom:{Wisdom}" + Environment.NewLine +
           $"Charisma:{Charisma}" + Environment.NewLine +

           $"SavingThrows:{SavingThrows}" + Environment.NewLine +
           $"Skills:{Skills}" + Environment.NewLine +
           $"DamageResistance:{DamageResistance}" + Environment.NewLine +
           $"DamageImmunity:{DamageImmunity}" + Environment.NewLine +
           $"Senses:{Senses}" + Environment.NewLine +
           $"Languages:{Languages}" + Environment.NewLine +
           $"Challenge:{Challenge}" + Environment.NewLine +

           $"InnateSpellcasting:{InnateSpellcasting}" + Environment.NewLine +
           $"Spellcasting:{Spellcasting}" + Environment.NewLine +

           $"Traits:{string.Join(";", Traits)}" + Environment.NewLine +
           $"Actions:{string.Join(";", Actions)}" + Environment.NewLine +
           $"Reactions:{string.Join(";", Reactions)}" + Environment.NewLine +
           $"LegendaryActions:{string.Join(";", LegendaryActions)}" + Environment.NewLine +

           $"Uri:{Uri}";
        }
    }
}
