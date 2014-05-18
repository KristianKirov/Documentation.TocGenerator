using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Documentation.TocGenerator.Toc
{
    public class Contents : TocEntryBase
    {
        public override string Serialize(int depth)
        {
            ++depth;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<Contents>");
            foreach (var item in this.Children)
            {
                sb.AppendLine(item.Serialize(depth));
            }
            sb.Append("</Contents>");

            return sb.ToString();
        }
    }
}
