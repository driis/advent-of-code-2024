var input = File.ReadAllLines(args.FirstOrDefault() ?? "input.txt");
var records = input.Select(line => line.Split(' ').Select(x => x.ToInt()).ToArray());

bool IsSafe(int[] record) => IsSafeRecur(record, record, 0);
bool IsSafeRecur(int[] record, int[] orgRecord, int recurPos)
{
    bool descending = record[1] - record[0] < 0;
    if (descending)
        record = record.Reverse().ToArray();
        
    int i = record[0];
    foreach (int n in record.Skip(1))
    {
        if (n < i || n - i < 1 || n - i > 3)
        {
            if (recurPos < orgRecord.Length)
            {
                var retryRecord = orgRecord[..recurPos].Concat(orgRecord[(recurPos+1)..]).ToArray();
                return IsSafeRecur(retryRecord, orgRecord, recurPos + 1);
            }
            return false;
        }
        i = n;
    }

    return true;
}

int count = records.Count(IsSafe);
WriteLine(count);