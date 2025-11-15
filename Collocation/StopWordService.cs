namespace Collocation;

public interface IStopWordService
{
    bool IsStopword(string word);
}

public class StopWordService : IStopWordService
{
    private readonly Lazy<HashSet<string>> _stopWords;

    public StopWordService()
    {
        _stopWords = new Lazy<HashSet<string>>(LoadStopWords);
    }
    
    private HashSet<string> LoadStopWords()
    {
        var baseDirectory = AppContext.BaseDirectory;
        // https://github.com/skupriienko/Ukrainian-Stopwords/tree/master
        var filePath = Path.Combine(baseDirectory, "stopwords_ua.txt");

        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException(
                "File stopwords_ua.txt was not found.",
                filePath);
        }

        var lines = File.ReadAllLines(filePath);
        var stopWordSet = new HashSet<string>(
            lines.Where(line => !string.IsNullOrWhiteSpace(line))
                .Select(line => line.Trim().ToLower()),
            StringComparer.OrdinalIgnoreCase);

        return stopWordSet;
    }

    public bool IsStopword(string word)
    {
        return _stopWords.Value.Contains(word);
    }
}