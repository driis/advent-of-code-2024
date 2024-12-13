var input = File.ReadAllLines(args.FirstOrDefault() ?? "input.txt");

Map map = new (input);
Dictionary<Point, IEnumerable<Point[]>> paths = new();
var trailHeads = map.AllPoints.Where(p => map[p] == 0);
foreach (var pos in trailHeads)
{
    paths.Add(pos, map.PathsFrom([pos]).ToArray());
}

// Part 1
var score = paths.Select(kp => kp.Value.Select(path => path.Last()).Distinct().Count()).Sum();
WriteLine(score);
// Part 2 
var distinct = paths.SelectMany(kp => kp.Value)
    .Select(path => String.Join(" | ", path.Select(kp => $"{kp.X},{kp.Y}"))).Distinct();
WriteLine(distinct.Count());

public class Map(string[] data)
{
    private bool WithinBounds(Point p) => p.Y >= 0 && p.X >= 0 && p.Y < data.Length && p.X < data[0].Length;

    public IEnumerable<Point> AllPoints => data.SelectMany((line,y) => line.Select((_,x)=> new Point(x,y)));
    public IEnumerable<Point[]> PathsFrom(Point[] soFar)
    {
        if (this[soFar.Last()] == 9)
            yield return soFar;
        
        var validMoves = ValidMovesFrom(soFar.Last());
        foreach (var m in validMoves)
        {
            foreach (var path in PathsFrom(soFar.Concat([m]).ToArray()))
            {
                yield return path;
            }
        }
    }

    private IEnumerable<Point> ValidMovesFrom(Point pos)
    {
        Point up = pos with { Y = pos.Y - 1 };
        Point down = pos with { Y = pos.Y + 1 };
        Point left = pos with { X = pos.X - 1 };
        Point right = pos with { X = pos.X + 1 };

        int target = this[pos] + 1;
        IEnumerable<Point> moves = [up, down, left, right];
        return moves.Where(m => WithinBounds(m) && this[m] == target);
    }

    public int this[Point p] => data[p.Y][p.X] - '0';
}

public record Point(int X, int Y);