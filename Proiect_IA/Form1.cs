using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Proiect_IA
{
    public partial class Form1 : Form
    {
        private Board _board;
        private Bitmap _boardImage;
        private List<Button> _buttons;
        private TreeSearch alg;

        public Form1()
        {
            InitializeComponent();
            try
            {
                _boardImage = (Bitmap)Image.FromFile("board.png");
            }
            catch
            {
                MessageBox.Show("Nu se poate incarca board.png");
                Environment.Exit(1);
            }

            _board = new Board();

            this.ClientSize = new System.Drawing.Size(927, 600);
            this.pictureBoxBoard.Size = new System.Drawing.Size(500, 500);

            _buttons = new List<Button>() { btn00, btn01, btn02, btn03, btn10, btn11, btn12, btn13, btn20, btn21, btn22, btn23, btn30, btn31, btn32, btn33 };
            alg = new TreeSearch(_board);

            pictureBoxBoard.Refresh();

            statusBar.Enabled = false;
        }

        public TreeSearch TreeSearch
        {
            get => default;
            set
            {
            }
        }

        public Board Board
        {
            get => default;
            set
            {
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            stateLabel.Text = "Etapa 1";

            
            for (int i = 0; i < Convert.ToInt32(textBox_NrSim.Text); i++)
            {
                alg.Step(textBox1);
            }

            // Add the scroll throughout simulation feature

            int stepsCount = getPlayerMoves().Count - 1;

            statusBar.Maximum = stepsCount;

            statusBar.Enabled = true;

            updateScreen(0);


        }

        private List<string> getPlayerMoves()
        {
            var moves = textBox1.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);

            List<string> playerMoves = new List<string>();

            for (int i = 0; i < moves.Length - 1; i++)
            {
                if (moves[i] != "")
                {
                    playerMoves[playerMoves.Count - 1] += moves[i] + " ";
                }
                else
                {
                    playerMoves.Add("");
                }
            }

            return playerMoves;
        }

        private void updateScreen(int step)
        {
            List<string> playerMoves = getPlayerMoves();

            var buttonValues = playerMoves[step].Split(' ');

            for (int i = 0; i < 16; i++)
            {
                _buttons[i].Text = buttonValues[i];

                if(buttonValues[i] == "0")
                {
                    _buttons[i].BackColor = Color.Green;
                }
                else if(buttonValues[i] == "1")
                {
                    _buttons[i].BackColor = Color.Black;
                    _buttons[i].ForeColor = Color.White;
                }
                else if(buttonValues[i] == "2")
                {
                    _buttons[i].BackColor = Color.White;
                    _buttons[i].ForeColor = Color.Black;
                }
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void btn00_Click(object sender, EventArgs e)
        {

        }

        private void statusBar_Scroll(object sender, EventArgs e)
        {
            statusBar.ValueChanged += trackbar1_ValueChanged;
        }

        private void trackbar1_ValueChanged(object sender, EventArgs e)
        {
            int status = statusBar.Value;
            stateLabel.Text = "Etapa " + (status + 1).ToString();
            updateScreen(status);
        }
    }
}
