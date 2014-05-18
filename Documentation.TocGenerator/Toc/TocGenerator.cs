using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Humanizer;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Documentation.TocGenerator.Toc
{
    public class TocGenerator
    {
        public void GenerateInFile(string rootDirectory, string outputFile)
        {
            DirectoryInfo unzipedDir = new DirectoryInfo(rootDirectory);
            DirectoryInfo repoRootDir = unzipedDir.EnumerateDirectories().First();
            Contents tocRoot = new Contents();
            this.GenerateToc(repoRootDir, tocRoot);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            sb.Append(tocRoot.Serialize(0));

            File.WriteAllText(outputFile, sb.ToString());
        }

        private void GenerateToc(DirectoryInfo directoryInfo, TocEntryBase parentItem)
        {
            FileInfo[] mdFiles = directoryInfo.GetFiles("*.md", SearchOption.TopDirectoryOnly);
            foreach (FileInfo mdFile in mdFiles)
            {
                Article article = this.CreateArticle(mdFile);
                if (article != null && !string.IsNullOrEmpty(article.Slug))
                {
                    parentItem.Children.Add(article);
                }
            }

            DirectoryInfo[] childDirectories = directoryInfo.GetDirectories("*", SearchOption.TopDirectoryOnly);
            foreach (DirectoryInfo childDirecotry in childDirectories)
            {
                Section section = this.GenerateSection(childDirecotry);
                this.GenerateToc(childDirecotry, section);
                if (section.Children.Count > 0)
                {
                    parentItem.Children.Add(section);
                }
            }

            parentItem.Children.Sort((first, secon) =>
                {
                    if (first.Ordinal < secon.Ordinal)
                    {
                        return -1;
                    }

                    if (first.Ordinal > secon.Ordinal)
                    {
                        return 1;
                    }

                    return first.Title.CompareTo(secon.Title);
                });
        }

        private Section GenerateSection(DirectoryInfo directoryInfo)
        {
            Section section = new Section();
            
            SectionNameParser sectionNameParser = new SectionNameParser();
            string sectionNameWithoutOrdinal = sectionNameParser.GetSectionNameWithoutOrdinal(directoryInfo.Name);
            section.Title = sectionNameWithoutOrdinal.Replace('-', ' ').Humanize(LetterCasing.Title);
            float? ordinal = sectionNameParser.GetOrdinalFromString(directoryInfo.Name.ToLower());
            if (ordinal != null)
            {
                section.Ordinal = ordinal.Value;
            }

            section.Url = Regex.Replace(sectionNameWithoutOrdinal.ToLowerInvariant(), @"[^\w\-\!\$\'\(\)\=\@\d_]+", "-");

            return section;
        }

        private Article CreateArticle(FileInfo mdFile)
        {
            string fileName = mdFile.Name;
            if (fileName.Equals("readme.md", StringComparison.InvariantCultureIgnoreCase))
            {
                return null;
            }

            MarkdownFileMetadata metadata = this.GetMetadata(mdFile);
            if (metadata == null || !metadata.Publish)
            {
                return null;
            }

            ArticleFilenameParser fileNameParser = new ArticleFilenameParser();
            string title = metadata.Title;
            if (string.IsNullOrEmpty(title))
            {
                title = fileNameParser.GetArticleFilenameWithoutExtension(fileName).Replace('-', ' ').Humanize(LetterCasing.Title);
            }

            float? ordinal = fileNameParser.GetOrdinalFromString(fileName.ToLowerInvariant());
            if (ordinal == null && metadata.Ordinal != 0)
            {
                ordinal = metadata.Ordinal;
            }

            Article newArticle = new Article();
            newArticle.Title = title;
            newArticle.Slug = metadata.Slug;
            if (ordinal != null)
            {
                newArticle.Ordinal = ordinal.Value;
            }

            return newArticle;
        }

        private MarkdownFileMetadata GetMetadata(FileInfo mdFile)
        {
            var reader = mdFile.OpenText();

            MarkdownFileMetadata markdownFileMetadata = ExtractMetadata(reader);

            return markdownFileMetadata;
        }

        private MarkdownFileMetadata ExtractMetadata(StreamReader reader)
        {
            string firstLine = reader.ReadLine();
            if (!(firstLine.StartsWith("-") || firstLine.StartsWith("<!--"))) //there is no metadata
            {
                return null;
            }

            MarkdownFileMetadata metadata = new MarkdownFileMetadata()
            {
                Publish = true
            };

            while (reader.Peek() != '-')
            {
                string line = reader.ReadLine();
                if (string.IsNullOrWhiteSpace(line))
                {
                    if (reader.Peek() == -1)
                    {
                        throw new Exception("MalformedMetadata");
                    }

                    continue;
                }

                var check = line.ToLowerInvariant();

                if (check.StartsWith("title"))
                {
                    // extract title from line
                    string title = GetValueFromLine(line);
                    metadata.Title = title;
                }
                else if (check.StartsWith("publish"))
                {
                    metadata.Publish = check.Contains("true");
                }
                else if (check.StartsWith("slug"))
                {
                    var slug = GetValueFromLine(line);
                    if (!string.IsNullOrEmpty(slug))
                    {
                        metadata.Slug = slug;
                    }
                }
                else if (check.StartsWith("ordinal"))
                {
                    var rawOrdinal = GetValueFromLine(line);
                    float ordinal;
                    if (float.TryParse(rawOrdinal, NumberStyles.Any, new CultureInfo("en-US"), out ordinal))
                    {
                        metadata.Ordinal = ordinal;
                    }
                }
            }

            return metadata;
        }

        private static string GetValueFromLine(string line)
        {
            var index = line.IndexOf(":");
            return line.Substring(index + 1).Trim();
        }
    }
}
