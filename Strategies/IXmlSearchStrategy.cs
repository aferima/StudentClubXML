using StudentClubsXml.Models;

namespace StudentClubsXml.Strategies;

public interface IXmlSearchStrategy
{
    List<Club> Search(string filePath, SearchCriteria criteria);

    List<string> GetAttributeValues(string filePath, string attributeName);
}
