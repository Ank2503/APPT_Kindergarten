using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FileSystemParser
{
    internal struct Directory
    {
        /// <summary>
        /// Name of a directory
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// List of subdirectories
        /// </summary>
        public Directory[] SubDirectories { get; set; }
        
        /// <summary>
        /// List of files
        /// </summary>
        public string[] Files { get; set; }
    }

    public class YouScrewedUpException : Exception
    {
        public YouScrewedUpException(string message) : base(message)
        {
            
        }
    }
    
    internal class Program
    {
        private const string StructureFileName = "Structure.txt";

        private static Directory ParseDirectory(string[] content, int startPosition = 0, int level = 1)
        {
            var directory = new Directory()
            {
                Name = string.Empty,
                SubDirectories = Array.Empty<Directory>(),
                Files = Array.Empty<string>()
            };

            var trimmed = content[startPosition].TrimStart('-');

            if (content[startPosition].Length - trimmed.Length == level && !trimmed.StartsWith('*'))
            {
                directory.Name = trimmed;
                Queue<Directory> subdirsQueue = new();
                Queue<string> filesQueue = new();

                for (var i = startPosition + 1; i < content.Length; i++)
                {
                    var trimmedSub = content[i].TrimStart('-');
                    if (content[i].Length - trimmedSub.Length <= level)
                        break;

                    if (content[i].Length - trimmedSub.Length == level + 1)
                    {
                        if (trimmedSub.StartsWith('*'))
                        {

                            filesQueue.Enqueue(trimmedSub.TrimStart('*'));
                        }
                        else
                        {
                            subdirsQueue.Enqueue(ParseDirectory(content, i, level + 1));
                        }
                    }
                }
                directory.Files = filesQueue.ToArray(); ;
                directory.SubDirectories = subdirsQueue.ToArray(); ;

                subdirsQueue.Clear();
                filesQueue.Clear();
            }

            return directory;
        }

        private static Directory ParseTree()
        {
            if (!File.Exists(StructureFileName))
                throw new YouScrewedUpException("File does not exist!");

            string[] content = File.ReadAllLines(StructureFileName);

            if (content.Length < 1)
                throw new YouScrewedUpException("File is empty!");

            return ParseDirectory(content);
        }

        private static void SortFiles(string[] input, ref string[] output, string extension)
        {
            Queue<string> sortedFiles = new();

            foreach (var f in input) 
            { 
                if (f.EndsWith(extension))
                {
                    sortedFiles.Enqueue(f);
                }
            }

            output = sortedFiles.ToArray();
            sortedFiles.Clear();
        }

        private static string[] GetAllFilesWithExtensionRecursively(Directory directory, string extension)
        {
            var files = Array.Empty<string>();

            SortFiles(directory.Files, ref files, extension);

            foreach (var d in directory.SubDirectories)
                SortFiles(GetAllFilesWithExtensionRecursively(d, extension), ref files, extension);

            return files;
        }
        
        public static void Main(string[] args)
        {
            var cDrive = ParseTree();
           
            if (cDrive.Name != "C")
                throw new YouScrewedUpException("Root directory name must be C");
            
            if (cDrive.SubDirectories?.Length != 3 || cDrive.Files?.Length != 0)
                throw new YouScrewedUpException("Root directory must contain 3 subdirectories and 0 files, " +
                                                "still arrays must be intialized and stay empty");

            var system32Folder = cDrive.SubDirectories.FirstOrDefault(
                d => d.Name == "Windows").SubDirectories?.FirstOrDefault(
                d => d.Name == "System32");
            
            if (system32Folder?.Name != "System32" 
                && system32Folder?.SubDirectories?.Length != 1
                && system32Folder?.Files?.Length != 3)
                throw new YouScrewedUpException("C:\\Windows\\System32 folder must exist, " +
                                                "contain 3 files and one subfolder");
            
            if (system32Folder?.SubDirectories?.First().Files.Any(f => f == "hosts") != true)
                throw new YouScrewedUpException("C:\\Windows\\System32\\drivers must contain one hosts file");

            var officeFolder = cDrive.SubDirectories.FirstOrDefault(
                d => d.Name == "Program Files").SubDirectories?.FirstOrDefault(
                d => d.Name == "Microsoft").SubDirectories?.FirstOrDefault(
                d => d.Name == "Office");
           
            if (officeFolder?.Files?.Length != 3 || officeFolder?.SubDirectories.Length != 1)
                throw new YouScrewedUpException("Office folder should must contain 3 files and 1 subfolder");

            var lnkFiles = GetAllFilesWithExtensionRecursively(cDrive, ".lnk");
            
            var lnkFilesThatShouldExist = new[]
                {"Excel.lnk", "Word.lnk", "PowerPoint.lnk", "Calculator.lnk", "SeaBeach.lnk"};

            if (lnkFiles?.Length != 5 || lnkFiles.All(
                    f => f != lnkFilesThatShouldExist[new Random().Next(lnkFilesThatShouldExist.Length)]))
                throw new YouScrewedUpException("C drive tree contains 5 lnk files in total " +
                                                "and random element from lnkFilesThatShouldExist should be present");
        }
    }
}
