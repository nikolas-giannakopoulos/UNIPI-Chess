
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data.SQLite;
using System.Windows.Forms;
using System.Media;
using skaki.Properties;

namespace skaki
{
    public partial class Game : Form
    {
        Form1 form;
        List<Player> p_list;
        SQLiteConnection connection;
        Form3 Finish_Form;
        public bool success;
        int timeleft = 600;
        int timeleft2 = 600;
        bool firstplayer = true;
        bool mouseDown = false;
        int oldX;
        int oldY;
        int whitecaptured = 0;
        int blackcaptured = 0;
        bool first = true;
        Point defaultlocation;
        List<Pawn> pawns_list = new List<Pawn>();

        public Game(Form1 form, List<Player> p_list)
        {
            this.form = form;
            this.p_list = p_list;
            InitializeComponent();
        }

        private void Game_Load(object sender, EventArgs e)
        {
            connection = new SQLiteConnection("Data source=chess.db;Version=3");            //
            connection.Open();                                                              //
            String TableSQLString = "Create table if not exists ChessGames(" +              //
                                    "GameID Integer primary key autoincrement," +           //
                                    "Player1_Name Text," +                                  //
                                    "Player1_Age Text," +                                   //  Δημιουργεί table εάν
                                    "Player2_Name Text," +                                  //  δεν υπάρχει
                                    "Player2_Age Text," +                                   //
                                    "Winner Text," +                                        //
                                    "Date Text)";                                           //
            SQLiteCommand CreateCommand = new SQLiteCommand(TableSQLString, connection);    //
            CreateCommand.ExecuteNonQuery();                                                //
            connection.Close();                                                             //

            label2.Text = p_list[0].name;                                                   // Βάζει το όνομα τον παιχτών
            label4.Text = p_list[1].name;                                                   // στα αντίστοιχα labels

            form.Visible = false;
            this.FormClosing += new FormClosingEventHandler(Form_Closing);

            //Δημιουργεί Objects πιόνια, και βάζει τα αντίστοιχα στοιχεία, και τα PictureBoxes
            Pawn B_pirgos = new Black(pictureBox2, "pirgos");
            pawns_list.Add(B_pirgos);
            Pawn B_alogo = new Black(pictureBox3, "alogo");
            pawns_list.Add(B_alogo);
            Pawn B_aksiomatikos = new Black(pictureBox5, "aksiomatikos");
            pawns_list.Add(B_aksiomatikos);
            Pawn B_basilisa = new Black(pictureBox8, "basilisa");
            pawns_list.Add(B_basilisa);
            Pawn B_basilias = new Black(pictureBox7, "basilias");
            pawns_list.Add(B_basilias);
            Pawn B_aksiomatikos2 = new Black(pictureBox6, "aksiomatikos");
            pawns_list.Add(B_aksiomatikos2);
            Pawn B_alogo2 = new Black(pictureBox4, "alogo");
            pawns_list.Add(B_alogo2);
            Pawn B_pirgos2 = new Black(pictureBox1, "pirgos");
            pawns_list.Add(B_pirgos2);
            for (int i = 0; i < 8; i++)
            {
                String name = "pictureBox" + (9 + i).ToString();
                this.Controls.Find(Name, true);
                Pawn B_stratioths = new Black((PictureBox)this.Controls.Find(name, true)[0], "stratioths");
                pawns_list.Add(B_stratioths);
            }

            Pawn W_pirgos = new White(pictureBox25, "pirgos");
            pawns_list.Add(W_pirgos);
            Pawn W_alogo = new White(pictureBox27, "alogo");
            pawns_list.Add(W_alogo);
            Pawn W_aksiomatikos = new White(pictureBox29, "aksiomatikos");
            pawns_list.Add(W_aksiomatikos);
            Pawn W_basilisa = new White(pictureBox32, "basilisa");
            pawns_list.Add(W_basilisa);
            Pawn W_basilias = new White(pictureBox31, "basilias");
            pawns_list.Add(W_basilias);
            Pawn W_aksiomatikos2 = new White(pictureBox30, "aksiomatikos");
            pawns_list.Add(W_aksiomatikos2);
            Pawn W_alogo2 = new White(pictureBox28, "alogo");
            pawns_list.Add(W_alogo2);
            Pawn W_pirgos2 = new White(pictureBox26, "pirgos");
            pawns_list.Add(W_pirgos2);
            for (int i = 0; i < 8; i++)
            {
                String name = "pictureBox" + (17 + i).ToString();
                this.Controls.Find(Name, true);
                Pawn w_stratioths = new White((PictureBox)this.Controls.Find(name, true)[0], "stratioths");
                pawns_list.Add(w_stratioths);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)    //Μέθοδος του Timer
        {
            if (firstplayer)                                                                                    //   
            {                                                                                                   //
                if (timeleft > 0)                                                                               //
                {                                                                                               //
                    timeleft--;                                                                                 //  Εάν παίζει ο πρώτος παίχτης
                    timelabel1.Text = "0" + (timeleft / 60).ToString() + ":";                                   //  και δεν έχει τελείωσει ο 
                    if ((timeleft % 60) > 9)                                                                    //  χρόνος του, μειώνει τον 
                    {                                                                                           //  υπολοιπόμενο χρόνο κάτα 1
                        timelabel1.Text += (timeleft % 60).ToString();                                          //  άνα τικ του τιμερ
                    }                                                                                           //
                    else                                                                                        //
                    {                                                                                           //
                        timelabel1.Text += "0" + (timeleft % 60).ToString();                                    //
                    }                                                                                           //
                }                                                                                               //
                else                                                                                            
                {                                                                                               //
                    timer1.Stop();                                                                              //
                    MessageBox.Show("Time of " + p_list[0].name + " ran out! \n" + p_list[1].name + " Wins!");  //
                    connection.Open();                                                                          //
                    String insertSQL = "Insert into ChessGames(" +                                              //
                                       "Player1_Name,Player1_Age,Player2_Name,Player2_Age,Winner,Date) " +      //  Διαφορετικά τερματίζει το 
                                       "values(@p1_name,@p1_age,@p2_name,@p2_age,@winner,@date)";               //  παιχνίδι και ανακοινώνει
                    SQLiteCommand command = new SQLiteCommand(insertSQL, connection);                           //  ως νικητή τον αντίπαλο
                    command.Parameters.AddWithValue("p1_name", p_list[0].name);                                 //
                    command.Parameters.AddWithValue("p1_age", p_list[0].age);                                   //
                    command.Parameters.AddWithValue("p2_name", p_list[1].name);                                 //
                    command.Parameters.AddWithValue("p2_age", p_list[1].age);                                   //
                    command.Parameters.AddWithValue("winner", p_list[1].name);                                  //
                    command.Parameters.AddWithValue("date", DateTime.Now.ToString("dd / MM / yyyy HH: mm"));    //
                    command.ExecuteNonQuery();                                                                  //
                    connection.Close();                                                                         //
                    success = true;                                                                             //
                    this.Close();                                                                               //
                }
            }
            else   //Η ίδια διαδικασία αλλά για τον δεύτερο παίχτη
            {
                if (timeleft2 > 0)
                {
                    timeleft2--;
                    timelabel2.Text = "0" + (timeleft2 / 60).ToString() + ":";
                    if ((timeleft2 % 60) > 9)
                    {
                        timelabel2.Text += (timeleft2 % 60).ToString();
                    }
                    else
                    {
                        timelabel2.Text += "0" + (timeleft2 % 60).ToString();
                    }
                }
                else
                {
                    timer1.Stop();
                    MessageBox.Show("Time of " + p_list[1].name + " ran out! \n" + p_list[0].name + " Wins!");
                    connection.Open();
                    String insertSQL = "Insert into ChessGames(" +
                                           "Player1_Name,Player1_Age,Player2_Name,Player2_Age,Winner,Date) " +
                                           "values(@p1_name,@p1_age,@p2_name,@p2_age,@winner,@date)";
                    SQLiteCommand command = new SQLiteCommand(insertSQL, connection);
                    command.Parameters.AddWithValue("p1_name", p_list[0].name);
                    command.Parameters.AddWithValue("p1_age", p_list[0].age);
                    command.Parameters.AddWithValue("p2_name", p_list[1].name);
                    command.Parameters.AddWithValue("p2_age", p_list[1].age);
                    command.Parameters.AddWithValue("winner", p_list[0].name);
                    command.Parameters.AddWithValue("date", DateTime.Now.ToString("dd / MM / yyyy HH: mm"));
                    command.ExecuteNonQuery();
                    connection.Close();
                    success = true;
                    this.Close();
                }
            }
        }

        private void Startbutton_click(object sender, EventArgs e)  //Μέθοδος Start/Finish κουμπιού
        {
            if (first)
            {
                timer1.Start();
                label2.Font = new Font(label2.Font, FontStyle.Bold | FontStyle.Underline);
                foreach (Pawn pawn in pawns_list)
                {
                    if (pawn.GetType() == typeof(White))
                    {
                        pawn.image.Enabled = true;
                    }
                }
                first = false;
                Startbutton.Text = "Finish";
            }
            else
            {
                Finish_Form = new Form3(this, p_list);
                Finish_Form.StartPosition = FormStartPosition.Manual;
                Finish_Form.Location = new Point(this.Location.X + this.Width / 2 - Finish_Form.Width / 2, this.Location.Y + this.Height / 2 - Finish_Form.Height / 2);
                Finish_Form.Show();
                timer1.Stop();
            }
        }










        private void button2_Click(object sender, EventArgs e)
        {
            firstplayer = !firstplayer;
        }
        private void pictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            defaultlocation = ((PictureBox)sender).Location;
            ((PictureBox)sender).BringToFront();
            mouseDown = true;

            oldX = e.X;
            oldY = e.Y;
        }

