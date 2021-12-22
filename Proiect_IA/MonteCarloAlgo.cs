using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proiect_IA
{
    /// <summary>
    /// Clasa ce are ca rol principal implementarea algoritmului Monte Carlo
    /// </summary>
    static class MonteCarloAlgo
    {
        #region SELECTION
        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="rootPlayer"></param>
        /// <returns></returns>
        public static Node SelectChild(this Node node, byte rootPlayer)
        {
            var isRootPlayer = node.State.Player == rootPlayer;

            Node bestChild = null;
            var maxEvaluation = double.MinValue;
            foreach(var childNode in node.ChildNodes)
            {
                var childEvaluation = childNode.GetEvaluation(isRootPlayer);
                if(childEvaluation > maxEvaluation)
                {
                    bestChild = childNode;
                    maxEvaluation = childEvaluation;
                }
            }
            return bestChild;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="isRootPlayer"></param>
        /// <returns></returns>
        public static double GetEvaluation(this Node node, bool isRootPlayer)
        {
            var wins = isRootPlayer ? node.Wins : node.Visits - node.Wins;
            return wins / node.Visits + 0.85 * Math.Sqrt(Math.Log(node.ParentNode.Visits) / node.Visits);
        }

        #endregion

        #region EXPANSION

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static Node ExpandChild(this Node node)
        {
            return node;
        }
        #endregion

        #region SIMULATION

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="rootPlayer"></param>
        /// <returns></returns>
        public static float Simulate(this Node node, byte rootPlayer)
        {
            return 0;
        }
        #endregion

        #region BACKPROPAGATION

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="reward"></param>
        public static void BackPropagate(this Node node, float reward)
        {

        }
        #endregion
    }
}
