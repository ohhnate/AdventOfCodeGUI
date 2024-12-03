// -----------------------------------------------------------
// Advent of Code 2024 - Utility Functions
// Common helper functions for solving AoC puzzles
// -----------------------------------------------------------

namespace Advent2024.Helpers
{
    public class GridHelper
    {
        // Common directions for grid traversal
        public static readonly (int dx, int dy)[] CardinalDirections =
        [
            (0, 1),   // North
            (1, 0),   // East
            (0, -1),  // South
            (-1, 0)   // West
        ];

        public static readonly (int dx, int dy)[] DiagonalDirections =
        [
            (1, 1),   // Northeast
            (1, -1),  // Southeast
            (-1, -1), // Southwest
            (-1, 1)   // Northwest
        ];

        public static readonly (int dx, int dy)[] AllDirections =
        [
            .. CardinalDirections,
            .. DiagonalDirections
        ];

        // Check if coordinates are within grid bounds
        public static bool IsInBounds<T>(T[,] grid, int x, int y) => x >= 0 && x < grid.GetLength(0) && y >= 0 && y < grid.GetLength(1);

        // Get all valid neighbors in specified directions
        public static IEnumerable<(int x, int y)> GetNeighbors<T>(T[,] grid, int x, int y, (int dx, int dy)[] directions)
        {
            foreach ((int dx, int dy) in directions)
            {
                int newX = x + dx;
                int newY = y + dy;
                if (IsInBounds(grid, newX, newY))
                {
                    yield return (newX, newY);
                }
            }
        }

        // Print a grid (useful for debugging)
        public static void PrintGrid<T>(T[,] grid)
        {
            int rows = grid.GetLength(0);
            int cols = grid.GetLength(1);
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    Console.Write(grid[i, j]?.ToString() ?? ".");
                }
                Console.WriteLine();
            }
        }

        // Find all positions of a specific value in the grid
        public static IEnumerable<(int x, int y)> FindAll<T>(T[,] grid, T value) where T : IEquatable<T>
        {
            int rows = grid.GetLength(0);
            int cols = grid.GetLength(1);
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (grid[i, j].Equals(value))
                    {
                        yield return (i, j);
                    }
                }
            }
        }

        // Rotate a grid 90 degrees clockwise
        public static T[,] RotateClockwise<T>(T[,] grid)
        {
            int rows = grid.GetLength(0);
            int cols = grid.GetLength(1);
            T[,] rotated = new T[cols, rows];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    rotated[j, rows - 1 - i] = grid[i, j];
                }
            }
            return rotated;
        }

        // Flip a grid horizontally
        public static T[,] FlipHorizontal<T>(T[,] grid)
        {
            int rows = grid.GetLength(0);
            int cols = grid.GetLength(1);
            T[,] flipped = new T[rows, cols];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    flipped[i, cols - 1 - j] = grid[i, j];
                }
            }
            return flipped;
        }
    }
}