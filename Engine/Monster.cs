using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class Monster : LivingCreature
    {

        // Hereda propiedades MaximumHitPoints y CurrentHitPoints de la clase padre "LivingCreature"
        public int ID { get; set; }
        public string Name { get; set; }
        public int MaximumDamage { get; set; }
        public int RewardExperiencePoints { get; set; }
        public int RewardGold { get; set; }
        public int Armor { get; set; }
        public List<LootItem> LootTable { get; set; }

        // public constructor con algunos parámetros derivados de la clase padre (base)
        public Monster ( int id, string name, int maximumHitPoints, int currentHitPoints, int maximumDamage, int rewardExperiencePoints, int rewardGold, int armor)
            : base ( maximumHitPoints, currentHitPoints)
        {
            ID = id;
            Name = name;
            MaximumDamage = maximumDamage;
            RewardExperiencePoints = rewardExperiencePoints;
            RewardGold = rewardGold;
            Armor = armor;
            LootTable = new List<LootItem>();
        }

    }
}
