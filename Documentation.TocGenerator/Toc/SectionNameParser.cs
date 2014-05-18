using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Documentation.TocGenerator.Toc
{
    public class SectionNameParser : OrdinalParser
    {
        public string GetSectionNameWithoutOrdinal(string sectionName)
        {
            return OrdinalRegex.Replace(sectionName, "");
        }
    }
}
