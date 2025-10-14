using Markdig;
using MarkPad.Application.Interfaces;

namespace MarkPad.Infrastructure.Services;

/// <summary>
/// Markdig-based markdown renderer
/// </summary>
public class MarkdownRenderer : IMarkdownRenderer
{
    private readonly MarkdownPipeline _pipeline;
    
    public MarkdownRenderer()
    {
        // Configure pipeline with GitHub Flavored Markdown extensions
        _pipeline = new MarkdownPipelineBuilder()
            .UseAdvancedExtensions() // Tables, task lists, strikethrough, etc.
            .Build();
    }
    
    public string RenderToHtml(string markdown)
    {
        if (string.IsNullOrEmpty(markdown))
        {
            return string.Empty;
        }
        
        return Markdown.ToHtml(markdown, _pipeline);
    }
}
