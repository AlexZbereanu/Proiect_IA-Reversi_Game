using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proiect_IA
{
    /// <summary>
    /// Reprezinta mutarea unei singure piese
    /// </summary>
    public class Move
    {
        /// <summary>
        /// Pozitia mutarii
        /// </summary>
        public Point Position { get; private set; }

        /// <summary>
        /// O lista cu celulele ce vor fi capturate in urma mutarii
        /// </summary>
        public List<Point> CapturableCells { get; private set; }

        /// <summary>
        /// Creeaza o noua mutare.
        /// </summary>
        /// <param name="pos">Pozitia.</param>
        /// <param name="cellsToCapture">O lista cu celulele ce vor fi capturate cu aceasta mutare.</param>
        public Move(Point pos, List<Point> cellsToCapture)
        {
            if (pos.X > 3 || pos.Y > 3 || pos.X < 0 || pos.Y < 0)
            {
                throw new IndexOutOfRangeException("Move: " + "(" + pos.X + "," + pos.Y + ")" + " is out of bounds of the 4x4 game area");
            }

            if (cellsToCapture.Count == 0)
            {
                throw new Exception("No cells will be captured as a result of this move, at least one cell must be captured");
            }

            CapturableCells = cellsToCapture;
            Position = pos;
        }

        /// <summary>
        /// Pozitia X a mutarii
        /// </summary>
        public int X
        {
            get { return Position.X; }
        }

        /// <summary>
        /// Pozitia Y a mutarii
        /// </summary>
        public int Y
        {
            get { return Position.Y; }
        }

        /// <summary>
        /// String representation
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "(" + X + "," + Y + ")";
        }

        public Point Point
        {
            get => default;
            set
            {
            }
        }

        public Node Node
        {
            get => default;
            set
            {
            }
        }
    }
}
