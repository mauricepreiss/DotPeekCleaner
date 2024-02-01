using System.Diagnostics;

namespace DotPeekCleaner
{
    internal class Program
    {
        static void Main()
        {
            Console.Write("Please insert the folder path of your C# project: ");
            string path = Console.ReadLine()!;
            if (string.IsNullOrWhiteSpace(path))
            {
                WriteErrorLine("Path is empty. Type in a valid path.");
                Reset();
                return;
            }

            if (!Directory.Exists(path))
            {
                WriteErrorLine("Path is does not exist.");
                Reset();
                return;
            }

            //if (Directory.GetFiles(path, "*.sln", SearchOption.TopDirectoryOnly).Length <= 0)
            //{
            //    WriteErrorLine("This is not a valid C# project folder.");
            //    Reset();
            //    return;
            //}

            ProcessFiles(path);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Finished.");
            Console.ReadKey();
            Process.Start("explorer.exe", path);
        }

        static void Reset()
        {
            Console.Clear();
            Main();
        }

        static void ProcessFiles(string targetDirectory)
        {
            string[] fileEntries = Directory.GetFiles(targetDirectory, "*.cs", SearchOption.AllDirectories);
            foreach (string fileName in fileEntries)
            {
                ProcessFile(fileName);
            }

            string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
            foreach (string subdirectory in subdirectoryEntries)
            {
                ProcessFiles(subdirectory);
            }
        }

        static void ProcessFile(string path)
        {
            string[] lines = File.ReadAllLines(path).SkipWhile(line => line.TrimStart().StartsWith("//") || string.IsNullOrWhiteSpace(line)).ToArray();

            //if (lines.Length > 0 && string.IsNullOrWhiteSpace(lines[^1]))
            //{
            //    lines = lines.Take(lines.Length - 1).ToArray();
            //}

            int count = 0;
            for (int i = lines.Length -1; i >= 0; i--)
            {
                if (string.IsNullOrWhiteSpace(lines[i]))
                {
                    count++;
                }
            }
            Array.Resize(ref lines, lines.Length - count);

            File.WriteAllLines(path, lines);
        }

        static void WriteErrorLine(string message, bool writeErrorPreMessage = false)
        {
            var colorBefore = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            string consoleMessage = "";
            if (writeErrorPreMessage)
            {
                consoleMessage += "[ERROR]: ";
            }

            consoleMessage += message;
            Console.WriteLine(consoleMessage);
            Console.ForegroundColor = colorBefore;
            Console.ReadKey();
        }
    }
}