using TreasureMap.Features.IO;
using TreasureMap.Features.Simulation;

string executionDir = AppContext.BaseDirectory;
string projectRootDir = Path.GetFullPath(Path.Combine(executionDir, "..", "..", ".."));

string inputFilePath = Path.Combine(projectRootDir, "inputFile.txt");
string outputFilePath = Path.Combine(projectRootDir, "output.txt");

if (!File.Exists(inputFilePath))
{
    throw new FileNotFoundException(
        $"Le fichier spécifié est introuvable à la racine du projet : {inputFilePath}",
        inputFilePath
    );
}

var inputLines = await File.ReadAllLinesAsync(inputFilePath);

var initialState = MapParser.Parse(inputLines);

var handler = new RunSimulationCommandHandler();
var command = new RunSimulationCommand { InitialState = initialState };
var finalState = handler.Handler(command);

var outputContent = MapParser.Serialize(finalState);

Console.WriteLine("--- ÉTAT FINAL DE LA SIMULATION ---");
Console.WriteLine(outputContent);
Console.WriteLine("-----------------------------------");

// 5. Écriture du fichier physique
await File.WriteAllTextAsync(outputFilePath, outputContent);

Console.WriteLine($"Fichier de sortie généré avec succès à l'emplacement : {outputFilePath}");
