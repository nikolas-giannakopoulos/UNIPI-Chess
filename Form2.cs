using skaki.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace skaki
{
    public partial class Form2 : Form
    {
        bool white;
        Pawn pawn;
        Game game;
        public Form2(bool white,Pawn pawn, Game game)
        {
            this.white = white;
            this.pawn = pawn;
            this.game = game;
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            if (white)
            {
                button1.BackgroundImage = Resources.w_aksiomatikos;
                button2.BackgroundImage = Resources.w_alogo;
                button3.BackgroundImage = Resources.w_basilisa;
                button4.BackgroundImage = Resources.w_pirgos;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            pawn.image.Image = button1.BackgroundImage;
            pawn.eidos = "aksiomatikos";
            game.Enabled = true;
            game.timer1.Start();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            pawn.image.Image = button2.BackgroundImage;
            pawn.eidos = "alogo";
            game.Enabled = true;
            game.timer1.Start();
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            pawn.image.Image = button3.BackgroundImage;
            pawn.eidos = "basilisa";
            game.Enabled = true;
            game.timer1.Start();
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            pawn.image.Image = button4.BackgroundImage;
            pawn.eidos = "pirgos";
            game.Enabled = true;
            game.timer1.Start();
            this.Close();
        }
    }
}
