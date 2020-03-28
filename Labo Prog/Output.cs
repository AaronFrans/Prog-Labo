using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Linq;

namespace Labo_Prog
{
    class Output
    {

        public static void SerializeProvincies(List<Provincie> toSerialize)
        {
            using (Stream s = File.Open(@"C:\Users\aaron\Downloads\Provincies.txt", FileMode.Create))
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(s, toSerialize);
            }

        }

        public static void MakeReport(List<Provincie> toReport)
        {
            var report = File.Create(@"C:\Users\aaron\Downloads\Rapport.txt");
            report.Close();
            ReportNrOfStreets(toReport);
            ReportStraatInfoForProvincie(toReport);
        }

        private static void ReportNrOfStreets(List<Provincie> toReport)
        {
            List<int> nrOfStreetsPerProvinie = new List<int>();
            int totalNrOfStreets = 0;

            foreach (Provincie provincie in toReport)
            {
                nrOfStreetsPerProvinie.Add(provincie.NrOfStreets());
                totalNrOfStreets += provincie.NrOfStreets();

            }
            using (StreamWriter sw = File.AppendText(@"C:\Users\aaron\Downloads\Rapport.txt"))
            {


                sw.WriteLine($"<totaal aantal straten: {totalNrOfStreets}>\n");

                sw.WriteLine("Aantal straten per provincie:");
                for (int i = 0; i < toReport.Count; i++)
                {
                    sw.WriteLine($"  *  <{toReport[i].m_Naam}>: <{nrOfStreetsPerProvinie[i]}>");
                }

            }

        }
        private static void ReportStraatInfoForProvincie(List<Provincie> toReport)
        {
            using (StreamWriter sw = File.AppendText(@"C:\Users\aaron\Downloads\Rapport.txt"))
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

