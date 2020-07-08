using System;
using System.Collections.Generic;
using System.Text;

namespace Slotlogic.MobileApp.Models.OutputData
{
    public class ModelClub
    {
        public Error ErrorData { get; set; }
        public string ClubName { get; set; }
        public string AboutClub { get; set; }

        public ModelClub() { }

        public ModelClub(Error error)
        {
            ErrorData = error;
            AboutClub = string.Empty;
            ClubName = string.Empty;
        }

        public ModelClub(string clubName, string aboutClub)
        {
            ErrorData = new Error();
            ClubName = clubName;
            AboutClub = aboutClub;
        }
    }
}
