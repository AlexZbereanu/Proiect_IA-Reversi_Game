using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Proiect_IA
{
    public class Board 
    {


        /// <summary>
        /// Piesele tablei de joc.
        /// </summary>
        public int[,] boardContents;

        /// <summary>
        /// Numarul de piese pe orizontala.
        /// </summary>
        public int Width
        {
            get
            {
                return boardContents.GetLength(0);
            }
        }

        /// <summary>
        /// Numarul de piese pe verticala.
        /// </summary>
        public int Height
        {
            get
            {
                return boardContents.GetLength(1);
            }
        }

        /// <summary>
        /// Returneaza o piese din tabla de joc in functie de indecsi dati ca parametrii. 
        /// </summary>
        /// <param name="x">Indexul x.</param>
        /// <param name="y">Indexul y.</param>
        /// <returns>O piesa de pe tabla in functie de x si y.</returns>
        public int GetCell(int x, int y)
        {
            if (x < 0 || x >= Width || y < 0 || y >= Height)
            {
                throw new IndexOutOfRangeException("Piece (" + x + "," + y + ") is out of range of the " + Width + "*" + Height + " game board area");
            }
            return boardContents[x, y];
        }

        /// <summary>
        /// O reprezentare vizuala a tablei de joc.
        /// </summary>
        /// <returns>O reprezentare vizuala a tablei de joc.</returns>
        public override string ToString()
        {
            string result = Environment.NewLine;

            for (int y = Height - 1; y >= 0; y--)
            {
                for (int x = 0; x < Width; x++)
                {
                    result += boardContents[x, y];

                    /*
                    if (_buttons.Count != 0)
                    {
                        //_buttons[0][0].Text = boardContents[x, y].ToString();
                    }*/

                    /*

                    if(_buttons.Count != 0)
                    {
                        _buttons[x][y].Text = boardContents[x, y].ToString();
                    }
                    
                     */

                    if (x != Width - 1)
                    {
                        result += " ";
                    }
                }

                if (y != 0)
                {
                    result += Environment.NewLine;
                }
            }

            return result;
        }

        /// <summary>
        /// Id-ul jucatorului curent.
        /// </summary>
        public int CurrentPlayer { get; set; }

        /// <summary>
        /// Castigatorul <para/>
        /// -1 nu este un castigator inca <para/>
        /// 0 egalitate <para/>
        /// Orice id positiv care sa reprezinte un id de jucator.
        /// </summary>
        protected int winner = -1;

        /// <summary>
        /// O lista cu toate mutarile posibile.
        /// </summary>
        protected List<Move> possibleMoves;

        /// <summary>
        /// The score awarded to a player after a win
        /// </summary>
        private const float WIN_SCORE = 1;

        /// <summary>
        /// The score awarded to a player after a draw
        /// </summary>
        private const float DRAW_SCORE = 0.5f;


        /// <summary>
        /// Creeaza o tabla noua de joc, reprezentant o tabla statica.
        /// </summary>
        public Board()
        {
            CurrentPlayer = 1;
            boardContents = new int[4, 4];

            boardContents[1, 1] = 2;
            boardContents[1, 2] = 1;
            boardContents[2, 1] = 1;
            boardContents[2, 2] = 2;

            //Begin by populating the possible moves list
            CalculatePossibleMoves();
        }

        /// <summary>
        /// Copiaza instanta curenta a tablei.
        /// </summary>
        /// <param name="board">instanta tablei pentru care se face copierea.</param>
        public Board(Board board)
        {
            CurrentPlayer = board.CurrentPlayer;
            winner = board.Winner;
            boardContents = (int[,])board.boardContents.Clone();
            possibleMoves = new List<Move>(board.possibleMoves);
        }

        /// <summary>
        /// Simulates random plays on this board until the game has ended
        /// </summary>
        /// <returns>The resultant board after it has been simulated</returns>
        public Board SimulateUntilEnd(TextBox console, List<Button> buttons)
        {
            var rand = new Random();
            Board temp = new Board(this);
            console.Text += temp.ToString();

            // Update buttons


            // End update buttons

            console.Text += Environment.NewLine;
            while (temp.Winner == -1)
            {
                temp.MakeMove(temp.PossibleMoves()[rand.Next(0, temp.PossibleMoves().Count())], buttons);
                console.Text += temp.ToString();

                // Update until there is a winner

                console.Text += Environment.NewLine;
            }

            return temp;
        }


        /// <summary>
        /// Gets the score for the provided player at this board state<para/>
        /// No score is returned if the game is not over
        /// </summary>
        /// <param name="player">The player to get the score of at this board state</param>
        /// <returns>The score for the given player at this board state</returns>
        public float GetScore(int player)
        {
            if (winner == 0)
            {
                return DRAW_SCORE;
            }
            else if (winner == player)
            {
                return WIN_SCORE;
            }

            return 0;
        }


        /// <summary>
        /// Jucatorul anterior.
        /// </summary>
        /// <returns></returns>
        public int PreviousPlayer
        {
            get
            {
                if (CurrentPlayer - 1 <= 0)
                {
                    return PlayerCount();
                }
                else
                {
                    return CurrentPlayer - 1;
                }
            }
        }

        /// <summary>
        /// Returneaza ID-ul urmatorului jucator
        /// </summary>
        /// <returns>T</returns>
        public int NextPlayer
        {
            get
            {
                if (CurrentPlayer + 1 > PlayerCount())
                {
                    return 1;
                }
                else
                {
                    return CurrentPlayer + 1;
                }
            }
        }



        /// <summary>
        ///  <para/>
        /// -1 daca nu este niciun castigator <para/>
        /// 0 daca e egalitate <para/>
        /// un ID de jucator
        /// </summary>
        /// <returns></returns>
        public int Winner
        {
            get { return winner; }
        }
        /// <summary>
        /// Returneaza o lista cu punctele ce vor fi "mancate"
        /// </summary>
        /// <param name="pos">Positia noii piese.</param>
        /// <param name="player">Jucatorul curent.</param>
        /// <returns>O lista cu piesele de joc ce vor fi castigate in urma mutarii.n</returns>
        private List<Point> GetSandwichedPieces(Point pos, int player)
        {
            //The list of all points that represent cells that will be sandwiched
            List<Point> result = new List<Point>();

            //The list of points that will be sandwiched in the current direction
            List<Point> currentPoints = new List<Point>();

            #region Check cells to the left
            for (int i = pos.X - 1; i >= 0; i--)
            {
                if (boardContents[i, pos.Y] != 0 && boardContents[i, pos.Y] != player)
                {
                    currentPoints.Add(new Point(i, pos.Y));
                }
                else if (boardContents[i, pos.Y] == player && currentPoints.Count > 0)
                {
                    result.AddRange(currentPoints);
                    break;
                }
                else
                {
                    break;
                }
            }
            #endregion

            currentPoints.Clear();

            #region Check cells to the right
            for (int i = pos.X + 1; i < Width; i++)
            {
                if (boardContents[i, pos.Y] != 0 && boardContents[i, pos.Y] != player)
                {
                    currentPoints.Add(new Point(i, pos.Y));
                }
                else if (boardContents[i, pos.Y] == player && currentPoints.Count > 0)
                {
                    result.AddRange(currentPoints);
                    break;
                }
                else
                {
                    break;
                }
            }
            #endregion

            currentPoints.Clear();

            #region Check cells above
            for (int i = pos.Y - 1; i >= 0; i--)
            {
                if (boardContents[pos.X, i] != 0 && boardContents[pos.X, i] != player)
                {
                    currentPoints.Add(new Point(pos.X, i));
                }
                else if (boardContents[pos.X, i] == player && currentPoints.Count > 0)
                {
                    result.AddRange(currentPoints);
                    break;
                }
                else
                {
                    break;
                }
            }
            #endregion

            currentPoints.Clear();

            #region Check cells below

            for (int i = pos.Y + 1; i < Height; i++)
            {
                if (boardContents[pos.X, i] != 0 && boardContents[pos.X, i] != player)
                {
                    currentPoints.Add(new Point(pos.X, i));
                }
                else if (boardContents[pos.X, i] == player && currentPoints.Count > 0)
                {
                    result.AddRange(currentPoints);
                    break;
                }
                else
                {
                    break;
                }
            }
            #endregion

            currentPoints.Clear();

            #region Check cells up and right
            for (int x = pos.X + 1, y = pos.Y + 1; x < Width && y < Height; x++, y++)
            {
                if (boardContents[x, y] != 0 && boardContents[x, y] != player)
                {
                    currentPoints.Add(new Point(x, y));
                }
                else if (boardContents[x, y] == player && currentPoints.Count > 0)
                {
                    result.AddRange(currentPoints);
                    break;
                }
                else
                {
                    break;
                }
            }
            #endregion

            currentPoints.Clear();

            #region Check cells up and left
            for (int x = pos.X - 1, y = pos.Y + 1; x >= 0 && y < Height; x--, y++)
            {
                if (boardContents[x, y] != 0 && boardContents[x, y] != player)
                {
                    currentPoints.Add(new Point(x, y));
                }
                else if (boardContents[x, y] == player && currentPoints.Count > 0)
                {
                    result.AddRange(currentPoints);
                    break;
                }
                else
                {
                    break;
                }
            }
            #endregion

            currentPoints.Clear();

            #region Check cells down and left
            for (int x = pos.X - 1, y = pos.Y - 1; x >= 0 && y >= 0; x--, y--)
            {
                if (boardContents[x, y] != 0 && boardContents[x, y] != player)
                {
                    currentPoints.Add(new Point(x, y));
                }
                else if (boardContents[x, y] == player && currentPoints.Count > 0)
                {
                    result.AddRange(currentPoints);
                    break;
                }
                else
                {
                    break;
                }
            }
            #endregion

            currentPoints.Clear();

            #region Check cells down and right
            for (int x = pos.X + 1, y = pos.Y - 1; x < Width && y >= 0; x++, y--)
            {
                if (boardContents[x, y] != 0 && boardContents[x, y] != player)
                {
                    currentPoints.Add(new Point(x, y));
                }
                else if (boardContents[x, y] == player && currentPoints.Count > 0)
                {
                    result.AddRange(currentPoints);
                    break;
                }
                else
                {
                    break;
                }
            }
            #endregion

            return result;
        }

        /// <summary>
        /// Returnam tabla nopua de joc in functie de mutarea facuta.
        /// </summary>
        /// <param name="move">mutarea de facut.</param>
        /// <returns>O referinta catre noua tabla de joc.</returns>
        public Board MakeMove(Move move, List<Button> buttons)
        {
            Move m = move;

            boardContents[m.Position.X, m.Position.Y] = CurrentPlayer;

            foreach (Point p in m.CapturableCells)
            {
                boardContents[p.X, p.Y] = CurrentPlayer;
            }

            CurrentPlayer = NextPlayer;

            


            CalculatePossibleMoves();

            if (possibleMoves.Count == 0)
            {
                DetermineWinner();
            }

            return this;
        }

        

        /// <summary>
        /// Returneaza mutarile posibile.
        /// </summary>
        /// <returns></returns>
        public List<Move> PossibleMoves()
        {
            return possibleMoves;
        }

        /// <summary>
        /// Calculeaza mutarile posibile.
        /// </summary>
        private void CalculatePossibleMoves()
        {
            possibleMoves = new List<Move>();

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    if (boardContents[x, y] == 0)
                    {
                        if (GetSandwichedPieces(new Point(x, y), CurrentPlayer).Count != 0)
                        {
                            possibleMoves.Add(new Move(new Point(x, y), GetSandwichedPieces(new Point(x, y), CurrentPlayer)));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Determina castigatorul jocului.
        /// </summary>
        protected void DetermineWinner()
        {
            //Daca mai exista mutari posibile, nu avem inca un castigator
            if (possibleMoves.Count != 0)
            {
                return;
            }

            int player1Pieces = 0;
            int player2Pieces = 0;

            //numaram piesele jucatorilor
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    if (boardContents[x, y] == 1)
                    {
                        player1Pieces++;
                    }
                    else if (boardContents[x, y] == 2)
                    {
                        player2Pieces++;
                    }
                }
            }

            //determinare castigator
            if (player1Pieces > player2Pieces)
            {
                winner = 1;
            }
            else if (player2Pieces > player1Pieces)
            {
                winner = 2;
            }
            else
            {
                winner = 0;
            }
        }


        /// <summary>
        /// Numarul de jucatori de pe tabla.
        /// </summary>
        /// <returns></returns>
        protected int PlayerCount()
        {
            return 2;
        }
    }
}
