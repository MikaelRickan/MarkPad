using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MarkPad.Application.Interfaces;

namespace MarkPad.Desktop.ViewModels;

/// <summary>
/// Main view model supporting multiple document tabs
/// </summary>
public class MainViewModel : INotifyPropertyChanged
{
    private readonly IDocumentManagerService _documentManager;
    private string _statusMessage = "Ready";
    private DocumentTabViewModel? _selectedTab;
    
    public ObservableCollection<DocumentTabViewModel> Tabs { get; } = new();
    
    public event PropertyChangedEventHandler? PropertyChanged;
    
    public DocumentTabViewModel? SelectedTab
    {
        get => _selectedTab;
        set
        {
            if (_selectedTab != value)
            {
                _selectedTab = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(WindowTitle));
                OnPropertyChanged(nameof(StatusMessage));
                OnPropertyChanged(nameof(WordCount));
                OnPropertyChanged(nameof(CharacterCount));
                OnPropertyChanged(nameof(PreviewZoom));
                OnPropertyChanged(nameof(PreviewZoomPercentage));
                
                // Update active document in manager
                if (_selectedTab?.Document != null)
                {
                    _documentManager.SetActiveDocument(_selectedTab.Document);
                }
            }
        }
    }
    
    public string WindowTitle
    {
        get
        {
            if (_selectedTab != null)
                return _selectedTab.WindowTitle;
            return "MarkPad";
        }
    }
    
    public string StatusMessage
    {
        get => _statusMessage;
        set
        {
            if (_statusMessage != value)
            {
                _statusMessage = value;
                OnPropertyChanged();
            }
        }
    }
    
    public double PreviewZoom
    {
        get => _selectedTab?.PreviewZoom ?? 1.0;
        set
        {
            if (_selectedTab != null)
            {
                _selectedTab.PreviewZoom = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(PreviewZoomPercentage));
            }
        }
    }
    
    public string PreviewZoomPercentage => _selectedTab?.PreviewZoomPercentage ?? "100%";
    
    public int WordCount => _selectedTab?.WordCount ?? 0;
    public int CharacterCount => _selectedTab?.CharacterCount ?? 0;
    
    public MainViewModel(IDocumentManagerService documentManager)
    {
        _documentManager = documentManager ?? throw new ArgumentNullException(nameof(documentManager));
        
        // Create initial tab
        var initialDoc = _documentManager.CreateNew();
        var initialTab = new DocumentTabViewModel(initialDoc);
        Tabs.Add(initialTab);
        SelectedTab = initialTab;
    }
    
    public Task NewDocument()
    {
        var document = _documentManager.CreateNew();
        var tab = new DocumentTabViewModel(document);
        Tabs.Add(tab);
        SelectedTab = tab;
        StatusMessage = "New document created";
        return Task.CompletedTask;
    }
    
    public async Task OpenDocument()
    {
        var document = await _documentManager.OpenAsync();
        if (document != null)
        {
            var tab = new DocumentTabViewModel(document);
            Tabs.Add(tab);
            SelectedTab = tab;
            StatusMessage = $"Opened: {document.GetDisplayName()}";
        }
    }
    
    public async Task OpenFiles(string[] filePaths)
    {
        foreach (var filePath in filePaths)
        {
            var document = await _documentManager.OpenFileAsync(filePath);
            if (document != null)
            {
                var tab = new DocumentTabViewModel(document);
                Tabs.Add(tab);
                SelectedTab = tab;
            }
        }
        
        if (filePaths.Length == 1)
            StatusMessage = $"Opened: {System.IO.Path.GetFileName(filePaths[0])}";
        else
            StatusMessage = $"Opened {filePaths.Length} files";
    }
    
    public async Task SaveDocument()
    {
        if (_selectedTab?.Document == null)
            return;
        
        await _documentManager.SaveAsync(_selectedTab.Document);
        OnPropertyChanged(nameof(WindowTitle));
        StatusMessage = $"Saved: {_selectedTab.Document.GetDisplayName()}";
    }
    
    public async Task SaveDocumentAs()
    {
        if (_selectedTab?.Document == null)
            return;
        
        await _documentManager.SaveAsAsync(_selectedTab.Document);
        OnPropertyChanged(nameof(WindowTitle));
        StatusMessage = $"Saved as: {_selectedTab.Document.GetDisplayName()}";
    }
    
    public async Task<bool> CloseTab(DocumentTabViewModel tab)
    {
        if (tab?.Document == null)
            return false;
        
        var closed = await _documentManager.CloseDocumentAsync(tab.Document);
        if (closed)
        {
            Tabs.Remove(tab);
            
            // If we closed the last tab, create a new one
            if (Tabs.Count == 0)
            {
                var newDoc = _documentManager.CreateNew();
                var newTab = new DocumentTabViewModel(newDoc);
                Tabs.Add(newTab);
                SelectedTab = newTab;
            }
            // Otherwise, select the next tab or the previous one
            else if (SelectedTab == tab)
            {
                SelectedTab = Tabs.First();
            }
            
            StatusMessage = "Document closed";
        }
        
        return closed;
    }
    
    public async Task<bool> CloseAllTabs()
    {
        return await _documentManager.CloseAllDocumentsAsync();
    }
    
    // Formatting methods delegate to the selected tab
    public void ApplyFormatting(string prefix, string suffix, int selectionStart, int selectionLength)
    {
        _selectedTab?.ApplyFormatting(prefix, suffix, selectionStart, selectionLength);
    }
    
    public void ApplyLineFormatting(string prefix, int cursorPosition)
    {
        _selectedTab?.ApplyLineFormatting(prefix, cursorPosition);
    }
    
    public void ZoomIn() => _selectedTab?.ZoomIn();
    public void ZoomOut() => _selectedTab?.ZoomOut();
    public void ZoomReset() => _selectedTab?.ZoomReset();
    public void AdjustZoom(double delta) => _selectedTab?.AdjustZoom(delta);
    
    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
