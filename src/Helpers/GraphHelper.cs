// -----------------------------------------------------------
// Advent of Code 2024 - Utility Functions
// Common helper functions for solving AoC puzzles
// -----------------------------------------------------------

namespace Advent2024.Helpers
{
    public class GraphHelper
    {
        // Generic Breadth-First Search
        public static Dictionary<T, int> Bfs<T>(T start, Func<T, IEnumerable<T>> getNeighbors) where T : notnull
        {
            Dictionary<T, int> distances = new() { [start] = 0 };
            Queue<T> queue = new();
            queue.Enqueue(start);
            while (queue.Count > 0)
            {
                T current = queue.Dequeue();
                foreach (T neighbor in getNeighbors(current))
                {
                    if (!distances.ContainsKey(neighbor))
                    {
                        distances[neighbor] = distances[current] + 1;
                        queue.Enqueue(neighbor);
                    }
                }
            }
            return distances;
        }

        // Dijkstra's Algorithm for shortest paths
        public static Dictionary<T, long> Dijkstra<T>(
            T start,
            Func<T, IEnumerable<(T node, long cost)>> getNeighbors,
            long maxCost = long.MaxValue) where T : notnull
        {
            Dictionary<T, long> distances = new() { [start] = 0 };
            PriorityQueue<T, long> pq = new();
            pq.Enqueue(start, 0);
            while (pq.Count > 0)
            {
                T current = pq.Dequeue();
                long currentDist = distances[current];
                if (currentDist > maxCost) continue;

                foreach ((T neighbor, long cost) in getNeighbors(current))
                {
                    long newDist = currentDist + cost;
                    if (!distances.TryGetValue(neighbor, out long value) || newDist < value)
                    {
                        value = newDist;
                        distances[neighbor] = value;
                        pq.Enqueue(neighbor, newDist);
                    }
                }
            }
            return distances;
        }

        // Find all cycles in a directed graph
        public static IEnumerable<List<T>> FindCycles<T>(T start, Func<T, IEnumerable<T>> getNeighbors) where T : notnull
        {
            HashSet<T> visited = [];
            List<T> path = [];
            List<List<T>> cycles = [];
            void Dfs(T current)
            {
                int pathIndex = path.IndexOf(current);
                if (pathIndex != -1)
                {
                    cycles.Add(path.Skip(pathIndex).ToList());
                    return;
                }
                if (!visited.Add(current)) return;

                path.Add(current);
                foreach (T neighbor in getNeighbors(current))
                {
                    Dfs(neighbor);
                }
                path.RemoveAt(path.Count - 1);
                visited.Remove(current);
            }
            Dfs(start);
            return cycles;
        }
    }
}