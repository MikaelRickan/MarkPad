namespace MarkPad.Application.Interfaces;

/// <summary>
/// Service for showing message boxes and confirmations
/// </summary>
public interface IMessageBoxService
{
    /// <summary>
    /// Shows a Yes/No/Cancel confirmation dialog
    /// </summary>
    /// <param name="title">Dialog title</param>
    /// <param name="message">Dialog message</param>
    /// <returns>User's choice: Yes, No, or Cancel</returns>
    Task<MessageBoxResult> ShowYesNoCancelAsync(string title, string message);
    
    /// <summary>
    /// Shows an error message dialog
    /// </summary>
    /// <param name="title">Dialog title</param>
    /// <param name="message">Error message</param>
    Task ShowErrorAsync(string title, string message);
}

/// <summary>
/// Result from a message box dialog
/// </summary>
public enum MessageBoxResult
{
    Yes,
    No,
    Cancel
}
