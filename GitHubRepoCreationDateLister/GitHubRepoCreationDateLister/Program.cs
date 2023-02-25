using System.Diagnostics;

if ( args.Length != 2) {
    Console.WriteLine("usage: gh repo list -L 1000|GitHubRepoCreationDateLister INPUT_FILE_NAME  OUTPUT_FILE_NAME");
    return;
}

var list = new List<Repo>();

using (var reader = System.IO.File.OpenText(args[0]))
{

    for (; ; )
    {
        var s = reader.ReadLine();
        if (s == null) break;
        var d = s.Split('\t');
        list.Add(new Repo() { Name = getName(d[0]), Desc = d[1] });
    }
}

// TBW

using (var writer = System.IO.File.CreateText(args[1]))
{
    foreach (var item in list)
    {
        writer.WriteLine($"{item.Name} {item.Desc}");
    }
}

ProcessStartInfo pi = new ProcessStartInfo()
{
    FileName = args[1],
    UseShellExecute = true,
};
Process.Start(pi);

string getName(string s) {
    int index = s.IndexOf("/");
    return s.Substring(index + 1);
}

class Repo {
    public string? Name { get; set; }
    public string? Desc { get; set; }
    public string? Created { get; set; }
}
