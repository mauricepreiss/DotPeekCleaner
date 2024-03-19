# DOTPeekCleaner

DOTPeekCleaner is a tool to clean a by JetBrains's **dotPeek** exported project from all the annoying comments on top of every file. Additional: The tool removes the empty line in every C# file.
Example.:
```
// Decompiled with JetBrains decompiler
// Type: System.Runtime.CompilerServices.RefSafetyRulesAttribute
// Assembly: {YourAssemblyName}, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: {YourMVID}
// Assembly location: {YourAssemblyLocation}
```

## Installation

1. Go to [Releases](https://github.com/mauricepreiss/DotPeekCleaner/releases) and choose the latest version to install. Download the "*DOTPeekCleaner-v{version}*.zip" file to your local computer.
2. Open the Zip file in a program you want ([7Zip](https://www.7-zip.org/), [WinRAR](https://winrar.de/) or the windows internal Zip tool).
3. Extract all files from the Zip to your local program files folder (*C:\Program Files\DOTPeekCleaner*) or any path you want it to be. It works everywhere.
4. Start the *.exe* file and follow the instructions.

## Usage
1. Open the *DotPeekCleaner.exe*. It should look like that:

![UI Image 1](https://raw.githubusercontent.com/mauricepreiss/mauricepreiss/main/dotPeekCleaner-screenshot.png)

2. Paste the path of the folder where the *.csproj* file is located:

![UI Image 1](https://raw.githubusercontent.com/mauricepreiss/mauricepreiss/main/dotPeekCleaner-screenshot2.png)

3. After all files are processed, it should look like this:

![UI Image 1](https://raw.githubusercontent.com/mauricepreiss/mauricepreiss/main/dotPeekCleaner-screenshot3.png)

## Code Example
Here is the internal used code for cleaning the C# project's *.cs* files:

```csharp
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
            count++;
        else
            break; // end loop when non-empty line was found
    }

    Array.Resize(ref lines, lines.Length - count);

    lines = lines.Select(line => line.TrimEnd()).ToArray();
    string text = string.Join(Environment.NewLine, lines);
    File.WriteAllText(path, text);

    path = path.Replace(_fullPath!, "...");
    WriteInfoLine("Processed file: " + path);
}
```

## License

[MIT](https://choosealicense.com/licenses/mit/)