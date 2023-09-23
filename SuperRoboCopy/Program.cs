using System;
using System.Diagnostics;

namespace SuperRoboCopy
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Enter source path: ");
            string sourcePath = Console.ReadLine();

            Console.Write("Enter destination path: ");
            string destPath = Console.ReadLine();

            // Start three robocopy processes for small, medium, and large files
            StartRobocopyProcess(sourcePath, destPath, "Small");
            StartRobocopyProcess(sourcePath, destPath, "Medium");
            StartRobocopyProcess(sourcePath, destPath, "Large");

            Console.WriteLine("Robocopy processes started. Press any key to exit...");
            Console.ReadKey();
        }

        static void StartRobocopyProcess(string source, string dest, string sizeCategory)
        {
            string maxFileSize = "";
            string minFileSize = "";
            switch (sizeCategory)
            {
                case "Small":
                    maxFileSize = "10M";
                    break;
                case "Medium":
                    minFileSize = "10M";
                    maxFileSize = "500M";
                    break;
                case "Large":
                    minFileSize = "500M"; // start from just above the "Medium" category's limit
                    break;
            }

            string robocopyArgs = $"/B \"{source}\" \"{dest}\" /MIR /EFSRAW /R:1 /W:1 /SEC";
            if (!string.IsNullOrEmpty(maxFileSize))
            {
                robocopyArgs += $" /MAX:{maxFileSize}";
            }
            if (!string.IsNullOrEmpty(minFileSize))
            {
                robocopyArgs += $" /MIN:{minFileSize}";
            }

            // Setting the window title to indicate size category
            string titleCommand = $"title Robocopy {sizeCategory} Files Window";

            // Using cmd to set title, run robocopy and then pause
            string args = $"/C {titleCommand} & robocopy {robocopyArgs} & pause";

            Process process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = args,
                    UseShellExecute = true,
                    CreateNoWindow = false,
                }
            };

            process.Start();
        }
    }
}
