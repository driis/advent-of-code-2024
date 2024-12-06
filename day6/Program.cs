// Normally we will start by reading lines from an input file
var input = File.ReadAllLines(args.FirstOrDefault() ?? "input.txt");
var matrix = input.Select(line => line.ToArray()).ToArray();
var map = matrix.Select(x => x.ToArray()).ToArray();

(int X, int Y) FindGuard()
{
    for (int y = 0; y < matrix.Length; y++)
    {
        var line = matrix[y];
        for (int x = 0; x < line.Length; x++)
        {
            if (line[x] == '^')
                return (x, y);
        }
    }

    throw new Exception("not found guard");
}

var pos = FindGuard();
HashSet<(int X, int Y)> visited = new();
bool Move(int x, int y, Direction direction, int steps = 0)
{
    visited.Add((x,y));
    var (newX, newY) = direction.Move(x, y);
    if (newY < matrix.Length && newY >= 0 && newX < matrix[newY].Length && newX >= 0)
    {
        if (matrix[newY][newX] == '#')
        {
            direction = direction.Dir switch
            {
                '^' => Direction.Right,
                '>' => Direction.Down,
                'd' => Direction.Left,
                '<' => Direction.Up,
                _ => throw new Exception("invalid direction")
            };
            return Move(x,y, direction, steps+1);
        }

        // If steps not exhausted, try move
        if (steps < 10000)
        {
            return Move(newX,newY, direction, steps + 1);
        }

        // Steps exhausted, obstruction works
        return true;
    }

    // We got out before exhausting steps, obstruction does not work
    return false;
}

// Part 1
Move(pos.X, pos.Y, Direction.Up);
WriteLine(visited.Count);

// Part 2
int obstructionPlacements = 0;
for (int y = 0; y < matrix.Length; y++)
{
    var line = matrix[y];
    for (int x = 0; x < matrix[y].Length; x++)
    {
        if (line[x] == '.')
        {
            line[x] = '#';
            if (Move(pos.X, pos.Y, Direction.Up))
                obstructionPlacements++;
            line[x] = '.';
        }
    }
}
WriteLine(obstructionPlacements);

record Direction(int XD, int YD, char Dir)  
{
    public static Direction Up = new (0, -1, '^');
    public static Direction Down  = new (0, 1, 'd');
    public static Direction Right = new (1, 0, '>');
    public static Direction Left = new(-1, 0, '<');

    public (int X, int Y) Move(int x, int y)
    {
        return (x+XD, y+YD);
    }
}