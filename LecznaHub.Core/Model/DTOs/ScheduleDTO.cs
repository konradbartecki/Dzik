using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenLeczna.DTOs
{
    public class ScheduleDTO
    {
        //public string Station { get; set; }
        public string DestinationCity { get; set; }
        public string Carrier { get; set; }
        /// <summary>
        /// Enum casted to string
        /// Possible values:
        /// "Weekdays", "Saturdays", "Sundays", "Special"
        /// </summary>
        public string ApplicableDays { get; set; }
        
    }

    public class ScheduleDetailsDTO : ScheduleDTO
    {
        public ICollection<DepartureDTO> Departures { get; set; }
    }
}
