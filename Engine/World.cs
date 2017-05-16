using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public static class World
    {

        public static readonly List<Item> Items = new List<Item>();
        public static readonly List<Monster> Monsters = new List<Monster>();
        public static readonly List<Quest> Quests = new List<Quest>();
        public static readonly List<Location> Locations = new List<Location>();

        public const int ITEM_ID_RUSTY_SWORD = 1;
        public const int ITEM_ID_RAT_TAIL = 2;
        public const int ITEM_ID_PIECE_OF_FUR = 3;
        public const int ITEM_ID_SNAKE_FANG = 4;
        public const int ITEM_ID_SNAKESKIN = 5;
        public const int ITEM_ID_CLUB = 6;
        public const int ITEM_ID_HEALING_POTION = 7;
        public const int ITEM_ID_SPIDER_FANG = 8;
        public const int ITEM_ID_SPIDER_SILK = 9;
        public const int ITEM_ID_ADVENTURER_PASS = 10;
        

        public const int MONSTER_ID_RAT = 1;
        public const int MONSTER_ID_SNAKE = 2;
        public const int MONSTER_ID_GIANT_SPIDER = 3;

        public const int QUEST_ID_CLEAR_ALCHEMIST_GARDEN = 1;
        public const int QUEST_ID_CLEAR_FARMERS_FIELD = 2;

        public const int LOCATION_ID_HOME = 1;
        public const int LOCATION_ID_TOWN_SQUARE = 2;
        public const int LOCATION_ID_GUARD_POST = 3;
        public const int LOCATION_ID_ALCHEMIST_HUT = 4;
        public const int LOCATION_ID_ALCHEMISTS_GARDEN = 5;
        public const int LOCATION_ID_FARMHOUSE = 6;
        public const int LOCATION_ID_FARM_FIELD = 7;
        public const int LOCATION_ID_BRIDGE = 8;
        public const int LOCATION_ID_SPIDER_FIELD = 9;
        public const int LOCATION_ID_BATHROOM = 10;

        static World()
        {
            PopulateItems();
            PopulateMonsters();
            PopulateQuests();
            PopulateLocations();
        }

        private static void PopulateItems()
        {
            Items.Add(new Weapon(ITEM_ID_RUSTY_SWORD, "Espada oxidada", "Espadas oxidadas", 1, 5));
            Items.Add(new Item(ITEM_ID_RAT_TAIL, "Cola de rata", "Colas de rata"));
            Items.Add(new Item(ITEM_ID_PIECE_OF_FUR, "Pieza de piel", "Piezas de piel"));
            Items.Add(new Item(ITEM_ID_SNAKE_FANG, "Colmillo de serpiente", "Colmillos de serpiente"));
            Items.Add(new Item(ITEM_ID_SNAKESKIN, "Escama de serpiente", "Escamas de serpiente"));
            Items.Add(new Weapon(ITEM_ID_CLUB, "Porra", "Porras", 3, 10));
            Items.Add(new HealingPotion(ITEM_ID_HEALING_POTION, "Poción curativa", "Pociones curativas", 5));
            Items.Add(new Item(ITEM_ID_SPIDER_FANG, "Colmillo de araña", "Colmillos de araña"));
            Items.Add(new Item(ITEM_ID_SPIDER_SILK, "Seda de araña", "Sedas de araña"));
            Items.Add(new Item(ITEM_ID_ADVENTURER_PASS, "Pase de Aventurero", "Pases de Aventurero"));
            
        }

        private static void PopulateMonsters()
        {
            Monster rat = new Monster(MONSTER_ID_RAT, "Rata", 5, 3, 10, 3, 3, 5);
            rat.LootTable.Add(new LootItem(ItemByID(ITEM_ID_RAT_TAIL), 75, false));
            rat.LootTable.Add(new LootItem(ItemByID(ITEM_ID_PIECE_OF_FUR), 75, true));

            Monster snake = new Monster(MONSTER_ID_SNAKE, "Serpiente", 5, 5, 10, 3, 3, 5);
            snake.LootTable.Add(new LootItem(ItemByID(ITEM_ID_SNAKE_FANG), 75, false));
            snake.LootTable.Add(new LootItem(ItemByID(ITEM_ID_SNAKESKIN), 75, true));

            Monster giantSpider = new Monster(MONSTER_ID_GIANT_SPIDER, "Araña gigante", 20, 5, 40, 10, 10, 7);
            giantSpider.LootTable.Add(new LootItem(ItemByID(ITEM_ID_SPIDER_FANG), 75, true));
            giantSpider.LootTable.Add(new LootItem(ItemByID(ITEM_ID_SPIDER_SILK), 25, false));

            Monsters.Add(rat);
            Monsters.Add(snake);
            Monsters.Add(giantSpider);
        }

        private static void PopulateQuests()
        {
            Quest clearAlchemistGarden =
                new Quest(
                    QUEST_ID_CLEAR_ALCHEMIST_GARDEN,
                    "'Despeja el jardín del Alquimista'",
                    "Mata ratas en el jardín del Alquimista y trae de vuelta 3 colas de rata. A cambio recibiras una poción curativa y 10 piezas de oro.", 20, 10);

            clearAlchemistGarden.QuestCompletionItems.Add(new QuestCompletionItem(ItemByID(ITEM_ID_RAT_TAIL), 3));

            clearAlchemistGarden.RewardItem = ItemByID(ITEM_ID_HEALING_POTION);

            Quest clearFarmersField =
                new Quest(
                    QUEST_ID_CLEAR_FARMERS_FIELD,
                    "'Despeja el campo de labranza'",
                    "Mata serpientes en el campo de labranza y trae de vuelta 3 colmillos de serpiente. Recibirás a cambio un Pase de Aventurero y 20 piezas de oro.", 20, 20);

            clearFarmersField.QuestCompletionItems.Add(new QuestCompletionItem(ItemByID(ITEM_ID_SNAKE_FANG), 3));

            clearFarmersField.RewardItem = ItemByID(ITEM_ID_ADVENTURER_PASS);

            Quests.Add(clearAlchemistGarden);
            Quests.Add(clearFarmersField);
        }

        private static void PopulateLocations()
        {
            // Crea cada localización usando el constructor
            Location home = new Location(LOCATION_ID_HOME, "Casa", "Tu acogedora cabaña de madera. Tus pocas posesiones están desperdigadas por el suelo sin ningún orden. Deberías estar fuera labrándote un nombre como aventurero.");

            Location townSquare = new Location(LOCATION_ID_TOWN_SQUARE, "Plaza del pueblo", "Todo parece tranquilo en la plaza del pueblo, no se ve un alma. Unos pocos pájaros beben de la borboteante fuente que decora el centro de la plaza.");

            Location alchemistHut = new Location(LOCATION_ID_ALCHEMIST_HUT, "Choza del Alquimista", "Un humo verdoso se eleva desde la chimenea de la choza. Numerosas hierbas decoran las mesas y burbujeantes retortas de extraños colores atraen tu atención.");
            alchemistHut.QuestAvailableHere = QuestByID(QUEST_ID_CLEAR_ALCHEMIST_GARDEN);

            Location alchemistsGarden = new Location(LOCATION_ID_ALCHEMISTS_GARDEN, "Jardín del Alquimista", "Aquí crecen numerosas plantas cuyos nombres desconoces, algunas con aspecto realmente amenazador.");
            alchemistsGarden.MonsterLivingHere = MonsterByID(MONSTER_ID_RAT);

            Location farmhouse = new Location(LOCATION_ID_FARMHOUSE, "Casa de labranza", "Un granjero te contempla desde la puerta de su cabaña. Un carro rebosante de maíz indica que la cosecha ha debido terminar hace poco.");
            farmhouse.QuestAvailableHere = QuestByID(QUEST_ID_CLEAR_FARMERS_FIELD);

            Location farmersField = new Location(LOCATION_ID_FARM_FIELD, "Campo de labranza", "Un maizal recientemente cosechado se extiende hasta donde alcanza la vista. Un desvencijado espantapájaros se mece con la brisa de otoño.");
            farmersField.MonsterLivingHere = MonsterByID(MONSTER_ID_SNAKE);

            Location guardPost = new Location(LOCATION_ID_GUARD_POST, "Puesto de guardia", "Una cabaña de piedra y un portón de madera te cortan el paso. Un enorme guardia con cara de pocos amigos vigila el portón.", ItemByID(ITEM_ID_ADVENTURER_PASS));

            Location bridge = new Location(LOCATION_ID_BRIDGE, "Puente de piedra", "Un antiguo puente de piedra cruza el ruidoso río. ");

            Location spiderField = new Location(LOCATION_ID_SPIDER_FIELD, "Bosque", "Este bosque es realmente antiguo y los árboles están tan juntos que cuesta ver la luz del sol. Telas de araña se extienden entre las ramas más altas y el aire trae un aroma siniestro.");
            spiderField.MonsterLivingHere = MonsterByID(MONSTER_ID_GIANT_SPIDER);


            // Enlaza todas las localizaciones entre ellas creando el mapa.
            home.LocationToNorth = townSquare;
            

            townSquare.LocationToNorth = alchemistHut;
            townSquare.LocationToSouth = home;
            townSquare.LocationToEast = guardPost;
            townSquare.LocationToWest = farmhouse;

            farmhouse.LocationToEast = townSquare;
            farmhouse.LocationToWest = farmersField;

            farmersField.LocationToEast = farmhouse;

            alchemistHut.LocationToSouth = townSquare;
            alchemistHut.LocationToNorth = alchemistsGarden;

            alchemistsGarden.LocationToSouth = alchemistHut;

            guardPost.LocationToEast = bridge;
            guardPost.LocationToWest = townSquare;

            bridge.LocationToWest = guardPost;
            bridge.LocationToEast = spiderField;

            spiderField.LocationToWest = bridge;

            // Añade las distintas localizaciones a la lista estática
            Locations.Add(home);
            Locations.Add(townSquare);
            Locations.Add(guardPost);
            Locations.Add(alchemistHut);
            Locations.Add(alchemistsGarden);
            Locations.Add(farmhouse);
            Locations.Add(farmersField);
            Locations.Add(bridge);
            Locations.Add(spiderField);
        }

        public static Item ItemByID(int id)
        {
            foreach (Item item in Items)
            {
                if (item.ID == id)
                {
                    return item;
                }
            }

            return null;
        }

        public static Monster MonsterByID(int id)
        {
            foreach (Monster monster in Monsters)
            {
                if (monster.ID == id)
                {
                    return monster;
                }
            }

            return null;
        }

        public static Quest QuestByID(int id)
        {
            foreach (Quest quest in Quests)
            {
                if (quest.ID == id)
                {
                    return quest;
                }
            }

            return null;
        }

        public static Location LocationByID(int id)
        {
            foreach (Location location in Locations)
            {
                if (location.ID == id)
                {
                    return location;
                }
            }

            return null;
        }
    }
}
