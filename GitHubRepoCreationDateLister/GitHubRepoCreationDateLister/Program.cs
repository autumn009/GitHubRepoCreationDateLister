using System.Xml.Xsl;

if ( args.Length != 0) {
    Console.WriteLine("usage: gh repo list -L 1000|GitHubRepoCreationDateLister");
    return;
}

var list = new List<Repo>();

for(; ; ) {
    var s = Console.ReadLine();
    if (s == null) break;
    var d = s.Split('\t');
    list.Add(new Repo() { Name = getName(d[0]), Desc = d[1] });
}


foreach (var item in list) {
    Console.WriteLine($"{item.Name} {item.Desc}");
}

Console.WriteLine("Done");

string getName(string s) {
    int index = s.IndexOf("/");
    return s.Substring(index + 1);
}

class Repo {
    public string? Name { get; set; }
    public string? Desc { get; set; }
    public string? Created { get; set; }
}
