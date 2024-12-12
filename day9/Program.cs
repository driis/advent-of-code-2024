// Normally we will start by reading lines from an input file

const int emptyBlock = -1;

var input = System.IO.File.ReadAllLines(args.FirstOrDefault() ?? "input.txt");

int[] CreateFs() => input[0]
    .SelectMany((ch, i) => Enumerable.Repeat(i % 2 == 0 ? i/2 : emptyBlock, ch -'0')).ToArray();

// Part 1
var fs = CreateFs();
int pick = fs.Length - 1;
for (int i = 0; i < pick; i++)
{
    bool empty = fs[i] == emptyBlock;
    if (empty)
    {
        while (pick >= 0 && fs[pick] == emptyBlock)
        {
            pick--;
        }
        fs[i] = fs[pick];
        fs[pick] = emptyBlock;
    }
}

var checksum = fs.Where( f => f >= 0).Select((f, i) => (long)f * (f < 0 ? 0 : i)).Sum();
WriteLine(checksum);

// Part 2
int Length(int[] data, int x, int delta = 1)
{
    int c = 0;
    while (x + c * delta < data.Length && x + c * delta >= 0 && data[x + c * delta] == data[x])
    {
        c++;
    }

    return c;
}

int LengthBackwards(int[] data, int x) => Length(data, x, -1);

var data = CreateFs();
HashSet<int> movedBlocks = new HashSet<int>();
for (int n = data.Length - 1; n >= 0; n--)
{
    if (data[n] == emptyBlock || movedBlocks.Contains(data[n]))
        continue;
    int blockLength = LengthBackwards(data, n);
    for (int i = 0; i < n; i++)
    {
        if (data[i] == emptyBlock)
        {
            int emptyLength = Length(data, i);
            if (emptyLength >= blockLength)
            {
                movedBlocks.Add(data[n]);
                Array.Copy(data, n - blockLength + 1, data, i, blockLength);
                Array.Fill(data,-1, n - blockLength + 1, blockLength);
                break;
            }
        }
    }
    n -= (blockLength - 1);
}

if (data.Length < 50)
{
    WriteLine(String.Join("", data).Replace("-1", "."));
}

var checksumPart2 = data.Select((f,i) => f < 0 ? 0 : (long)f * i).Sum();
WriteLine(checksumPart2);