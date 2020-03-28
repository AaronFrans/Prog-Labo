using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Labo_Prog
{
    [Serializable()]
    public class Graaf : ISerializable
    {

        #region Constructor

        public Graaf(int graafID)
        {
            m_GraafID = graafID;
            m_Map = new Dictionary<Knoop, List<Segment>>();
        }
        #endregion
        private Graaf(SerializationInfo info, StreamingContext context)
        {
            m_GraafID = (int)info.GetValue("m_GraafID", typeof(int));
            m_Map = (Dictionary<Knoop, List<Segment>>)info.GetValue("m_Map", typeof(Dictionary<Knoop, List<Segment>>));
        }

        #region HelperFunctions
        public void ShowGraaf()
        {
            Console.WriteLine($"Graaf met ID: {m_GraafID}\n");
            foreach (KeyValuePair<Knoop, List<Segment>> pair in m_Map)
            {
                Console.WriteLine("Alle segmenten met beginknoop: " + pair.Key.m_KnoopID + " begin");

                foreach (Segment segment in pair.Value)
                {
                    Console.WriteLine(segment.ToString());
                }

                Console.WriteLine("Alle segmenten met beginknoop: " + pair.Key.m_KnoopID + " einde");
            }
        }
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("m_GraafID", m_GraafID);
            info.AddValue("m_Map", m_Map);
        }
        #endregion

        #region Functions
        public static Graaf BuildGraaf(int graafID, List<Segment> segments)
        {
            Graaf ToReturn = new Graaf(graafID);
            foreach (Segment segment in segments)
            {
                if (ToReturn.m_Map.Count == 0)
                {
                    ToReturn.m_Map.Add(segment.m_BeginKnoop, new List<Segment>() { segment });
                }
                else
                {
                    if (ToReturn.m_Map.ContainsKey(segment.m_BeginKnoop))
                    {
                        ToReturn.m_Map[segment.m_BeginKnoop].Add(segment);
                    }
                    else
                    {
                        ToReturn.m_Map.Add(segment.m_BeginKnoop, new List<Segment>() { segment });
                    }
                }
            }
            return ToReturn;

        }

        public List<Knoop> GetKnopen()
        {
            List<Knoop> knopen = new List<Knoop>();

            foreach (KeyValuePair<Knoop, List<Segment>> pair in m_Map)
            {

                foreach (Segment segment in pair.Value)
                {
                    knopen.Add(segment.m_BeginKnoop);
                    knopen.Add(segment.m_EindKnoop);
                }
            }

            return knopen;
        }

        public double LengthOfGraaf()
        {
            double toReturn = 0;
            foreach (KeyValuePair<Knoop, List<Segment>> pair in m_Map)
            {
                foreach (Segment segment in pair.Value)
                {
                    toReturn += segment.LengthOfSegment();
                }
            }

            return toReturn;
        }


        #endregion

        #region Properties
        public int m_GraafID { get; set; }
        public Dictionary<Knoop, List<Segment>> m_Map { get; set; }

        #endregion
    }
}
