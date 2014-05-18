using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Documentation.TocGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Repo Owner: ");
            string repoOwner = Console.ReadLine();
            Console.WriteLine("Repo Name: ");
            string repoName = Console.ReadLine();
            Console.WriteLine("Repo Branch: ");
            string repoBranch = Console.ReadLine();

            string zipNameWithoutExtension = string.Concat(repoOwner, "-", repoName);

            Console.WriteLine(string.Format("Downloading: {0}/{1}/{2}", repoOwner, repoName, repoBranch));
            GitHubFilesDownloader downloader = new GitHubFilesDownloader(repoOwner, repoName, repoBranch);
            downloader.DownloadRepositoryFiles(zipNameWithoutExtension);
            Console.WriteLine("Repo downloaded");

            FastZip zipper = new FastZip();
            
            string extractDir = Path.Combine(Environment.CurrentDirectory, zipNameWithoutExtension);
            if (Directory.Exists(extractDir))
            {
                Console.WriteLine(string.Format("Deleting: {0}", extractDir));
                Directory.Delete(extractDir, true);
            }

            string zipName = string.Concat(zipNameWithoutExtension, ".zip");
            Console.WriteLine(string.Format("Uzipping {0} in {1}", zipName, extractDir));
            zipper.ExtractZip(zipName, extractDir, null);
            Console.WriteLine("Unzip finished");

            string tocPath = Path.Combine(Environment.CurrentDirectory, "bladetoc.xml");
            Console.WriteLine(string.Format("Start reading metadata and generating TOC in {0}", tocPath));
            Toc.TocGenerator tg = new Toc.TocGenerator();
            tg.GenerateInFile(extractDir, tocPath);
            Console.WriteLine("TOC generation finished!");
        }
    }
}
