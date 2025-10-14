namespace MarkPad.Application.Interfaces;

/// <summary>
/// Service for showing file dialogs (Open/Save)
/// </summary>
public interface IDialogService
{
    /// <summary>
    /// Shows an open file dialog and returns the selected file path
    /// </summary>
    /// <returns>Selected file path or null if cancelled</returns>
    Task<string?> ShowOpenFileDialogAsync();
    
    /// <summary>
    /// Shows a save file dialog and returns the selected file path
    /// </summary>
    /// <param name="defaultFileName">Suggested file name</param>
    /// <returns>Selected file path or null if cancelled</returns>
    Task<string?> ShowSaveFileDialogAsync(string? defaultFileName = null);
}
