// Normally we will start by reading lines from an input file
var input = File.ReadAllLines(args.FirstOrDefault() ?? "input.txt");
string line = input.Single();

var numbers = line.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(x => x.ToLong()).ToArray();

long[] ApplyRules(long[] state) => state.SelectMany(ApplyRulesToElement).ToArray();

long[] ApplyRulesToElement(long element)
{
    if (element == 0)
        return [1];
    string str = element.ToString();
    if (str.Length % 2 == 0)
    {
        int n = str.Length / 2;
        return [str[..n].ToLong(), str[n..].ToLong()];
    }

    return [element * 2024];
}

const int blinks = 25;
long sum = 0;
foreach (var number in numbers)
{
    long[] temp = [number];
    for (int n = 0; n < blinks; n++)
    {
        temp = ApplyRules(temp);
    }

    WriteLine($"{number} expanded to {temp.Length} stones.");
    sum += temp.Length;
}

WriteLine(sum);