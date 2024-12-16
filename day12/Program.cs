// Normally we will start by reading lines from an input file
var input = File.ReadAllLines(args.FirstOrDefault() ?? "input.txt");

List<Region> areas = new List<Region>();
for (int y = 0; y < input.Length; y++)
{
    var line = input[y];
    for (int x = 0; x < line.Length; x++)
    {
        var plant = line[x];
        var point = new Point(x, y);
        var areaBordering = areas.Where(a => a.Plant == plant && a.Borders(point)).ToArray();
        if (!areaBordering.Any())
        {
            areas.Add(new Region([point], plant));
        }
        else
        {
            var newArea = areaBordering[0];
            newArea.Points.Add(point);
            foreach (var aToJoin in areaBordering.Skip(1))
            {
                areas.Remove(aToJoin);
                foreach (Point p in aToJoin.Points)
                {
                    newArea.Points.Add(p);
                }
            }
        }
    }
}

int answer = areas.Sum(a => a.Price(input));
WriteLine(answer);

public record Region(HashSet<Point> Points, char Plant)
{
    public bool Contains(Point p) => Points.Contains(p);
    public bool Borders(Point p) => Neighbors(p).Any(Contains);
    public IEnumerable<Point> Neighbors(Point p)
    {
        yield return p with { X = p.X - 1 };
        yield return p with { X = p.X + 1 };
        yield return p with { Y = p.Y - 1 };
        yield return p with { Y = p.Y + 1 };
    }
    
    public int Area => Points.Count;

    public int Perimeter(string[] map)
    {
        return Points.Sum(p => Neighbors(p).Count(np => !WithinBounds(map, np) || map[np.Y][np.X] != Plant));
    }
    
    public int Price(string[] map) => Perimeter(map) * Area;

    private bool WithinBounds(string[] map, Point point) => point is { X: >= 0, Y: >= 0 } && point.Y < map.Length && point.X < map[point.Y].Length;

    public override string ToString()
    {
        return $"Plant = {Plant}\tArea = {Area}";
    }
}
public record Point(int X, int Y);