using TreasureMap.Common;
using TreasureMap.Domain;

namespace TreasureMap.Features.IO;

public static class MapParser
{
    public static SimulationState Parse(string[] lines)
    {
        Map? map = null;
        List<Mountain> mountains = [];
        List<Treasure> treasures = [];
        List<Adventurer> adventurers = [];

        foreach (var line in lines)
        {
            var cleanLine = line.Trim(' ', '"', ',');
            if (string.IsNullOrWhiteSpace(cleanLine) || cleanLine.StartsWith('#'))
                continue;

            var parts = cleanLine.Split('-');
            switch (parts[0].Trim())
            {
                case "C":
                    map = new Map(int.Parse(parts[1]), int.Parse(parts[2]));
                    break;

                case "M":
                    mountains.Add(
                        new Mountain(new Position(int.Parse(parts[1]), int.Parse(parts[2])))
                    );
                    break;

                case "T":
                    treasures.Add(
                        new Treasure
                        {
                            Position = new Position(int.Parse(parts[1]), int.Parse(parts[2])),
                            Count = int.Parse(parts[3]),
                        }
                    );
                    break;

                case "A":
                    adventurers.Add(
                        new Adventurer
                        {
                            Name = parts[1].Trim(),
                            Position = new Position(int.Parse(parts[2]), int.Parse(parts[3])),
                            Orientation = Enum.Parse<Orientation>(parts[4].Trim()),
                            Moves = new Queue<Movement>(
                                parts[5].Trim().Select(c => Enum.Parse<Movement>(c.ToString()))
                            ),
                        }
                    );
                    break;
            }
        }

        return new SimulationState
        {
            Map = map ?? throw new InvalidOperationException("La carte est manquante."),
            Mountains = mountains,
            Treasures = treasures,
            Adventurers = adventurers,
        };
    }

    public static string Serialize(SimulationState state)
    {
        List<string> lines = [$"C - {state.Map.Width} - {state.Map.Height}"];

        foreach (var m in state.Mountains)
            lines.Add($"M - {m.Position.X} - {m.Position.Y}");

        foreach (var t in state.Treasures.Where(t => t.Count > 0))
            lines.Add($"T - {t.Position.X} - {t.Position.Y} - {t.Count}");

        foreach (var a in state.Adventurers)
            lines.Add(
                $"A - {a.Name} - {a.Position.X} - {a.Position.Y} - {a.Orientation} - {a.CollectedTreasures}"
            );

        return string.Join(Environment.NewLine, lines);
    }
}
