using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Documentation.TocGenerator.Toc
{
    public class Article : TocEntryBase
    {
        public string Slug { get; set; }

        public override string Serialize(int depth)
        {
            return string.Format("{0}<Article Title=\"{1}\" Slug=\"{2}\" />", new string(' ', depth * 4), this.Title, this.Slug);
        }
    }
}
