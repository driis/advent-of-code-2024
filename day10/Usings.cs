global using static System.Console;
global using System.Text.RegularExpressions;
global using System.IO;

public static class Extensions
{
    public static int ToInt(this string value)
    {
        return int.TryParse(value, out int n) ? 
            n : 
            throw new ApplicationException($"{value} could not be parsed as an integer");
    }

        public static int? ToIntNullable(this string value)
    {
        return int.TryParse(value, out int n) ? 
            n : 
            null;
    }
        
    public static long ToLong(this string value)
    {
        return long.TryParse(value, out long n) ? 
            n : 
            throw new ApplicationException($"{value} could not be parsed as a long");
    }

    public static IEnumerable<T> DumpConsole<T>(this IEnumerable<T> items)
    {
        var arr = items.ToArray();
        foreach (var item in arr)
        {
            WriteLine(item);
        }

        return arr;
    }
}
