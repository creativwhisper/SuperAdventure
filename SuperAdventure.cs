using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

using Engine; // Llamada a la librería de clases donde almacenamos la lógica de juego.

namespace SuperAdventure
{
    public partial class SuperAdventure : Form
    {
        private Player _player;
        private Monster _currentMonster;
        private const string PLAYER_DATA_FILE_NAME = "PlayerData.xml";

        private int experienceRequiredToLevel = 100;

        public SuperAdventure()
        {
            InitializeComponent();

            if (File.Exists(PLAYER_DATA_FILE_NAME))
            {
                _player = Player.CreatePlayerFromXmlString(File.ReadAllText(PLAYER_DATA_FILE_NAME));
            }
            else
            {
                _player = Player.CreateDefaultPlayer();
                rtbMessages.Text += "Comienzo de la aventura" + Environment.NewLine;
            }

            MoveTo(_player.CurrentLocation);
            
            // _player.Inventory.Add(new InventoryItem(World.ItemByID(World.ITEM_ID_RUSTY_SWORD), 1));

            UpdatePlayerStats();
        }

        private void btnNorth_Click(object sender, EventArgs e)
        {
            MoveTo(_player.CurrentLocation.LocationToNorth);
        }

        private void btnEast_Click(object sender, EventArgs e)
        {
            MoveTo(_player.CurrentLocation.LocationToEast);
        }

        private void btnSouth_Click(object sender, EventArgs e)
        {
            MoveTo(_player.CurrentLocation.LocationToSouth);
        }

        private void btnWest_Click(object sender, EventArgs e)
        {
            MoveTo(_player.CurrentLocation.LocationToWest);
        }

