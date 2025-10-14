using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using Avalonia.Styling;
using MarkPad.Desktop.ViewModels;
using System.Linq;

namespace MarkPad.Desktop;

public partial class MainWindow : Window
{
    private MainViewModel? ViewModel => DataContext as MainViewModel;
    
    public MainWindow()
    {
        InitializeComponent();
        
        // Setup drag and drop
        AddHandler(DragDrop.DropEvent, Drop);
        AddHandler(DragDrop.DragOverEvent, DragOver);
    }
    
    // Drag and Drop Handlers
    private void DragOver(object? sender, DragEventArgs e)
    {
        // Only allow file drops
        if (e.Data.Contains(DataFormats.Files))
        {
            e.DragEffects = DragDropEffects.Copy;
        }
        else
        {
            e.DragEffects = DragDropEffects.None;
        }
    }
    
    private async void Drop(object? sender, DragEventArgs e)
    {
        if (ViewModel == null) return;
        
        var files = e.Data.GetFiles();
        if (files != null)
        {
            var markdownFiles = files
                .Where(f => f.Path.LocalPath.EndsWith(".md", System.StringComparison.OrdinalIgnoreCase) ||
                           f.Path.LocalPath.EndsWith(".markdown", System.StringComparison.OrdinalIgnoreCase))
                .Select(f => f.Path.LocalPath)
                .ToArray();
            
            if (markdownFiles.Length > 0)
            {
                await ViewModel.OpenFiles(markdownFiles);
            }
        }
    }
    
    // File Menu Handlers
    private async void NewDocument_Click(object? sender, RoutedEventArgs e)
    {
        if (ViewModel != null)
        {
            await ViewModel.NewDocument();
        }
    }
    
    private async void OpenDocument_Click(object? sender, RoutedEventArgs e)
    {
        if (ViewModel != null)
        {
            await ViewModel.OpenDocument();
        }
    }
    
    private async void SaveDocument_Click(object? sender, RoutedEventArgs e)
    {
        if (ViewModel != null)
        {
            await ViewModel.SaveDocument();
        }
    }
    
    private async void SaveDocumentAs_Click(object? sender, RoutedEventArgs e)
    {
        if (ViewModel != null)
        {
            await ViewModel.SaveDocumentAs();
        }
    }
    
    private async void CloseTab_Click(object? sender, RoutedEventArgs e)
    {
        if (ViewModel?.SelectedTab != null)
        {
            await ViewModel.CloseTab(ViewModel.SelectedTab);
        }
    }
    
    private async void CloseTabButton_Click(object? sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.CommandParameter is DocumentTabViewModel tab && ViewModel != null)
        {
            await ViewModel.CloseTab(tab);
        }
    }
    
    private void Exit_Click(object? sender, RoutedEventArgs e)
    {
        Close();
    }
    
    // Theme switching handlers
    private void ThemeSystem_Click(object? sender, RoutedEventArgs e)
    {
        RequestedThemeVariant = ThemeVariant.Default;
    }
    
    private void ThemeLight_Click(object? sender, RoutedEventArgs e)
    {
        RequestedThemeVariant = ThemeVariant.Light;
    }
    
    private void ThemeDark_Click(object? sender, RoutedEventArgs e)
    {
        RequestedThemeVariant = ThemeVariant.Dark;
    }
    
    // Formatting toolbar handlers
    private void Bold_Click(object? sender, RoutedEventArgs e)
    {
        ApplyWrappingFormat("**", "**");
    }
    
    private void Italic_Click(object? sender, RoutedEventArgs e)
    {
        ApplyWrappingFormat("*", "*");
    }
    
    private void Strikethrough_Click(object? sender, RoutedEventArgs e)
    {
        ApplyWrappingFormat("~~", "~~");
    }
    
    private void Heading1_Click(object? sender, RoutedEventArgs e)
    {
        ApplyLinePrefix("# ");
    }
    
    private void Heading2_Click(object? sender, RoutedEventArgs e)
    {
        ApplyLinePrefix("## ");
    }
    
    private void Heading3_Click(object? sender, RoutedEventArgs e)
    {
        ApplyLinePrefix("### ");
    }
    
    private void BulletList_Click(object? sender, RoutedEventArgs e)
    {
        ApplyLinePrefix("- ");
    }
    
    private void NumberedList_Click(object? sender, RoutedEventArgs e)
    {
        ApplyLinePrefix("1. ");
    }
    
    private void TaskList_Click(object? sender, RoutedEventArgs e)
    {
        ApplyLinePrefix("- [ ] ");
    }
    
    private void Link_Click(object? sender, RoutedEventArgs e)
    {
        ApplyWrappingFormat("[", "](https://example.com)");
    }
    
    private void Image_Click(object? sender, RoutedEventArgs e)
    {
        ApplyWrappingFormat("![", "](https://example.com/image.png)");
    }
    
    private void Code_Click(object? sender, RoutedEventArgs e)
    {
        ApplyWrappingFormat("```\n", "\n```");
    }
    
    private void InlineCode_Click(object? sender, RoutedEventArgs e)
    {
        ApplyWrappingFormat("`", "`");
    }
    
    // Helper methods for formatting
    private void ApplyWrappingFormat(string prefix, string suffix)
    {
        if (ViewModel?.SelectedTab == null) return;
        
        // Find the EditorTextBox in the current tab's visual tree
        var editor = this.FindControl<AvaloniaEdit.TextEditor>("EditorTextBox");
        if (editor == null) return;
        
        int selectionStart = editor.SelectionStart;
        int selectionLength = editor.SelectionLength;
        
        ViewModel.ApplyFormatting(prefix, suffix, selectionStart, selectionLength);
        
        // Restore focus to editor
        editor.Focus();
    }
    
    private void ApplyLinePrefix(string prefix)
    {
        if (ViewModel?.SelectedTab == null) return;
        
        // Find the EditorTextBox in the current tab's visual tree
        var editor = this.FindControl<AvaloniaEdit.TextEditor>("EditorTextBox");
        if (editor == null) return;
        
        int selectionStart = editor.SelectionStart;
        
        ViewModel.ApplyLineFormatting(prefix, selectionStart);
        
        // Restore focus to editor
        editor.Focus();
    }
}
