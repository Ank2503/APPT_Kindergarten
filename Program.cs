using System;
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
        
        private static Directory ParseTree()
        {
            throw new NotImplementedException("This method must be implemented as a home task");
        }

        private static string[] GetAllFilesWithExtensionRecursively(Directory directory, string extension)
        {
            throw new NotImplementedException("This method must be implemented as a home task");
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
