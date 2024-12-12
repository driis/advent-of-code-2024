// Normally we will start by reading lines from an input file

const int emptyBlock = -1;

var input = System.IO.File.ReadAllLines(args.FirstOrDefault() ?? "input.txt");
var fs = input[0]
    .SelectMany((ch, i) => Enumerable.Repeat(i % 2 == 0 ? i/2 : emptyBlock, ch -'0')).ToArray();

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
