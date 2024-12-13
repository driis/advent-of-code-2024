var input = File.ReadAllLines(args.FirstOrDefault() ?? "input.txt");
Map map = new (input);
var trailHeads = map.AllPoints.Where(map.IsTrailHead);
var trails = trailHeads.Select(pos => map.PathsFrom([pos]).ToArray()).ToArray();

// Part 1
var score = trails.Select(trail => trail.Select(path => path.Last()).Distinct().Count()).Sum();
WriteLine(score);
// Part 2 
var distinct = trails.SelectMany(x => x).Select(path => String.Join(" | ", path.Select(kp => $"{kp.X},{kp.Y}"))).Distinct();
WriteLine(distinct.Count());

public class Map(string[] data)
{
    public IEnumerable<Point> AllPoints => data.SelectMany((line,y) => line.Select((_,x)=> new Point(x,y)));
    public IEnumerable<Point[]> PathsFrom(Point[] soFar)
    {
        var loc = soFar.Last();
        if (this[loc] == 9)
            yield return soFar;
        
        var validMoves = ValidMovesFrom(loc);
        foreach (var m in validMoves)
        {
            foreach (var path in PathsFrom(soFar.Concat([m]).ToArray()))
            {
                yield return path;
            }
        }
    }

    public bool IsTrailHead(Point p) => this[p] == 0;

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

    private int this[Point p] => data[p.Y][p.X] - '0';
    private bool WithinBounds(Point p) => p.Y >= 0 && p.X >= 0 && p.Y < data.Length && p.X < data[0].Length;
}

public record Point(int X, int Y);