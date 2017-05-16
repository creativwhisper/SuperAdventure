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
        public int CurrentStrength { get; set; }
        public int CurrentDexterity { get; set; }
        public Location CurrentLocation { get; set; }
        public List<InventoryItem> Inventory { get; set; }
        public List<PlayerQuest> Quests { get; set; }

        

        public Player(int maximumHitPoints, int currentHitPoints, int gold, int experiencePoints, int level, int currentStrength, int currentDexterity)
            : base (maximumHitPoints, currentHitPoints)
        {
            Gold = gold;
            ExperiencePoints = experiencePoints;
            Level = level;
            CurrentStrength = currentStrength;
            CurrentDexterity = currentDexterity;
            Inventory = new List<InventoryItem>();
            Quests = new List<PlayerQuest>();
            
        }

        public int SetNewLevel()
        {
            Level++;
            return Level;

        }

        public int SetNewDexterity()
        {
            CurrentDexterity++;
            return CurrentDexterity;
        }

        public int SetNewStrength()
        {
            CurrentStrength++;
            return CurrentStrength;
        }

        public int SetNewMaxHealth()
        {
            MaximumHitPoints += 5;
            return MaximumHitPoints;
        }

        public bool HasRequiredItemToEnterThisLocation(Location location)
        {
            if (location.ItemRequiredToEnter == null)
            {
                // No hay requisitos para entrar en la ubicación, así que devolvemos true
                return true;
            }

            // Comprueba si el jugador tiene el objeto que se requiere para entrar
            foreach (InventoryItem ii in Inventory)
            {
                if (ii.Details.ID == location.ItemRequiredToEnter.ID)
                {
                    // Hemos encontrado el objeto así que devolvemos true
                    return true;
                }
            }

            // No hemos encontrado el objeto requerido así que devolvemos false
            return false;
        }

        public bool HasThisQuest(Quest quest)
        {
            foreach (PlayerQuest playerQuest in Quests)
            {
                if (playerQuest.Details.ID == quest.ID)
                {
                    return true;
                }
            }

            return false;
        }

        public bool CompletedThisQuest(Quest quest)
        {
            foreach (PlayerQuest playerQuest in Quests)
            {
                if (playerQuest.Details.ID == quest.ID)
                {
                    return playerQuest.IsCompleted;
                }
            }

            return false;
        }

        public bool HasAllQuestCompletionItems(Quest quest)
        {
            // Comprueba si el jugador tiene todos los objetos para completar la misión
            foreach (QuestCompletionItem qci in quest.QuestCompletionItems)
            {
                bool foundItemInPlayersInventory = false;

                // Comprueba cada item en el inventario para ver si tiene el necesario y en la cantidad correcta
                foreach (InventoryItem ii in Inventory)
                {
                    if (ii.Details.ID == qci.Details.ID) // El jugador tiene el item en el inventario
                    {
                        foundItemInPlayersInventory = true;

                        if (ii.Quantity < qci.Quantity) // El jugador no tiene la cantidad requerida para la misión.
                        {
                            return false;
                        }
                    }
                }

                // El jugador no tiene ningún objeto de los requeridos.
                if (!foundItemInPlayersInventory)
                {
                    return false;
                }
            }

            // Si el jugador tiene el item y además en la cantidad correcta para completar la misión
            return true;
        }

        public void RemoveQuestCompletionItems(Quest quest)
        {
            foreach (QuestCompletionItem qci in quest.QuestCompletionItems)
            {
                foreach (InventoryItem ii in Inventory)
                {
                    if (ii.Details.ID == qci.Details.ID)
                    {
                        // Quita el número de objetos necesarios para completar la misión del inventario del jugador.
                        ii.Quantity -= qci.Quantity;
                        break;
                    }
                }
            }
        }

        public void AddItemToInventory(Item itemToAdd)
        {
            foreach (InventoryItem ii in Inventory)
            {
                if (ii.Details.ID == itemToAdd.ID)
                {
                    // Tiene el objeto en el inventario, así que añadimos uno más y salimos de la función
                    ii.Quantity++;

                    return; 
                }
            }

            // No tiene ninguno en el inventario así que lo añadimos con cantidad 1
            Inventory.Add(new InventoryItem(itemToAdd, 1));
        }

        public void MarkQuestCompleted(Quest quest)
        {
            // Busca la misión en la lista de misiones del jugador
            foreach (PlayerQuest pq in Quests)
            {
                if (pq.Details.ID == quest.ID)
                {
                    // Marca la misión como completada y salimos de la función
                    pq.IsCompleted = true;

                    return;
                }
            }
        }
    }
}
