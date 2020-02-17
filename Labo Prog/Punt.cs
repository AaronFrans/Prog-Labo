using System;
using System.Collections.Generic;
using System.Text;

namespace Labo_Prog
{
    class Punt
    {
        #region Constructor
        public Punt(float x,float y)
        {
            m_X = x;
            m_Y = y;
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
        public float m_X { get; private set; }
        public float m_Y { get; private set; }
        #endregion
    }
}
