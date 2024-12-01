// Normally we will start by reading lines from an input file
var input = File.ReadAllLines(args.FirstOrDefault() ?? "input.txt");

var intList = input.Select(line => 
    line.Split(' ', '\t', StringSplitOptions.RemoveEmptyEntries)
        .Select(x => x.ToInt()).ToArray());

var leftList = intList.Select(n => n[0]).OrderBy(n => n).ToArray();
var rightList = intList.Select(n => n[1]).OrderBy(n => n).ToArray();

var distances = leftList.Select((n, idx) => Math.Abs(n - rightList[idx]));
WriteLine(distances.Sum());

var similarity = leftList.Select(n => n * rightList.Count(x => x == n));
WriteLine(similarity.Sum());