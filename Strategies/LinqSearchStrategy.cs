using System.Xml.Linq;
using StudentClubsXml.Models;

namespace StudentClubsXml.Strategies;

public class LinqSearchStrategy : IXmlSearchStrategy
{
    public List<Club> Search(string filePath, SearchCriteria criteria)
    {
        var doc = XDocument.Load(filePath);
        IEnumerable<XElement> query = doc.Root?.Elements("club") ?? Enumerable.Empty<XElement>();

        if (!string.IsNullOrWhiteSpace(criteria.Faculty))
            query = query.Where(e => Equal((string?)e.Attribute("faculty"), criteria.Faculty));
        if (!string.IsNullOrWhiteSpace(criteria.Department))
            query = query.Where(e => Equal((string?)e.Attribute("department"), criteria.Department));
        if (!string.IsNullOrWhiteSpace(criteria.Type))
            query = query.Where(e => Equal((string?)e.Attribute("type"), criteria.Type));
        if (!string.IsNullOrWhiteSpace(criteria.Course))
            query = query.Where(e => Equal((string?)e.Attribute("course"), criteria.Course));
        if (!string.IsNullOrWhiteSpace(criteria.Day))
            query = query.Where(e => Equal((string?)e.Element("meeting")?.Attribute("day"), criteria.Day));
        if (!string.IsNullOrWhiteSpace(criteria.Name))
            query = query.Where(e => Contains((string?)e.Attribute("name"), criteria.Name));
        if (!string.IsNullOrWhiteSpace(criteria.Topic))
            query = query.Where(e => Contains((string?)e.Attribute("topic"), criteria.Topic));
        if (!string.IsNullOrWhiteSpace(criteria.Leader))
            query = query.Where(e => Contains((string?)e.Element("leader"), criteria.Leader));

        return query.Select(c => new Club
        {
            Id = (string?)c.Attribute("id") ?? "",
            Name = (string?)c.Attribute("name") ?? "",
            Faculty = (string?)c.Attribute("faculty") ?? "",
            Department = (string?)c.Attribute("department") ?? "",
            Type = (string?)c.Attribute("type"),
            Course = (string?)c.Attribute("course"),
            Topic = (string?)c.Attribute("topic"),
            Day = (string?)c.Element("meeting")?.Attribute("day") ?? "",
            Time = (string?)c.Element("meeting")?.Attribute("time") ?? "",
            Starosta = (string?)c.Element("starosta") ?? "",
            Leader = (string?)c.Element("leader") ?? ""
        })
            .Where(criteria.Matches)
            .ToList();
    }

    private static bool Equal(string? actual, string? wanted) =>
        string.Equals(actual, wanted, StringComparison.OrdinalIgnoreCase);

    private static bool Contains(string? actual, string? wanted) =>
        actual?.Contains(wanted!, StringComparison.OrdinalIgnoreCase) ?? false;

    public List<string> GetAttributeValues(string filePath, string attributeName)
    {
        var doc = XDocument.Load(filePath);
        return doc.Descendants()
            .Where(e => e.Name == "club" || e.Name == "meeting")
            .Select(e => (string?)e.Attribute(attributeName))
            .Where(v => !string.IsNullOrWhiteSpace(v))
            .Select(v => v!)
            .Distinct()
            .OrderBy(v => v)
            .ToList();
    }
}
