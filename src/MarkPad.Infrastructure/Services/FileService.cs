using System.Text;
using MarkPad.Application.Interfaces;

namespace MarkPad.Infrastructure.Services;

/// <summary>
/// File I/O service implementation (pure file system operations)
/// </summary>
public class FileService : IFileService
{
    public async Task<string> ReadTextAsync(string filePath)
    {
        return await File.ReadAllTextAsync(filePath, Encoding.UTF8);
    }
    
    public async Task WriteTextAsync(string filePath, string content)
    {
        await File.WriteAllTextAsync(filePath, content, Encoding.UTF8);
    }
}
