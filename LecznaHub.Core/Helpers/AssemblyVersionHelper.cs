using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LecznaHub.Core.Helpers
{
    public static class AssemblyVersionHelper
    {
        /// <summary>
        /// Put 'this' object
        /// </summary>
        /// <returns>Format: 1.1.5579.28304</returns>
        public static string GetAssemblyVersion(object o)
        {
            return o.GetType().GetTypeInfo().Assembly.GetName().Version.ToString(4);
        }

        public static string GetAssemblyVersion(object o, int numbersToString)
        {
            return o.GetType().GetTypeInfo().Assembly.GetName().Version.ToString(numbersToString);
        }

        /// <summary>
        /// Put 'this' object
        /// </summary>
        /// <returns>Build number for example: 5579</returns>
        public static int GetBuildNumber(object o)
        {
            return o.GetType().GetTypeInfo().Assembly.GetName().Version.Build;
        }

        /// <summary>
        /// Put 'this' object
        /// </summary>
        /// <returns>For example "Version: 1.1 Build 5579</returns>
        public static string GetFullVersionString(object o)
        {
            var version = GetAssemblyVersion(o, 2);
            var build = GetBuildNumber(o);
            return String.Format("{0} Build {1}", version, build);
        }
    }
}
