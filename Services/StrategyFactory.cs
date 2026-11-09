using StudentClubsXml.Models;
using StudentClubsXml.Strategies;

namespace StudentClubsXml.Services;

public class StrategyFactory
{
    public IXmlSearchStrategy Create(SearchType type)
    {
        return type switch
        {
            SearchType.Sax => new SaxSearchStrategy(),

            SearchType.Dom => new DomSearchStrategy(),

            SearchType.Linq => new LinqSearchStrategy(),

            _ => throw new ArgumentOutOfRangeException(
                nameof(type),
                type,
                "Невідомий метод аналізу")
        };
    }
}
