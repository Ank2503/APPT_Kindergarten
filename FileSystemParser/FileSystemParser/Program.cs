using System;
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
        private const string StructureFileName = @"c:\Users\IvanBerkut\Documents\Structure.txt";

        private static Directory ParseDirectory(string[] lines, int startPosition = 0, int level = 1)
        {
            var directory = new Directory()
            {
                Name = String.Empty,
                SubDirectories = new Directory[] { },
                Files = new string[] { }
            };

            var trimmed = lines[startPosition].TrimStart('-');

            if (lines[startPosition].Length - trimmed.Length == level && !trimmed.StartsWith('*'))
            {
                directory.Name = trimmed;
                var files = new string[] { };
                var subdirs = new Directory[] { };

                for (var i = startPosition + 1; i < lines.Length; i++)
                {
                    var trimmedSub = lines[i].TrimStart('-');
                    if (lines[i].Length - trimmedSub.Length <= level)
                        break;

                    if (lines[i].Length - trimmedSub.Length == level + 1)
                    {
                        if (trimmedSub.StartsWith('*'))
                        {
                            Array.Resize(ref files, files.Length + 1);
                            files[^1] = trimmedSub.TrimStart('*');
                        }
                        else
                        {
                            Array.Resize(ref subdirs, subdirs.Length + 1);
                            subdirs[^1] = ParseDirectory(lines, i, level + 1);
                        }
                    }
                }
                directory.Files = files;
                directory.SubDirectories = subdirs;
            }

            return directory;
        }

        private static Directory ParseTree()
        {
            if (!File.Exists(StructureFileName))
                throw new YouScrewedUpException("File doesn't exist!");

            string[] lines = File.ReadAllLines(StructureFileName);

            return ParseDirectory(lines);
        }

        private static void ParseFiles(string[] input, ref string[] output, string extension)
        {
            foreach (var f in input)
                if (f.EndsWith(extension))
                {
                    Array.Resize(ref output, output.Length + 1);
                    output[^1] = f;
                }
        }

        private static string[] GetAllFilesWithExtensionRecursively(Directory directory, string extension)
        {
            var files = new string[] { };

            ParseFiles(directory.Files, ref files, extension);

            foreach (var d in directory.SubDirectories)
                ParseFiles(GetAllFilesWithExtensionRecursively(d, extension), ref files, extension);

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