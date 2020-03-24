using System;
using System.Collections.Generic;
using System.Text;

namespace Labo_Prog
{
    class Gemeente
    {
        public Gemeente()
        {

        }
        public Gemeente(int gemeenteID, string naam, List<Straat> straten)
        {
            this.m_GemeenteID = gemeenteID;
            this.m_Naam = naam;
            this.m_Straten = straten;
        }


        #region Properties
        public int m_GemeenteID { get; private set; }
        public string m_Naam { get; private set; }
        public List<Straat> m_Straten { get; private set; }
        #endregion
    }
}
