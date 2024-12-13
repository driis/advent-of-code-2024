// Normally we will start by reading lines from an input file

using System.Runtime.Intrinsics.X86;

var input = File.ReadAllLines(args.FirstOrDefault() ?? "input.txt");


// Part 1 
Map map = new (input);
List<int> trailHeads = new();
int score = 0;
for (int y = 0; y < input.Length; y++)
{
    var line = input[y];
    for (int x = 0; x < line.Length; x++)
    {
        if (line[x] == '0')
        {
            // Possible trail head
            trailHeads.Add(map.ScoreForTrail(new(x, y)));
        }
    }
}
trailHeads.DumpConsole();
score = trailHeads.Sum();
WriteLine(score);

public class Map(string[] data)
{
    private bool WithinBounds(Point p) => p.Y >= 0 && p.X >= 0 && p.Y < data.Length && p.X < data[0].Length;

    public int ScoreForTrail(Point pos)
    {
        if (this[pos] == 9)
        {
            return 1;
        }

        var moves = ValidMovesFrom(pos);
        return moves.Sum(ScoreForTrail);
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

    private int this[Point p] => data[p.Y][p.X] - '0';
}

public record Point(int X, int Y);