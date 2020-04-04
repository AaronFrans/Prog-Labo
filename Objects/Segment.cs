using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Objects
{
    [Serializable()]
    public class Segment : ISerializable
    {
        #region Constructors

        public Segment(int segmentID, Knoop beginKnoop, Knoop eindKnoop, List<Punt> vertices)
        {
            m_SegmentID = segmentID;
            m_BeginKnoop = beginKnoop;
            m_EindKnoop = eindKnoop;
            m_Vertices = vertices;
        }
        private Segment(SerializationInfo info, StreamingContext context)
        {
            m_SegmentID = (int)info.GetValue("m_SegmentID", typeof(int));
            m_BeginKnoop = (Knoop)info.GetValue("m_BeginKnoop", typeof(Knoop));
            m_EindKnoop = (Knoop)info.GetValue("m_EindKnoop", typeof(Knoop));
            m_Vertices = (List<Punt>)info.GetValue("m_Vertices", typeof(List<Punt>));
        }
        #endregion

        #region HelperFunctions
        public override bool Equals(object obj)
        {
            return obj is Segment segment &&
                   EqualityComparer<Knoop>.Default.Equals(m_BeginKnoop, segment.m_BeginKnoop) &&
                   EqualityComparer<Knoop>.Default.Equals(m_EindKnoop, segment.m_EindKnoop) &&
                   m_SegmentID == segment.m_SegmentID;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(m_BeginKnoop, m_EindKnoop, m_SegmentID);
        }

        public override string ToString()
        {
            string toReturn = "";
            toReturn += $"Segment: {m_SegmentID}\n";

            toReturn += "Begin Knoop: " + m_BeginKnoop.ToString() + "\n";

            toReturn += "Eind Knoop: " + m_EindKnoop.ToString() + "\n";

            toReturn += "Punten: \n";

            foreach (Punt punt in m_Vertices)
            {
                toReturn += punt.ToString() + "\n";
            }


            return toReturn;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("m_SegmentID", m_SegmentID);
            info.AddValue("m_BeginKnoop", m_BeginKnoop);
            info.AddValue("m_EindKnoop", m_EindKnoop);
            info.AddValue("m_Vertices", m_Vertices);
        }

        #endregion

        #region Functions

        public double LengthOfSegment()
        {
            double toReturn = 0;
            for (int i = 0; i < m_Vertices.Count -1; i++)
            {
                toReturn += Punt.DistanceBetweenTwoPoint(m_Vertices[i], m_Vertices[i + 1]);
            }

            return toReturn;
        }

        #endregion

        #region Properties
        public Knoop m_BeginKnoop { get; set; }
        public Knoop m_EindKnoop { get; set; }

        public int m_SegmentID { get; set; }

        public List<Punt> m_Vertices { get; set; }

        #endregion
    }
}
