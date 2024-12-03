// -----------------------------------------------------------
// Advent of Code 2024 - Day [1]
// https://adventofcode.com/2024
// -----------------------------------------------------------

namespace Advent2024.Days;

public class Day1() : BaseDaySolver(1)
{
    public override object SolvePart1()
    {
        (List<int> column1, List<int> column2) = FileHelper.ReadColumns(
            GetInputFileName(),
            int.Parse,
            int.Parse,
            " "
        );
        column1.Sort();
        column2.Sort();

        int distance = 0;
        for (int i = 0; i < column1.Count; i++)
        {
            distance += Math.Max(column1[i], column2[i]) - Math.Min(column1[i], column2[i]);
        }
        return distance;
    }

    public override object SolvePart2()
    {
        (List<int> column1, List<int> column2) = FileHelper.ReadColumns(
            GetInputFileName(),
            int.Parse,
            int.Parse,
            " "
        );
        column1.Sort();
        column2.Sort();
        return column1.Sum(t => column2.Count(x => x == t) * t);
    }
}