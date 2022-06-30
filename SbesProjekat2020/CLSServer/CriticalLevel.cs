using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLSServer
{
    public class CriticalLevel
    {
        public string Type { get; set; }
        public DateTime DatumIVreme { get; set; }
        public string User { get; set; }

        public CriticalLevel()
        {

        }

        public CriticalLevel(string typ, DateTime dat, string user)
        {
            Type = typ;
            DatumIVreme = dat;
            User = user;
        }
    }
}
