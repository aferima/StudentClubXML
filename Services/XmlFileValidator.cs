using System.Xml.Linq;

namespace StudentClubsXml.Services;

public static class XmlFileValidator
{
    public static void Validate(string filePath)
    {
        var doc = XDocument.Load(filePath);

        if (doc.Root?.Name != "clubs" || !doc.Root.Elements("club").Any())
            throw new InvalidDataException("Файл не містить очікуваної структури /clubs/club.");
    }
}