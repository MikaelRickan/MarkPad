using MarkPad.Domain.Entities;

namespace MarkPad.Application.Interfaces;

/// <summary>
/// High-level service for document operations
/// </summary>
public interface IDocumentService
{
    /// <summary>
    /// Current document being edited
    /// </summary>
    Document CurrentDocument { get; }
    
    /// <summary>
    /// Creates a new blank document (checks for unsaved changes first)
    /// </summary>
    Task CreateNewAsync();
    
    /// <summary>
    /// Opens a document from file (with dialog)
    /// </summary>
    Task OpenAsync();
    
    /// <summary>
    /// Saves the current document
    /// </summary>
    Task SaveAsync();
    
    /// <summary>
    /// Saves the current document to a new location (with dialog)
    /// </summary>
    Task SaveAsAsync();
}
