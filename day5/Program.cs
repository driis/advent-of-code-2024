// Normally we will start by reading lines from an input file

using System.Collections;

var input = File.ReadAllLines(args.FirstOrDefault() ?? "input.txt");
var rules = input.TakeWhile(x => x != "").Select(x =>
{
    var p = x.Split('|').Select(n => n.ToInt()).ToArray();
    return new Rule(p[0], p[1]);
});
var updates = input.SkipWhile(x => x != "").Where(x => !String.IsNullOrEmpty(x)).Select(x =>
    new Update(x.Split(',').Select(x => x.ToInt()).ToArray()));

var validUpdates = updates.Where(x => rules.All(r => r.CheckUpdate(x)));
var answer = validUpdates.Sum(u => u.Middle);
WriteLine(answer);

var invalid = updates.Where(x => !rules.All(r => r.CheckUpdate(x)));
var answerPart2 = invalid.Select(x => x.AccordingToRules(rules.ToArray())).Sum(x => x.Middle);
WriteLine(answerPart2);

record Rule(int First, int Second)
{
    public bool CheckUpdate(Update update)
    {
        int n = update.IndexOfPage(First); 
        int z = update.IndexOfPage(Second);
        return n < z || z == -1;
    }
};

record Update(int[] Pages)
{
    public int Middle => Pages[Pages.Length / 2];

    public int IndexOfPage(int page) => Pages
        .Select((n, i) => new { n, i })
        .FirstOrDefault(x => x.n == page)?.i ?? -1;

    public Update AccordingToRules(Rule[] rules)
    {
        var pages = Pages.Select(x => x).ToArray();
        Array.Sort(pages, new RuleComparer(rules));
        return new Update(pages);
    }
}

class RuleComparer(Rule[] rules) : IComparer<int>
{
    public int Compare(int x, int y)
    {
        foreach (var rule in rules)
        {
            if (rule.First == x && rule.Second == y)
                return -1;
        }

        return 1;
    }
}