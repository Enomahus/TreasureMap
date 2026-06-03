using TreasureMap.Common;

namespace TreasureMap.Domain;

public class SimulationState
{
    public required Map Map { get; init; }
    public List<Mountain> Mountains { get; init; } = [];
    public List<Treasure> Treasures { get; init; } = [];
    public List<Adventurer> Adventurers { get; init; } = [];

    public void ProcessMove(Adventurer adventurer, Movement move)
    {
        if (move is Movement.G or Movement.D)
        {
            adventurer.Orientation = Turn(adventurer.Orientation, move);
            return;
        }

        if (move == Movement.A)
        {
            var nextPosition = GetNextPosition(adventurer.Position, adventurer.Orientation);

            if (CanMoveTo(nextPosition, adventurer))
            {
                adventurer.Position = nextPosition;
                TryCollectTreasure(nextPosition, adventurer);
            }
        }
    }

    public bool CanMoveTo(Position position, Adventurer currentAdventurer)
    {
        return IsWithinMapBounds(position)
            && !IsMountain(position)
            && !IsOccupiedByAnotherAdventurer(position, currentAdventurer);
    }

    public void TryCollectTreasure(Position position, Adventurer adventurer)
    {
        var treasure = Treasures.FirstOrDefault(t => t.Position == position && t.Count > 0);

        if (treasure != null)
        {
            adventurer.CollectedTreasures++;
            treasure.Count--;
        }
    }

    private bool IsWithinMapBounds(Position pos) =>
        pos.X >= 0 && pos.X < Map.Width && pos.Y >= 0 && pos.Y < Map.Height;

    private bool IsMountain(Position position) => Mountains.Any(m => m.Position == position);

    private bool IsOccupiedByAnotherAdventurer(Position position, Adventurer currentAdventurer) =>
        Adventurers.Any(a => a != currentAdventurer && a.Position == position);

    private static Orientation Turn(Orientation current, Movement turnDirection) =>
        (current, turnDirection) switch
        {
            (Orientation.N, Movement.G) => Orientation.W,
            (Orientation.N, Movement.D) => Orientation.E,
            (Orientation.E, Movement.G) => Orientation.N,
            (Orientation.E, Movement.D) => Orientation.S,
            (Orientation.S, Movement.G) => Orientation.E,
            (Orientation.S, Movement.D) => Orientation.W,
            (Orientation.W, Movement.G) => Orientation.S,
            (Orientation.W, Movement.D) => Orientation.N,
            _ => current,
        };

    private static Position GetNextPosition(Position current, Orientation orientation) =>
        orientation switch
        {
            Orientation.N => current with { Y = current.Y - 1 },
            Orientation.S => current with { Y = current.Y + 1 },
            Orientation.E => current with { X = current.X + 1 },
            Orientation.W => current with { X = current.X - 1 },
            _ => current,
        };
}
