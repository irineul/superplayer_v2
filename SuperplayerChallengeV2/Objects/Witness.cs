using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;

namespace SuperplayerChallenge.Objects
{
    public class Witness
    {
        public Killer Killer { get; set; }

        public int IsHim(int Suspect, int Local, int Gun)
        {
            /* Compare the suspect with the killer and return the answear */
            if (Suspect != Killer.Suspect.Id)
            {
                return 1;
            }

            if (Local != Killer.Local.Id)
            {
                return 2;
            }

            if (Gun != Killer.Gun.Id)
            {
                return 3;
            }

            /* The Suspect is the killer */
            return 0;
        }

    }
}
