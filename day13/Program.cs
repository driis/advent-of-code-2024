// Normally we will start by reading lines from an input file

using System.Xml.Linq;

var input = File.ReadAllLines(args.FirstOrDefault() ?? "input.txt");
var machines = input.Select((line, idx) => new { line, idx }).GroupBy(n => n.idx / 4)
    .Select(g => Machine.Parse(g.Select(line => line.line).ToArray()));

var solutions = machines.SelectMany(m => m.Solutions().OrderBy(s => s.Cost).Take(1));
int totalCost = solutions.Sum(x => x.Cost);
WriteLine($"Part 1: {totalCost}");

record Button(int dX, int dY)
{
    private static Regex parseEx = new Regex("Button \\w: X\\+(\\d+), Y\\+(\\d+)");
    public static Button Parse(string input)
    {
        var match = parseEx.Match(input);
        if (!match.Success)
            throw new ApplicationException($"Unexpected input {input}");
        return new Button(match.Groups[1].Value.ToInt(), match.Groups[2].Value.ToInt());
    }
}

record Target(int X, int Y)
{
    private static Regex parseEx = new Regex("Prize: X\\=(\\d+), Y\\=(\\d+)");
    public static Target Parse(string input)
    {
        var match = parseEx.Match(input);
        if (!match.Success)
            throw new ApplicationException($"Unexpected input {input}");
        return new Target(match.Groups[1].Value.ToInt(), match.Groups[2].Value.ToInt());
    }
}

public record Solution(int Apresses, int Bpresses)
{
    public int Cost => Apresses * 3 + Bpresses;
};

class Machine(Button A, Button B, Target target)
{
    public IEnumerable<Solution> Solutions()
    {
        int aMax = Math.Max(target.X / A.dX, target.Y / A.dY);
        if (aMax > 100) aMax = 100;

        for (int a = aMax; a >= 0; a--)
        {
            // For this A, which B press solutions exists ? 
            int x = a * A.dX;
            int y = a * A.dY;
            int rx = target.X - x;
            int ry = target.Y - y;
            if (rx % B.dX == 0 && ry % B.dY == 0)
            {
                int b = rx / B.dX;
                if (ry / B.dY == b)
                {
                    yield return new Solution(a, b);
                }
            }
        }
    }
    
    public static Machine Parse(string[] input)
    {
        return new Machine(
            Button.Parse(input[0]), Button.Parse(input[1]), Target.Parse(input[2]));
    }
}