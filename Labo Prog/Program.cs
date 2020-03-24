using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;

namespace Labo_Prog
{
    class Program
    {
        static void Main(string[] args)
        {
            //    Stopwatch stopWatch = new Stopwatch();
            //    stopWatch.Start();
            //    List<Provincy> gemeentes = Tools.MaakProvincies(@"E:\School\Hogent\prog 3\Labo\WRdata-master\");
            //    stopWatch.Stop();
            //    // Get the elapsed time as a TimeSpan value.
            //    TimeSpan ts = stopWatch.Elapsed;
            //    string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            //    ts.Hours, ts.Minutes, ts.Seconds,
            //    ts.Milliseconds / 10);
            //    Console.WriteLine("RunTime " + elapsedTime);
            //    Console.ReadLine();
            //}

            Knoop testje = new Knoop(9,new Punt(8.945, 95.54654));
            XmlSerializer xs = new XmlSerializer(typeof(Knoop));

            
            using (TextWriter txtWriter = new StreamWriter(@"C:\Users\aaron\Downloads\Punt.xml"))
            {
                xs.Serialize(txtWriter, testje);
            }
                
            TextReader txtReader = new StreamReader(@"C:\Users\aaron\Downloads\Punt.xml");

            Knoop TestRead = null;

            TestRead = (Knoop)xs.Deserialize(txtReader);

            Console.WriteLine(TestRead);
            //List<Knoop> test = new List<Knoop>();
            //test.Add(new Knoop(91, new Punt(5, 9)));
            //test.Add(new Knoop(171, new Punt(8, 1)));
            //test.Add(new Knoop(566423, new Punt(-75, 247)));




        }
    }
}
