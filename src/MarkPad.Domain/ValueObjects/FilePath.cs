namespace MarkPad.Domain.ValueObjects;

/// <summary>
/// Value object representing a file path with validation
/// </summary>
public class FilePath
{
    public string Value { get; }
    
    private FilePath(string value)
    {
        Value = value;
    }
    
    public static FilePath Create(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            throw new ArgumentException("File path cannot be empty", nameof(path));
        }
        
        return new FilePath(path);
    }
    
    public static FilePath? CreateOrNull(string? path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            return null;
        }
        
        return new FilePath(path);
    }
    
    public string GetFileName()
    {
        return Path.GetFileName(Value);
    }
    
    public string GetDirectory()
    {
        return Path.GetDirectoryName(Value) ?? string.Empty;
    }
    
    public override string ToString() => Value;
    
    public override bool Equals(object? obj)
    {
        return obj is FilePath other && Value.Equals(other.Value, StringComparison.OrdinalIgnoreCase);
    }
    
    public override int GetHashCode()
    {
        return Value.GetHashCode(StringComparison.OrdinalIgnoreCase);
    }
}
