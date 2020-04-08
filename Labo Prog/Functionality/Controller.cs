using System;
using System.Collections.Generic;
using System.IO;
using Objects;

namespace Tool1
{
    class Controller
    {

        public static void RunTool1()
        {
            bool endProgram = false;
            while (endProgram == false)
            {
                try
                {
                    List<Provincie> provincies = SetupProvincies();
                    Output.MakeOutputFiles(provincies);

                    Console.Clear();
                    Console.WriteLine("****************************************");
                    Console.WriteLine("Programma Klaar: Het mag gesloten worden.");
                    Console.WriteLine("Alle output folders zijn in de locale documents foldre geplaatst, onder WRData-Output");
                    Console.WriteLine("****************************************");
                    endProgram = true;
                }
                catch (FileNotFoundException fnf)
                {
                    Console.Clear();
                    Console.WriteLine("******************************");
                    Console.WriteLine(fnf.GetType());
                    Console.WriteLine("Een van de WRdata-Master files onbreekt.(Misschien is een van de files nog gezipt)");
                    Console.WriteLine("******************************");

                    Console.WriteLine("Als dit probleem blijft voorkomen, geef X in om het programma te sluiten. Kijk of alle files aanwezig zijn");
                    if (Console.ReadLine() == "X")
                    {
                        endProgram = true;
                    }

                }
                catch (DirectoryNotFoundException dnf)
                {
                    Console.Clear();
                    Console.WriteLine("******************************");
                    Console.WriteLine(dnf.GetType());
                    Console.WriteLine("Geef een geldig pad op dat de WRdata-Master folder bevat");
                    Console.WriteLine("******************************");



                }
                catch (IdException ie)
                {
                    Console.Clear();
                    Console.WriteLine("******************************");
                    Console.WriteLine(ie.GetType());
                    Console.WriteLine(ie.Message);
                    Console.WriteLine("******************************");


                    Console.WriteLine("Dit probleem kan niet door het programma opgelost worden. Sluit het programma af door X in te vullen");
                    if (Console.ReadLine() == "X")
                    {
                        endProgram = true;
                    }

                }
                catch (CoordinateException ce)
                {
                    Console.Clear();
                    Console.WriteLine("******************************");
                    Console.WriteLine(ce.GetType());
                    Console.WriteLine(ce.Message);
                    Console.WriteLine("******************************");


                    Console.WriteLine("Dit probleem kan niet door het programma opgelost worden. Sluit het programma af door X in te vullen");
                    if (Console.ReadLine() == "X")
                    {
                        endProgram = true;
                    }
                }
                catch (Exception e)
                {
                    Console.Clear();
                    Console.WriteLine("******************************");
                    Console.WriteLine(e.GetType());
                    Console.WriteLine(e.Message);
                    Console.WriteLine("******************************");

                    Console.WriteLine("Dit was een onverwachte fout gelieve het programma af te sluiten door X in te vullen");
                    if (Console.ReadLine() == "X")
                    {
                        endProgram = true;
                    }
                }
            }

        }
        private static List<Provincie> SetupProvincies()
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
            if (areUnziptAnswer.Equals("N"))
            {
                Tools.UnzipFiles(path);
            }

            List<Provincie> provincies = Tools.MaakProvincies(path);
            return provincies;
        }


    }
}
