using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class Weapon : Item
    {
        // TODO: Formula para el combate debería tener una probabilidad de acertar y luego una fórmula de daño
        // Fórmula de impacto: Base de acierto + ( Destreza - armadura ) * 10
        // Fórmula de daño: Base del arma + bonus - mitigación.


        // Hereda propiedades ID, Name y NamePlural de la clase padre "Item"

        public int MinimumDamage { get; set; }
        public int MaximumDamage { get; set; }
        public int BonusAFuerza { get; set; }
        public int BonusADestreza { get; set; }
        public int BonusAVidaMaxima { get; set; }
        public int BonusAPrecision { get; set; }
        public string SpecialName { get; set; }
        

        // public constructor con algunos parámetros derivados de la clase padre (base)
        public Weapon ( int id, string name, string namePlural, int minimumDamage, int maximumDamage) : base (id, name, namePlural)
        {
            MinimumDamage = minimumDamage;
            MaximumDamage = maximumDamage;
        }

        public Weapon ( int id, string name, string namePlural, string specialName, int minimumDamage, int maximumDamage, int bonusAFuerza, int bonusADestreza, int bonusAVidaMaxima, int bonusAPrecision) : base (id, name, namePlural)
        {
            SpecialName = specialName;
            MinimumDamage = minimumDamage;
            MaximumDamage = maximumDamage;
            BonusAFuerza = bonusAFuerza;
            BonusADestreza = bonusADestreza;
            BonusAVidaMaxima = bonusAPrecision;
            BonusAPrecision = bonusAPrecision;
            
        }
    }
}
