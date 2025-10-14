namespace MarkPad.Application.Interfaces;

/// <summary>
/// Service for file system operations (read/write only)
/// </summary>
public interface IFileService
{
    /// <summary>
    /// Reads text content from a file
    /// </summary>
    Task<string> ReadTextAsync(string filePath);
    
    /// <summary>
    /// Writes text content to a file
    /// </summary>
    Task WriteTextAsync(string filePath, string content);
}
