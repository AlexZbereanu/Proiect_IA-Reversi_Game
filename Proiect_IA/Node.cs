using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proiect_IA
{
    class Node
    {
        public State State { get; }
        public long ParentMove { get; }
        public Node ParentNode { get; }
        public List<Node> ChildNodes { get; }
        public List<long> UntriedMoves { get; }
        public int Visits { get; set; }
        public float Wins { get; set; }

        public Node(Node parentNode, long parentMove)
        {
            ParentMove = parentMove;

            Visits = 0;
            Wins = 0;

            ParentNode = parentNode;
            ChildNodes = new List<Node>(UntriedMoves.Count);

        }

    }
}
