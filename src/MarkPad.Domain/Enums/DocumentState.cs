namespace MarkPad.Domain.Enums;

/// <summary>
/// Represents the current state of a document
/// </summary>
public enum DocumentState
{
    /// <summary>
    /// New document that has never been saved
    /// </summary>
    New,
    
    /// <summary>
    /// Document has been saved and matches the file on disk
    /// </summary>
    Saved,
    
    /// <summary>
    /// Document has unsaved changes
    /// </summary>
    Modified
}
