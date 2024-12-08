// Normally we will start by reading lines from an input file
var input = File.ReadAllLines(args.FirstOrDefault() ?? "input.txt");
HashSet<Point> antinodes = new();
var frequencies = input.SelectMany((data, y) =>
    data.Select((f, x) => new { Frequency = f, At = new Point(x, y) }))
    .Where(x => x.Frequency != '.')
    .GroupBy(x => x.Frequency)
    .ToArray();

// Part 1
var h = input.Length;
var w = input[0].Length;
foreach (var frequency in frequencies)
{
    foreach (Point p in frequency.Select(x => x.At))
    {
        var otherPoints = frequency.Select(x => x.At).Where(op => op != p);
        foreach (Point op in otherPoints)
        {
            int dx = op.X - p.X;
            int dy = op.Y - p.Y;
            var antinode = new Point(p.X - dx, p.Y - dy);
            if (antinode is { Y: >= 0, X: >= 0 } && antinode.Y < h && antinode.X < w)
            {
                antinodes.Add(antinode);
            }
        }
    }
}

WriteLine(antinodes.Count);

antinodes.Clear();
// Part 2
foreach (var frequency in frequencies)
{
    foreach (Point p in frequency.Select(x => x.At))
    {
        var otherPoints = frequency.Select(x => x.At).Where(op => op != p);
        foreach (Point op in otherPoints)
        {
            int dx = op.X - p.X;
            int dy = op.Y - p.Y;
            var antinode = new Point(p.X - dx, p.Y - dy);
            while(antinode is { Y: >= 0, X: >= 0 } && antinode.Y < h && antinode.X < w)
            {
                antinodes.Add(antinode);
                antinodes.Add(op);
                antinodes.Add(p);
                antinode = new Point(antinode.X - dx, antinode.Y - dy);
            }
        }
    }
}

antinodes.OrderBy(x => x.Y).ThenBy(x => x.X).DumpConsole();
WriteLine(antinodes.Count);
record Point(int X, int Y);