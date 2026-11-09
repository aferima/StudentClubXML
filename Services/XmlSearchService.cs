using StudentClubsXml.Models;
using StudentClubsXml.Strategies;

namespace StudentClubsXml.Services;

public class XmlSearchService
{
    private IXmlSearchStrategy? _strategy;

    public void SetStrategy(IXmlSearchStrategy strategy) =>
        _strategy = strategy;

    public List<Club> Search(
        string filePath,
        SearchCriteria criteria) =>
        Strategy.Search(filePath, criteria);

    public List<string> GetAttributeValues(
        string filePath,
        string attributeName) =>
        Strategy.GetAttributeValues(filePath, attributeName);

    private IXmlSearchStrategy Strategy =>
        _strategy ?? throw new InvalidOperationException(
            "Метод аналізу не обрано.");
}
