using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Objects;

namespace Tool1
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
            int counter = 0;
            foreach (var line in lines)
            {
                counter++;
                if (!int.TryParse(line[line.Length - 1], out int linksStraatID))
                {
                    throw new IdException("Een van de linksStraatIDs in WRData.csv is geen nummer van het type Int. Gelieve dit te veranderen");
                }
                if (!int.TryParse(line[line.Length - 2], out int rechtStraatID))
                {
                    throw new IdException("Een van de linksStraatIDs in WRData.csv is geen nummer van het type Int. Gelieve dit te veranderen");
                }
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

            int counter = 0;
            foreach (var line in lines.Skip(1))
            {
                counter++;
                if (!int.TryParse(line[0], out int ID))
                {
                    throw new IdException("Een van de IDs in WRStraatnamen.csv is geen nummer van het type Int. Gelieve dit te veranderen. Dit is op lijn " + counter + ".");
                }

                if (ID > 0)
                {
                    straten.Add(ID, line[1].Trim(' '));
                }

            }

            return straten;

        }

        public static Dictionary<int, List<int>> ParseStratenInGemeente(string path, string fileName)
        {
            Dictionary<int, List<int>> stratenInGemeente = new Dictionary<int, List<int>>();
            List<string[]> lines = FileSplitter(path, fileName, "csv", ';');
            int counter = 0;
            foreach (var line in lines.Skip(1))
            {
                counter++;
                if (!int.TryParse(line[0], out int straatID))
                {
                    throw new IdException("Een van de straatIDs in WRGemeenteID.csv is geen nummer van het type Int. Gelieve dit te veranderen. Dit is op lijn " + counter + ".");
                }
                if (!int.TryParse(line[1], out int gemeenteID))
                {
                    throw new IdException("Een van de straatIDs in WRGemeenteID.csv is geen nummer van het type Int. Gelieve dit te veranderen. Dit is op lijn " + counter + ".");
                }
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

            int counter = 0;
            foreach (var line in lines.Skip(1))
            {
                counter++;
                bool isAdded = false;
                if (line[2] == "nl")
                {

                    if (!int.TryParse(line[1], out int gemeenteID))
                    {
                        throw new IdException("Een van de gemeenteID in WRGemeentenaam.csv is geen nummer van het type Int. Gelieve dit te veranderen. Dit is op lijn " + counter + ".");
                    }
                    foreach (KeyValuePair<int, string> pair in gemeente)
                    {
                        if (gemeenteID == pair.Key)
                        {
                            isAdded = true;
                        }
                    }
                    if (isAdded == false)
                        gemeente.Add(gemeenteID, line[3]);




                }
            }
            return gemeente;
        }

        public static Dictionary<int, string> ParseProvincieNaam(string path, string fileName1, string fileName2)
        {
            Dictionary<int, string> Provincie = new Dictionary<int, string>();
            List<string[]> lines = FileSplitter(path, fileName1, "csv", ',');
            List<int> neededProvincieIDs = new List<int>();
            int counter = 0;
            foreach (var line in lines)
            {
                counter++;
                foreach (string st in line)
                {
                    if (!int.TryParse(st, out int ID))
                    {
                        throw new IdException("Een van de IDs in ProvincieIDsVlaanderen.csv is geen nummer van het type Int. Gelieve dit te veranderen. Dit is op lijn " + counter + ".");
                    }


                    neededProvincieIDs.Add(ID);
                }
            }

            lines = FileSplitter(path, fileName2, "csv", ';');
            counter = 0;
            foreach (var line in lines.Skip(1))
            {


                if (!int.TryParse(line[1], out int provincieID))
                {
                    throw new IdException("Een van de provincieID in ProvincieInfo.csv is geen nummer van het type Int. Gelieve dit te veranderen. Dit is op lijn " + counter + ".");
                }
                if (line[2] == "nl")
                {
                    bool isNeeded = false;
                    foreach (int ID in neededProvincieIDs)
                    {
                        if (ID == provincieID)
                        {
                            isNeeded = true;
                        }
                    }

                    if (isNeeded)
                    {
                        bool isAdded = false;


                        
                        foreach (KeyValuePair<int, string> pair in Provincie)
                        {
                            if (provincieID == pair.Key)
                            {
                                isAdded = true;
                            }
                        }
                        if (isAdded == false)
                            Provincie.Add(provincieID, line[3]);

                    }
                }
            }
            return Provincie;

        }

        public static Dictionary<int, List<int>> ParseGemeentesinProvincie(string path, string fileName1, string fileName2)
        {
            Dictionary<int, List<int>> gemeenteInProvincie = new Dictionary<int, List<int>>();
            List<string[]> lines = FileSplitter(path, fileName1, "csv", ',');
            List<int> neededProvincieIDs = new List<int>();
            int counter = 0;
            foreach (var line in lines)
            {
                counter++;
                foreach (string st in line)
                {
                    if (!int.TryParse(st, out int ID))
                    {
                        throw new IdException("Een van de IDs in ProvincieIDsVlaanderen.csv is geen nummer van het type Int. Gelieve dit te veranderen. Dit is op lijn " + counter + ".");
                    }


                    neededProvincieIDs.Add(ID);
                }
            }

            lines = FileSplitter(path, fileName2, "csv", ';');
            counter = 0;
            foreach (var line in lines.Skip(1))
            {
                counter++;

                if (!int.TryParse(line[1], out int provincieID))
                {
                    throw new IdException("Een van de provincieIDs in ProvincieInfo.csv is geen nummer van het type Int. Gelieve dit te veranderen. Dit is op lijn " + counter + ".");
                }
                if (!int.TryParse(line[0], out int gemeenteID))
                {
                    throw new IdException("Een van de gemeenteIDs in ProvincieInfo.csv is geen nummer van het type Int. Gelieve dit te veranderen. Dit is op lijn " + counter + ".");
                }
                if (provincieID > 0)
                {
                    if (line[2] == "nl")
                    {
                        bool isNeeded = false;
                        foreach (int ID in neededProvincieIDs)
                        {
                            if (ID == provincieID)
                            {
                                isNeeded = true;
                            }
                        }

                        if (isNeeded)
                        {
                            bool isFound = false;
                            if (gemeenteInProvincie.Count == 0)
                            {

                                gemeenteInProvincie.Add(provincieID, new List<int>() { gemeenteID });

                            }
                            else
                            {
                                foreach (KeyValuePair<int, List<int>> Provincie in gemeenteInProvincie)
                                {

                                    if (provincieID == Provincie.Key)
                                    {
                                        isFound = true;
                                    }


                                }
                                if (!isFound)
                                {
                                    gemeenteInProvincie.Add(provincieID, new List<int>() { gemeenteID });
                                }
                                else
                                {
                                    gemeenteInProvincie[provincieID].Add(gemeenteID);
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
