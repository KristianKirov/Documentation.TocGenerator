using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Documentation.TocGenerator.Toc
{
    public class Section : TocEntryBase
    {
        public string Url { get; set; }

        public override string Serialize(int depth)
        {
            string prefix = new string(' ', depth * 4);
            ++depth;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Format("{0}<Section Title=\"{1}\" URL=\"{2}\">", prefix, this.Title, this.Url));
            foreach (var item in this.Children)
            {
                sb.AppendLine(item.Serialize(depth));
            }
            sb.Append(prefix);
            sb.Append("</Section>");

            return sb.ToString();
        }
    }
}
