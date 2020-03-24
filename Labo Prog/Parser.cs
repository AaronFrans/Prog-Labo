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
                int linksStraatID, rechtStraatID;
                int.TryParse(line[line.Length - 1], out linksStraatID);
                int.TryParse(line[line.Length - 2], out rechtStraatID);
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
                int id;
                int.TryParse(line[0], out id);
                if (id > 0)
                {
                    straten.Add(id, line[1].Trim(' ', '"'));
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
                int gemeenteID, straatID;
                int.TryParse(line[0], out straatID);
                int.TryParse(line[1], out gemeenteID);
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
                    int id;
                    int.TryParse(line[1], out id);
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


        public static Dictionary<int, string> ParseProvincyNaam(string path, string fileName1, string fileName2)
        {
            Dictionary<int, string> provincy = new Dictionary<int, string>();
            List<string[]> lines = FileSplitter(path, fileName1, "csv", ',');
            List<int> neededProvincyIDs = new List<int>();
            foreach (var line in lines)
            {
                foreach (string st in line)
                {
                    int.TryParse(st, out int id);
                    neededProvincyIDs.Add(id);
                }
            }

            lines = FileSplitter(path, fileName2, "csv", ';');

            foreach (var line in lines)
            {

                int provincyID;
                int.TryParse(line[1], out provincyID);
                if (line[2] == "nl")
                {
                    bool isNeeded = false;
                    foreach (int id in neededProvincyIDs)
                    {
                        if (id == provincyID)
                        {
                            isNeeded = true;
                        }
                    }

                    if (isNeeded)
                    {
                        bool isAdded = false;

                        int id;
                        int.TryParse(line[1], out id);
                        foreach (KeyValuePair<int, string> pair in provincy)
                        {
                            if (id == pair.Key)
                            {
                                isAdded = true;
                            }
                        }
                        if (isAdded == false)
                            provincy.Add(id, line[3]);

                    }
                }
            }
            provincy = provincy.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
            return provincy;

        }

        public static Dictionary<int, List<int>> ParseGemeentesinProvincy(string path, string fileName1, string fileName2)
        {
            Dictionary<int, List<int>> gemeenteInProvincy = new Dictionary<int, List<int>>();
            List<string[]> lines = FileSplitter(path, fileName1, "csv", ',');
            List<int> neededProvincyIDs = new List<int>();
            foreach (var line in lines)
            {
                foreach (string st in line)
                {
                    int.TryParse(st, out int id);
                    neededProvincyIDs.Add(id);
                }
            }

            lines = FileSplitter(path, fileName2, "csv", ';');
            foreach (var line in lines)
            {
                int gemeenteID, provincyID;
                int.TryParse(line[1], out provincyID);
                int.TryParse(line[0], out gemeenteID);
                if (provincyID > 0)
                {
                    if (line[2] == "nl")
                    {
                        bool isNeeded = false;
                        foreach (int id in neededProvincyIDs)
                        {
                            if (id == provincyID)
                            {
                                isNeeded = true;
                            }
                        }

                        if (isNeeded)
                        {
                            bool isFound = false;
                            if (gemeenteInProvincy.Count == 0)
                            {

                                gemeenteInProvincy.Add(provincyID, new List<int>() { gemeenteID });

                            }
                            else
                            {
                                foreach (KeyValuePair<int, List<int>> Provincy in gemeenteInProvincy)
                                {

                                    if (provincyID == Provincy.Key)
                                    {
                                        isFound = true;
                                    }


                                }
                                if (!isFound)
                                {
                                    gemeenteInProvincy.Add(provincyID, new List<int>() { gemeenteID });
                                }
                                else
                                {
                                    gemeenteInProvincy[provincyID].Add(gemeenteID);
                                }
                            }
                        }

                    }


                }

            }

            return gemeenteInProvincy;
        }


    }
}
