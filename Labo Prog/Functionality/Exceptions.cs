using System;
using System.Collections.Generic;
using System.Text;

namespace Labo_Prog
{
    class IdException : Exception
    {
        public IdException(string message) : base(message)
        {

        }
    }
    
    class CoordinateException : Exception
    {
        public CoordinateException(string message) : base(message)
        {

        }
    }
}
