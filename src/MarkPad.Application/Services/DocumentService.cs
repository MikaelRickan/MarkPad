using MarkPad.Application.Interfaces;
using MarkPad.Domain.Entities;

namespace MarkPad.Application.Services;

/// <summary>
/// Implementation of document operations
/// </summary>
public class DocumentService : IDocumentService
{
    private readonly IFileService _fileService;
    private readonly IDialogService _dialogService;
    private readonly IMessageBoxService _messageBoxService;
    
    public Document CurrentDocument { get; private set; }
    
    public DocumentService(
        IFileService fileService, 
        IDialogService dialogService,
        IMessageBoxService messageBoxService)
    {
        _fileService = fileService;
        _dialogService = dialogService;
        _messageBoxService = messageBoxService;
        CurrentDocument = new Document();
    }
    
    public async Task CreateNewAsync()
    {
        // Check for unsaved changes
        if (CurrentDocument.HasUnsavedChanges)
        {
            var result = await _messageBoxService.ShowYesNoCancelAsync(
                "Unsaved Changes",
                "You have unsaved changes. Do you want to save them?");
            
            if (result == MessageBoxResult.Yes)
            {
                await SaveAsync();
                if (CurrentDocument.HasUnsavedChanges)
                {
                    // Save was cancelled or failed
                    return;
                }
            }
            else if (result == MessageBoxResult.Cancel)
            {
                return;
            }
            // No - continue without saving
        }
        
        CurrentDocument.MarkAsNew();
    }
    
    public async Task OpenAsync()
    {
        // Check for unsaved changes
        if (CurrentDocument.HasUnsavedChanges)
        {
            var result = await _messageBoxService.ShowYesNoCancelAsync(
                "Unsaved Changes",
                "You have unsaved changes. Do you want to save them?");
            
            if (result == MessageBoxResult.Yes)
            {
                await SaveAsync();
                if (CurrentDocument.HasUnsavedChanges)
                {
                    // Save was cancelled or failed
                    return;
                }
            }
            else if (result == MessageBoxResult.Cancel)
            {
                return;
            }
            // No - continue without saving
        }
        
        var filePath = await _dialogService.ShowOpenFileDialogAsync();
        
        if (filePath == null)
        {
            return; // User cancelled
        }
        
        try
        {
            var content = await _fileService.ReadTextAsync(filePath);
            CurrentDocument.Load(filePath, content);
        }
        catch (Exception ex)
        {
            await _messageBoxService.ShowErrorAsync("Error Opening File", 
                $"Failed to open file: {ex.Message}");
        }
    }
    
    public async Task SaveAsync()
    {
        if (CurrentDocument.FilePath == null)
        {
            await SaveAsAsync();
            return;
        }
        
        try
        {
            await _fileService.WriteTextAsync(CurrentDocument.FilePath.Value, CurrentDocument.Content);
            CurrentDocument.Save();
        }
        catch (Exception ex)
        {
            await _messageBoxService.ShowErrorAsync("Error Saving File",
                $"Failed to save file: {ex.Message}");
        }
    }
    
    public async Task SaveAsAsync()
    {
        var fileName = CurrentDocument.GetDisplayName();
        var filePath = await _dialogService.ShowSaveFileDialogAsync(fileName);
        
        if (filePath == null)
        {
            return; // User cancelled
        }
        
        try
        {
            await _fileService.WriteTextAsync(filePath, CurrentDocument.Content);
            CurrentDocument.Save(filePath);
        }
        catch (Exception ex)
        {
            await _messageBoxService.ShowErrorAsync("Error Saving File",
                $"Failed to save file: {ex.Message}");
        }
    }
}
