using MarkPad.Domain.Enums;
using MarkPad.Domain.ValueObjects;

namespace MarkPad.Domain.Entities;

/// <summary>
/// Represents a markdown document with its content and metadata
/// </summary>
public class Document
{
    private string _content;
    private string _originalContent;
    
    public FilePath? FilePath { get; private set; }
    
    public string Content 
    { 
        get => _content;
        set
        {
            _content = value;
            UpdateState();
        }
    }
    
    public DocumentState State { get; private set; }
    
    public bool HasUnsavedChanges => State == DocumentState.Modified;
    
    public Document()
    {
        _content = string.Empty;
        _originalContent = string.Empty;
        State = DocumentState.New;
    }
    
    public void Load(string filePath, string content)
    {
        FilePath = ValueObjects.FilePath.Create(filePath);
        _content = content;
        _originalContent = content;
        State = DocumentState.Saved;
    }
    
    public void Save(string? newFilePath = null)
    {
        if (newFilePath != null)
        {
            FilePath = ValueObjects.FilePath.Create(newFilePath);
        }
        
        _originalContent = _content;
        State = DocumentState.Saved;
    }
    
    public void MarkAsNew()
    {
        FilePath = null;
        _content = string.Empty;
        _originalContent = string.Empty;
        State = DocumentState.New;
    }
    
    private void UpdateState()
    {
        if (FilePath == null)
        {
            State = DocumentState.New;
        }
        else if (_content != _originalContent)
        {
            State = DocumentState.Modified;
        }
        else
        {
            State = DocumentState.Saved;
        }
    }
    
    public string GetDisplayName()
    {
        if (FilePath != null)
        {
            return FilePath.GetFileName();
        }
        
        return "Untitled";
    }
}
