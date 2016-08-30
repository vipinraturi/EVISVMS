using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evis.VMS.Utilities
{
    public class Utility
    {
        public static string TimeSince(TimeSpan timeSince)
        {
            if (timeSince.Hours > 0)
                return string.Format("Visited {0} hours ago", timeSince.Hours);
            else if (timeSince.Minutes > 0)
                return string.Format("Visited {0} minutes ago", timeSince.Minutes);
            else
                return string.Format("Visited {0} seconds ago", timeSince.Seconds);
        }

    }
}
