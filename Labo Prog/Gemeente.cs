using System;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace Labo_Prog
{
    [Serializable()]
    class Gemeente : ISerializable
    {
        public Gemeente(int gemeenteID, string naam, List<Straat> straten)
        {
            this.m_GemeenteID = gemeenteID;
            this.m_Naam = naam;
            this.m_Straten = straten;
        }
        private Gemeente(SerializationInfo info, StreamingContext context)
        {
            m_GemeenteID = (int)info.GetValue("m_GemeenteID", typeof(int));
            m_Naam = (string)info.GetValue("m_Naam", typeof(string));
            m_Straten = (List<Straat>)info.GetValue("m_Straten", typeof(List<Straat>));
        }

        #region Helper Functions
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("m_GemeenteID", m_GemeenteID);
            info.AddValue("m_Naam", m_Naam);
            info.AddValue("m_Straten", m_Straten);
        }

        public void ShowGemeente()
        {
            Console.WriteLine($"Gemeente: {m_Naam} met ID: {m_GemeenteID}\n");

            foreach(Straat straat in m_Straten)
            {
                straat.ShowStraat();
            }
        }
        #endregion
        #region Functions

        public int NrOfStreets()
        {
            return m_Straten.Count;
        }

        
        #endregion

        #region Properties
        public int m_GemeenteID { get; private set; }
        public string m_Naam { get; private set; }
        public List<Straat> m_Straten { get; private set; }



        #endregion
    }
}
