using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Engine; // Llamada a la librería de clases donde almacenamos la lógica de juego.

namespace SuperAdventure
{
    public partial class SuperAdventure : Form
    {
        // instancia de la clase
        public Player _player;

        public SuperAdventure()
        {
            InitializeComponent(); // Añadido por VS

            Location location = new Location(1, "Home", "This is your house.");
            

            _player = new Player(20, 20, 10, 0, 1); // Creación de instancia de la clase.

           

            lblHitPoints.Text = _player.CurrentHitPoints.ToString();
            lblGold.Text = _player.CurrentHitPoints.ToString();
            lblExperience.Text = _player.ExperiencePoints.ToString();
            lblLevel.Text = _player.Level.ToString();


        }

        private void btnNorth_Click(object sender, EventArgs e)
        {

        }

        private void btnWest_Click(object sender, EventArgs e)
        {

        }

        private void btnEast_Click(object sender, EventArgs e)
        {

        }

        private void btnSouth_Click(object sender, EventArgs e)
        {

        }

        private void btnUseWeapon_Click(object sender, EventArgs e)
        {

        }

        private void btnUsePotion_Click(object sender, EventArgs e)
        {

        }
    }
}
