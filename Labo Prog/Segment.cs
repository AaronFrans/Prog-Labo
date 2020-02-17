using System;
using System.Collections.Generic;
using System.Text;

namespace Labo_Prog
{
    class Segment
    {
        #region Constructor
        public Segment(int segmentID, Knoop beginKnoop, Knoop eindKnoop, List<Punt> vertices)
        {
            m_SegmentID = segmentID;
            m_BeginKnoop = beginKnoop;
            m_EindKnoop = eindKnoop;
            m_Vertices = vertices;
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
            toReturn += m_BeginKnoop.ToString();
            foreach(Punt punt in m_Vertices)
            {
                toReturn += punt.ToString();
            }
            toReturn += m_EindKnoop.ToString();
            return base.ToString();
        }

        #endregion

        #region Properties
        public Knoop m_BeginKnoop { get; private set; }
        public Knoop m_EindKnoop { get; private set; }

        public int m_SegmentID { get; private set; }

        public List<Punt> m_Vertices = new List<Punt>();

        #endregion
    }
}
