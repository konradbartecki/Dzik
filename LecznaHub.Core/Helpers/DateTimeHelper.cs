using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LecznaHub.Core.Annotations;

namespace LecznaHub.Core.Helpers
{
    public static class DateTimeHelper
    {
        public enum DateTimeCompared
        {
            FirstParamIsEarlier = -1,
            ParamsAreEqual = 0,
            SecondParamIsEarlier = 1
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stringTime">Time in string for ex. "12:30"</param>
        /// <param name="time">Time in DateTime</param>
        /// <returns>
        /// Returns -1 if stringTime is earlier than provided DateTime
        /// Returns 0 if stringTime is the same time as provided DateTime
        /// Returns 1 if stringTime is later than provided DateTime
        /// </returns>
        public static DateTimeCompared CompareStringAndDateTime([NotNull] string stringTime, DateTime time)
        {
            if (string.IsNullOrWhiteSpace(stringTime)) throw new ArgumentNullException(nameof(stringTime));
            if (time == null) throw new ArgumentException(nameof(time));

            DateTime DateTimeFromString;
            if (!DateTime.TryParse(stringTime, out DateTimeFromString))
            {
                throw new ArgumentException("DateTime provided in string is invalid: {0}", nameof(stringTime));
            }

            return (DateTimeCompared) DateTime.Compare(DateTimeFromString, time);
        }
    }
}
