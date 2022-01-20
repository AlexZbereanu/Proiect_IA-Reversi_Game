using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proiect_IA
{
    /// <summary>
    /// Un nod ce foloseste in Arborele de Cautare Monte Carlo.
    /// </summary>
    public class Node
    {
        /// <summary>
        /// Configuratia tablei de joc in acest nod.
        /// </summary>
        public Board Board { get; set; }
        /// <summary>
        /// Acest nod este nodul parinte.
        /// </summary>
        public Node Parent { get; set; }
        /// <summary>
        /// O lista ce contine copii acestui nod.
        /// </summary>
        public List<Node> Childrens { get; set; }
        /// <summary>
        /// O lista ce contine mutari posibile ce nu au fost explorate.
        /// </summary>
        public List<Move> UntriedMoves { get; set; }
        /// <summary>
        /// Scorul nodului.
        /// </summary>
        public float TotalScore { get; set; }
        /// <summary>
        /// Reprezinta numarul de vizite facute acestui nod.
        /// </summary>
        public long Visits { get; set; }
        /// <summary>
        /// Adancimea nodului.
        /// </summary>
        public int Depth { get; set; }

        /// <summary>
        /// Creeaza un nou nod.
        /// </summary>
        /// <param name="ParentNode">Nodul parinte, null daca este radacina.</param>
        /// <param name="board">Tabla de joc a nodului.</param>
        public Node(Node ParentNode, Board board)
        {
            Parent = ParentNode;
            Board = board;
            Childrens = new List<Node>(Board.PossibleMoves().Count);
            UntriedMoves = new List<Move>(Board.PossibleMoves());

            if (ParentNode == null)
            {
                Depth = 0;
            }
            else
            {
                Depth = ParentNode.Depth + 1;
            }
        }

        /// <summary>
        /// Daca mai sunt mutari posibile dupa acest nod, expandam nodul curent.
        /// </summary>
        public Node Expand()
        {
            Random random = new Random();
            if (UntriedMoves.Count == 0 || IsLeafNode)
            {
                return this;
            }

            Move move = UntriedMoves.ElementAt(random.Next(0, UntriedMoves.Count));

            Board newBoard = new Board(Board);
            newBoard.MakeMove(move, new List<System.Windows.Forms.Button>());

            Node child = new Node(this, newBoard);
            Childrens.Add(child);

            UntriedMoves.Remove(move);

            return child;
        }

        /// <summary>
        /// Actualizam scorul si vizitele nodului curent
        /// Folosit pentru etapa de backPropagation
        /// </summary>
        /// <param name="score"></param>
        public void Update(float score)
        {
            TotalScore += score;

            Visits++;
        }

        /// <summary>
        /// Returneaza valoarea UCB a nodului
        /// </summary>
        /// <returns>Valoarea UCB a acestui nod.</returns>
        public double UCBValue()
        {
            if (Visits == 0)
            {
                return double.MaxValue;
            }

            return AverageScore + Math.Sqrt(2 * Math.Log(Parent.Visits) / Visits);
        }

        /// <summary>
        /// Returneaza cel mai bun copil al nodului curent, folosit ca sa vedem cea mai buna mutare pentru nodul curent. <param/>
        /// Cel mai bun copil este copilul cu cele mai multe vizite.
        /// </summary>
        /// <returns></returns>
        public Node GetBestChild()
        {
            if (Visits <= 1)
            {
                return null;
            }

            Node bestChild = null;
            float highestChildVisits = float.MinValue;

            foreach (Node child in Childrens)
            {
                if (child.Visits > highestChildVisits)
                {
                    bestChild = child;
                    highestChildVisits = bestChild.Visits;
                }
                else if (child.Visits == highestChildVisits)
                {
                    if (child.AverageScore > bestChild.AverageScore)
                    {
                        bestChild = child;
                    }
                }
            }

            return bestChild;
        }

        /// <summary>
        /// Returneaza daca un nod e frunza sau nu. <para/>
        /// Un nod este frunza daca nu mai are mutari de facut.
        /// </summary>
        public bool IsLeafNode
        {
            get { return Board.Winner != -1; }
        }

        /// <summary>
        /// Scorul mediu. <para/>
        ///Este determinat din scorul nodului si visitele efectuate.
        /// </summary>
        public float AverageScore
        {
            get { return (Visits == 0 ? 0 : TotalScore / Visits); }
        }
    }
}