        private void MoveTo(Location newLocation)
        {
            // Confirma que el sitio al que vamos tiene objeto requerido
            if (!_player.HasRequiredItemToEnterThisLocation(newLocation))
            {
                rtbMessages.Text += "Tienes que tener un " + newLocation.ItemRequiredToEnter.Name + " para entrar aquí." + Environment.NewLine;
                return;
            }

            // Actualiza la posición del jugador
            _player.CurrentLocation = newLocation;

            // Muestra u oculta los botones de dirección según si hay o no algo en aquella dirección.
            btnNorth.Visible = (newLocation.LocationToNorth != null);
            btnEast.Visible = (newLocation.LocationToEast != null);
            btnSouth.Visible = (newLocation.LocationToSouth != null);
            btnWest.Visible = (newLocation.LocationToWest != null);

            // Muestra en pantalla la localización actual y su descripción
            rtbLocation.Text = newLocation.Name + Environment.NewLine;
            rtbLocation.Text += newLocation.Description + Environment.NewLine; // Enviroment.NewLine es un retorno de carro para que el texto vaya a la siguiente linea.

            // Sana completamente al jugador.
            _player.CurrentHitPoints = _player.MaximumHitPoints;

            // Actualiza la salud en la pantalla
            lblHitPoints.Text = _player.CurrentHitPoints.ToString();

            // Comprueba si hay alguna misión en esta localización
            if (newLocation.QuestAvailableHere != null)
            {
                // Comprueba si el jugador tiene la quest y luego si ya la ha completado.
                bool playerAlreadyHasQuest = _player.HasThisQuest(newLocation.QuestAvailableHere);
                bool playerAlreadyCompletedQuest = _player.CompletedThisQuest(newLocation.QuestAvailableHere);
                

                // Comprueba que el jugador tenga la quest.
                if (playerAlreadyHasQuest)
                {
                    // Si el jugador no ha completado esta quest todavía
                    if (!playerAlreadyCompletedQuest)
                    {
                        // Comprueba si el jugador tiene todos los objetos requeridos para superar la misión.
                        bool playerHasAllItemsToCompleteQuest = _player.HasAllQuestCompletionItems(newLocation.QuestAvailableHere);

                        // Si el jugador tiene todos los objetos necesarios
                        if (playerHasAllItemsToCompleteQuest)
                        {
                            // Muestra el mensaje
                            rtbMessages.Text += Environment.NewLine;
                            rtbMessages.Text += "Has completado la misión '" + newLocation.QuestAvailableHere.Name + "'." + Environment.NewLine;

                            // Elimina los objetos de quest
                            _player.RemoveQuestCompletionItems(newLocation.QuestAvailableHere);

                            // Entrega al jugador las recompensas por superar la misióno
                            rtbMessages.Text += "Recibes: " + Environment.NewLine;
                            rtbMessages.Text += newLocation.QuestAvailableHere.RewardExperiencePoints.ToString() + " puntos de experiencia" + Environment.NewLine;
                            rtbMessages.Text += newLocation.QuestAvailableHere.RewardGold.ToString() + " piezas de oro" + Environment.NewLine;
                            rtbMessages.Text += newLocation.QuestAvailableHere.RewardItem.Name + Environment.NewLine;
                            rtbMessages.Text += Environment.NewLine;

                            _player.ExperiencePoints += newLocation.QuestAvailableHere.RewardExperiencePoints;
                            _player.Gold += newLocation.QuestAvailableHere.RewardGold;

                            _player.AddItemToInventory(newLocation.QuestAvailableHere.RewardItem);

                            // Marca la misión como completada
                            // Busca la misión en la lista de misiones del jugador.
                            _player.MarkQuestCompleted(newLocation.QuestAvailableHere);
                        }
                    }
                }
                else
                {
                    // El jugador no tiene la misión aún

                    // Muestra los mensajes
                    rtbMessages.Text += "Recibes la misión " + newLocation.QuestAvailableHere.Name + "." + Environment.NewLine;
                    rtbMessages.Text += newLocation.QuestAvailableHere.Description + Environment.NewLine;
                    rtbMessages.Text += "Para completarla, regresa con:" + Environment.NewLine;
                    foreach (QuestCompletionItem qci in newLocation.QuestAvailableHere.QuestCompletionItems)
                    {
                        if (qci.Quantity == 1)
                        {
                            rtbMessages.Text += qci.Quantity.ToString() + " " + qci.Details.Name + Environment.NewLine;
                        }
                        else
                        {
                            rtbMessages.Text += qci.Quantity.ToString() + " " + qci.Details.NamePlural + Environment.NewLine;
                        }
                    }
                    rtbMessages.Text += Environment.NewLine;

                    // Añade la misión a la lista de misiones del jugador.
                    _player.Quests.Add(new PlayerQuest(newLocation.QuestAvailableHere));
                }
            }

            // Hay un monstruo en esta localización?
            if (newLocation.MonsterLivingHere != null)
            {
                rtbMessages.Text += "Ves una " + newLocation.MonsterLivingHere.Name + Environment.NewLine;

                // Crea un monstruo usando los valores de la tabla general de monstruos
                Monster standardMonster = World.MonsterByID(newLocation.MonsterLivingHere.ID);

                _currentMonster = new Monster(standardMonster.ID, standardMonster.Name, standardMonster.MaximumDamage,
                    standardMonster.RewardExperiencePoints, standardMonster.RewardGold, standardMonster.CurrentHitPoints, standardMonster.MaximumHitPoints, standardMonster.Armor);

                // Añade los objetos que recibirá el jugador al matarlo a la lista de Loot del monstruo.
                foreach (LootItem lootItem in standardMonster.LootTable)
                {
                    _currentMonster.LootTable.Add(lootItem);
                }

                // Activa los botones y desplegables de Ataque y Uso de Pociones.
                cboWeapons.Visible = true;
                cboPotions.Visible = true;
                btnUseWeapon.Visible = true;
                btnUsePotion.Visible = true;
            }
            else
            {
                // Marca la instancia como inexistente
                _currentMonster = null;

                // Desactiva los botones y desplegables de Ataque y Uso de Pociones.
                cboWeapons.Visible = false;
                cboPotions.Visible = false;
                btnUseWeapon.Visible = false;
                btnUsePotion.Visible = false;
            }

            // Actualiza el inventario del jugador.
            UpdateInventoryListInUI();

            // Actualiza la lista de misiones del jugador.
            UpdateQuestListInUI();

            // Actualiza el desplegable de armas del jugador.
            UpdateWeaponListInUI();

            // Actualiza el desplegable de pociones del jugador.
            UpdatePotionListInUI();
        }

