﻿using System;
using System.Collections.Generic;

namespace Labo_Prog
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<int, List<Segment>> segmentenPerStraat = Parser.ParseSegment(@"E:\School\Hogent\prog 3\Labo\WRdata-master\WRdata", "WRdata", "csv", ';');

            List<Straat> straten = Tools.MaakStraten(segmentenPerStraat);

            foreach(Straat straat in straten)
            {
                straat.ShowStraat();
            }

        }
    }
}
