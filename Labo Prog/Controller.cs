using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace Labo_Prog
{
    class Controller
    {

        public static void RunTool1()
        {
			try
			{
				string path = "";
				Console.WriteLine("Geef a.u.b. het pad naar de WRdata-master folder.");
				path = Console.ReadLine() + @"\WRdata-master";
				Console.WriteLine("Zijn alle files unzipt? Y/N");
				string areUnziptAnswer = Console.ReadLine();
				while (!(areUnziptAnswer.Equals("Y") || areUnziptAnswer.Equals("N")))
				{
					Console.WriteLine("Geef aub een geldig antwoord: Y of N");
					areUnziptAnswer = Console.ReadLine();
				}
				if(areUnziptAnswer.Equals("N"))
				{
					UnzipFiles(path);
				}

				List<Provincie> provincies = Tools.MaakProvincies(path);
			}
			catch (FileNotFoundException fnf)
			{
				Console.Clear();
				Console.WriteLine("******************************");
				Console.WriteLine(fnf.GetType());
				Console.WriteLine("Een van de WRdata-Master files onbreekt.(Misschien is een van de files nog gezipt)");
				Console.WriteLine("******************************");

			}
			catch (DirectoryNotFoundException dnf)
			{
				Console.Clear();
				Console.WriteLine("******************************");
				Console.WriteLine(dnf.GetType());
				Console.WriteLine("Geef een geldig pad op");
				Console.WriteLine("******************************");

			}

        }

		private static void UnzipFiles(string path)
		{
			var directory = new DirectoryInfo(path);
			foreach (var file in directory.GetFiles())
			{
				if(file.Extension.Equals(".zip"))
				{
					
					ZipFile.ExtractToDirectory(file.FullName, path);
				}
			}

		}
	}
}
