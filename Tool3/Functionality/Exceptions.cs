using System;
using System.Collections.Generic;
using System.Text;

namespace Tool3.Functionality
{
    class NotAnIntException : Exception
    {
        public NotAnIntException(string message) : base(message)
        {

        }
    }
}
