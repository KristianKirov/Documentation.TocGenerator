using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Documentation.TocGenerator.Toc
{
    public class OrdinalParser
    {
        public static readonly Regex OrdinalRegex = new Regex("^(\\d+)\\s*[)-._ ]\\s*");

        public float? GetOrdinalFromString(string s)
        {
            Match match = OrdinalRegex.Match(s);
            if (match.Success && match.Groups.Count > 0)
            {
                Group group = match.Groups[1];
                if (group.Success)
                {
                    string rawCapture = group.Value;
                    float parsedInt;
                    if (float.TryParse(rawCapture, out parsedInt))
                    {
                        return parsedInt;
                    }
                }
            }

            return null;
        }
    }
}
