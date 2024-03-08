using System.Diagnostics;
using System.Reflection;

namespace DotPeekCleaner
{
    internal class Program
    {
        private static string? _fullPath;

        static void Main()
        {
            Console.BackgroundColor = ConsoleColor.Black;
            WriteHeader();
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("\n Please insert the folder path of your C# project: ");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
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

            _fullPath = path;

            ProcessFiles(path);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(" Finished.");
            Console.ReadKey();
            Process.Start("explorer.exe", path);
        }

        static void WriteHeader()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            string text =
                "\r\n ██████╗░░█████╗░████████╗██████╗░███████╗███████╗██╗░░██╗   ░█████╗░██╗░░░░░███████╗░█████╗░███╗░░██╗███████╗██████╗░" +
                "\r\n ██╔══██╗██╔══██╗╚══██╔══╝██╔══██╗██╔════╝██╔════╝██║░██╔╝   ██╔══██╗██║░░░░░██╔════╝██╔══██╗████╗░██║██╔════╝██╔══██╗" +
                "\r\n ██║░░██║██║░░██║░░░██║░░░██████╔╝█████╗░░█████╗░░█████═╝░   ██║░░╚═╝██║░░░░░█████╗░░███████║██╔██╗██║█████╗░░██████╔╝" +
                "\r\n ██║░░██║██║░░██║░░░██║░░░██╔═══╝░██╔══╝░░██╔══╝░░██╔═██╗░   ██║░░██╗██║░░░░░██╔══╝░░██╔══██║██║╚████║██╔══╝░░██╔══██╗" +
                "\r\n ██████╔╝╚█████╔╝░░░██║░░░██║░░░░░███████╗███████╗██║░╚██╗   ╚█████╔╝███████╗███████╗██║░░██║██║░╚███║███████╗██║░░██║" +
                "\r\n ╚═════╝░░╚════╝░░░░╚═╝░░░╚═╝░░░░░╚══════╝╚══════╝╚═╝░░╚═╝   ░╚════╝░╚══════╝╚══════╝╚═╝░░╚═╝╚═╝░░╚══╝╚══════╝╚═╝░░╚═╝";

            Console.WriteLine(text);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\n Author: Maurice Preiß");
            Console.WriteLine(" Version: " + Assembly.GetExecutingAssembly().GetName().Version);
            Console.Write(" Github-Repo: ");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("https://www.github.com/mauricepreiss/DotPeekCleaner");
            Console.ResetColor();
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
            string[] lines = File.ReadAllLines(path)
                .Select(line => line.TrimEnd()) // removes trailing whitespaces
                .SkipWhile(line => line.TrimStart().StartsWith("//") || string.IsNullOrWhiteSpace(line))
                .ToArray();

            int count = 0;
            for (int i = lines.Length - 1; i >= 0; i--)
            {
                if (string.IsNullOrWhiteSpace(lines[i]))
                {
                    count++;
                }
                else
                {
                    break; // end loop when non-empty line was found
                }
            }

            Array.Resize(ref lines, lines.Length - count);

            lines = lines.Select(line => line.TrimEnd()).ToArray();
            string text = string.Join(Environment.NewLine, lines);
            File.WriteAllText(path, text);

            path = path.Replace(_fullPath!, "...");
            WriteInfoLine("Processed file: " + path);
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
            Console.WriteLine(" " + consoleMessage);
            Console.ForegroundColor = colorBefore;
            Console.ReadKey();
        }

        static void WriteInfoLine(string message)
        {
            var colorBefore = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(" [INFO]: " + message);
            Console.ForegroundColor = colorBefore;
        }
    }
}