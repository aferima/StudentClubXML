using System.Xml;
using StudentClubsXml.Models;

namespace StudentClubsXml.Strategies;

public class DomSearchStrategy : IXmlSearchStrategy
{
    private const string Upper = "ABCDEFGHIJKLMNOPQRSTUVWXYZАБВГҐДЕЄЖЗИІЇЙКЛМНОПРСТУФХЦЧШЩЬЮЯ";

    private const string Lower = "abcdefghijklmnopqrstuvwxyzабвгґдеєжзиіїйклмнопрстуфхцчшщьюя";

    public List<Club> Search(string filePath, SearchCriteria criteria)
    {
        var doc = new XmlDocument();
        doc.Load(filePath);

        string xpath = "/clubs/club";

        xpath += Exact("@faculty", criteria.Faculty);
        xpath += Exact("@department", criteria.Department);
        xpath += Exact("@type", criteria.Type);
        xpath += Exact("@course", criteria.Course);
        xpath += Exact("meeting/@day", criteria.Day);

        xpath += Partial("@name", criteria.Name);
        xpath += Partial("@topic", criteria.Topic);
        xpath += Partial("leader/text()", criteria.Leader);

        var result = new List<Club>();

        var nodes = doc.SelectNodes(xpath);

        if (nodes == null)
            return result;

        foreach (XmlNode node in nodes)
        {
            var meeting = node.SelectSingleNode("meeting");

            var club = new Club
            {
                Id = node.Attributes?["id"]?.Value ?? "",
                Name = node.Attributes?["name"]?.Value ?? "",
                Faculty = node.Attributes?["faculty"]?.Value ?? "",
                Department = node.Attributes?["department"]?.Value ?? "",

                Type = node.Attributes?["type"]?.Value,
                Course = node.Attributes?["course"]?.Value,
                Topic = node.Attributes?["topic"]?.Value,

                Day = meeting?.Attributes?["day"]?.Value ?? "",
                Time = meeting?.Attributes?["time"]?.Value ?? "",

                Starosta = node.SelectSingleNode("starosta")?.InnerText ?? "",

                Leader = node.SelectSingleNode("leader")?.InnerText ?? ""
            };

            if (criteria.Matches(club))
            {
                result.Add(club);
            }
        }

        return result;
    }

    private static string Exact(string path, string? wanted)
    {
        if (string.IsNullOrWhiteSpace(wanted))
            return "";

        return
            $"[translate({path}, '{Upper}', '{Lower}') = " + $"{Literal(wanted.ToLowerInvariant())}]";
    }

    private static string Partial(string path, string? wanted)
    {
        if (string.IsNullOrWhiteSpace(wanted))
            return "";

        return
            $"[contains(translate({path}, '{Upper}', '{Lower}'), " + $"{Literal(wanted.ToLowerInvariant())})]";
    }

    private static string Literal(string value)
    {
        if (!value.Contains('\''))
            return $"'{value}'";

        if (!value.Contains('"'))
            return $"\"{value}\"";

        return "concat('" + value.Replace("'", "',\"'\",'") + "')";
    }

    public List<string> GetAttributeValues(string filePath, string attributeName)
    {
        var values = new HashSet<string>();

        var doc = new XmlDocument();
        doc.Load(filePath);

        var elements = doc.SelectNodes("/clubs/club | /clubs/club/meeting");

        if (elements == null)
            return new List<string>();

        foreach (XmlNode element in elements)
        {
            var value = element.Attributes?[attributeName]?.Value;

            if (!string.IsNullOrWhiteSpace(value))
            {
                values.Add(value);
            }
        }

        return values.OrderBy(v => v).ToList();
    }
}
