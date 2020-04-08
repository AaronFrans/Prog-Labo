using Objects;
using Syroot.Windows.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Tool2.Utilities
{
    class Parser
    {
        public static List<Provincie> Deserialize()
        {
            List<Provincie> toReturn = null;
            Console.WriteLine("Is WRData-Output nog aaltijd in de Documents folder? Y/N");
            string answer = Console.ReadLine();
            while (!(answer.Equals("Y") || answer.Equals("N")))
            {
                Console.WriteLine("Geef aub een geldig antwoord: Y of N");
                answer = Console.ReadLine();
            }
            if (answer.Equals("N"))
            {
                Console.WriteLine("Geef het nieuw pad naar de WRData-Output folder");
                string path = Console.ReadLine();
              
                using (Stream s = File.Open(@$"{path}\WRData-Output\ProvinciesSerialized.txt", FileMode.Open))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    toReturn = (List<Provincie>)bf.Deserialize(s);
                }
            }
            else
            {
                KnownFolder userDocuments = new KnownFolder(KnownFolderType.Documents);

                using (Stream s = File.Open(@$"{userDocuments.Path}\WRData-Output\ProvinciesSerialized.txt", FileMode.Open))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    toReturn = (List<Provincie>)bf.Deserialize(s);
                }

            }
            Console.WriteLine("*****************************************************");
            Console.WriteLine("ProvinciesSerialized.txt gedeserializeerd.");
            Console.WriteLine("*****************************************************");

            return toReturn;

        }
    }
}
