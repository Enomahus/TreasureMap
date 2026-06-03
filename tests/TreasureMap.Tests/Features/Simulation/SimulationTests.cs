using TreasureMap.Common;
using TreasureMap.Domain;
using TreasureMap.Features.IO;
using TreasureMap.Features.Simulation;

namespace TreasureMap.Tests.Features.Simulation
{
    public class SimulationTests
    {
        [Fact]
        public void RunSimulation_WithLaraExample_ShouldReturnCorrectState()
        {
            var inputLines = new[]
            {
                "C - 3 - 4",
                "M - 1 - 0",
                "M - 2 - 1",
                "T - 0 - 3 - 2",
                "T - 1 - 3 - 3",
                "A - Lara - 1 - 1 - S - AADADAGGA",
            };

            var initialState = MapParser.Parse(inputLines);
            var handler = new RunSimulationCommandHandler();
            var command = new RunSimulationCommand { InitialState = initialState };

            // Act
            var finalState = handler.Handler(command);

            // Assert
            var lara = finalState.Adventurers.First();
            Assert.Equal("Lara", lara.Name);
            Assert.Equal(0, lara.Position.X);
            Assert.Equal(3, lara.Position.Y);
            Assert.Equal(Orientation.S, lara.Orientation);
            Assert.Equal(3, lara.CollectedTreasures);

            //Format attendu
            var output = MapParser.Serialize(finalState);
            Assert.Contains("C - 3 - 4", output);
            Assert.Contains("M - 1 - 0", output);
            Assert.Contains("M - 2 - 1", output);
            Assert.Contains("T - 1 - 3 - 2", output); // Il reste 2 trésors sur (1,3)
            Assert.Contains("A - Lara - 0 - 3 - S - 3", output);
        }

        [Fact]
        public void MountainCollision_ShouldIgnoreMove()
        {
            // Arrange
            var state = new SimulationState
            {
                Map = new Map(3, 4),
                Mountains = [new Mountain(new Position(1, 1))],
                Adventurers =
                [
                    new Adventurer
                    {
                        Name = "Indiana",
                        Position = new Position(1, 2),
                        Orientation = Orientation.N,
                        Moves = new Queue<Movement>([Movement.A]),
                    },
                ],
            };

            var handler = new RunSimulationCommandHandler();

            // Act
            var result = handler.Handler(new RunSimulationCommand { InitialState = state });

            // Assert
            Assert.Equal(1, result.Adventurers[0].Position.X);
            Assert.Equal(2, result.Adventurers[0].Position.Y);
        }
    }
}
