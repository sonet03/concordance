using Lemmatizer;
using NlpKwic.Components.Models;

namespace Collocation;

public class CollocationService(ILemmatizerService lemmatizer, IStopWordService stopWordService)
{
    public CollocationAnalysis GenerateCollocations(List<ConcordanceResult> results, int topN = 5)
    {
        var analysis = new CollocationAnalysis();
        if (!results.Any())
        {
            return analysis;
        }

        var leftWordforms = results
            .Where(r => r.Left != null) 
            .Select(r => r.Left!);

        var rightWordforms = results
            .Where(r => r.Right != null)
            .Select(r => r.Right!);

        analysis.LeftCollocations = GroupAndLemmatize(leftWordforms, topN);
        analysis.RightCollocations = GroupAndLemmatize(rightWordforms, topN);

        return analysis;
    }

    private List<CollocationItem> GroupAndLemmatize(IEnumerable<string> wordforms, int topN)
    {
        return wordforms
            .GroupBy(wf => wf.ToLower())
            .Select(group => new 
            { 
                Count = group.Count(), 
                Lemma = lemmatizer.Lemmatize(group.Key) 
            })
            .Where(item => !stopWordService.IsStopword(item.Lemma))
            .GroupBy(item => item.Lemma)
            .Select(lemmaGroup => new CollocationItem(
                lemmaGroup.Key,
                lemmaGroup.Sum(item => item.Count) 
            ))
            .OrderByDescending(item => item.Count)
            .Take(topN)
            .ToList();
    }
}