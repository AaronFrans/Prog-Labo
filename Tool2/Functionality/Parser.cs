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
            KnownFolder userDocuments = new KnownFolder(KnownFolderType.Documents);

            List<Provincie> toReturn = null;
            using (Stream s = File.Open(@$"{userDocuments.Path}\WRData-Output\ProvinciesSerialized.txt", FileMode.Open))
            {
                BinaryFormatter bf = new BinaryFormatter();
                toReturn = (List<Provincie>)bf.Deserialize(s);
            }

            return toReturn;

        }
    }
}
