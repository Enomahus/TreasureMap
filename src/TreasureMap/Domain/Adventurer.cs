using TreasureMap.Common;

namespace TreasureMap.Domain;

public class Adventurer
{
    public required string Name { get; init; }
    public Position Position { get; set; }
    public Orientation Orientation { get; set; }
    public required Queue<Movement> Moves { get; init; }
    public int CollectedTreasures { get; set; } = 0;
}
