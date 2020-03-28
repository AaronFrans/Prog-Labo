using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Labo_Prog
{
    class Parser
    {
        public static List<string[]> FileSplitter(string path, string fileName, string extension, char delim)
        {
            List<string[]> splitLines = new List<string[]>();
            using (StreamReader reader = new StreamReader(path + $@"\{fileName}.{extension}"))
            {

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(delim);
                    splitLines.Add(values);
                }
            }

            return splitLines;

        }

        public static Dictionary<int, List<Segment>> ParseSegment(string path, string fileName)
        {
            List<string[]> lines = FileSplitter(path, fileName, "csv", ';');
            lines = lines.Skip(1).ToList(); //get rid of 1st useless line
            Dictionary<int, List<Segment>> toReturn = new Dictionary<int, List<Segment>>();
            foreach (string[] line in lines)
            {
                int.TryParse(line[line.Length - 1], out int linksStraatID);
                int.TryParse(line[line.Length - 2], out int rechtStraatID);
                if (linksStraatID == rechtStraatID)
                {
                    if (linksStraatID != -9)
                    {

                        Segment toAdd = Tools.MakeSegment(line);

                        if (toReturn.Count == 0)
                        {
                            toReturn.Add(linksStraatID, new List<Segment>() { toAdd });
                        }
                        else
                        {
                            if (toReturn.ContainsKey(linksStraatID))
                            {
                                if (!toReturn[linksStraatID].Contains(toAdd))
                                    toReturn[linksStraatID].Add(toAdd);
                            }
                            else
                            {
                                toReturn.Add(linksStraatID, new List<Segment>() { toAdd });
                            }
                        }




                    }
                }
                else
                {
                    if (linksStraatID == -9 || rechtStraatID == -9)
                    {
                        if (linksStraatID == -9)
                        {
                            Segment toAdd = Tools.MakeSegment(line);
                            if (toReturn.Count == 0)
                            {
                                toReturn.Add(rechtStraatID, new List<Segment>() { toAdd });
                            }
                            else
                            {
                                if (toReturn.ContainsKey(rechtStraatID))
                                {
                                    if (!toReturn[rechtStraatID].Contains(toAdd))
                                        toReturn[rechtStraatID].Add(toAdd);
                                }
                                else
                                {
                                    toReturn.Add(rechtStraatID, new List<Segment>() { toAdd });
                                }
                            }

                        }
                        else
                        {
                            Segment toAdd = Tools.MakeSegment(line);
                            if (toReturn.Count == 0)
                            {
                                toReturn.Add(linksStraatID, new List<Segment>() { toAdd });
                            }
                            else
                            {
                                if (toReturn.ContainsKey(linksStraatID))
                                {
                                    if (!toReturn[linksStraatID].Contains(toAdd))
                                        toReturn[linksStraatID].Add(toAdd);
                                }
                                else
                                {
                                    toReturn.Add(linksStraatID, new List<Segment>() { toAdd });
                                }
                            }
                        }
                    }
                    else
                    {
                        Segment toAdd = Tools.MakeSegment(line);
                        if (toReturn.Count == 0)
                        {
                            toReturn.Add(rechtStraatID, new List<Segment>() { toAdd });
                            toReturn.Add(linksStraatID, new List<Segment>() { toAdd });
                        }
                        else
                        {
                            if (toReturn.ContainsKey(rechtStraatID))
                            {
                                if (!toReturn[rechtStraatID].Contains(toAdd))
                                    toReturn[rechtStraatID].Add(toAdd);
                            }
                            else
                            {
                                toReturn.Add(rechtStraatID, new List<Segment>() { toAdd });
                            }
                            if (toReturn.ContainsKey(linksStraatID))
                            {
                                if (!toReturn[linksStraatID].Contains(toAdd))
                                    toReturn[linksStraatID].Add(toAdd);
                            }
                            else
                            {
                                toReturn.Add(linksStraatID, new List<Segment>() { toAdd });
                            }
                        }

                    }

                }



            }

            return toReturn;
        }

        public static Dictionary<int, string> ParseStraatNamen(string path, string fileName)
        {
            Dictionary<int, string> straten = new Dictionary<int, string>();

            List<string[]> lines = FileSplitter(path, fileName, "csv", ';');


            foreach (var line in lines)
            {
                
                int.TryParse(line[0], out int id);
                if (id > 0)
                {
                    straten.Add(id, line[1].Trim(' '));
                }

            }

            return straten;

        }

        public static Dictionary<int, List<int>> ParseStratenInGemeente(string path, string fileName)
        {
            Dictionary<int, List<int>> stratenInGemeente = new Dictionary<int, List<int>>();
            List<string[]> lines = FileSplitter(path, fileName, "csv", ';');
            foreach (var line in lines)
            {
                 
                int.TryParse(line[0], out int straatID);
                int.TryParse(line[1], out int gemeenteID);
                if (gemeenteID > 0)
                {
                    bool isFound = false;
                    if (stratenInGemeente.Count == 0)
                    {

                        stratenInGemeente.Add(gemeenteID, new List<int>() { straatID });

                    }
                    else
                    {
                        foreach (KeyValuePair<int, List<int>> Gemeente in stratenInGemeente)
                        {

                            if (gemeenteID == Gemeente.Key)
                            {
                                isFound = true;
                            }


                        }
                        if (!isFound)
                        {
                            stratenInGemeente.Add(gemeenteID, new List<int>() { straatID });
                        }
                        else
                        {
                            stratenInGemeente[gemeenteID].Add(straatID);
                        }
                    }

                }

            }

            return stratenInGemeente;
        }

        public static Dictionary<int, string> ParseGemeenteNaam(string path, string fileName)
        {
            Dictionary<int, string> gemeente = new Dictionary<int, string>();

            List<string[]> lines = FileSplitter(path, fileName, "csv", ';');

            foreach (var line in lines)
            {
                bool isAdded = false;
                if (line[2] == "nl")
                {
                    
                    int.TryParse(line[1], out int id);
                    foreach (KeyValuePair<int, string> pair in gemeente)
                    {
                        if (id == pair.Key)
                        {
                            isAdded = true;
                        }
                    }
                    if (isAdded == false)
                        gemeente.Add(id, line[3]);




                }
            }
            gemeente = gemeente.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
            return gemeente;
        }


        public static Dictionary<int, string> ParseProvincieNaam(string path, string fileName1, string fileName2)
        {
            Dictionary<int, string> Provincie = new Dictionary<int, string>();
            List<string[]> lines = FileSplitter(path, fileName1, "csv", ',');
            List<int> neededProvincieIDs = new List<int>();
            foreach (var line in lines)
            {
                foreach (string st in line)
                {
                    int.TryParse(st, out int id);
                    neededProvincieIDs.Add(id);
                }
            }

            lines = FileSplitter(path, fileName2, "csv", ';');

            foreach (var line in lines)
            {

                
                int.TryParse(line[1], out int ProvincieID);
                if (line[2] == "nl")
                {
                    bool isNeeded = false;
                    foreach (int id in neededProvincieIDs)
                    {
                        if (id == ProvincieID)
                        {
                            isNeeded = true;
                        }
                    }

                    if (isNeeded)
                    {
                        bool isAdded = false;


                        int.TryParse(line[1], out int id);
                        foreach (KeyValuePair<int, string> pair in Provincie)
                        {
                            if (id == pair.Key)
                            {
                                isAdded = true;
                            }
                        }
                        if (isAdded == false)
                            Provincie.Add(id, line[3]);

                    }
                }
            }
            Provincie = Provincie.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
            return Provincie;

        }

        public static Dictionary<int, List<int>> ParseGemeentesinProvincie(string path, string fileName1, string fileName2)
        {
            Dictionary<int, List<int>> gemeenteInProvincie = new Dictionary<int, List<int>>();
            List<string[]> lines = FileSplitter(path, fileName1, "csv", ',');
            List<int> neededProvincieIDs = new List<int>();
            foreach (var line in lines)
            {
                foreach (string st in line)
                {
                    int.TryParse(st, out int id);
                    neededProvincieIDs.Add(id);
                }
            }

            lines = FileSplitter(path, fileName2, "csv", ';');
            foreach (var line in lines)
            {
                int.TryParse(line[1], out int ProvincieID);
                int.TryParse(line[0], out int gemeenteID);
                if (ProvincieID > 0)
                {
                    if (line[2] == "nl")
                    {
                        bool isNeeded = false;
                        foreach (int id in neededProvincieIDs)
                        {
                            if (id == ProvincieID)
                            {
                                isNeeded = true;
                            }
                        }

                        if (isNeeded)
                        {
                            bool isFound = false;
                            if (gemeenteInProvincie.Count == 0)
                            {

                                gemeenteInProvincie.Add(ProvincieID, new List<int>() { gemeenteID });

                            }
                            else
                            {
                                foreach (KeyValuePair<int, List<int>> Provincie in gemeenteInProvincie)
                                {

                                    if (ProvincieID == Provincie.Key)
                                    {
                                        isFound = true;
                                    }


                                }
                                if (!isFound)
                                {
                                    gemeenteInProvincie.Add(ProvincieID, new List<int>() { gemeenteID });
                                }
                                else
                                {
                                    gemeenteInProvincie[ProvincieID].Add(gemeenteID);
                                }
                            }
                        }

                    }


                }

            }

            return gemeenteInProvincie;
        }


    }
}
