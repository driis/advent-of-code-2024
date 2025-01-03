// Normally we will start by reading lines from an input file
var input = File.ReadAllLines(args.FirstOrDefault() ?? "input.txt");
string line = input.Single();

var numbers = line.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(x => x.ToLong()).ToArray();

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

int CountExpandedElements(long element, int iterations)
{
    if (iterations == 0)
        return 1;
    
    var result = ApplyRulesToElement(element);
    return result.Sum(e => CountExpandedElements(e, iterations - 1));
}

// Part 1
var answerPartOne = numbers.Sum(n => CountExpandedElements(n, 25));
WriteLine(answerPartOne);

// Part 2
const int blinks = 75;
long sum = 0;
Parallel.ForEach(numbers, n => sum += CountExpandedElements(n, blinks));
WriteLine(sum);