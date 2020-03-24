using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Labo_Prog
{
    public class Punt
    {
        #region Constructor
        //public Punt()
        //{

        //}


        public Punt(double x, double y)
        {
            m_X = x;
            m_Y = y;
        }

        private Punt()
        {
           
        }
        #endregion

        #region HelperFunctions
        public override string ToString()
        {
            return $"X: {m_X}, Y: {m_Y}\n";
        }
        public override bool Equals(object obj)
        {
            return obj is Punt punt &&
                   m_X == punt.m_X &&
                   m_Y == punt.m_Y;
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(m_X, m_Y);
        }

        
        #endregion

        #region Proprties
        public double m_X { get; set; }
        public double m_Y { get; set; }
        #endregion
    }
}
