using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Labo_Prog
{
    class Tools
    {
        public static List<Straat> MaakStraten(Dictionary<int, List<Segment>> straten, Dictionary<int, string> straatNaamLookup)
        {
            List<Straat> toReturn = new List<Straat>();
            int GraafIDCounter = 1;
            foreach(KeyValuePair<int, List<Segment>> segmentenInStraat in straten)
            {
                if(straatNaamLookup.ContainsKey(segmentenInStraat.Key))
                {
                    Graaf graafInStraat = Graaf.BuildGraaf(GraafIDCounter, segmentenInStraat.Value);
                    Straat toAdd = new Straat(segmentenInStraat.Key, straatNaamLookup[segmentenInStraat.Key], graafInStraat);
                    GraafIDCounter++;
                    toReturn.Add(toAdd);
                }
           
            }

            return toReturn;
        }
        public static Segment MakeSegment(string[] line)
        {
            string toTrim = "LINESTRING( )";
            List<Punt> segmentPunten = SplitPunten(line[1].Trim(toTrim.ToCharArray()));
            Knoop beginKnoop, eindKnoop;
            int.TryParse(line[4], out int beginKnoopID);
            int.TryParse(line[5], out int eindKnoopID);
            beginKnoop = new Knoop(beginKnoopID, segmentPunten.First());
            eindKnoop = new Knoop(eindKnoopID, segmentPunten.Last());
            int.TryParse(line[0], out int segmentID);
            Segment toReturn = new Segment(segmentID, beginKnoop, eindKnoop, segmentPunten);

            return toReturn;
        }
        public static List<Punt> SplitPunten(string punten)
        {
            string[] puntenSplit = punten.Split(",");
            List<Punt> toReturn = new List<Punt>();
            foreach (string punt in puntenSplit)
            {
                string puntTrimmed = punt.Trim();
                string[] cordinaten = puntTrimmed.Split(' ');
                decimal.TryParse(cordinaten[0], out decimal x);
                decimal.TryParse(cordinaten[1], out decimal y);
                Punt punt1 = new Punt(x, y);
                toReturn.Add(punt1);
            }

            return toReturn;


        }



    }
}
