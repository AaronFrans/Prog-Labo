using System;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace Labo_Prog
{
    [Serializable()]
    class Provincie : ISerializable
    {
        public Provincie(int ProvincieID, string naam, List<Gemeente> gemeentes)
        {
            this.m_ProvincieID = ProvincieID;
            this.m_Naam = naam;
            this.m_Gemeenten = gemeentes;
        }

        private Provincie(SerializationInfo info, StreamingContext context)
        {
            m_ProvincieID = (int)info.GetValue("m_ProvincieID", typeof(int));
            m_Naam = (string)info.GetValue("m_Naam", typeof(string));
            m_Gemeenten = (List<Gemeente>)info.GetValue("m_Gemeenten", typeof(List<Gemeente>));

        }

        #region Helper Funtions

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("m_ProvincieID", m_ProvincieID);
            info.AddValue("m_Naam", m_Naam);
            info.AddValue("m_Gemeenten", m_Gemeenten);
        }

        public void ShowProvincie()
        {
            Console.WriteLine($"Provincie: {m_Naam} met ID: {m_ProvincieID}");
            foreach(Gemeente gemeente in m_Gemeenten)
            {
                gemeente.ShowGemeente();
            }
        }

        #endregion

        #region Functions 
        public int NrOfStreets()
        {
            int toReturn = 0;
            foreach(Gemeente gemeente in m_Gemeenten)
            {
                toReturn += gemeente.NrOfStreets();
            }
            return toReturn;
        }
        #endregion

        #region Properties
        public int m_ProvincieID { get; private set; }
        public string m_Naam { get; private set; }
        public List<Gemeente> m_Gemeenten { get; private set; }

       
        #endregion
    }
}
