using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class Player : LivingCreature
    {
        
        // Hereda propiedades MaximumHitPoints y CurrentHitPoints de la clase padre "LivingCreature"
        public int Gold { get; set; }
        public int ExperiencePoints { get; set; }
        public int Level { get; set; }

        public Player(int maximumHitPoints, int currentHitPoints, int gold, int experiencePoints, int level)
            : base (maximumHitPoints, currentHitPoints)
        {
            Gold = gold;
            ExperiencePoints = experiencePoints;
            Level = level;
        }


    }
}
