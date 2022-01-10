using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proiect_IA
{
    /// <summary>
    /// Clasa ce tine o evidenta a punctelor de pe tabla de joc in functie de coordonatele acestora.
    /// </summary>
    public struct Point
    {
        /// <summary>
        /// Coordonata X a punctului.
        /// </summary>
        public int X;

        /// <summary>
        /// Coordonata Y a punctului.
        /// </summary>
        public int Y;

        /// <summary>
        /// Creeaza un nou punct cu coordonatele date X si Y.
        /// </summary>
        /// <param name="x">Coordonata X a punctului de setat.</param>
        /// <param name="y">Coordonata Y a punctului de setat.</param>
        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