        private void UpdateInventoryListInUI()
        {
            dgvInventory.RowHeadersVisible = false;

            dgvInventory.ColumnCount = 2;
            dgvInventory.Columns[0].Name = "Nombre";
            dgvInventory.Columns[0].Width = 197;
            dgvInventory.Columns[1].Name = "Cantidad";

            dgvInventory.Rows.Clear();

            foreach (InventoryItem inventoryItem in _player.Inventory)
            {
                if (inventoryItem.Quantity > 0)
                {
                    dgvInventory.Rows.Add(new[] { inventoryItem.Details.Name, inventoryItem.Quantity.ToString() });
                }
            }
        }

        private void UpdateQuestListInUI()
        {
            dgvQuests.RowHeadersVisible = false;

            dgvQuests.ColumnCount = 2;
            dgvQuests.Columns[0].Name = "Nombre";
            dgvQuests.Columns[0].Width = 197;
            dgvQuests.Columns[1].Name = "Completada";

            dgvQuests.Rows.Clear();

            foreach (PlayerQuest playerQuest in _player.Quests)
            {
                dgvQuests.Rows.Add(new[] { playerQuest.Details.Name, playerQuest.IsCompleted.ToString() });
            }
        }

        private void UpdateWeaponListInUI()
        {
            List<Weapon> weapons = new List<Weapon>();

            foreach (InventoryItem inventoryItem in _player.Inventory)
            {
                if (inventoryItem.Details is Weapon)
                {
                    if (inventoryItem.Quantity > 0)
                    {
                        weapons.Add((Weapon)inventoryItem.Details);
                    }
                }
            }

            if (weapons.Count == 0)
            {
                // El jugador no tiene armas así que ocultamos el botón y el combobox
                cboWeapons.Visible = false;
                btnUseWeapon.Visible = false;
            }
            else
            {
                cboWeapons.DataSource = weapons;
                cboWeapons.DisplayMember = "Name";
                cboWeapons.ValueMember = "ID";

                cboWeapons.SelectedIndex = 0;
            }
        }

        private void UpdatePotionListInUI()
        {
            List<HealingPotion> healingPotions = new List<HealingPotion>();

            foreach (InventoryItem inventoryItem in _player.Inventory)
            {
                if (inventoryItem.Details is HealingPotion)
                {
                    if (inventoryItem.Quantity > 0)
                    {
                        healingPotions.Add((HealingPotion)inventoryItem.Details);
                    }
                }
            }

            if (healingPotions.Count == 0)
            {
                // El jugador no tiene pociones así que escondemos el botón y el combobox
                cboPotions.Visible = false;
                btnUsePotion.Visible = false;
            }
            else
            {
                cboPotions.DataSource = healingPotions;
                cboPotions.DisplayMember = "Name";
                cboPotions.ValueMember = "ID";

                cboPotions.SelectedIndex = 0;
            }
        }



