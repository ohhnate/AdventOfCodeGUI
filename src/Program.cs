// -----------------------------------------------------------
// Advent of Code 2024 - Core Framework
// Base functionality for running AoC solutions
// -----------------------------------------------------------

namespace Advent2024;

public abstract class Program
{
    public static void Main(string[] args)
    {
        // If no command line args, start GUI
        if (args.Length == 0)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new AdventForm());
            return;
        }
        // Otherwise run command line version
        RunDay(args[0]);
        // Wait for input before closing
        Console.WriteLine("\nPress any key to exit...");
        Console.ReadKey();
    }

    public static void RunDay(string dayInput)
    {
        if (!int.TryParse(dayInput, out int day) || day < 1 || day > 25)
        {
            Console.WriteLine("Day must be a number between 1 and 25");
            return;
        }
        Type? solverType = Type.GetType($"Advent2024.Days.Day{day}");
        if (solverType == null)
        {
            Console.WriteLine($"Solution for day {day} not found");
            return;
        }
        if (Activator.CreateInstance(solverType) is not BaseDaySolver solver)
        {
            Console.WriteLine($"Day {day} solution does not inherit from BaseDaySolver");
            return;
        }
        Console.WriteLine($"=== Day {day} ===");
        Console.WriteLine($"Part 1: {solver.SolvePart1()}");
        Console.WriteLine($"Part 2: {solver.SolvePart2()}");
    }
}