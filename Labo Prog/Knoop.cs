using System;
using System.Collections.Generic;
using System.Text;

namespace Labo_Prog
{
    class Knoop
    {
        #region Constructor
        public Knoop(int knoopID, Punt punt)
        {
            m_KnoopID = knoopID;
            m_Punt = punt;
        }
        #endregion

        #region HelperFunctions

        public override string ToString()
        {
            return $"Knoop: {m_KnoopID}\n" + m_Punt.ToString();
        }

        public override bool Equals(object obj)
        {
            return obj is Knoop knoop &&
                   m_KnoopID == knoop.m_KnoopID &&
                   EqualityComparer<Punt>.Default.Equals(m_Punt, knoop.m_Punt);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(m_KnoopID, m_Punt);
        }
        #endregion

        #region Properties
        public int m_KnoopID { get; private set; }
        public Punt m_Punt { get; private set;}


        #endregion
    }
}
