using System.Text.Json;

var lemmaDictionary = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

// https://github.com/michmech/lemmatization-lists/blob/master/lemmatization-uk.txt
var inputFilePath = "lemmatization-uk.txt"; 
var linesProcessed = 0;
var duplicatesSkipped = 0;

foreach (var line in File.ReadLines(inputFilePath))
{
    if (string.IsNullOrWhiteSpace(line)) continue;

    var parts = line.Split('\t');
    
    if (parts.Length == 2)
    {
        var lemma = parts[0].Trim();
        var wordform = parts[1].Trim();

        if (string.IsNullOrEmpty(wordform)) continue;

        if (!lemmaDictionary.TryAdd(wordform, lemma))
        {
            duplicatesSkipped++;
        }
        
        linesProcessed++;
    }
}

Console.WriteLine($"Processed lines: {linesProcessed}");
Console.WriteLine($"Skipped duplicates: {duplicatesSkipped}");
Console.WriteLine($"Dictionary length: {lemmaDictionary.Count}");

var outputJson = JsonSerializer.Serialize(lemmaDictionary);
File.WriteAllText("lemmas.json", outputJson);