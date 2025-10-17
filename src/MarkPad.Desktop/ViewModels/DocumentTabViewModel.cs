using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using MarkPad.Domain.Entities;
using AvaloniaEdit.Document;
using LiveMarkdown.Avalonia;

namespace MarkPad.Desktop.ViewModels;

/// <summary>
/// Represents a single document tab with its own editor and preview state
/// </summary>
public class DocumentTabViewModel : INotifyPropertyChanged
{
    private readonly Document _document;
    private double _previewZoom = 1.0;
    
    // AvaloniaEdit document
    private readonly TextDocument _editorDocument = new();
    public TextDocument EditorDocument => _editorDocument;
    
    // LiveMarkdown builder
    private readonly ObservableStringBuilder _markdownBuilder = new();
    public ObservableStringBuilder MarkdownBuilder => _markdownBuilder;
    
    public event PropertyChangedEventHandler? PropertyChanged;
    
    /// <summary>
    /// The underlying domain document
    /// </summary>
    public Document Document => _document;
    
    /// <summary>
    /// Display name for the tab header
    /// </summary>
    public string TabHeader
    {
        get
        {
            var modified = _document.HasUnsavedChanges ? "*" : "";
            return $"{modified}{_document.GetDisplayName()}";
        }
    }
    
    /// <summary>
    /// Full title for the window
    /// </summary>
    public string WindowTitle
    {
        get
        {
            var modified = _document.HasUnsavedChanges ? "*" : "";
            var name = _document.GetDisplayName();
            return $"{modified}{name} - MarkPad";
        }
    }
    
    public double PreviewZoom
    {
        get => _previewZoom;
        set
        {
            if (_previewZoom != value)
            {
                _previewZoom = Math.Clamp(value, 0.5, 3.0);
                OnPropertyChanged();
                OnPropertyChanged(nameof(PreviewZoomPercentage));
                OnPropertyChanged(nameof(PreviewZoomPercent));
            }
        }
    }
    
    public string PreviewZoomPercentage => $"{(int)(_previewZoom * 100)}%";
    public decimal PreviewZoomPercent
    {
        get => (decimal)(_previewZoom * 100);
        set => PreviewZoom = (double)(value / 100m);
    }
    
    public int WordCount
    {
        get
        {
            var content = _editorDocument.Text;
            if (string.IsNullOrWhiteSpace(content))
                return 0;
                
            var words = content.Split(new[] { ' ', '\n', '\r', '\t' }, 
                StringSplitOptions.RemoveEmptyEntries);
            return words.Length;
        }
    }
    
    public int CharacterCount => _editorDocument.Text.Length;
    
    public DocumentTabViewModel(Document document)
    {
        _document = document ?? throw new ArgumentNullException(nameof(document));
        
        // Initialize editor with document content
        _editorDocument.Text = document.Content;
        _markdownBuilder.Append(document.Content);
        
        // Wire up text changes to update the domain document
        _editorDocument.TextChanged += (s, e) =>
        {
            var newText = _editorDocument.Text;
            _document.Content = newText;
            
            // Update LiveMarkdown builder
            _markdownBuilder.Clear();
            _markdownBuilder.Append(newText);
            
            OnPropertyChanged(nameof(TabHeader));
            OnPropertyChanged(nameof(WindowTitle));
            OnPropertyChanged(nameof(WordCount));
            OnPropertyChanged(nameof(CharacterCount));
        };
    }
    
    public void ApplyFormatting(string prefix, string suffix, int selectionStart, int selectionLength)
    {
        var text = _editorDocument.Text;
        
        if (selectionLength > 0)
        {
            var selectedText = text.Substring(selectionStart, selectionLength);
            var prefixLength = prefix.Length;
            var suffixLength = suffix.Length;
            
            // Case 1: Check if selection INCLUDES the markers
            var selectionStartsWith = selectedText.StartsWith(prefix);
            var selectionEndsWith = selectedText.EndsWith(suffix);
            
            if (selectionStartsWith && selectionEndsWith && selectedText.Length > prefixLength + suffixLength)
            {
                var unwrapped = selectedText.Substring(prefixLength, selectedText.Length - prefixLength - suffixLength);
                _editorDocument.Replace(selectionStart, selectionLength, unwrapped);
                return;
            }
            
            // Case 2: Check if formatting exists AROUND selection
            var hasPrefixBefore = selectionStart >= prefixLength && 
                                  text.Substring(selectionStart - prefixLength, prefixLength) == prefix;
            var hasSuffixAfter = selectionStart + selectionLength + suffixLength <= text.Length && 
                                 text.Substring(selectionStart + selectionLength, suffixLength) == suffix;
            
            if (hasPrefixBefore && hasSuffixAfter)
            {
                _editorDocument.Remove(selectionStart + selectionLength, suffixLength);
                _editorDocument.Remove(selectionStart - prefixLength, prefixLength);
            }
            else
            {
                var formatted = prefix + selectedText + suffix;
                _editorDocument.Replace(selectionStart, selectionLength, formatted);
            }
        }
        else
        {
            _editorDocument.Insert(selectionStart, prefix + suffix);
        }
    }
    
    public void ApplyLineFormatting(string prefix, int cursorPosition)
    {
        var text = _editorDocument.Text;
        int lineStart = text.LastIndexOf('\n', Math.Max(0, cursorPosition - 1)) + 1;
        _editorDocument.Insert(lineStart, prefix);
    }
    
    public void ZoomIn() => PreviewZoom += 0.1;
    public void ZoomOut() => PreviewZoom -= 0.1;
    public void ZoomReset() => PreviewZoom = 1.0;
    public void AdjustZoom(double delta) => PreviewZoom += delta * 0.1;
    
    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