        private void pictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            int centerX = ((PictureBox)sender).Location.X  + (((PictureBox)sender).Width / 2);
            int centerY = ((PictureBox)sender).Location.Y + (((PictureBox)sender).Height / 2);
            int newX = ((centerX - 218) / 62) ;
            int newY = ((centerY - 32) / 62) ;

            if (centerX > 218 && centerX < 714 && centerY > 32 && centerY < 528)
            {
                bool flag = true;
                foreach (Pawn pawn in pawns_list)
                {
                    if (defaultlocation == new Point(218 + newX * 62, 32 + newY * 62))
                    {
                        ((PictureBox)sender).Location = defaultlocation;
                        flag = false;
                        break;
                    } 
                    if (pawn.image.Location == new Point(218 + newX * 62, 32 + newY * 62))
                    {
                        if (!(pawn.GetType() == ((Pawn)((PictureBox)sender).Tag).GetType()) && !(pawn.eidos == "basilias"))
                        {
                            if (((Pawn)((PictureBox)sender).Tag).GetType() == typeof(Black))
                            {
                                if (blackcaptured > 11) { pawn.image.Visible = false; }
                                pawn.image.Location = new Point(774 + (blackcaptured % 3) * 74, 5 + (blackcaptured / 3) * 65);
                                pawn.image.Enabled = false;
                                pawn.eaten = true;
                                blackcaptured++;
                            }
                            else
                            {
                                if (whitecaptured > 11) { pawn.image.Visible = false; }
                                pawn.image.Location = new Point(774 + (whitecaptured % 3) * 74, 296 + (whitecaptured / 3) * 65);
                                pawn.image.Enabled = false;
                                pawn.eaten = true;
                                whitecaptured++;
                            }
                            break;
                        }
                        else
                        {
                            ((PictureBox)sender).Location = defaultlocation;
                            flag = false;
                            break;
                        }
                    }
                }

                if (flag)
                {
                    firstplayer = !firstplayer;
                    Font labelfont = label2.Font;
                    label2.Font = label4.Font;
                    label4.Font = labelfont;
                    ((PictureBox)sender).Location = new Point(218 + newX * 62, 32 + newY * 62);
                    foreach (Pawn pawn2 in pawns_list)
                    {
                        if (!pawn2.eaten)
                        {
                            pawn2.image.Enabled = !(pawn2.image.Enabled);
                        }
                    }
                    if (((Pawn)((PictureBox)sender).Tag).GetType() == typeof(White) && ((Pawn)((PictureBox)sender).Tag).eidos == "stratioths" && (((PictureBox)sender).Location == new Point(((PictureBox)sender).Location.X, 32)) && !((Pawn)((PictureBox)sender).Tag).upgraded)
                    {
                        ((Pawn)((PictureBox)sender).Tag).upgraded = true;
                        Form2 Upgrade_Form = new Form2(true, ((Pawn)((PictureBox)sender).Tag),this);
                        Upgrade_Form.StartPosition = FormStartPosition.Manual;
                        Upgrade_Form.Location = new Point(this.Location.X + this.Width / 2 - Upgrade_Form.Width / 2, this.Location.Y + this.Height / 2 - Upgrade_Form.Height / 2);
                        Upgrade_Form.Show();
                        timer1.Stop();
                        this.Enabled = false;
                    }
                    if (((Pawn)((PictureBox)sender).Tag).GetType() == typeof(Black) && ((Pawn)((PictureBox)sender).Tag).eidos == "stratioths" && (((PictureBox)sender).Location == new Point(((PictureBox)sender).Location.X, 466)) && !((Pawn)((PictureBox)sender).Tag).upgraded)
                    {
                        ((Pawn)((PictureBox)sender).Tag).upgraded = true;
                        Form2 Upgrade_Form = new Form2(false, ((Pawn)((PictureBox)sender).Tag), this);
                        Upgrade_Form.StartPosition = FormStartPosition.Manual;
                        Upgrade_Form.Location = new Point(this.Location.X + this.Width / 2 - Upgrade_Form.Width / 2, this.Location.Y + this.Height / 2 - Upgrade_Form.Height / 2);
                        Upgrade_Form.Show();
                        timer1.Stop();
                        this.Enabled = true;
                    }
                    SoundPlayer moveSound = new SoundPlayer(Resources.move_self);
                    moveSound.Play();
                }
            }
            else
            {
                ((PictureBox)sender).Location = defaultlocation;
            }
            mouseDown = false;
        }

        private void pictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                ((PictureBox)sender).Location = new Point(((PictureBox)sender).Location.X - (oldX - e.X), ((PictureBox)sender).Location.Y - (oldY - e.Y));

                this.Refresh();
            }
        }

        private void Form_Closing(object sender, CancelEventArgs e)
        {
            if (!success)
            {
                if (MessageBox.Show("Θέλετε να τερματίσετε το παιχνίδι; Τα στοιχεία δεν θα αποθηκευτούν", "Chess", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    e.Cancel = true;
                }
                else
                {
                    form.Visible = true;
                    form.textBox1.Text = "";
                    form.textBox3.Text = "";
                    form.textBox5.Text = "";
                    form.textBox6.Text = "";
                }
            }
            else
            {
                form.Visible = true;
                form.textBox1.Text = "";
                form.textBox3.Text = "";
                form.textBox5.Text = "";
                form.textBox6.Text = "";
            }

        }
    }

}

