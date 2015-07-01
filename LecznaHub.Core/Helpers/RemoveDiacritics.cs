using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LecznaHub.Core.Helpers
{
    public static class Diacritics
    {
        public static string Remove(string text)
        {
            StringBuilder builder = new StringBuilder();
            foreach (char c in text)
            {
                builder.Append(NormaliseLWithStroke(c));
            }

            return builder.ToString();
        }

        private static char NormaliseLWithStroke(char c)
        {
            c = Char.ToLower(c);
            switch (c)
            {
                case 'ą':
                    return 'a';
                case 'ć':
                    return 'c';
                case 'ę':
                    return 'e';
                case 'ł':
                    return 'l';
                case 'ń':
                    return 'n';
                case 'ó':
                    return 'o';
                case 'ś':
                    return 's';
                case 'ź':
                case 'ż':
                    return 'z';
                default:
                    return c;
            }
        }
    }
}
