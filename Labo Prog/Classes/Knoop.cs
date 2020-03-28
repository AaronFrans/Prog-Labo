using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Labo_Prog
{
    [Serializable()]
    public class Knoop : ISerializable
    {
        #region Constructor

        public Knoop(int knoopID, Punt punt)
        {
            m_KnoopID = knoopID;
            m_Punt = punt;
        }
        private  Knoop(SerializationInfo info, StreamingContext context)
        {
            m_KnoopID = (int)info.GetValue("m_KnoopID", typeof(int));
            m_Punt = (Punt)info.GetValue("m_Punt", typeof(Punt));
        }
        #endregion

        #region HelperFunctions

        public override string ToString()
        {
            return $"Knoop: {m_KnoopID} \n" + m_Punt.ToString();
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

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("m_KnoopID", m_KnoopID);
            info.AddValue("m_Punt", m_Punt);
        }
        #endregion

        #region Properties
        public int m_KnoopID { get; set; }
        public Punt m_Punt { get; set; }


        #endregion
    }
}
