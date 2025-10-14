using System.Threading.Tasks;
using Avalonia.Platform.Storage;
using MarkPad.Application.Interfaces;

namespace MarkPad.Desktop.Services;

/// <summary>
/// Avalonia-based dialog service implementation
/// </summary>
public class AvaloniaDialogService : IDialogService
{
    private readonly IStorageProvider _storageProvider;
    
    public AvaloniaDialogService(IStorageProvider storageProvider)
    {
        _storageProvider = storageProvider;
    }
    
    public async Task<string?> ShowOpenFileDialogAsync()
    {
        var options = new FilePickerOpenOptions
        {
            Title = "Open Markdown File",
            AllowMultiple = false,
            FileTypeFilter = new[]
            {
                new FilePickerFileType("Markdown Files")
                {
                    Patterns = new[] { "*.md", "*.markdown" }
                },
                new FilePickerFileType("Text Files")
                {
                    Patterns = new[] { "*.txt" }
                },
                new FilePickerFileType("All Files")
                {
                    Patterns = new[] { "*.*" }
                }
            }
        };
        
        var files = await _storageProvider.OpenFilePickerAsync(options);
        return files.Count > 0 ? files[0].Path.LocalPath : null;
    }
    
    public async Task<string?> ShowSaveFileDialogAsync(string? defaultFileName = null)
    {
        var options = new FilePickerSaveOptions
        {
            Title = "Save Markdown File",
            SuggestedFileName = defaultFileName ?? "Untitled.md",
            FileTypeChoices = new[]
            {
                new FilePickerFileType("Markdown Files")
                {
                    Patterns = new[] { "*.md" }
                },
                new FilePickerFileType("Text Files")
                {
                    Patterns = new[] { "*.txt" }
                }
            }
        };
        
        var file = await _storageProvider.SaveFilePickerAsync(options);
        return file?.Path.LocalPath;
    }
}
