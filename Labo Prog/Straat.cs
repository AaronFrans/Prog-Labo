using System;
using System.Collections.Generic;
using System.Text;

namespace Labo_Prog
{
    class Straat
    {
        #region Constructor
        public Straat(int id, string naam, Graaf graaf)
        {
            m_ID = id;
            m_Naam = naam;
            m_Graaf = graaf;
        }
        #endregion

        #region HelperFunctions
        public void ShowStraat()
        {
            Console.WriteLine($"Naam: {m_Naam}, ID: {m_ID}");
            m_Graaf.ShowGraaf();
        }
        #endregion

        #region Functions
        public List<Knoop> GetKnopen()
        {
            return m_Graaf.GetKnopen();
        }
        #endregion

        #region Properties
        Graaf m_Graaf;
        int m_ID;
        string m_Naam;
        #endregion


    }
}
