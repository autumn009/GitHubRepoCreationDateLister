using System.Diagnostics;

if ( args.Length != 2) {
    Console.WriteLine("usage: gh repo list -L 1000|GitHubRepoCreationDateLister INPUT_FILE_NAME  OUTPUT_FILE_NAME");
    Console.WriteLine("use 'gh auth login' before use");
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
        list.Add(new Repo() {FullName=d[0], Name = getName(d[0]), Desc = d[1], Created = await getCreated(d[0]) });
    }
}

using (var writer = System.IO.File.CreateText(args[1]))
{
    foreach (var item in list.OrderByDescending(c=>c.Created))
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

async Task<string> getCreated(string fullName)
{
    var all = await detailLoader(fullName);
    int index = all.IndexOf("created_at");
    int index2 = all.IndexOf("2", index);
    int index3 = all.IndexOf("Z", index2);
    return all.Substring(index2, index3 - index2 + 1);
}

async Task<string> detailLoader(string fullName)
{
    var command = @" api https://api.github.com/repos/" + fullName;
    await Console.Out.WriteLineAsync(command);
    ProcessStartInfo pi = new ProcessStartInfo()
    {
        FileName = "gh",
        Arguments = command,
        RedirectStandardOutput = true,
    };
    var process = Process.Start(pi);
    var r = process.StandardOutput.ReadToEnd();
    process.WaitForExit();
    return r;
}

class Repo {
    public string? FullName { get; set; }
    public string? Name { get; set; }
    public string? Desc { get; set; }
    public string? Created { get; set; }
}
