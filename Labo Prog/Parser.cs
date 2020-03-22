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
        public static Dictionary<int, List<Segment>> ParseSegment(string path, string fileName, string extension, char delim)
        {
            List<string[]> lines = FileSplitter(path, fileName, extension, delim);
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


    }
}
