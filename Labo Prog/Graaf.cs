using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

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


        public void ShowGraaf()
        {
            foreach(KeyValuePair< Knoop, List<Segment>> pair in m_Map)
            {
                Console.WriteLine(pair.Key.ToString());
                foreach(Segment segment in pair.Value)
                {
                    Console.WriteLine(segment.ToString());
                }
            }
        }
        #endregion

        #region Functions
        public static Graaf BuildGraaf(int graafID, List<Segment> segments)
        {
            Graaf ToReturn = new Graaf(graafID);
            foreach(Segment segment in segments)
            {
                if(ToReturn.m_Map.Count == 0)
                {
                    ToReturn.m_Map.Add(segment.m_BeginKnoop,new List<Segment>() { segment });
                }
                else
                {
                    if(ToReturn.m_Map.ContainsKey(segment.m_BeginKnoop))
                    {
                        ToReturn.m_Map[segment.m_BeginKnoop].Add(segment);
                    }
                    else
                    {
                        ToReturn.m_Map.Add(segment.m_BeginKnoop, new List<Segment>() { segment });
                    }
                }
            }
            ToReturn.m_Map.Add(segments[0].m_BeginKnoop, segments);
            return ToReturn;

        }

        public List<Knoop> GetKnopen()
        {
            List<Knoop> knopen = new List<Knoop>();
            foreach (KeyValuePair<Knoop, List<Segment>> pair in m_Map)
            {
                knopen.Add(pair.Key);
            }

            return knopen;
        }
        #endregion

        #region Properties
        public int m_GraafID { get; private set; }
        public Dictionary<Knoop, List<Segment>> m_Map { get; set; } = new Dictionary<Knoop, List<Segment>>(); 

        #endregion
    }
}
