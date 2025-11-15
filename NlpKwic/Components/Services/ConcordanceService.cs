using System.Text.RegularExpressions;
using Lemmatizer;
using NlpKwic.Components.Models;

namespace NlpKwic.Components.Services;

public class ConcordanceService(ILemmatizerService lemmatizer)
{
    private readonly Regex _wordRegex = new(@"\b[\w'-]+\b", RegexOptions.Compiled);

    private readonly Regex _sentenceRegex = new(@"(?<=[.!?])\s+", RegexOptions.Compiled);
    
    public List<ConcordanceResult> GenerateConcordance(string textInput, string targetWord)
    {
        var results = new List<ConcordanceResult>();

        if (string.IsNullOrWhiteSpace(textInput) || string.IsNullOrWhiteSpace(targetWord))
        {
            return results;
        }
        
        var targetLemma = lemmatizer.Lemmatize(targetWord);

        var sentences = _sentenceRegex.Split(textInput);

        foreach (var sentence in sentences)
        {
            if (string.IsNullOrWhiteSpace(sentence)) continue;

            var matches = _wordRegex.Matches(sentence);
            var words = matches.Select(m => m.Value).ToArray();

            if (words.Length == 0) continue;
            
            var lemmatizedWords = words.Select(lemmatizer.Lemmatize).ToList();

            for (var i = 0; i < lemmatizedWords.Count; i++)
            {
                if (string.Equals(lemmatizedWords[i], targetLemma, StringComparison.OrdinalIgnoreCase))
                {
                    var leftWord = (i > 0) 
                        ? words[i - 1] 
                        : "--- [ПОЧАТОК РЕЧЕННЯ] ---";
                    
                    var rightWord = (i < words.Length - 1) 
                        ? words[i + 1] 
                        : "--- [КІНЕЦЬ РЕЧЕННЯ] ---";

                    results.Add(new ConcordanceResult
                    {
                        Left = leftWord,
                        Middle = words[i],
                        Right = rightWord
                    });
                }
            }
        }

        return results;
    }
}