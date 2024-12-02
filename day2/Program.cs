// Normally we will start by reading lines from an input file
var input = File.ReadAllLines(args.FirstOrDefault() ?? "input.txt");
var records = input.Select(line => line.Split(' ').Select(x => x.ToInt()).ToArray());

bool IsSsafe(int[] record)
{
    bool descending = record[1] - record[0] < 0;
    if (descending)
        record = record.Reverse().ToArray();
        
    int i = record[0];
    foreach (int n in record.Skip(1))
    {
        if (n < i || n - i < 1 || n - i > 3)
            return false;
        i = n;
    }

    return true;
}

int count = records.Count(IsSsafe);
if (count < 10)
{
    var safeRecords = records.Select(x => new { Safe = IsSsafe(x), Record = String.Join(" ",x) });
    safeRecords.DumpConsole();
}
WriteLine(count);