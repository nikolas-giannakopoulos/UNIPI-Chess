using System.Data.SQLite;
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
    public partial class Form3 : Form
    {
        Game game;
        List<Player> p_list;
        public bool success = false;
        SQLiteConnection connection;
        public Form3(Game game, List<Player> p_list)
        {
            this.game = game;
            this.p_list = p_list;
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            connection = new SQLiteConnection("Data source=chess.db;Version=3");
            this.FormClosing += new FormClosingEventHandler(Form_Closing);
            game.Enabled = false;
            comboBox1.Items.Add("Checkmate");
            comboBox1.Items.Add("Stalemate");
            comboBox2.Items.Add(p_list[0].name);
            comboBox2.Items.Add(p_list[1].name);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(comboBox1.SelectedIndex == 0)
            {
                label2.Visible = true;
                comboBox2.Visible = true;
            }
            else
            {
                label2.Visible = false;
                comboBox2.Visible = false;
            }
        }

        private void SumbitButton_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex != -1 && (comboBox1.SelectedIndex == 1 || comboBox2.SelectedIndex != -1))
            {
                connection.Open();
                String insertSQL = "Insert into ChessGames(" +
                                       "Player1_Name,Player1_Age,Player2_Name,Player2_Age,Winner,Date) " +
                                       "values(@p1_name,@p1_age,@p2_name,@p2_age,@winner,@date)";
                SQLiteCommand command = new SQLiteCommand(insertSQL, connection);
                command.Parameters.AddWithValue("p1_name", p_list[0].name);
                command.Parameters.AddWithValue("p1_age", p_list[0].age);
                command.Parameters.AddWithValue("p2_name", p_list[1].name);
                command.Parameters.AddWithValue("p2_age", p_list[1].age);
                if (comboBox1.Text == "Checkmate")
                {
                    command.Parameters.AddWithValue("winner", comboBox2.Text);
                }
                else
                {
                    command.Parameters.AddWithValue("winner", "-");
                }
                command.Parameters.AddWithValue("date", DateTime.Now.ToString("dd / MM / yyyy HH: mm"));
                command.ExecuteNonQuery();
                connection.Close();
                game.success = success = true;
                this.Close();
            }
        }
        private void Form_Closing(object sender, CancelEventArgs e)
        {
            if (success)
            {
                MessageBox.Show("Το παιχνίδι έληξε με " + comboBox1.Text);
                game.Close();
            }
            else
            {
                game.timer1.Start();
                game.Enabled = true;
            }
        }
    }
}
