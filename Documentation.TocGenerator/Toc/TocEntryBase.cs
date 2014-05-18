using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Documentation.TocGenerator.Toc
{
    public abstract class TocEntryBase
    {
        public string Title { get; set; }
        public List<TocEntryBase> Children { get; set; }
        public float Ordinal { get; set; }

        public abstract string Serialize(int depth);

        public TocEntryBase()
        {
            this.Children = new List<TocEntryBase>();
            this.Ordinal = 10000;
        }
    }
}
