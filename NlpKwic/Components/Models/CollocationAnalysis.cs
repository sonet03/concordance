namespace NlpKwic.Components.Models;

public class CollocationAnalysis
{
    public List<CollocationItem> LeftCollocations { get; set; } = new();
    public List<CollocationItem> RightCollocations { get; set; } = new();
}