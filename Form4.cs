using System;
using System.Drawing;
using System.Data.SQLite;
using System.Windows.Forms;

namespace skaki
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }

        SQLiteConnection connection;
        private void Form4_Load(object sender, EventArgs e)
        {
            connection = new SQLiteConnection("Data source=chess.db;Version=3");
            connection.Open();
            String RecentString = "Select * from ChessGames order by GameID desc limit 5"; //Επιλέγει τις 5 πιο πρόσφατες εγγραφες, ξεκινώντας από το πιο πρόσφατο
            SQLiteCommand LoadMatches = new SQLiteCommand(RecentString, connection);
            SQLiteDataReader MatchesReader = LoadMatches.ExecuteReader();
            int count = 0;
            while ( MatchesReader.Read() )
            {
                Label date = new Label();                               //
                date.Location = new Point(0, 43+ 25 * count);           //
                date.Text = MatchesReader.GetString(6);                 // Ρυθμισεις για label για να φαινονται
                date.AutoSize = false;                                  // οι εγγραφες η μια μετα την αλλη
                date.Size = new Size(130, 20);                          //
                date.TextAlign = ContentAlignment.MiddleCenter;         //
                this.Controls.Add(date);                                //

                Label p1_name = new Label();
                p1_name.Location = new Point(120, 43 + 25 * count);
                p1_name.Text = MatchesReader.GetString(1);
                p1_name.AutoSize = false;
                p1_name.Size = new Size(86, 18);
                p1_name.TextAlign = ContentAlignment.MiddleCenter;
                this.Controls.Add(p1_name);

                Label p1_age = new Label();
                p1_age.Location = new Point(225, 43 + 25 * count);
                p1_age.Text = MatchesReader.GetString(2);
                p1_age.AutoSize = false;
                p1_age.TextAlign = ContentAlignment.MiddleCenter;
                p1_age.Size = new Size(25,20);
                this.Controls.Add(p1_age);

                Label p2_name = new Label();
                p2_name.Location = new Point(273, 43 + 25 * count);
                p2_name.Text = MatchesReader.GetString(3);
                p2_name.AutoSize = false;
                p2_name.Size = new Size(86, 18);
                p2_name.TextAlign = ContentAlignment.MiddleCenter;
                this.Controls.Add(p2_name);

                Label p2_age = new Label();
                p2_age.Location = new Point(370, 43 + 25 * count);
                p2_age.Text = MatchesReader.GetString(4);
                p2_age.AutoSize = false;
                p2_age.TextAlign = ContentAlignment.MiddleCenter;
                p2_age.Size = new Size(25, 20);
                this.Controls.Add(p2_age);

                Label winner = new Label();
                winner.Location = new Point(410, 43 + 25 * count);
                winner.Text = MatchesReader.GetString(5);
                winner.AutoSize = false;
                winner.Size = new Size(86, 18);
                winner.TextAlign = ContentAlignment.MiddleCenter;
                this.Controls.Add(winner);

                count++;
            }
            connection.Close();
        }
    }
}
