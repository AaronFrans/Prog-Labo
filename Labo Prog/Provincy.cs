using System;
using System.Collections.Generic;
using System.Text;

namespace Labo_Prog
{
    class Provincy
    {
        public Provincy(int provincyID, string naam, List<Gemeente> gemeentes)
        {
            this.provincyID = provincyID;
            this.m_Naam = naam;
            this.m_Gemeentes = gemeentes;
        }

        #region Properties
        public int provincyID { get; private set; }
        public string m_Naam { get; private set; }
        public List<Gemeente> m_Gemeentes { get; private set; }
        #endregion
    }
}
