// -----------------------------------------------------------
// Advent of Code 2024 - Utility Functions
// Common helper functions for solving AoC puzzles
// -----------------------------------------------------------

namespace Advent2024.Helpers
{
    public class FileHelper
    {
        private readonly string _baseInputPath;

        public FileHelper(string? baseInputPath = null)
        {
            // If no path specified, use the executing assembly's directory
            _baseInputPath = baseInputPath ?? AppDomain.CurrentDomain.BaseDirectory;
            if (!Directory.Exists(_baseInputPath))
            {
                Directory.CreateDirectory(_baseInputPath);
            }
        }

        private string ResolvePath(string fileName)
        {
            // Try multiple possible locations
            string[] possiblePaths =
            [
                Path.Combine(_baseInputPath, fileName),                    // Direct in base path
                Path.Combine(_baseInputPath, "Inputs", fileName),          // In Inputs subfolder
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName), // In execution directory
                Path.Combine(Directory.GetCurrentDirectory(), fileName),    // In current directory
                Path.Combine("..", "..", "..", fileName)                   // Up three levels (usually source directory)
            ];
            string? foundPath = possiblePaths.FirstOrDefault(File.Exists);
            if (foundPath == null)
            {
                throw new FileNotFoundException(
                    $"Input file '{fileName}' not found. Searched in:\n" +
                    string.Join("\n", possiblePaths));
            }
            return foundPath;
        }

        public string[] ReadAllLines(string fileName)
        {
            try
            {
                string filePath = ResolvePath(fileName);
                return File.ReadAllLines(filePath);
            }
            catch (Exception ex) when (ex is not FileNotFoundException)
            {
                throw new IOException($"Error reading file '{fileName}': {ex.Message}", ex);
            }
        }
        
        public static int[] StringToDigits(string input)
        {
            return input.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToArray();
        }

        public IEnumerable<string> ReadLinesLazy(string fileName)
        {
            try
            {
                string filePath = ResolvePath(fileName);
                return File.ReadLines(filePath);
            }
            catch (Exception ex) when (ex is not FileNotFoundException)
            {
                throw new IOException($"Error reading file '{fileName}': {ex.Message}", ex);
            }
        }
        
        public IEnumerable<int> ReadLinesAsIntegers(string fileName) => ReadAllLines(fileName).Select(int.Parse);

        public IEnumerable<double> ReadLinesAsDoubles(string fileName) => ReadAllLines(fileName).Select(double.Parse);

        // Multi-column reading
        public List<T>[] ReadColumns<T>(string fileName, int columnCount, Func<string, T> parser, string separator = " ")
        {
            List<T>[] columns = new List<T>[columnCount];
            for (int i = 0; i < columnCount; i++)
            {
                columns[i] = [];
            }
            string[] lines = ReadAllLines(fileName);
            for (int lineNumber = 0; lineNumber < lines.Length; lineNumber++)
            {
                try
                {
                    string line = lines[lineNumber];
                    string[] parts = line.Split([separator], StringSplitOptions.RemoveEmptyEntries);
                    
                    if (parts.Length < columnCount)
                    {
                        throw new FormatException($"Line {lineNumber + 1} does not contain enough columns");
                    }
                    for (int i = 0; i < columnCount; i++)
                    {
                        try
                        {
                            columns[i].Add(parser(parts[i]));
                        }
                        catch (Exception ex)
                        {
                            throw new FormatException($"Error parsing column {i + 1} on line {lineNumber + 1}: {ex.Message}");
                        }
                    }
                }
                catch (Exception ex) when (ex is not FormatException)
                {
                    throw new FormatException($"Error processing line {lineNumber + 1}: {ex.Message}");
                }
            }
            return columns;
        }

        public (List<T1>, List<T2>) ReadColumns<T1, T2>(string fileName,
            Func<string, T1> parser1,
            Func<string, T2> parser2,
            string separator = " ")
        {
            List<T1> col1 = [];
            List<T2> col2 = [];
            string[] lines = ReadAllLines(fileName);
            for (int lineNumber = 0; lineNumber < lines.Length; lineNumber++)
            {
                try
                {
                    string line = lines[lineNumber];
                    string[] parts = line.Split([separator], StringSplitOptions.RemoveEmptyEntries);
                    
                    if (parts.Length < 2)
                    {
                        throw new FormatException($"Line {lineNumber + 1} does not contain enough columns");
                    }
                    try
                    {
                        col1.Add(parser1(parts[0]));
                    }
                    catch (Exception ex)
                    {
                        throw new FormatException($"Error parsing first column on line {lineNumber + 1}: {ex.Message}");
                    }
                    try
                    {
                        col2.Add(parser2(parts[1]));
                    }
                    catch (Exception ex)
                    {
                        throw new FormatException($"Error parsing second column on line {lineNumber + 1}: {ex.Message}");
                    }
                }
                catch (Exception ex) when (ex is not FormatException)
                {
                    throw new FormatException($"Error processing line {lineNumber + 1}: {ex.Message}");
                }
            }
            return (col1, col2);
        }

