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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!(textBox1.Text == "") && !(textBox3.Text == "") && (!(textBox5.Text == "") || int.TryParse(textBox5.Text, out int value)) && (!(textBox6.Text == "") || int.TryParse(textBox6.Text, out int value2)) && !(textBox1.Text == textBox3.Text))
            {
                Player player1 = new Player(textBox1.Text, int.Parse(textBox5.Text));
                Player player2 = new Player(textBox3.Text, int.Parse(textBox6.Text));
                List<Player> p_list = new List<Player>
                {
                    player1,
                    player2
                };
                Game chess = new Game(this, p_list);
                chess.Show();
            }
            else
            {
                MessageBox.Show("Τα στοιχεία που δώσατε δεν είναι έγκυρα");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form4 recent_form = new Form4();
            recent_form.StartPosition = FormStartPosition.Manual;
            recent_form.Location = new Point(this.Location.X + this.Width / 2 - recent_form.Width / 2, this.Location.Y + this.Height / 2 - recent_form.Height / 2);
            recent_form.Show();
        }
    }
}
