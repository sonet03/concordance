using System.Text.RegularExpressions;
using Lemmatizer;
using NlpKwic.Components.Models;

namespace NlpKwic.Components.Services;

public class ConcordanceService(ILemmatizerService lemmatizer)
{
    private readonly Regex _wordRegex = new(@"\b[\w'-]+\b", RegexOptions.Compiled);
    private readonly Regex _sentenceRegex = new(@"(?<=[.!?])\s+", RegexOptions.Compiled);

    public ConcordanceAnalysis AnalyzeText(string textInput, string targetWord)
    {
        var analysis = new ConcordanceAnalysis();
        if (string.IsNullOrWhiteSpace(textInput) || string.IsNullOrWhiteSpace(targetWord))
        {
            return analysis;
        }

        var targetLemma = lemmatizer.Lemmatize(targetWord);
        
        var highlightMap = new Dictionary<(int, int), string>();
        var sentences = _sentenceRegex.Split(textInput);
        
        for (var s_index = 0; s_index < sentences.Length; s_index++)
        {
            var sentence = sentences[s_index];
            if (string.IsNullOrWhiteSpace(sentence)) continue;

            var matches = _wordRegex.Matches(sentence);
            var originalWords = matches.Cast<Match>().Select(m => m.Value).ToArray();
            if (originalWords.Length == 0) continue;

            List<string> lemmatizedWords = originalWords.Select(lemmatizer.Lemmatize).ToList();

            for (var i = 0; i < lemmatizedWords.Count; i++)
            {
                if (string.Equals(lemmatizedWords[i], targetLemma, StringComparison.OrdinalIgnoreCase))
                {
                    var hasLeft = i > 0;
                    var hasRight = i < originalWords.Length - 1;

                    var res = new ConcordanceResult
                    {
                        Left = hasLeft ? originalWords[i - 1] : null,
                        Middle = originalWords[i],
                        Right = hasRight ? originalWords[i + 1] : null
                    };
                    
                    analysis.Results.Add(res);
                    
                    highlightMap[(s_index, i)] = "ctx-middle";
                    if (hasLeft) highlightMap.TryAdd((s_index, i - 1), "ctx-left");
                    if (hasRight) highlightMap.TryAdd((s_index, i + 1), "ctx-right");
                }
            }
        }

        if (!analysis.Results.Any()) return analysis;

        for (var s_idx = 0; s_idx < sentences.Length; s_idx++)
        {
            var sentence = sentences[s_idx];
            var highlightedSentence = new HighlightedSentence();
            var matches = _wordRegex.Matches(sentence);
            var lastIndex = 0;

            for (var w_idx = 0; w_idx < matches.Count; w_idx++)
            {
                var match = matches[w_idx];
                
                if (match.Index > lastIndex)
                {
                    highlightedSentence.Tokens.Add(new HighlightedToken {
                        Text = sentence.Substring(lastIndex, match.Index - lastIndex),
                        CssClass = "" 
                    });
                }
                
                highlightMap.TryGetValue((s_idx, w_idx), out var cssClass);
                highlightedSentence.Tokens.Add(new HighlightedToken {
                    Text = match.Value,
                    CssClass = cssClass ?? ""
                });
                
                lastIndex = match.Index + match.Length;
            }
            
            if (lastIndex < sentence.Length)
            {
                highlightedSentence.Tokens.Add(new HighlightedToken {
                    Text = sentence.Substring(lastIndex),
                    CssClass = ""
                });
            }
            
            highlightedSentence.Tokens.Add(new HighlightedToken { Text = "\n", CssClass = "" }); 
            
            analysis.HighlightedSentences.Add(highlightedSentence);
        }

        return analysis;
    }
}