// Normally we will start by reading lines from an input file
var input = File.ReadAllLines(args.FirstOrDefault() ?? "input.txt");
int count = 0;
for (int y = 0; y < input.Length; y++)
{
    string line = input[y];
    for (int x = 0; x < line.Length; x++)
    {
        char c = line[x];
        if (c == 'X' || c == 'S') 
        {
            // Search forwards
            if (x + 3 < line.Length)
            {
                var hw = line[x..(x+4)];
                if (hw == "XMAS" || hw == "SAMX")
                    count++;
            }
            
            // Search downwards
            if (y + 3 < line.Length)
            {
                string vw = new String([input[y][x], input[y + 1][x], input[y + 2][x], input[y + 3][x]]);
                if (vw == "XMAS" || vw == "SAMX")
                    count++;
            }

            if (x + 3 < line.Length && y + 3 < line.Length)
            {
                string ac1 = new String([input[y][x], input[y+1][x+1], input[y+2][x+2], input[y+3][x+3]]);
                if (ac1 == "XMAS" || ac1 == "SAMX")
                    count++;
            }

            if (x + 3 < line.Length && y >= 3)
            {
                string ac2 = new String([input[y][x], input[y-1][x+1], input[y-2][x+2], input[y-3][x+3]]);
                if (ac2 == "XMAS" || ac2 == "SAMX")
                    count++;
            }
        }
    }
}
WriteLine(count);

// Part 2
int result = 0;
for (int y = 0; y < input.Length - 2; y++)
{
    string line = input[y];
    for (int x = 0; x < line.Length - 2; x++)
    {
        Range r = x..(x + 3);
        var str = input[y][r] + input[y + 1][r] + input[y + 2][r];
        
        if (Regex.Match(str, 
            "M.S" +
            ".A." +
            "M.S").Success ||
            
            Regex.Match(str, 
            "S.S" +
            ".A." +
            "M.M").Success ||
            
            Regex.Match(str, 
            "M.M" +
            ".A." +
            "S.S").Success || 
            
            Regex.Match(str,
            "S.M" +
            ".A." +
            "S.M").Success)
        {
            result++;
        }
    }
}
WriteLine(result);