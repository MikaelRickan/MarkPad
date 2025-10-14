using MarkPad.Application.Interfaces;
using MarkPad.Domain.Entities;

namespace MarkPad.Application.Services;

/// <summary>
/// Manages multiple document instances and their lifecycle
/// </summary>
public class DocumentManagerService : IDocumentManagerService
{
    private readonly IFileService _fileService;
    private readonly IDialogService _dialogService;
    private readonly IMessageBoxService _messageBoxService;
    
    public Document? ActiveDocument { get; private set; }
    
    public DocumentManagerService(
        IFileService fileService,
        IDialogService dialogService,
        IMessageBoxService messageBoxService)
    {
        _fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
        _dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));
        _messageBoxService = messageBoxService ?? throw new ArgumentNullException(nameof(messageBoxService));
    }
    
    public Document CreateNew()
    {
        var document = new Document();
        ActiveDocument = document;
        return document;
    }
    
    public async Task<Document?> OpenAsync()
    {
        var filePath = await _dialogService.ShowOpenFileDialogAsync();
        
        if (filePath == null)
        {
            return null; // User cancelled
        }
        
        return await OpenFileAsync(filePath);
    }
    
    public async Task<Document?> OpenFileAsync(string filePath)
    {
        try
        {
            var content = await _fileService.ReadTextAsync(filePath);
            var document = new Document();
            document.Load(filePath, content);
            ActiveDocument = document;
            return document;
        }
        catch (Exception ex)
        {
            await _messageBoxService.ShowErrorAsync("Error Opening File",
                $"Failed to open file: {ex.Message}");
            return null;
        }
    }
    
    public async Task SaveAsync(Document document)
    {
        if (document == null)
            throw new ArgumentNullException(nameof(document));
        
        if (document.FilePath == null)
        {
            await SaveAsAsync(document);
            return;
        }
        
        try
        {
            await _fileService.WriteTextAsync(document.FilePath.Value, document.Content);
            document.Save();
        }
        catch (Exception ex)
        {
            await _messageBoxService.ShowErrorAsync("Error Saving File",
                $"Failed to save file: {ex.Message}");
        }
    }
    
    public async Task SaveAsAsync(Document document)
    {
        if (document == null)
            throw new ArgumentNullException(nameof(document));
        
        var fileName = document.GetDisplayName();
        var filePath = await _dialogService.ShowSaveFileDialogAsync(fileName);
        
        if (filePath == null)
        {
            return; // User cancelled
        }
        
        try
        {
            await _fileService.WriteTextAsync(filePath, document.Content);
            document.Save(filePath);
        }
        catch (Exception ex)
        {
            await _messageBoxService.ShowErrorAsync("Error Saving File",
                $"Failed to save file: {ex.Message}");
        }
    }
    
    public async Task<bool> CloseDocumentAsync(Document document)
    {
        if (document == null)
            throw new ArgumentNullException(nameof(document));
        
        // Check for unsaved changes
        if (document.HasUnsavedChanges)
        {
            var displayName = document.GetDisplayName();
            var result = await _messageBoxService.ShowYesNoCancelAsync(
                "Unsaved Changes",
                $"'{displayName}' has unsaved changes. Do you want to save them?");
            
            if (result == MessageBoxResult.Yes)
            {
                await SaveAsync(document);
                // Check if save was successful (user might have cancelled)
                if (document.HasUnsavedChanges)
                {
                    return false; // Save was cancelled
                }
            }
            else if (result == MessageBoxResult.Cancel)
            {
                return false; // User cancelled the close operation
            }
            // result == No: Continue without saving
        }
        
        if (ActiveDocument == document)
        {
            ActiveDocument = null;
        }
        
        return true;
    }
    
    public async Task<bool> CloseAllDocumentsAsync()
    {
        // For now, this is mainly used when exiting the application
        // In a real multi-document scenario, we'd iterate through all documents
        // For simplicity, we just check the active document
        if (ActiveDocument != null)
        {
            return await CloseDocumentAsync(ActiveDocument);
        }
        
        return true;
    }
    
    public void SetActiveDocument(Document document)
    {
        ActiveDocument = document ?? throw new ArgumentNullException(nameof(document));
    }
}
