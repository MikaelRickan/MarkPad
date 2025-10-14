using MarkPad.Domain.Entities;

namespace MarkPad.Application.Interfaces;

/// <summary>
/// Manages multiple document instances and their lifecycle
/// </summary>
public interface IDocumentManagerService
{
    /// <summary>
    /// Gets the currently active document
    /// </summary>
    Document? ActiveDocument { get; }
    
    /// <summary>
    /// Creates a new document
    /// </summary>
    Document CreateNew();
    
    /// <summary>
    /// Opens a document via file dialog
    /// </summary>
    Task<Document?> OpenAsync();
    
    /// <summary>
    /// Opens a specific file
    /// </summary>
    Task<Document?> OpenFileAsync(string filePath);
    
    /// <summary>
    /// Saves the specified document
    /// </summary>
    Task SaveAsync(Document document);
    
    /// <summary>
    /// Saves the specified document with a new file name
    /// </summary>
    Task SaveAsAsync(Document document);
    
    /// <summary>
    /// Closes a document after checking for unsaved changes
    /// </summary>
    Task<bool> CloseDocumentAsync(Document document);
    
    /// <summary>
    /// Closes all documents after checking for unsaved changes
    /// </summary>
    Task<bool> CloseAllDocumentsAsync();
    
    /// <summary>
    /// Sets the currently active document
    /// </summary>
    void SetActiveDocument(Document document);
}
