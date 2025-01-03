// Normally we will start by reading lines from an input file
var input = File.ReadAllLines(args.FirstOrDefault() ?? "input.txt");
var robots = input.Select(Robot.Parse).ToArray();

WriteLine($"Robots parsed, {robots.Length} robots ready.");

for (int i = 0; i < 100; i++)
{
    robots = robots.Select(r => r.Advance()).ToArray();
}

var quadrantsWithRobots = robots.GroupBy(r => r.Quadrant)
    .Select(q => new {Qudarant = q.Key, Robots = q.Count()});
quadrantsWithRobots.DumpConsole();

int sf = quadrantsWithRobots.Where(x => x.Qudarant != -1).Select(x => x.Robots)
    .Aggregate((a, b) => a * b);
 WriteLine($"Part 1 Safety Factor {sf}");
public record Robot(int X, int Y, int Dx, int Dy)
{
    public const int Width = 101;
    public const int Height = 103;
    private static readonly Regex regInput = new Regex("p=(-?\\d+),(-?\\d+) v=(-?\\d+),(-?\\d+)");
    public static Robot Parse(string line)
    {
        var match = regInput.Match(line);
        if (!match.Success)
            throw new ApplicationException($"Cannot parse line: {line}");
        
        var x = int.Parse(match.Groups[1].Value);
        var y = int.Parse(match.Groups[2].Value);
        var dx = int.Parse(match.Groups[3].Value);
        var dy = int.Parse(match.Groups[4].Value);
        return new Robot(x, y, dx, dy);
    }

    public Robot Advance()
    {
        int newX = (X + Dx) % Width;
        int newY = (Y + Dy) % Height;
        if (newX < 0) newX = Width + newX;
        if (newY < 0) newY = Height + newY;
        return this with { X = newX, Y = newY };
    }

    public int Quadrant
    {
        get
        {
            int xh = Width / 2;
            int yh = Height / 2;
            int xq = X < xh ? 0 : X > xh ? 1 : -1;
            int yq = Y < yh ? 0 : Y > yh ? 1 : -1;
            return (xq, yq) switch
            {
                (0, 0) => 1,
                (1, 0) => 2,
                (0, 1) => 3,
                (1, 1) => 4,
                _ => -1
            };
        }
    }
}