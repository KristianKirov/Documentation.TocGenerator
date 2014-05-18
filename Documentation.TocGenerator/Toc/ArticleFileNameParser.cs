using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Documentation.TocGenerator.Toc
{
    public class ArticleFilenameParser : OrdinalParser
    {
        private const string MARKDOWN_FILE_EXTENSION = ".md";

        public string GetArticleFilenameWithoutExtension(string articleName)
        {
            string articleFilenameWithoutExtension = articleName.Replace(MARKDOWN_FILE_EXTENSION, "");

            articleFilenameWithoutExtension = OrdinalRegex.Replace(articleFilenameWithoutExtension, "", 1);

            return articleFilenameWithoutExtension;
        }
    }
}
