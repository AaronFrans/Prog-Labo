using Objects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Tool2.Utilities
{
    class Parser
    {
        public static List<Provincie> Deserialize(string path)
        {
            List<Provincie> toReturn = null;
            using (Stream s = File.Open(@$"{path}\Provincies.txt", FileMode.Open))
            {
                BinaryFormatter bf = new BinaryFormatter();
                toReturn = (List<Provincie>)bf.Deserialize(s);
            }

            return toReturn;

        }
    }
}
