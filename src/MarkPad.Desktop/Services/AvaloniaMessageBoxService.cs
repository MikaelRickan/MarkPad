using System.Threading.Tasks;
using Avalonia.Controls;
using MarkPad.Application.Interfaces;

namespace MarkPad.Desktop.Services;

/// <summary>
/// Avalonia-based message box service implementation
/// </summary>
public class AvaloniaMessageBoxService : IMessageBoxService
{
    private readonly Window _owner;
    
    public AvaloniaMessageBoxService(Window owner)
    {
        _owner = owner;
    }
    
    public async Task<MessageBoxResult> ShowYesNoCancelAsync(string title, string message)
    {
        var dialog = new Window
        {
            Title = title,
            Width = 450,
            Height = 180,
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
            CanResize = false
        };
        
        MessageBoxResult result = MessageBoxResult.Cancel;
        
        var panel = new StackPanel
        {
            Margin = new Avalonia.Thickness(20)
        };
        
        panel.Children.Add(new TextBlock
        {
            Text = message,
            TextWrapping = Avalonia.Media.TextWrapping.Wrap,
            Margin = new Avalonia.Thickness(0, 0, 0, 20)
        });
        
        var buttonPanel = new StackPanel
        {
            Orientation = Avalonia.Layout.Orientation.Horizontal,
            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Right,
            Spacing = 10
        };
        
        var yesButton = new Button
        {
            Content = "Yes",
            Width = 80,
            IsDefault = true
        };
        yesButton.Click += (s, e) =>
        {
            result = MessageBoxResult.Yes;
            dialog.Close();
        };
        
        var noButton = new Button
        {
            Content = "No",
            Width = 80
        };
        noButton.Click += (s, e) =>
        {
            result = MessageBoxResult.No;
            dialog.Close();
        };
        
        var cancelButton = new Button
        {
            Content = "Cancel",
            Width = 80,
            IsCancel = true
        };
        cancelButton.Click += (s, e) =>
        {
            result = MessageBoxResult.Cancel;
            dialog.Close();
        };
        
        buttonPanel.Children.Add(yesButton);
        buttonPanel.Children.Add(noButton);
        buttonPanel.Children.Add(cancelButton);
        
        panel.Children.Add(buttonPanel);
        dialog.Content = panel;
        
        await dialog.ShowDialog(_owner);
        return result;
    }
    
    public async Task ShowErrorAsync(string title, string message)
    {
        var dialog = new Window
        {
            Title = title,
            Width = 400,
            Height = 150,
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
            CanResize = false
        };
        
        var panel = new StackPanel
        {
            Margin = new Avalonia.Thickness(20)
        };
        
        panel.Children.Add(new TextBlock
        {
            Text = message,
            TextWrapping = Avalonia.Media.TextWrapping.Wrap,
            Margin = new Avalonia.Thickness(0, 0, 0, 20)
        });
        
        var okButton = new Button
        {
            Content = "OK",
            Width = 80,
            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Right,
            IsDefault = true
        };
        okButton.Click += (s, e) => dialog.Close();
        
        panel.Children.Add(okButton);
        dialog.Content = panel;
        
        await dialog.ShowDialog(_owner);
    }
}
