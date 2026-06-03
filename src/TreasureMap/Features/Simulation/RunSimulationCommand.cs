using TreasureMap.Domain;

namespace TreasureMap.Features.Simulation;

public class RunSimulationCommand
{
    public required SimulationState InitialState { get; init; }
}

public class RunSimulationCommandHandler
{
    public static SimulationState Handler(RunSimulationCommand command)
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
                    state.ProcessMove(adventurer, move);
                }
            }
        }
        return state;
    }
}
