using TreasureMap.Common;
using TreasureMap.Domain;
using TreasureMap.Features.IO;

namespace TreasureMap.Tests.Features.IO
{
    public class MapParserTests
    {
        [Fact]
        public void Parse_WhenValidInput_ShouldCreateCorrectSimulationState()
        {
            // Arrange : Un jeu de données simulant le fichier d'entrée
            var inputLines = new[]
            {
                "# Voici un commentaire qui doit être ignoré",
                "C - 3 - 4",
                "M - 1 - 0",
                "T - 0 - 3 - 2",
                "A - Indiana - 1 - 1 - S - AADADA",
                "   ", // Ligne vide avec des espaces qui doit être ignorée
                "# Fin du fichier",
            };

            // Act
            var state = MapParser.Parse(inputLines);

            // Assert : Vérification de la carte
            Assert.NotNull(state.Map);
            Assert.Equal(3, state.Map.Width);
            Assert.Equal(4, state.Map.Height);

            // Assert : Vérification des montagnes
            Assert.Single(state.Mountains);
            Assert.Equal(1, state.Mountains[0].Position.X);
            Assert.Equal(0, state.Mountains[0].Position.Y);

            // Assert : Vérification des trésors
            Assert.Single(state.Treasures);
            Assert.Equal(0, state.Treasures[0].Position.X);
            Assert.Equal(3, state.Treasures[0].Position.Y);
            Assert.Equal(2, state.Treasures[0].Count);

            // Assert : Vérification des aventuriers et de leur séquence de mouvements
            Assert.Single(state.Adventurers);
            var adventurer = state.Adventurers[0];
            Assert.Equal("Indiana", adventurer.Name);
            Assert.Equal(1, adventurer.Position.X);
            Assert.Equal(1, adventurer.Position.Y);
            Assert.Equal(Orientation.S, adventurer.Orientation);

            // Vérification de la séquence "AADADA" transformée en Queue<Movement>
            var moves = adventurer.Moves.ToArray();
            Assert.Equal(6, moves.Length);
            Assert.Equal(Movement.A, moves[0]);
            Assert.Equal(Movement.A, moves[1]);
            Assert.Equal(Movement.D, moves[2]);
            Assert.Equal(Movement.A, moves[3]);
            Assert.Equal(Movement.D, moves[4]);
            Assert.Equal(Movement.A, moves[5]);
        }

        [Fact]
        public void Parse_WhenMissingMapDefinition_ShouldThrowInvalidOperationException()
        {
            // Arrange : Fichier invalide sans définition de la carte "C"
            var inputLines = new[]
            {
                "M - 1 - 0",
                "T - 0 - 3 - 2",
                "A - Lara - 1 - 1 - S - AADADA",
            };

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() =>
                MapParser.Parse(inputLines)
            );
            Assert.Equal("La carte est manquante.", exception.Message);
        }

        [Fact]
        public void Serialize_ValidState_ShouldGenerateCorrectStringFormat()
        {
            // Arrange : Création d'un état de simulation (fin de partie)
            var state = new SimulationState
            {
                Map = new Map(3, 4),
                Mountains = [new Mountain(new Position(1, 0)), new Mountain(new Position(2, 1))],
                Treasures =
                [
                    new Treasure { Position = new Position(1, 3), Count = 2 },
                    new Treasure { Position = new Position(0, 3), Count = 0 },
                ],
                Adventurers =
                [
                    new Adventurer
                    {
                        Name = "Lara",
                        Position = new Position(0, 3),
                        Orientation = Orientation.S,
                        Moves = new Queue<Movement>(),
                        CollectedTreasures = 3,
                    },
                ],
            };

            // Act
            var result = MapParser.Serialize(state);

            // Assert
            var expectedLines = new[]
            {
                "C - 3 - 4",
                "M - 1 - 0",
                "M - 2 - 1",
                "T - 1 - 3 - 2",
                "A - Lara - 0 - 3 - S - 3",
            };
            var expectedString = string.Join(Environment.NewLine, expectedLines);

            Assert.Equal(expectedString, result);

            // On s'assure spécifiquement que le trésor à 0 n'est pas présent dans la chaîne finale
            Assert.DoesNotContain("T - 0 - 3 - 0", result);
        }
    }
}
