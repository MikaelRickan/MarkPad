namespace MarkPad.Application.Interfaces;

/// <summary>
/// Service for rendering markdown to HTML
/// </summary>
public interface IMarkdownRenderer
{
    /// <summary>
    /// Converts markdown text to HTML
    /// </summary>
    string RenderToHtml(string markdown);
}
