using System.Text.RegularExpressions;

namespace Advent2024.Days;

public class Day3() : BaseDaySolver(3)
{
    private const string MulPattern = @"mul\((\d+),(\d+)\)";
    private const string DoPattern = @"do\(\)";
    private const string DontPattern = @"don't\(\)";

    public override object SolvePart1()
    {
        string[] lines = FileHelper.ReadAllLines(GetInputFileName());
        int total = 0;
        foreach (string line in lines)
        {
            MatchCollection matches = Regex.Matches(line, MulPattern);
            foreach (Match match in matches)
            {
                int value1 = int.Parse(match.Groups[1].Value);
                int value2 = int.Parse(match.Groups[2].Value);
                total += value1 * value2;
            }
        }
        return total;
    }

    public override object SolvePart2()
    {
        string[] lines = FileHelper.ReadAllLines(GetInputFileName());
        long total = 0;
        bool enabled = true;
        foreach (string line in lines)
        {
            // Find all instructions in order of appearance
            List<(int Index, string Type, Match Match)> matches = [];
            // Find mul instructions
            foreach (Match match in Regex.Matches(line, MulPattern))
            {
                matches.Add((match.Index, "mul", match));
            }
            // Find do() instructions
            foreach (Match match in Regex.Matches(line, DoPattern))
            {
                matches.Add((match.Index, "do", match));
            }
            // Find don't() instructions
            foreach (Match match in Regex.Matches(line, DontPattern))
            {
                matches.Add((match.Index, "dont", match));
            }
            // Sort by position in line to process in order
            matches.Sort((a, b) => a.Index.CompareTo(b.Index));
            // Process instructions in order
            foreach ((int Index, string Type, Match Match) instruction in matches)
            {
                switch (instruction.Type)
                {
                    case "mul" when enabled:
                        int value1 = int.Parse(instruction.Match.Groups[1].Value);
                        int value2 = int.Parse(instruction.Match.Groups[2].Value);
                        total += value1 * value2;
                        break;
                    case "do":
                        enabled = true;
                        break;
                    case "dont":
                        enabled = false;
                        break;
                }
            }
        }
        return total;
    }
}