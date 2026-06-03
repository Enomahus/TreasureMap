using TreasureMap.Common;
using TreasureMap.Domain;

namespace TreasureMap.Features.Simulation;

public class RunSimulationCommand
{
    public required SimulationState InitialState { get; init; }
}

public class RunSimulationCommandHandler
{
    public SimulationState Handler(RunSimulationCommand command)
    {
        var state = command.InitialState;
        bool hasMoreMoves = true;

        while (hasMoreMoves)
        {
            hasMoreMoves = false;

            foreach (var adventurer in state.Adventurers)
            {
                if (adventurer.Moves.TryDequeue(out var move))
                {
                    hasMoreMoves = true;
                    //ProcessMove(adventurer, move, state);
                    state.ProcessMove(adventurer, move);
                }
            }
        }
        return state;
    }

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

    private static void ProcessMove(Adventurer adventurer, Movement move, SimulationState state)
    {
        if (move is Movement.G or Movement.D)
        {
            adventurer.Orientation = Turn(adventurer.Orientation, move);
            return;
        }

        if (move == Movement.A)
        {
            TryMoveForward(adventurer, state);
        }
    }

    private static void TryMoveForward(Adventurer adventurer, SimulationState state)
    {
        var nextPosition = GetNextPosition(adventurer.Position, adventurer.Orientation);

        if (!CanMoveTo(nextPosition, state, adventurer))
        {
            return; // Le mouvement est bloqué
        }

        // Déplacement effectif
        adventurer.Position = nextPosition;

        // Interaction avec l'environnement
        TryCollectTreasure(nextPosition, adventurer, state);
    }

    private static bool CanMoveTo(
        Position position,
        SimulationState state,
        Adventurer currentAdventurer
    )
    {
        return IsWithinMapBounds(position, state.Map)
            && !IsMountain(position, state)
            && !IsOccupiedByAnotherAdventurer(position, state, currentAdventurer);
    }

    private static bool IsWithinMapBounds(Position pos, Map map)
    {
        return pos.X >= 0 && pos.X < map.Width && pos.Y >= 0 && pos.Y < map.Height;
    }

    private static bool IsMountain(Position position, SimulationState state)
    {
        return state.Mountains.Any(m => m.Position == position);
    }

    private static bool IsOccupiedByAnotherAdventurer(
        Position position,
        SimulationState state,
        Adventurer currentAdventurer
    )
    {
        return state.Adventurers.Any(a => a != currentAdventurer && a.Position == position);
    }

    private static void TryCollectTreasure(
        Position position,
        Adventurer adventurer,
        SimulationState state
    )
    {
        var treasure = state.Treasures.FirstOrDefault(t => t.Position == position && t.Count > 0);

        if (treasure != null)
        {
            adventurer.CollectedTreasures++;
            treasure.Count--;
        }
    }
}