        public (List<T1>, List<T2>, List<T3>) ReadColumns<T1, T2, T3>(string fileName,
            Func<string, T1> parser1,
            Func<string, T2> parser2,
            Func<string, T3> parser3,
            string separator = " ")
        {
            List<T1> col1 = [];
            List<T2> col2 = [];
            List<T3> col3 = [];
            
            string[] lines = ReadAllLines(fileName);
            for (int lineNumber = 0; lineNumber < lines.Length; lineNumber++)
            {
                try
                {
                    string line = lines[lineNumber];
                    string[] parts = line.Split([separator], StringSplitOptions.RemoveEmptyEntries);
                    
                    if (parts.Length < 3)
                    {
                        throw new FormatException($"Line {lineNumber + 1} does not contain enough columns");
                    }
                    try
                    {
                        col1.Add(parser1(parts[0]));
                        col2.Add(parser2(parts[1]));
                        col3.Add(parser3(parts[2]));
                    }
                    catch (Exception ex)
                    {
                        throw new FormatException($"Error parsing columns on line {lineNumber + 1}: {ex.Message}");
                    }
                }
                catch (Exception ex) when (ex is not FormatException)
                {
                    throw new FormatException($"Error processing line {lineNumber + 1}: {ex.Message}");
                }
            }
            return (col1, col2, col3);
        }
        
        public char[,] ReadAsCharGrid(string fileName)
        {
            try
            {
                string[] lines = ReadAllLines(fileName);
                if (lines.Length == 0)
                {
                    throw new FormatException("The file is empty");
                }
                int width = lines[0].Length;
                char[,] grid = new char[lines.Length, width];
                for (int i = 0; i < lines.Length; i++)
                {
                    if (lines[i].Length != width)
                    {
                        throw new FormatException($"Line {i + 1} has different length than the first line");
                    }
                    for (int j = 0; j < lines[i].Length; j++)
                    {
                        grid[i, j] = lines[i][j];
                    }
                }
                return grid;
            }
            catch (Exception ex) when (ex is not FormatException && ex is not FileNotFoundException)
            {
                throw new IOException($"Error reading grid from file '{fileName}': {ex.Message}", ex);
            }
        }

        public int[,] ReadAsIntGrid(string fileName, char separator = ' ')
        {
            try
            {
                string[] lines = ReadAllLines(fileName);
                if (lines.Length == 0)
                {
                    throw new FormatException("The file is empty");
                }
                string[] firstLine = lines[0].Split(separator, StringSplitOptions.RemoveEmptyEntries);
                int width = firstLine.Length;
                int[,] grid = new int[lines.Length, width];
                for (int i = 0; i < lines.Length; i++)
                {
                    string[] numbers = lines[i].Split(separator, StringSplitOptions.RemoveEmptyEntries);
                    if (numbers.Length != width)
                    {
                        throw new FormatException($"Line {i + 1} has different number of elements than the first line");
                    }
                    for (int j = 0; j < numbers.Length; j++)
                    {
                        try
                        {
                            grid[i, j] = int.Parse(numbers[j]);
                        }
                        catch (Exception ex)
                        {
                            throw new FormatException($"Error parsing number at position ({i + 1}, {j + 1}): {ex.Message}");
                        }
                    }
                }
                return grid;
            }
            catch (Exception ex) when (ex is not FormatException && ex is not FileNotFoundException)
            {
                throw new IOException($"Error reading grid from file '{fileName}': {ex.Message}", ex);
            }
        }
        
        public T ReadJson<T>(string fileName)
        {
            try
            {
                string filePath = ResolvePath(fileName);
                string jsonContent = File.ReadAllText(filePath);
                return System.Text.Json.JsonSerializer.Deserialize<T>(jsonContent)!;
            }
            catch (System.Text.Json.JsonException ex)
            {
                throw new FormatException($"Error deserializing JSON from file '{fileName}': {ex.Message}", ex);
            }
            catch (Exception ex) when (ex is not FileNotFoundException && ex is not FormatException)
            {
                throw new IOException($"Error reading JSON from file '{fileName}': {ex.Message}", ex);
            }
        }
        
        public IEnumerable<string[]> ReadLineGroups(string fileName, string groupSeparator = "")
        {
            List<string> currentGroup = [];
            string[] lines = ReadAllLines(fileName);
            for (int lineNumber = 0; lineNumber < lines.Length; lineNumber++)
            {
                string line = lines[lineNumber];
                if (line == groupSeparator)
                {
                    if (currentGroup.Count <= 0) continue;
                    
                    yield return currentGroup.ToArray();
                    currentGroup.Clear();
                }
                else
                {
                    currentGroup.Add(line);
                }
            }
            if (currentGroup.Count > 0)
            {
                yield return currentGroup.ToArray();
            }
        }
    }
}