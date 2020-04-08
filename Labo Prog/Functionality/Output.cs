using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Linq;
using Objects;
using Syroot.Windows.IO;

namespace Tool1
{
    class Output
    {
        public static void MakeOutputFiles(List<Provincie> toOutput)
        {
            KnownFolder userDocuments = new KnownFolder(KnownFolderType.Documents);
            Directory.CreateDirectory(userDocuments.Path + @"\WRData-Output");
            SerializeProvincies(toOutput, userDocuments.Path + @"\WRData-Output\ProvinciesSerialized.txt");
            Console.WriteLine("****************************************");
            Console.WriteLine("Serlializatie Klaar");
            Console.WriteLine("****************************************");
            MakeReport(toOutput , userDocuments.Path + @"\WRData-Output\Rapport.txt");
            Console.WriteLine("****************************************");
            Console.WriteLine("Rapport Klaar");
            Console.WriteLine("****************************************");
        }
        private static void SerializeProvincies(List<Provincie> toSerialize, string path)
        {
            using (Stream s = File.Open(path, FileMode.Create))
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(s, toSerialize);
            }

        }

        private static void MakeReport(List<Provincie> toReport, string path)
        {
            var report = File.Create(path);
            report.Close();
            ReportNrOfStreets(toReport, path);
            ReportStraatInfoForProvincie(toReport, path);
        }

        private static void ReportNrOfStreets(List<Provincie> toReport, string path)
        {
            List<int> nrOfStreetsPerProvinie = new List<int>();
            int totalNrOfStreets = 0;

            foreach (Provincie provincie in toReport)
            {
                nrOfStreetsPerProvinie.Add(provincie.NrOfStreets());
                totalNrOfStreets += provincie.NrOfStreets();

            }
            using (StreamWriter sw = File.AppendText(path))
            {


                sw.WriteLine($"<totaal aantal straten: {totalNrOfStreets}>\n");

                sw.WriteLine("Aantal straten per provincie:");
                for (int i = 0; i < toReport.Count; i++)
                {
                    sw.WriteLine($"  *  <{toReport[i].m_Naam}>: <{nrOfStreetsPerProvinie[i]}>");
                }

            }

        }
        private static void ReportStraatInfoForProvincie(List<Provincie> toReport, string path)
        {
            using (StreamWriter sw = File.AppendText(path))
            {
                foreach (Provincie provincie in toReport)
                {
                    sw.WriteLine($"StraatInfo <{provincie.m_Naam}>");
                    foreach (Gemeente gemeente in provincie.m_Gemeenten)
                    {
                        double totalLengthStraten = 0;
                        foreach (Straat straat in gemeente.m_Straten)
                        {
                            totalLengthStraten += straat.LengthOfStraat();
                        }

                        sw.WriteLine($"  *  <{gemeente.m_Naam}>: <aantal straten: {gemeente.NrOfStreets()}>,<totale lengte: {totalLengthStraten}>");
                        Straat langsteStraat = gemeente.m_Straten.OrderByDescending(s => s.LengthOfStraat()).First();
                        Straat kortsteStraat = gemeente.m_Straten.OrderBy(s => s.LengthOfStraat()).First();
                        sw.WriteLine($"    -  <kortste straat: ID: {kortsteStraat.m_StraatID} Naam: {kortsteStraat.m_Naam}, Lengte: {kortsteStraat.LengthOfStraat()}>");
                        sw.WriteLine($"    -  <langste straat: ID: {langsteStraat.m_StraatID} Naam: {langsteStraat.m_Naam}, Lengte: {langsteStraat.LengthOfStraat()}>");
                    }
                }
            }
        }

    }
}

