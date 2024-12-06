// Normally we will start by reading lines from an input file
var input = File.ReadAllText(args.FirstOrDefault() ?? "input.txt");
Regex regMultiply = new Regex("mul\\((\\d+),(\\d+)\\)");

int Multiplies(string data)
{
    var mulInstructions = regMultiply.Matches(data);
    return mulInstructions.Select(m =>
    {
        var x = m.Groups[1].Value.ToInt();
        var y = m.Groups[2].Value.ToInt();
        return x * y;
    }).Sum();
}

// Part 1
WriteLine(Multiplies(input));

// Part 2
var dos = input.Split("do()");
dos = dos.Select(x => x.Split("don't()").First()).ToArray();
string cleanedText = String.Join("", dos);
WriteLine(Multiplies(cleanedText));