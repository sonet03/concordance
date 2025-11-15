using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace Lemmatizer;

public class LemmatizerService :ILemmatizerService
{
    private readonly Lazy<Dictionary<string, string>> _lemmaDictionary;

    public LemmatizerService()
    {
        _lemmaDictionary = new Lazy<Dictionary<string, string>>(LoadDictionary);
    }

    private Dictionary<string, string> LoadDictionary()
    {
        var baseDirectory = AppContext.BaseDirectory;
        
        var jsonFilePath = Path.Combine(baseDirectory, "lemmas.json");

        if (!File.Exists(jsonFilePath))
        {
            throw new FileNotFoundException(
                "File lemmas.json is not found! ",
                jsonFilePath);
        }

        Console.WriteLine($"Load dictionary from: {jsonFilePath}");
        var jsonContent = File.ReadAllText(jsonFilePath);
        
        var loadedDict = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonContent, 
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        return new Dictionary<string, string>(
            loadedDict ?? new Dictionary<string, string>(), 
            StringComparer.OrdinalIgnoreCase);
    }

    public string Lemmatize(string word)
    {
        var dictionary = _lemmaDictionary.Value;
        var cleanWord = word.Trim('\'', '`', '’');

        return dictionary.TryGetValue(cleanWord, out var lemma) ? lemma : cleanWord.ToLower();
    }
}