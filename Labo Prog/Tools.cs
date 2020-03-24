using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Labo_Prog
{
    class Tools
    {
        public static List<Straat> MaakStraten(string path)
        {
            Dictionary<int, List<Segment>> straten = Parser.ParseSegment(path, "WRdata");
            Dictionary<int, string> straatNaamLookup = Parser.ParseStraatNamen(path, "WRstraatnamen");


            List<Straat> toReturn = new List<Straat>();
            int GraafIDCounter = 1;
            foreach (KeyValuePair<int, List<Segment>> segmentenInStraat in straten)
            {
                if (straatNaamLookup.ContainsKey(segmentenInStraat.Key))
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
                double.TryParse(cordinaten[0], out double x);
                double.TryParse(cordinaten[1], out double y);
                Punt punt1 = new Punt(x, y);
                toReturn.Add(punt1);
            }

            return toReturn;


        }

        public static List<Gemeente> MaakGemeentes(string path)
        {
            List<Straat> straten = MaakStraten(path);
            Dictionary<int, string> gemeenteNaamLookup = Parser.ParseGemeenteNaam(path, "WRgemeentenaam");
            Dictionary<int, List<int>> stratenPerGemeenteLookup = Parser.ParseStratenInGemeente(path, "WRgemeenteID");

            List<Gemeente> toReturn = new List<Gemeente>();
            foreach (KeyValuePair<int, List<int>> gemeente in stratenPerGemeenteLookup)
            {
                int gemeenteID = gemeente.Key;
                if (gemeenteNaamLookup.ContainsKey(gemeenteID))
                {
                    List<Straat> stratenInGemeente = new List<Straat>();
                    Parallel.ForEach(gemeente.Value, (straatID) =>
                    {

                        if (straten.Any(s => s.m_ID == straatID))
                        {
                            stratenInGemeente.Add(straten.First(s => s.m_ID == straatID));

                        }

                    });
                    Gemeente gemeenteToAdd = new Gemeente(gemeenteID, gemeenteNaamLookup[gemeenteID], stratenInGemeente);
                    toReturn.Add(gemeenteToAdd);
                    Console.WriteLine("added Gemeente: " + gemeenteID);
                }
            }
            Console.WriteLine("added all gemeentes");
            return toReturn;

        }

        public static List<Provincy> MaakProvincies(string path)
        {
            List<Gemeente> gemeentes = MaakGemeentes(path);
            Dictionary<int, string> provincyNaamLookup = Parser.ParseProvincyNaam(path, "ProvincieIDsVlaanderen", "ProvincieInfo");
            Dictionary<int, List<int>> GemeentesPerProvincyLookup = Parser.ParseGemeentesinProvincy(path, "ProvincieIDsVlaanderen", "ProvincieInfo");

            List<Provincy> toReturn = new List<Provincy>();

            foreach (KeyValuePair<int, List<int>> provincy in GemeentesPerProvincyLookup)
            {
                int provincyID = provincy.Key;
                if (provincyNaamLookup.ContainsKey(provincyID))
                {
                    List<Gemeente> gemeentesInProvincy = new List<Gemeente>();
                    foreach (int gemeenteID in provincy.Value)
                    {
                        if(gemeentes.Any(g => g.m_GemeenteID == gemeenteID))
                        {
                            gemeentesInProvincy.Add(gemeentes.First(g => g.m_GemeenteID == gemeenteID));

                        }
                    }
                    Provincy provincyToAdd = new Provincy(provincyID, provincyNaamLookup[provincyID], gemeentesInProvincy);
                    toReturn.Add(provincyToAdd);
                    Console.WriteLine("added Provincy: " + provincyID);

                }


            }


            return toReturn;
        }

    }
}
