namespace NlpKwic.Components.Models;

public class ConcordanceAnalysis
{
    public List<ConcordanceResult> Results { get; set; } = new();
    public List<HighlightedSentence> HighlightedSentences { get; set; } = new();
}

public record ConcordanceResult
{
    public string? Left { get; init; }
    public string Middle { get; init; } = string.Empty;
    public string? Right { get; init; }
}
public class HighlightedSentence
{
    public List<HighlightedToken> Tokens { get; set; } = new();
}
public class HighlightedToken
{
    public string Text { get; init; } = string.Empty;
    public string CssClass { get; init; } = string.Empty;
}