        private void btnUseWeapon_Click(object sender, EventArgs e)
        {
            if (_currentMonster != null)
            {
                // Toma el arma seleccionada en el combobox
                Weapon currentWeapon = (Weapon)cboWeapons.SelectedItem;

                // Determinamos el ratio de acierto y si baja de 20 lo igualamos a 20
                int chanceToHitMonster = 20 + (_player.CurrentDexterity - _currentMonster.Armor) * 10;
                
                if (chanceToHitMonster < 20)
                {
                    chanceToHitMonster = 20;
                } else if (chanceToHitMonster > 95)
                {
                    chanceToHitMonster = 95;
                }

                // Tiramos los dados
                int diceThrow = RandomNumberGenerator.DiceThrow();
                
                lblUltTirada.Text = diceThrow.ToString();

                // Comprobamos si el golpe impacta
                if (diceThrow <= chanceToHitMonster)
                {
                    // Calcula el daño que le hace al monstruo
                    int baseDamage = RandomNumberGenerator.NumberBetween(currentWeapon.MinimumDamage, currentWeapon.MaximumDamage);
                    int damageToMonster = (baseDamage + _player.CurrentStrength) - _currentMonster.Armor;

                    // Aplica el daño al monstruo actual.
                    _currentMonster.CurrentHitPoints -= damageToMonster;

                    // Muestra mensaje
                    rtbMessages.Text += "Golpeas a la " + _currentMonster.Name + " por un total de " + damageToMonster.ToString() + " puntos de daño." + Environment.NewLine;
                }
                else
                {
                    rtbMessages.Text += "¡Atacas al monstruo pero no consigues golpearle!" + Environment.NewLine;
                }

                // Comprueba si el monstruo ha muerto
                if (_currentMonster.CurrentHitPoints <= 0)
                {
                    // Si el monstruo ha muerto
                    rtbMessages.Text += Environment.NewLine;
                    rtbMessages.Text += "Has derrotado a la " + _currentMonster.Name + Environment.NewLine;

                    // Le da al jugador puntos de experiencia por la muerte
                    _player.ExperiencePoints += _currentMonster.RewardExperiencePoints;

                    // Comprueba si los puntos de experiencia ha llegado al requisito del nuevo nivel
                    if( _player.ExperiencePoints >= experienceRequiredToLevel)
                    {
                        // Llama a la función de subir nivel
                        _player.SetNewLevel();
                        // Algunas funciones preparadas para cuando haya subidas de nivel que sólo alteren algunos valores
                        // Ahora mismo deshabilitado
                        // Llama a la función que incrementa la fuerza
                        //_player.SetNewStrength();
                        // Llama a la función que incrementa la destreza
                        //_player.SetNewDexterity();
                        // Llama a la función que incrementa la vida máxima
                        //_player.SetNewMaxHealth();

                        // dobla el número de puntos necesario para subir de nivel la próxima vez
                        experienceRequiredToLevel = experienceRequiredToLevel * 2;
                        int nextLevel = _player.Level + 1;
                        rtbMessages.Text += "¡Has subido de nivel!" + Environment.NewLine;
                        rtbMessages.Text += "Necesitarás " + experienceRequiredToLevel.ToString() + " para alcanzar el nivel " + nextLevel + "." + Environment.NewLine;
                    }
                    rtbMessages.Text += "Has recibido " + _currentMonster.RewardExperiencePoints.ToString() + " puntos de experiencia." + Environment.NewLine;

                    // Le da al jugador oro por la muerte 
                    _player.Gold += _currentMonster.RewardGold;
                    rtbMessages.Text += "Recibes " + _currentMonster.RewardGold.ToString() + " piezas de oro" + Environment.NewLine;

                    // Recibe objetos de loot aleatorios de la lista
                    List<InventoryItem> lootedItems = new List<InventoryItem>();

                    // Añade objetos a la lista de loot según el porcentaje de aparición
                    foreach (LootItem lootItem in _currentMonster.LootTable)
                    {
                        if (RandomNumberGenerator.NumberBetween(1, 100) <= lootItem.DropPercentage)
                        {
                            lootedItems.Add(new InventoryItem(lootItem.Details, 1));
                        }
                    }

                    // Si no hay items aleatorios que entregar, se entregan los default.
                    if (lootedItems.Count == 0)
                    {
                        foreach (LootItem lootItem in _currentMonster.LootTable)
                        {
                            if (lootItem.IsDefaultItem)
                            {
                                lootedItems.Add(new InventoryItem(lootItem.Details, 1));
                            }
                        }
                    }

                    // Añade los objetos de loot al inventario del jugador
                    foreach (InventoryItem inventoryItem in lootedItems)
                    {
                        _player.AddItemToInventory(inventoryItem.Details);

                        if (inventoryItem.Quantity == 1)
                        {
                            rtbMessages.Text += "Recoges " + inventoryItem.Quantity.ToString() + " " + inventoryItem.Details.Name + " del cuerpo." + Environment.NewLine;
                        }
                        else
                        {
                            rtbMessages.Text += "Recoges " + inventoryItem.Quantity.ToString() + " " + inventoryItem.Details.NamePlural + " del cuerpo." + Environment.NewLine;
                        }
                    }

                    UpdatePlayerStats();

                    UpdateInventoryListInUI();
                    UpdateWeaponListInUI();
                    UpdatePotionListInUI();
                    _currentMonster = null;

                    // Añade una nueva línea al texto, sólo para añadir claridad.
                    rtbMessages.Text += Environment.NewLine;

                    
                }
                else
                {
                    // El monstruo sigue vivo

                    // Determina el daño que el monstruo le hace al jugador
                    int damageToPlayer = RandomNumberGenerator.NumberBetween(0, _currentMonster.MaximumDamage);

                    // Muestra mensaje
                    rtbMessages.Text += "La " + _currentMonster.Name + " te golpea por " + damageToPlayer.ToString() + " puntos de daño." + Environment.NewLine;

                    // Detrae el daño de la vida del jugador
                    _player.CurrentHitPoints -= damageToPlayer;

                    // Actualiza la vida del jugador en la UI
                    lblHitPoints.Text = _player.CurrentHitPoints.ToString();

                    if (_player.CurrentHitPoints <= 0)
                    {
                        // Muestra mensaje
                        rtbMessages.Clear();
                        rtbMessages.Text += "La " + _currentMonster.Name + " te ha matado." + Environment.NewLine;
                        rtbMessages.Text += "Comienzo de la aventura" + Environment.NewLine + Environment.NewLine;
                    
                        // Regresa el jugador a "Casa"
                        MoveTo(World.LocationByID(World.LOCATION_ID_HOME));
                    }
                }
            } else
            {
                rtbMessages.Text += "¡Tu golpe atraviesa el aire al no haber monstruos presentes!" + Environment.NewLine;
            }
        }

