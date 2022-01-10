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

            pictureBoxBoard.Refresh();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            TreeSearch alg = new TreeSearch(_board);
            alg.Step(textBox1);
        }
    }
}
