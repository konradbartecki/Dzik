using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenLeczna.DTOs
{
    public class DepartureDTO
    {
        public string Time { get; set; }
        public bool IsBetterBusAvailable { get; set; }

        public override string ToString()
        {
            return Time;
        }
    }

    public class SpecialDepartureDTO : DepartureDTO
    {
        public virtual List<DateTime> ApplicableDateTimes { get; set; }
    }
}
