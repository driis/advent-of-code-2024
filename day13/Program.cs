// Normally we will start by reading lines from an input file

using System.Xml.Linq;

var input = File.ReadAllLines(args.FirstOrDefault() ?? "input.txt");
var machines = input.Select((line, idx) => new { line, idx }).GroupBy(n => n.idx / 4)
    .Select(g => Machine.Parse(g.Select(line => line.line).ToArray())).ToArray();

long SolveCostFor(IReadOnlyList<Machine> machines)
{
    var solutions = machines.SelectMany(m => m.Solutions().OrderBy(s => s.Cost).Take(1));
    long totalCost = solutions.Sum(x => x.Cost);
    return totalCost;
}

long costPartOne = SolveCostFor(machines);
WriteLine($"Part 1: {costPartOne}");

var machinesPartTwo = machines.Select(machine => new Machine(machine.A, machine.B, new Target(machine.Target.X + 10000000000000L, machine.Target.Y + 10000000000000L))).ToArray();
long totalCostPartTwo = SolveCostFor(machinesPartTwo);
WriteLine($"Part 2: {totalCostPartTwo}");

record Button(int dX, int dY)
{
    private static readonly Regex ParseEx = new("Button \\w: X\\+(\\d+), Y\\+(\\d+)");
    public static Button Parse(string input)
    {
        var match = ParseEx.Match(input);
        if (!match.Success)
            throw new ApplicationException($"Unexpected input {input}");
        return new Button(match.Groups[1].Value.ToInt(), match.Groups[2].Value.ToInt());
    }
}

record Target(long X, long Y)
{
    private static readonly Regex ParseEx = new("Prize: X\\=(\\d+), Y\\=(\\d+)");
    public static Target Parse(string input)
    {
        var match = ParseEx.Match(input);
        if (!match.Success)
            throw new ApplicationException($"Unexpected input {input}");
        return new Target(match.Groups[1].Value.ToLong(), match.Groups[2].Value.ToLong());
    }
}

public record Solution(long Apresses, long Bpresses)
{
    public long Cost => Apresses * 3 + Bpresses;
}

record Machine(Button A, Button B, Target Target)
{
    public IEnumerable<Solution> Solutions()
    {
        // Cramer's rule
        var determinant = (A.dX * B.dY - B.dX * A.dY);
        if (determinant == 0)
            yield break;
        long ap = (Target.X * B.dY - B.dX * Target.Y) / determinant;
        long bp = (A.dX * Target.Y - Target.X * A.dY) / determinant;
        if (ap * A.dX + bp * B.dX == Target.X && ap * A.dY + bp * B.dY == Target.Y)
            yield return new Solution(ap,bp);
    }
    
    public static Machine Parse(string[] input)
    {
        return new Machine(
            Button.Parse(input[0]), Button.Parse(input[1]), Target.Parse(input[2]));
    }
}