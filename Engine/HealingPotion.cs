using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class HealingPotion : Item
    {
        // hereda propiedades ID, Name y NamePlural de la clase padre "Item"

        public int AmountToHeal { get; set; }
        
        // public constructor derivando algunos parámetros desde la clase padre (base)
        public HealingPotion ( int id, string name, string namePlural, int amountToHeal) : base( id, name, namePlural)
        {
            AmountToHeal = amountToHeal;
        }

    }
}
