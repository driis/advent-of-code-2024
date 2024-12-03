// Normally we will start by reading lines from an input file
var input = File.ReadAllText(args.FirstOrDefault() ?? "input.txt");
Regex regMultiply = new Regex("mul\\((\\d+),(\\d+)\\)");

int Multiplies(string data)
{
    var mulInstructions = regMultiply.Matches(data);
    var result = mulInstructions.Select(m =>
    {
        var x = m.Groups[1].Value.ToInt();
        var y = m.Groups[2].Value.ToInt();
        return x * y;
    }).Sum();
    return result;
}

// Part 1
WriteLine(Multiplies(input));

// Part 2
string data = String.Join("", input);
int cursor = 0;
bool doDont = true;
string result = "";
string Word(bool state) => state ? "don't()" : "do";
while (-1 != (cursor = data.IndexOf(Word(doDont), StringComparison.Ordinal)))
{
    if (doDont)
    {
        result += data[..cursor];
    }
    data = data[..(cursor+2)];
    doDont = !doDont;
}
WriteLine(Multiplies(result));