        private void btnUsePotion_Click(object sender, EventArgs e)
        {
            // Coge el tipo de poción seleccionada actualmente en la combobox
            HealingPotion potion = (HealingPotion)cboPotions.SelectedItem;

            // añade la vida a la del jugador.
            _player.CurrentHitPoints = (_player.CurrentHitPoints + potion.AmountToHeal);

            // Comprueba que la vida del jugador no exceda la vida máxima
            if (_player.CurrentHitPoints > _player.MaximumHitPoints)
            {
                _player.CurrentHitPoints = _player.MaximumHitPoints;
            }

            // Quita la poción del inventario del jugador
            foreach (InventoryItem ii in _player.Inventory)
            {
                if (ii.Details.ID == potion.ID)
                {
                    ii.Quantity--;
                    break;
                }
            }

            // Muestra mensaje
            rtbMessages.Text += "Bebes una " + potion.Name + Environment.NewLine;

            // El monstruo tiene su opción de atacar.

            // Determina el daño que le realizará al jugador
            int damageToPlayer = RandomNumberGenerator.NumberBetween(0, _currentMonster.MaximumDamage);

            // Muestra mensaje
            rtbMessages.Text += "La " + _currentMonster.Name + " te golpea por " + damageToPlayer.ToString() + " puntos de daño." + Environment.NewLine;

            // Detrae la vida de la del jugador
            _player.CurrentHitPoints -= damageToPlayer;

            if (_player.CurrentHitPoints <= 0)
            {
                // Muestra mensaje
                rtbMessages.Text += "La " + _currentMonster.Name + " te ha matado." + Environment.NewLine;

                // Mueve al jugador a "Casa"
                MoveTo(World.LocationByID(World.LOCATION_ID_HOME));
            }

            // Actualiza los datos del jugador en la UI
            lblHitPoints.Text = _player.CurrentHitPoints.ToString();
            UpdateInventoryListInUI();
            UpdatePotionListInUI();
        }

        private void UpdatePlayerStats()
        {
            lblHitPoints.Text = _player.CurrentHitPoints.ToString();
            lblGold.Text = _player.Gold.ToString();
            lblExperience.Text = _player.ExperiencePoints.ToString();
            lblLevel.Text = _player.Level.ToString();
        }

        // Cuando la ventana detecta que hay un cambio en el texto hace scroll hasta el final.
        private void rtbMessages_TextChanged(object sender, EventArgs e)
        {
            rtbMessages.SelectionStart = rtbMessages.Text.Length;
            rtbMessages.ScrollToCaret();
        }

        private void SuperAdventure_FormClosing(object sender, FormClosingEventArgs e)
        {
            File.WriteAllText(PLAYER_DATA_FILE_NAME, _player.ToXmlString());
        }
    }
}
