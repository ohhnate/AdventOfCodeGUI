// -----------------------------------------------------------
// Advent of Code 2024 - Utility Functions
// Common helper functions for solving AoC puzzles
// -----------------------------------------------------------

namespace Advent2024.Helpers
{
    public class MathHelper
    {
        // Greatest Common Divisor
        public static long Gcd(long a, long b)
        {
            while (b != 0)
            {
                long temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }

        // Least Common Multiple
        public static long Lcm(long a, long b) => Math.Abs(a * b) / Gcd(a, b);

        // LCM for a sequence of numbers
        public static long Lcm(IEnumerable<long> numbers) => numbers.Aggregate(Lcm);

        // Check if a number is prime
        public static bool IsPrime(long number)
        {
            if (number <= 1) return false;
            if (number == 2) return true;
            if (number % 2 == 0) return false;

            long boundary = (long)Math.Floor(Math.Sqrt(number));
            for (long i = 3; i <= boundary; i += 2)
            {
                if (number % i == 0)
                {
                    return false;
                }
            }
            return true;
        }

        // Get prime factors
        public static Dictionary<long, int> GetPrimeFactors(long number)
        {
            Dictionary<long, int> factors = new();
            long divisor = 2;
            while (number > 1)
            {
                if (number % divisor == 0)
                {
                    factors[divisor] = factors.GetValueOrDefault(divisor) + 1;
                    number /= divisor;
                }
                else
                {
                    divisor = divisor == 2 ? 3 : divisor + 2;
                }
            }
            return factors;
        }

        // Manhattan distance
        public static int ManhattanDistance((int x, int y) a, (int x, int y) b) => Math.Abs(a.x - b.x) + Math.Abs(a.y - b.y);

        // Check if two ranges overlap
        public static bool RangesOverlap((long start, long end) range1, (long start, long end) range2) =>
            range1.start <= range2.end && range2.start <= range1.end;

        // Merge overlapping ranges
        public static List<(long start, long end)> MergeRanges(IEnumerable<(long start, long end)> ranges)
        {
            List<(long start, long end)> sorted = ranges.OrderBy(r => r.start).ToList();
            List<(long start, long end)> merged = [];
            if (sorted.Count == 0) return merged;

            (long start, long end) current = sorted[0];
            for (int i = 1; i < sorted.Count; i++)
            {
                if (sorted[i].start <= current.end + 1)
                {
                    current.end = Math.Max(current.end, sorted[i].end);
                }
                else
                {
                    merged.Add(current);
                    current = sorted[i];
                }
            }
            merged.Add(current);
            return merged;
        }
    }
}