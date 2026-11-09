using System.Xml;
using StudentClubsXml.Models;

namespace StudentClubsXml.Strategies;

public class SaxSearchStrategy : IXmlSearchStrategy
{
    private static readonly XmlReaderSettings Settings = new()
    {
        IgnoreWhitespace = true,
        IgnoreComments = true,
        DtdProcessing = DtdProcessing.Prohibit
    };

    public List<Club> Search(string filePath, SearchCriteria criteria)
    {
        var results = new List<Club>();
        var conditions = BuildConditions(criteria);
        Club? current = null;
        string? textElement = null;

        using var reader = XmlReader.Create(filePath, Settings);
        while (reader.Read())
        {
            if (reader.NodeType == XmlNodeType.Element)
            {
                if (reader.Name == "club")
                {
                    current = new Club
                    {
                        Id = reader.GetAttribute("id") ?? "",
                        Name = reader.GetAttribute("name") ?? "",
                        Faculty = reader.GetAttribute("faculty") ?? "",
                        Department = reader.GetAttribute("department") ?? "",
                        Type = reader.GetAttribute("type"),
                        Course = reader.GetAttribute("course"),
                        Topic = reader.GetAttribute("topic")
                    };
                    if (reader.IsEmptyElement)
                    {
                        if (conditions.All(test => test(current))) results.Add(current);
                        current = null;
                    }
                }
                else if (reader.Name == "meeting" && current != null)
                {
                    current.Day = reader.GetAttribute("day") ?? "";
                    current.Time = reader.GetAttribute("time") ?? "";
                }
                else if (reader.Name == "starosta" || reader.Name == "leader")
                {
                    textElement = reader.IsEmptyElement ? null : reader.Name;
                }
            }
            else if ((reader.NodeType == XmlNodeType.Text || reader.NodeType == XmlNodeType.CDATA) && current != null)
            {
                if (textElement == "starosta") current.Starosta += reader.Value;
                if (textElement == "leader") current.Leader += reader.Value;
            }
            else if (reader.NodeType == XmlNodeType.EndElement)
            {
                if (reader.Name == "starosta" || reader.Name == "leader") textElement = null;
                if (reader.Name == "club" && current != null)
                {
                    if (conditions.All(test => test(current))) results.Add(current);
                    current = null;
                    textElement = null;
                }
            }
        }
        return results;
    }

    private static List<Func<Club, bool>> BuildConditions(SearchCriteria c)
    {
        var conditions = new List<Func<Club, bool>>();
        if (!string.IsNullOrWhiteSpace(c.Name)) conditions.Add(x => Contains(x.Name, c.Name));
        if (!string.IsNullOrWhiteSpace(c.Faculty)) conditions.Add(x => Equal(x.Faculty, c.Faculty));
        if (!string.IsNullOrWhiteSpace(c.Department)) conditions.Add(x => Equal(x.Department, c.Department));
        if (!string.IsNullOrWhiteSpace(c.Type)) conditions.Add(x => Equal(x.Type, c.Type));
        if (!string.IsNullOrWhiteSpace(c.Course)) conditions.Add(x => Equal(x.Course, c.Course));
        if (!string.IsNullOrWhiteSpace(c.Day)) conditions.Add(x => Equal(x.Day, c.Day));
        if (!string.IsNullOrWhiteSpace(c.Topic)) conditions.Add(x => Contains(x.Topic, c.Topic));
        if (!string.IsNullOrWhiteSpace(c.Leader)) conditions.Add(x => Contains(x.Leader, c.Leader));
        return conditions;
    }

    private static bool Equal(string? a, string? b) => string.Equals(a, b, StringComparison.OrdinalIgnoreCase);
    private static bool Contains(string? a, string? b) => a?.Contains(b!, StringComparison.OrdinalIgnoreCase) ?? false;

    public List<string> GetAttributeValues(string filePath, string attributeName)
    {
        var values = new HashSet<string>();
        string target = attributeName is "day" or "time" ? "meeting" : "club";
        using var reader = XmlReader.Create(filePath, Settings);
        while (reader.Read())
        {
            if (reader.NodeType != XmlNodeType.Element || reader.Name != target) continue;
            var value = reader.GetAttribute(attributeName);
            if (!string.IsNullOrWhiteSpace(value)) values.Add(value);
        }
        return values.OrderBy(v => v).ToList();
    }
}
