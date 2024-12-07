// Normally we will start by reading lines from an input file

using System.Runtime.CompilerServices;

var input = File.ReadAllLines(args.FirstOrDefault() ?? "input.txt");

var equations = input.Select(Record.Parse).ToArray();
var matches = equations.Where(e => e.HasMatch());
long sum = matches.Sum(m => m.Result);
WriteLine(sum);

record Record(long Result, long[] Numbers)
{
    public static Record Parse(string input)
    {
        var parts = input.Split(":");
        var numbers = parts[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(x => x.ToLong()).ToArray();
        return new Record(parts[0].ToLong(), numbers);
    }

    long Evaluate(Op[] operators)
    {
        long a = Numbers[0];
        for (int i = 0; i < operators.Length; i++)
        {
            a = operators[i].Apply(a, Numbers[i+1]);
        }

        return a;
    }
    bool Matches(Op[] combinations) => Evaluate(combinations) == Result;

    public bool HasMatch() => Op.Combinations(Numbers.Length - 1).Any(Matches);
}

record Op(char What)
{
    public long Apply(long lhs, long rhs) => What switch
    {
        '+' => lhs + rhs,
        '*' => lhs * rhs,
        '|' => $"{lhs}{rhs}".ToLong(),
        _ => throw new Exception($"Invalid operator: {What}")
    };

    public static IEnumerable<Op[]> Combinations(int length)
    {
        if (length > 1)
        {
            var rm = Combinations(length - 1).ToArray();
            var built = rm.Select(option => 
                option.Concat([new('*')]).ToArray()).Concat(rm.Select(option => 
                option.Concat([new('+')]).ToArray()).Concat(rm.Select(option => 
                option.Concat([new ('|')]).ToArray())));
            foreach (var op in built)
                yield return op;
            yield break;
        }

        yield return [new('*')];
        yield return [new('+')];
        yield return [new('|')];
    }
}
