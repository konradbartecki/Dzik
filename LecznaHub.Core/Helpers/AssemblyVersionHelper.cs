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
        /// <returns></returns>
        public static string GetAssemblyVersion(object o)
        {
            return o.GetType().GetTypeInfo().Assembly.GetName().Version.ToString(4);
        }
    }
}
