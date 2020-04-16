using System;
using Tool1.Functionality;
using Tool2.Functionality;
using Tool3.Functionality;



namespace WrDataProgram
{
    class Program
    {
        static void Main(string[] args)
        {
            bool quitProgram = false;
            while(!quitProgram)
            {
                Console.Clear();
                Console.WriteLine("Welke tool wilt u gebruiken? (standaart is tool 3)");
                bool inputTrue = int.TryParse(Console.ReadLine(), out int inputToolNumber);
                while(!inputTrue)
                {
                    Console.WriteLine("Geef aub een getal van het type int in");
                    inputTrue = int.TryParse(Console.ReadLine(), out inputToolNumber);
                }
                switch (inputToolNumber)
                {
                    case 1:
                        ControllerTool1.RunTool1();
                        break;
                    case 2:
                        ControllerTool2.RunTool2();
                        break;
                    default:
                        ControllerTool3.RunTool3();
                        break;

                }

                Console.WriteLine("Wilt u het programma sluiten? Y/N");
                string input = Console.ReadLine();
                while (!(input.Equals("Y") || input.Equals("N")))
                {
                    Console.WriteLine("Geef aub een geldig antwoord: Y of N");
                    input = Console.ReadLine();
                }
                if (input.Equals("Y"))
                {
                    quitProgram = true;
                }

            }
            
        }
    }
}
