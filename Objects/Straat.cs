using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Objects
{
    [Serializable()]
    public class Straat : ISerializable
    {
        #region Constructor
        public Straat(int id, string naam, Graaf graaf)
        {
            m_StraatID = id;
            m_Naam = naam;
            m_Graaf = graaf;
        }

        private Straat(SerializationInfo info, StreamingContext context)
        {
            m_StraatID = (int)info.GetValue("m_StraatID", typeof(int));
            m_Naam = (string)info.GetValue("m_Naam", typeof(string));
            m_Graaf = (Graaf)info.GetValue("m_Graaf", typeof(Graaf));
        }

        #endregion

        #region HelperFunctions
        public void ShowStraat()
        {
            Console.WriteLine($"Naam: {m_Naam}, ID: {m_StraatID}");
            m_Graaf.ShowGraaf();
        }
        #endregion

        #region Functions
        public List<Knoop> GetKnopen()
        {
            return m_Graaf.GetKnopen();
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {

            info.AddValue("m_StraatID", m_StraatID);
            info.AddValue("m_Naam", m_Naam);
            info.AddValue("m_Graaf", m_Graaf);
        }

        public double LengthOfStraat()
        {
            return m_Graaf.LengthOfGraaf();
        }
        #endregion

        #region Properties
        public int m_StraatID { get; private set; }
        public string m_Naam { get; private set; }
        public Graaf m_Graaf { get; private set; }
        #endregion


    }
}
