using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

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

        private Player(int currentHitPoints, int maximumHitPoints, int level, int currentStrength, int currentDexterity, int gold, int experiencePoints) : base(currentHitPoints, maximumHitPoints)
        {
            Level = level;
            CurrentStrength = currentStrength;
            CurrentDexterity = currentDexterity;
            Gold = gold;
            ExperiencePoints = experiencePoints;

            Inventory = new List<InventoryItem>();
            Quests = new List<PlayerQuest>();
        }

        public static Player CreateDefaultPlayer()
        {
            Player player = new Player(10, 10, 1, 5, 5, 20, 0);
            player.Inventory.Add(new InventoryItem(World.ItemByID(World.ITEM_ID_RUSTY_SWORD), 1));
            player.CurrentLocation = World.LocationByID(World.LOCATION_ID_HOME);

            return player;
        }

        public static Player CreatePlayerFromXmlString(string xmlPlayerData)
        {
            try
            {
                XmlDocument playerData = new XmlDocument();

                playerData.LoadXml(xmlPlayerData);

                int currentHitPoints = Convert.ToInt32(playerData.SelectSingleNode("/Player/Stats/CurrentHitPoints").InnerText);
                int maximumHitPoints = Convert.ToInt32(playerData.SelectSingleNode("/Player/Stats/MaximumHitPoints").InnerText);
                int level = Convert.ToInt32(playerData.SelectSingleNode("/Player/Stats/Level").InnerText);
                int currentStrength = Convert.ToInt32(playerData.SelectSingleNode("/Player/Stats/CurrentStrength").InnerText);
                int currentDexterity = Convert.ToInt32(playerData.SelectSingleNode("/Player/Stats/CurrentDexterity").InnerText);
                int gold = Convert.ToInt32(playerData.SelectSingleNode("/Player/Stats/Gold").InnerText);
                int experiencePoints = Convert.ToInt32(playerData.SelectSingleNode("/Player/Stats/ExperiencePoints").InnerText);

                Player player = new Player(currentHitPoints, maximumHitPoints, level, currentStrength, currentDexterity, gold, experiencePoints);

                int currentLocationID = Convert.ToInt32(playerData.SelectSingleNode("/Player/Stats/CurrentLocation").InnerText);
                player.CurrentLocation = World.LocationByID(currentLocationID);

                foreach (XmlNode node in playerData.SelectNodes("/Player/InventoryItems/InventoryItem"))
                {
                    int id = Convert.ToInt32(node.Attributes["ID"].Value);
                    int quantity = Convert.ToInt32(node.Attributes["Quantity"].Value);

                    for (int i = 0; i < quantity; i++)
                    {
                        player.AddItemToInventory(World.ItemByID(id));
                    }
                }

                foreach (XmlNode node in playerData.SelectNodes("/Player/PlayerQuests/PlayerQuest"))
                {
                    int id = Convert.ToInt32(node.Attributes["ID"].Value);
                    bool isCompleted = Convert.ToBoolean(node.Attributes["IsCompleted"].Value);

                    PlayerQuest playerQuest = new PlayerQuest(World.QuestByID(id));
                    playerQuest.IsCompleted = isCompleted;

                    player.Quests.Add(playerQuest);
                }

                return player;
            }
            catch
            {
                // If there was an error with the XML data, return a default player object
                return Player.CreateDefaultPlayer();
            }
        }

        // Función para convertir los datos de la ficha de personaje a un archivo XML
        public string ToXmlString()
        {
            // Crea el documento
            XmlDocument playerData = new XmlDocument();

            // Crea el nodo más alto de la jerarquía del que cuelgan Stats, InventoryItems y PlayerQuests
            XmlNode player = playerData.CreateElement("Player");
            playerData.AppendChild(player);

            // Crea el segundo nodo, Stats, de donde colgarán los nodos con los datos.
            XmlNode stats = playerData.CreateElement("Stats");
            player.AppendChild(stats);

            // Crea los nodos hijos del de Stats
            XmlNode currentHitPoints = playerData.CreateElement("CurrentHitPoints");
            currentHitPoints.AppendChild(playerData.CreateTextNode(this.CurrentHitPoints.ToString()));
            stats.AppendChild(currentHitPoints);

            XmlNode maximumHitPoints = playerData.CreateElement("MaximumHitPoints");
            maximumHitPoints.AppendChild(playerData.CreateTextNode(this.MaximumHitPoints.ToString()));
            stats.AppendChild(maximumHitPoints);

            XmlNode currentStrength = playerData.CreateElement("CurrentStrength");
            currentStrength.AppendChild(playerData.CreateTextNode(this.CurrentStrength.ToString()));
            stats.AppendChild(currentStrength);

            XmlNode currentDexterity = playerData.CreateElement("CurrentDexterity");
            currentDexterity.AppendChild(playerData.CreateTextNode(this.CurrentDexterity.ToString()));
            stats.AppendChild(currentDexterity);

            XmlNode level = playerData.CreateElement("Level");
            level.AppendChild(playerData.CreateTextNode(this.Level.ToString()));
            stats.AppendChild(level);

            XmlNode gold = playerData.CreateElement("Gold");
            gold.AppendChild(playerData.CreateTextNode(this.Gold.ToString()));
            stats.AppendChild(gold);

            XmlNode experiencePoints = playerData.CreateElement("ExperiencePoints");
            experiencePoints.AppendChild(playerData.CreateTextNode(this.ExperiencePoints.ToString()));
            stats.AppendChild(experiencePoints);

            XmlNode currentLocation = playerData.CreateElement("CurrentLocation");
            currentLocation.AppendChild(playerData.CreateTextNode(this.CurrentLocation.ID.ToString()));
            stats.AppendChild(currentLocation);

            // Crea el nodo InventoryItems del que colgarán todos los nodos con los objetos del inventario
            XmlNode inventoryItems = playerData.CreateElement("InventoryItems");
            player.AppendChild(inventoryItems);

            // Crea un nodo InventoryItem por cada objeto en el inventario del jugador.
            foreach (InventoryItem item in this.Inventory)
            {
                XmlNode inventoryItem = playerData.CreateElement("InventoryItem");

                XmlAttribute idAttribute = playerData.CreateAttribute("ID");
                idAttribute.Value = item.Details.ID.ToString();
                inventoryItem.Attributes.Append(idAttribute);

                XmlAttribute quantityAttribute = playerData.CreateAttribute("Quantity");
                quantityAttribute.Value = item.Quantity.ToString();
                inventoryItem.Attributes.Append(quantityAttribute);

                inventoryItems.AppendChild(inventoryItem);
            }

            // Crea el nodo hijo PlayerQuests del que colgarán los nodos de las misiones del jugador
            XmlNode playerQuests = playerData.CreateElement("PlayerQuests");
            player.AppendChild(playerQuests);

            // Crea un nodo PlayerQuest para cada quest que haya activa en la lista del jugador.
            foreach (PlayerQuest quest in this.Quests)
            {
                XmlNode playerQuest = playerData.CreateElement("PlayerQuest");

                XmlAttribute idAttribute = playerData.CreateAttribute("ID");
                idAttribute.Value = quest.Details.ID.ToString();
                playerQuest.Attributes.Append(idAttribute);

                XmlAttribute isCompletedAttribute = playerData.CreateAttribute("IsCompleted");
                isCompletedAttribute.Value = quest.IsCompleted.ToString();
                playerQuest.Attributes.Append(isCompletedAttribute);

                playerQuests.AppendChild(playerQuest);
            }

            return playerData.InnerXml; // Retorna el documento XML en un string, para poder salvarlo a disco.
        }

        public int SetNewLevel()
        {
            Level++;
            CurrentStrength++;
            CurrentDexterity++;
            MaximumHitPoints += 5;
            return Level;

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
