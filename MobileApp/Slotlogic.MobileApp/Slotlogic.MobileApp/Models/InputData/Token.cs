using System;
using System.Collections.Generic;
using System.Text;

namespace Slotlogic.MobileApp.Models.InputData
{
    public class Token
    {
        public DateTime DateTime { get; set; }
        public string Message { get; set; }
        public Exception Exc { get; set; }

        public Token(string message, Exception exc)
        {
            DateTime = DateTime.Now;
            Message = message;
            Exc = exc;
        }

        public Token(string message)
        {
            DateTime = DateTime.Now;
            Message = message;
            Exc = new Exception();
        }
    }
}
