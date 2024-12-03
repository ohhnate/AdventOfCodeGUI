// -----------------------------------------------------------
// Advent of Code 2024 - Core Framework
// Base functionality for running AoC solutions
// -----------------------------------------------------------

using Advent2024.Helpers;

namespace Advent2024;

public abstract class BaseDaySolver(int day)
{
    protected readonly FileHelper FileHelper = new();
    protected readonly MathHelper MathHelper = new();
    public readonly int Day = day;
    
    public abstract object SolvePart1();
    
    public abstract object SolvePart2();
    
    protected string GetInputFileName() => $"day{Day}input.txt";
}