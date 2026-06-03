using TreasureMap.Features.IO;
using TreasureMap.Features.Simulation;

// 1. Lecture du fichier physique
var inputLines = await File.ReadAllLinesAsync("input.txt");

// 2. Parsing (Feature IO)
var initialState = MapParser.Parse(inputLines);

// 3. Exécution de la simulation (Feature Simulation)
var handler = new RunSimulationCommandHandler();
var command = new RunSimulationCommand { InitialState = initialState };
var finalState = handler.Handler(command);

// 4. Sérialisation de l'état final (Feature IO) <-- C'EST ICI QU'ELLE EST UTILISÉE
var outputContent = MapParser.Serialize(finalState);

// 5. Écriture du fichier physique
await File.WriteAllTextAsync("output.txt", outputContent);
