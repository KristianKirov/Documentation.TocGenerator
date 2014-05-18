using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Documentation.TocGenerator.Toc
{
    public class MarkdownFileMetadata
    {
        public string Title { get; set; }

        public string Slug { get; set; }
        
        public bool Publish { get; set; }

        public float Ordinal { get; set; }
    }
}
