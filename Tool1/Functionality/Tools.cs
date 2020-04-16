using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;
using Objects;

namespace Tool1.Functionality
{
    class Tools
    {

        static public List<Straat> MaakStraten(string path)
        {
            Dictionary<int, List<Segment>> straten = Parser.ParseSegment(path, "WRdata");
            Dictionary<int, string> straatNaamLookup = Parser.ParseStraatNamen(path, "WRstraatnamen");


            List<Straat> toReturn = new List<Straat>();
            foreach (KeyValuePair<int, List<Segment>> segmentenInStraat in straten)
            {
                if (straatNaamLookup.ContainsKey(segmentenInStraat.Key))
                {
                    Graaf graafInStraat = Graaf.BuildGraaf(segmentenInStraat.Key, segmentenInStraat.Value);
                    Straat toAdd = new Straat(segmentenInStraat.Key, straatNaamLookup[segmentenInStraat.Key], graafInStraat);
                    toReturn.Add(toAdd);
                }
            }
            Console.WriteLine("****************************************");
            Console.WriteLine("Alle Straat objecten zijn aangemaakt");
            Console.WriteLine("****************************************");

            return toReturn;
        }

        public static void UnzipFiles(string path)
        {
            var directory = new DirectoryInfo(path);
            foreach (var file in directory.GetFiles())
            {
                if (file.Extension.Equals(".zip"))
                {
                    ZipFile.ExtractToDirectory(file.FullName, path);
                }
            }

        }

        public static Segment MakeSegment(string[] line)
        {
            string toTrim = "LINESTRING( )";
            List<Punt> segmentPunten = SplitPunten(line[1].Trim(toTrim.ToCharArray()));
            Knoop beginKnoop, eindKnoop;
            if (!int.TryParse(line[4], out int beginKnoopID))
            {
                throw new IdException("Een beginKnoopID in file WRdata.csv was geen nummer van het type Int. Gelieve dit te veranderen");
            }
            if (!int.TryParse(line[5], out int eindKnoopID))
            {
                throw new IdException("Een eindKnoop in file WRdata.csv was geen nummer van het type Int. Gelieve dit te veranderen");
            }
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
                if (!double.TryParse(cordinaten[0], out double x))
                {
                    throw new CoordinateException("Een C coordinaat in een van de puntlijsten in file WRdata.csv was geen nummer van het type Double. Gelieve dit te veranderen");
                }
                if (!double.TryParse(cordinaten[1], out double y))
                {
                    throw new CoordinateException("Een Y coordinaat in een van de puntlijsten in file WRdata.csv was geen nummer van het type Double. Gelieve dit te veranderen");
                }
                Punt punt1 = new Punt(x, y);
                toReturn.Add(punt1);
            }

            return toReturn;


        }

        public static List<Gemeente> MaakGemeenten(string path)
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

                        if (straten.Any(s => s.m_StraatID == straatID))
                        {
                            stratenInGemeente.Add(straten.First(s => s.m_StraatID == straatID));

                        }

                    });
                    Gemeente gemeenteToAdd = new Gemeente(gemeenteID, gemeenteNaamLookup[gemeenteID], stratenInGemeente);
                    if (gemeenteToAdd.m_Straten.Count != 0)
                    {
                        toReturn.Add(gemeenteToAdd);
                    }

                }
            }

            Console.WriteLine("****************************************");
            Console.WriteLine("Alle Gemeente objecten zijn aangemaakt");
            Console.WriteLine("****************************************");
            return toReturn;

        }

        public static List<Provincie> MaakProvincies(string path)
        {
            List<Gemeente> gemeentes = MaakGemeenten(path);
            Dictionary<int, string> ProvincieNaamLookup = Parser.ParseProvincieNaam(path, "ProvincieIDsVlaanderen", "ProvincieInfo");
            Dictionary<int, List<int>> GemeentesPerProvincieLookup = Parser.ParseGemeentesinProvincie(path, "ProvincieIDsVlaanderen", "ProvincieInfo");

            List<Provincie> toReturn = new List<Provincie>();

            foreach (KeyValuePair<int, List<int>> Provincie in GemeentesPerProvincieLookup)
            {
                int ProvincieID = Provincie.Key;
                if (ProvincieNaamLookup.ContainsKey(ProvincieID))
                {
                    List<Gemeente> gemeentesInProvincie = new List<Gemeente>();
                    foreach (int gemeenteID in Provincie.Value)
                    {
                        if (gemeentes.Any(g => g.m_GemeenteID == gemeenteID))
                        {
                            gemeentesInProvincie.Add(gemeentes.First(g => g.m_GemeenteID == gemeenteID));

                        }
                    }
                    Provincie ProvincieToAdd = new Provincie(ProvincieID, ProvincieNaamLookup[ProvincieID], gemeentesInProvincie);
                    toReturn.Add(ProvincieToAdd);

                }


            }

            Console.WriteLine("****************************************");
            Console.WriteLine("Alle Provincy objecten zijn aangemaakt");
            Console.WriteLine("****************************************");

            return toReturn;
        }

    }
}
