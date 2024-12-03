// -----------------------------------------------------------
// Advent of Code 2024 - Day [2]
// https://adventofcode.com/2024
// -----------------------------------------------------------

using Advent2024.Helpers;

namespace Advent2024.Days;

public class Day2() : BaseDaySolver(2)
{
    public override object SolvePart1()
    {
        int safeReports = 0;
        foreach (string line in FileHelper.ReadLinesLazy(GetInputFileName()))
        {
            //Console.WriteLine($"\nProcessing line: {line}");
            int[] digits = FileHelper.StringToDigits(line);
            //Console.WriteLine($"Converted to digits: {string.Join(", ", digits)}");
            if (IsMonotonic(digits) && IsWithinSteps(digits, 3))
            {
                safeReports++;
            }
        }
        return safeReports;
    }

    private static bool IsMonotonic(int[] digits)
    {
        bool shouldBeIncreasing = digits[1] > digits[0];
        for (int i = 1; i < digits.Length; i++)
        {
            if (digits[i] == digits[i - 1]) // Equal values not allowed
            {
                return false;
            }
            if (shouldBeIncreasing)
            {
                if (digits[i] <= digits[i - 1]) // Must keep increasing
                {
                    return false;
                }
            }
            else
            {
                if (digits[i] >= digits[i - 1]) // Must keep decreasing
                {
                    return false;
                }
            }
        }
        return true;
    }

    private static bool IsWithinSteps(int[] digits, int steps)
    {
        for (int i = 1; i < digits.Length; i++)
        {
            int difference = Math.Abs(digits[i - 1] - digits[i]);
            if (difference < 1 || difference > steps)
            {
                return false;
            }
        }
        return true;
    }

    public override object SolvePart2()
    {
        int safeReports = 0;
        foreach (string line in FileHelper.ReadLinesLazy(GetInputFileName()))
        {
            //Console.WriteLine($"\nProcessing line: {line}");
            int[] digits = FileHelper.StringToDigits(line);
            //Console.WriteLine($"Converted to digits: {string.Join(", ", digits)}");
            
            if (IsMonotonic(digits) && IsWithinSteps(digits, 3))
            {
                safeReports++;
                continue;
            }
            // need to check if removing each digit keeps the sequence safe
            for (int skip = 0; skip < digits.Length; skip++)
            {
                // remove current digit and iterate skipping the removed digit that increases each iteration
                int[] shortened = new int[digits.Length - 1];
                int index = 0;
                for (int i = 0; i < digits.Length; i++)
                {
                    if (i != skip)
                    {
                        shortened[index++] = digits[i];
                    }
                }
                // if its still safe after 1 removal then good to go
                if (IsMonotonic(shortened) && IsWithinSteps(shortened, 3))
                {
                    safeReports++;
                    break;
                }
            }
        }
        return safeReports;
    }
}