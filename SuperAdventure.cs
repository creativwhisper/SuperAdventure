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

            _player = new Player(); // Creación de instancia de la clase.

            _player.MaximumHitPoints = 10;
            _player.CurrentHitPoints = 10;
            _player.Gold = 20;
            _player.ExperiencePoints = 0;
            _player.Level = 1;

            lblHitPoints.Text = _player.CurrentHitPoints.ToString();
            lblGold.Text = _player.CurrentHitPoints.ToString();
            lblExperience.Text = _player.ExperiencePoints.ToString();
            lblLevel.Text = _player.Level.ToString();


        }

        

        
    }
}
