using System.Xml;
using System.Xml.Xsl;
using StudentClubsXml.Models;

namespace StudentClubsXml.Services;

public class XmlTransformer
{
    private readonly XslCompiledTransform _transform = new();

    public void Transform(
        string xmlPath,
        string xslPath,
        string outputPath,
        SearchCriteria criteria)
    {
        _transform.Load(xslPath);

        var args = new XsltArgumentList();
        args.AddParam("name", "", Value(criteria.Name));
        args.AddParam("faculty", "", Value(criteria.Faculty));
        args.AddParam("department", "", Value(criteria.Department));
        args.AddParam("type", "", Value(criteria.Type));
        args.AddParam("course", "", Value(criteria.Course));
        args.AddParam("day", "", Value(criteria.Day));
        args.AddParam("topic", "", Value(criteria.Topic));
        args.AddParam("leader", "", Value(criteria.Leader));

        using var writer = XmlWriter.Create(outputPath, _transform.OutputSettings);
        _transform.Transform(xmlPath, args, writer);
    }

    private static string Value(string? value) =>
        string.IsNullOrWhiteSpace(value) ? "" : value;
}
