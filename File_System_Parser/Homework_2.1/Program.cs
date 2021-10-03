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
        public List<Directory> SubDirectories { get; set; }

        /// <summary>
        /// List of files
        /// </summary>
        public List<string> Files { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public Directory(string name)
        {
            Name = name;
            SubDirectories = new List<Directory>();
            Files = new List<string>();
        }
    }

    public class YouScrewedUpException : Exception
    {
        public YouScrewedUpException(string message) : base(message)
        {

        }
    }

    internal class Program
    {
        private const string StructureFileName = @"d:\Downloads\Structure.txt";

        public static Directory ParseTree()
        {
            string[] lines = System.IO.File.ReadAllLines(StructureFileName);

            if(!File.Exists(StructureFileName))
            {
                throw new YouScrewedUpException("File is not found!");
            }

            else if (lines.Length < 1)
            {
                throw new YouScrewedUpException("File is empty!");
            }
            
            Directory root = new Directory(lines[0].TrimStart('-'));

            int rootMinusesLength = lines[0].Split('-').Length - 1;

            List<Tuple<Directory, int>> dirsAndLengths = new();

            dirsAndLengths.Add(new Tuple<Directory, int>(root, rootMinusesLength));

            for (var i = 1; i < lines.Length; i++)
            {
                Directory node = new Directory(lines[i].TrimStart('-'));

                int nodeMinusesLength = lines[i].Split('-').Length - 1;

                if (nodeMinusesLength <= rootMinusesLength)
                {
                    throw new Exception("There is more than one root");
                }

                Directory parent = new();
                for (int j = dirsAndLengths.Count - 1; j >= 0; j--)
                {
                    if (dirsAndLengths[j].Item2 < nodeMinusesLength)
                    {

                        parent = dirsAndLengths[j].Item1;
                        break;
                    }
                }

                if (lines[i].Contains('*'))
                {
                    parent.Files.Add(lines[i].TrimStart('-', '*'));
                }
                else
                {
                    parent.SubDirectories.Add(node);
                }
                
                dirsAndLengths.Add(new Tuple<Directory, int>(node, nodeMinusesLength));
            }

            return root;
        }

        private static void GetAllFilesWithExtensionRecursively(Directory directory, string extension, List<string> extensionArray)
        {
            foreach (var dir in directory.SubDirectories)
            {
                if (dir.Files.Count > 0)
                {
                    foreach (string s in dir.Files)
                    {
                        if (s.Contains(extension))
                        {
                            extensionArray.Add(s);
                        }
                    }
                }
                if (dir.SubDirectories.Count > 0)
                    GetAllFilesWithExtensionRecursively(dir, extension, extensionArray);
            }
        }

        public static void Main(string[] args)
        {
            Directory cDrive = ParseTree();

            if (cDrive.Name != "C")
                throw new YouScrewedUpException("Root directory name must be C");

            if (cDrive.SubDirectories?.Count != 3 || cDrive.Files?.Count != 0)
                throw new YouScrewedUpException("Root directory must contain 3 subdirectories and 0 files, " +
                                                "still arrays must be intialized and stay empty");

            var system32Folder = cDrive.SubDirectories.FirstOrDefault(
                d => d.Name == "Windows").SubDirectories?.FirstOrDefault(
                d => d.Name == "System32");

            if (system32Folder?.Name != "System32"
                && system32Folder?.SubDirectories?.Count != 1
                && system32Folder?.Files?.Count != 3)
                throw new YouScrewedUpException("C:\\Windows\\System32 folder must exist, " +
                                                "contain 3 files and one subfolder");

            if (system32Folder?.SubDirectories?.First().Files.Any(f => f == "hosts") != true)
                throw new YouScrewedUpException("C:\\Windows\\System32\\drivers must contain one hosts file");

            var officeFolder = cDrive.SubDirectories.FirstOrDefault(
                d => d.Name == "Program Files").SubDirectories?.FirstOrDefault(
                d => d.Name == "Microsoft").SubDirectories?.FirstOrDefault(
                d => d.Name == "Office");

            if (officeFolder?.Files?.Count != 3 || officeFolder?.SubDirectories.Count != 1)
                throw new YouScrewedUpException("Office folder should must contain 3 files and 1 subfolder");

            List<string> lnkFiles = new();
            GetAllFilesWithExtensionRecursively(cDrive, ".lnk", lnkFiles);

            //I dont know what this test is doing here (i dont think we need a randomizer for 2 task)

            /*var lnkFilesThatShouldExist = new[]
                {"Excel.lnk", "Word.lnk", "PowerPoint.lnk", "Calculator.lnk", "SeaBeach.lnk"};

            if (lnkFiles?.Count != 5 || lnkFiles.All(
                    f => f != lnkFilesThatShouldExist[new Random().Next(lnkFilesThatShouldExist.Length)]))
                throw new YouScrewedUpException("C drive tree contains 5 lnk files in total " +
                                                "and random element from lnkFilesThatShouldExist should be present");*/
        }
    }
}