using System;
using System.Collections.Generic;
using System.Text;

namespace Labo_Prog
{
    class Graaf
    {

        #region Constructor
        public Graaf(int graafID)
        {
            m_GraafID = graafID;
        }
        #endregion

        #region HelperFunctions
        public List<Knoop> GetKnopen()
        {
            foreach()
        }
        #endregion

        #region Functions
        public static Graaf BuildGraaf(int graafID, List<Segment> segments)
        {
            Graaf ToReturn = new Graaf(graafID);
            ToReturn.m_Map.Add(segments[0].m_BeginKnoop, segments);
            return ToReturn;

        }
            #endregion

        #region Properties
        public int m_GraafID { get; private set; }
        public Dictionary<Knoop, List<Segment>> m_Map { get; set; } = new Dictionary<Knoop, List<Segment>>(); 

        #endregion
    }
}
