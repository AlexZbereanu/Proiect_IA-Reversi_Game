using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Proiect_IA
{
    /// <summary>
    /// Arbore necesar pentru algoritm <para/>
    /// </summary>
    public class TreeSearch
    {
        /// <summary>
        /// Nodul radacina al arborelui
        /// </summary>
        public Node Root { get; set; }

        /// <summary>
        /// Semnal care indica ca algoritmul a terminat de rulat
        /// </summary>
        public bool Finished { get; set; }

        /// <summary>
        /// Numarul de noduri unice in arbore
        /// </summary>
        public int UniqueNodes { get; set; }

        /// <summary>
        /// O lista cu toate nodurile din arbore, in ordinea in care au fost adaugate.
        /// </summary>
        public List<Node> AllNodes { get; set; }

        /// <summary>
        /// O instanta a clasei form
        /// </summary>
        public List<Button> Buttons { get; set; }

        /// <summary>
        /// Creare arbore cu o tabla data.
        /// </summary>
        /// <param name="gameBoard">Tabla de joc.</param>
        public TreeSearch(Board gameBoard)
        {
            Root = new Node(null, gameBoard);

            UniqueNodes = 1;

            AllNodes = new List<Node>();
            AllNodes.Add(Root);
        }

        /// <summary>
        /// Perform a step of MCTS <para/>
        /// Selecteaza nodul cu functia de evaluare cea mai buna, il expandeaza, simuleaza copii si propaga inapoi rezultatul pe arbore.
        /// </summary>
        public void Step(TextBox console)
        {
            //Selection
            Node currentNode = Selection(Root);

            if (currentNode == null)
            {
                console.Text += "Something has went wrong during selection. Null node returned.";
            }

            //Expansion
            Node nodeBeforeExpansion = currentNode;
            currentNode = currentNode.Expand();
            if (nodeBeforeExpansion != currentNode)
            {
                UniqueNodes++;
                AllNodes.Add(currentNode);
            }
            //Simulation
            Board resultState = currentNode.Board.SimulateUntilEnd(console, Buttons);
            
            //Backpropogation
            while (currentNode != null)
            {
                currentNode.Update(resultState.GetScore(currentNode.Board.PreviousPlayer));
                currentNode = currentNode.Parent;
            }
            
        }

        /// <summary>
        /// Primul pas al algoritmului: Selectia. <para/>
        /// </summary>
        /// <param name="n">Nodul curent.</param>
        /// <returns>Cel mai bun nod pentru a fi expandat.</returns>
        private Node Selection(Node n)
        {
            if (n == null)
            {
                return null;
            }
            else if (n.Childrens.Count == 0 || n.UntriedMoves.Count != 0)
            {
                return n;
            }
            else
            {
                double highestUCB = float.MinValue;
                Node highestUCBChild = null;

                foreach (Node child in n.Childrens)
                {
                    double currentUCB1 = child.UCBValue();
                    if (currentUCB1 > highestUCB)
                    {
                        highestUCB = currentUCB1;
                        highestUCBChild = child;
                    }
                }

                return Selection(highestUCBChild);
            }
        }

        /// <summary>
        /// Alegerea celui mai bun nod.
        /// </summary>
        /// <param name="n"></param>
        /// <returns>Cel mai bun copil pentru nodul curent.</returns>
        public Node BestNodeChoice(Node n)
        {
            float smallestScore = float.MinValue;
            Node chosenNode = null;

            foreach (Node child in n.Childrens)
            {
                //Aleg copilul cu cel mai bun scor, daca mai multi copii au acelasi scor, il aleg pe cel ce are mai putini copii.
                if (child.AverageScore > smallestScore || child.AverageScore == smallestScore && chosenNode.Childrens.Count > child.Childrens.Count)
                {
                    smallestScore = child.AverageScore;
                    chosenNode = child;
                }
            }

            return chosenNode;
        }

        /// <summary>
        /// Functie de stopare a algoritmului.
        /// </summary>
        public void Finish()
        {
            Finished = true;
        }
    }
}
