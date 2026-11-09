using System.Xml.Xsl;

namespace StudentClubsXml.Services;

public class XmlTransformer
{
    private readonly XslCompiledTransform _transform = new();

    public void Transform(string xmlPath, string xslPath, string outputPath)
    {
        _transform.Load(xslPath);
        _transform.Transform(xmlPath, outputPath);
    }
}