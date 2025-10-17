using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Primitives.PopupPositioning;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.LogicalTree;
using Avalonia.Platform.Storage;
using Avalonia.Styling;
using Avalonia.VisualTree;
using AvaloniaEdit;
using MarkPad.Desktop.ViewModels;

namespace MarkPad.Desktop;

public partial class MainWindow : Window
{
    private MainViewModel? ViewModel => DataContext as MainViewModel;
    private TextEditor? _currentEditor;
    private ScrollViewer? _currentPreviewScrollViewer;
    
    public MainWindow()
    {
        InitializeComponent();
        
        // Setup drag and drop
        AddHandler(DragDrop.DropEvent, Drop);
        AddHandler(DragDrop.DragOverEvent, DragOver);
        
        // Setup keyboard shortcuts
        KeyDown += OnKeyDown;
        
        // Setup mouse wheel zoom and tab tracking when the window is loaded
        Loaded += MainWindow_Loaded;
        
        // Track DataContext changes to wire up tab changes
        DataContextChanged += MainWindow_DataContextChanged;
        
        // Add global pointer wheel handler at window level for better event capture
        AddHandler(PointerWheelChangedEvent, Window_PointerWheelChanged, RoutingStrategies.Tunnel);
    }
    
    private void MainWindow_DataContextChanged(object? sender, EventArgs e)
    {
        if (ViewModel != null)
        {
            ViewModel.PropertyChanged += ViewModel_PropertyChanged;
        }
    }
    
    private void ViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        // When the selected tab changes, find the new editor and preview
        if (e.PropertyName == nameof(MainViewModel.SelectedTab))
        {
            UpdateCurrentControls();
        }
    }
    
    private void MainWindow_Loaded(object? sender, RoutedEventArgs e)
    {
        // Setup mouse wheel zoom for preview  
        var tabControl = this.FindControl<TabControl>("DocumentTabs");
        if (tabControl != null)
        {
            tabControl.SelectionChanged += TabControl_SelectionChanged;
            
            // Also listen to when containers are prepared
            tabControl.Loaded += (s, args) => UpdateCurrentControls();
        }
        
        // Find initial editor with delay to ensure visual tree is ready
        Avalonia.Threading.Dispatcher.UIThread.Post(() => 
        {
            UpdateCurrentControls();
        }, Avalonia.Threading.DispatcherPriority.Loaded);
    }
    
    private void TabControl_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        // Delay the update to ensure the visual tree is ready after tab change
        Avalonia.Threading.Dispatcher.UIThread.Post(() => 
        {
            UpdateCurrentControls();
        }, Avalonia.Threading.DispatcherPriority.Loaded);
    }
    
    // Called when the TextEditor is loaded in a tab
    private void EditorTextBox_Loaded(object? sender, RoutedEventArgs e)
    {
        if (sender is TextEditor editor)
        {
            _currentEditor = editor;
            System.Diagnostics.Debug.WriteLine($"✅ Editor loaded and assigned: {editor.GetHashCode()}");
            
            // Auto-focus the editor when it first loads (for new documents or first tab)
            Avalonia.Threading.Dispatcher.UIThread.Post(() =>
            {
                if (editor.IsVisible && editor == _currentEditor)
                {
                    editor.Focus();
                    System.Diagnostics.Debug.WriteLine("✅ Editor auto-focused on load");
                }
            }, Avalonia.Threading.DispatcherPriority.Input);
        }
    }
    
    // Called when the PreviewScrollViewer is loaded in a tab
    private void PreviewScrollViewer_Loaded(object? sender, RoutedEventArgs e)
    {
        if (sender is ScrollViewer scrollViewer)
        {
            _currentPreviewScrollViewer = scrollViewer;
            
            // Remove old handlers if exists (in case of reload)
            scrollViewer.PointerWheelChanged -= PreviewScrollViewer_PointerWheelChanged;
            scrollViewer.PointerEntered -= PreviewScrollViewer_PointerEntered;
            scrollViewer.PointerExited -= PreviewScrollViewer_PointerExited;
            
            // Add new handlers
            scrollViewer.PointerWheelChanged += PreviewScrollViewer_PointerWheelChanged;
            scrollViewer.PointerEntered += PreviewScrollViewer_PointerEntered;
            scrollViewer.PointerExited += PreviewScrollViewer_PointerExited;
            
            System.Diagnostics.Debug.WriteLine($"✅ ScrollViewer loaded and wired: {scrollViewer.GetHashCode()}");
        }
    }
    
    private bool _isPointerOverPreview = false;
    
    private void PreviewScrollViewer_PointerEntered(object? sender, PointerEventArgs e)
    {
        _isPointerOverPreview = true;
        System.Diagnostics.Debug.WriteLine("🔍 Pointer entered preview area");
    }
    
    private void PreviewScrollViewer_PointerExited(object? sender, PointerEventArgs e)
    {
        _isPointerOverPreview = false;
        System.Diagnostics.Debug.WriteLine("🔍 Pointer exited preview area");
    }
    
    private void UpdateCurrentControls()
    {
        var tabControl = this.FindControl<TabControl>("DocumentTabs");
        if (tabControl == null) return;
        
        // Get the selected content presenter
        if (tabControl.SelectedContent is Control selectedContent)
        {
            // Find TextEditor in the visual tree
            var editors = selectedContent.GetVisualDescendants().OfType<TextEditor>().ToList();
            if (editors.Count > 0)
            {
                var previousEditor = _currentEditor;
                _currentEditor = editors.FirstOrDefault(e => e.IsVisible);
                
                if (_currentEditor != null)
                {
                    System.Diagnostics.Debug.WriteLine($"✅ Found editor: {_currentEditor.GetHashCode()}");
                    
                    // Auto-focus the editor when switching tabs or on first load
                    if (_currentEditor != previousEditor)
                    {
                        // Use a small delay to ensure the control is fully ready
                        Avalonia.Threading.Dispatcher.UIThread.Post(() =>
                        {
                            if (_currentEditor != null && _currentEditor.IsVisible)
                            {
                                _currentEditor.Focus();
                                System.Diagnostics.Debug.WriteLine("✅ Editor focused after tab switch");
                            }
                        }, Avalonia.Threading.DispatcherPriority.Input);
                    }
                }
            }
            
            // Find ScrollViewer in the visual tree (for the preview pane)
            var scrollViewers = selectedContent.GetVisualDescendants()
                .OfType<ScrollViewer>()
                .Where(sv => sv.Name == "PreviewScrollViewer")
                .ToList();
                
            if (scrollViewers.Count > 0)
            {
                var newScrollViewer = scrollViewers.FirstOrDefault();
                if (newScrollViewer != _currentPreviewScrollViewer)
                {
                    // Clean up old handlers
                    if (_currentPreviewScrollViewer != null)
                    {
                        _currentPreviewScrollViewer.PointerWheelChanged -= PreviewScrollViewer_PointerWheelChanged;
                        _currentPreviewScrollViewer.PointerEntered -= PreviewScrollViewer_PointerEntered;
                        _currentPreviewScrollViewer.PointerExited -= PreviewScrollViewer_PointerExited;
                    }
                    
                    // Set new ScrollViewer and wire up handlers
                    _currentPreviewScrollViewer = newScrollViewer;
                    if (_currentPreviewScrollViewer != null)
                    {
                        _currentPreviewScrollViewer.PointerWheelChanged += PreviewScrollViewer_PointerWheelChanged;
                        _currentPreviewScrollViewer.PointerEntered += PreviewScrollViewer_PointerEntered;
                        _currentPreviewScrollViewer.PointerExited += PreviewScrollViewer_PointerExited;
                        System.Diagnostics.Debug.WriteLine($"✅ Found and wired ScrollViewer: {_currentPreviewScrollViewer.GetHashCode()}");
                    }
                }
            }
        }
    }
    
    // Global pointer wheel handler to catch events at window level
    private void Window_PointerWheelChanged(object? sender, PointerWheelEventArgs e)
    {
        // Only process if Ctrl is held and pointer is over preview
        if (e.KeyModifiers.HasFlag(KeyModifiers.Control) && _isPointerOverPreview && ViewModel?.SelectedTab != null)
        {
            var delta = e.Delta.Y;
            ViewModel.SelectedTab.AdjustZoom(delta);
            e.Handled = true;
            System.Diagnostics.Debug.WriteLine($"🔍 Window-level zoom: delta={delta}, zoom={ViewModel.SelectedTab.PreviewZoomPercentage}");
        }
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
        if (ViewModel?.SelectedTab == null || _currentEditor == null) return;
        
        int selectionStart = _currentEditor.SelectionStart;
        int selectionLength = _currentEditor.SelectionLength;
        
        // Store cursor position for restoration
        int expectedCursorPos = selectionStart + prefix.Length + selectionLength;
        
        ViewModel.ApplyFormatting(prefix, suffix, selectionStart, selectionLength);
        
        // Restore focus and cursor position with a small delay
        Avalonia.Threading.Dispatcher.UIThread.Post(() =>
        {
            if (_currentEditor != null)
            {
                _currentEditor.Focus();
                _currentEditor.CaretOffset = expectedCursorPos;
                System.Diagnostics.Debug.WriteLine($"✅ Restored focus and cursor to position {expectedCursorPos}");
            }
        }, Avalonia.Threading.DispatcherPriority.Input);
    }
    
    private void ApplyLinePrefix(string prefix)
    {
        if (ViewModel?.SelectedTab == null || _currentEditor == null) return;
        
        int selectionStart = _currentEditor.SelectionStart;
        
        // Calculate expected cursor position (after the prefix)
        int expectedCursorPos = selectionStart + prefix.Length;
        
        ViewModel.ApplyLineFormatting(prefix, selectionStart);
        
        // Restore focus and cursor position with a small delay
        Avalonia.Threading.Dispatcher.UIThread.Post(() =>
        {
            if (_currentEditor != null)
            {
                _currentEditor.Focus();
                _currentEditor.CaretOffset = expectedCursorPos;
                System.Diagnostics.Debug.WriteLine($"✅ Restored focus and cursor to position {expectedCursorPos}");
            }
        }, Avalonia.Threading.DispatcherPriority.Input);
    }
    
    // Keyboard shortcuts handler
    private void OnKeyDown(object? sender, KeyEventArgs e)
    {
        var ctrl = e.KeyModifiers.HasFlag(KeyModifiers.Control);
        var shift = e.KeyModifiers.HasFlag(KeyModifiers.Shift);
        
        if (ctrl && !shift)
        {
            switch (e.Key)
            {
                case Key.B:
                    Bold_Click(null, new RoutedEventArgs());
                    e.Handled = true;
                    break;
                case Key.I:
                    Italic_Click(null, new RoutedEventArgs());
                    e.Handled = true;
                    break;
                case Key.K:
                    Link_Click(null, new RoutedEventArgs());
                    e.Handled = true;
                    break;
            }
        }
    }
    
    // Mouse wheel zoom handler for preview (ScrollViewer level)
    private void PreviewScrollViewer_PointerWheelChanged(object? sender, PointerWheelEventArgs e)
    {
        if (ViewModel?.SelectedTab == null) return;
        
        if (e.KeyModifiers.HasFlag(KeyModifiers.Control))
        {
            // Zoom in/out with Ctrl+Wheel
            var delta = e.Delta.Y;
            ViewModel.SelectedTab.AdjustZoom(delta);
            e.Handled = true;
            System.Diagnostics.Debug.WriteLine($"🔍 ScrollViewer zoom: delta={delta}, zoom={ViewModel.SelectedTab.PreviewZoomPercentage}");
        }
    }
    
    // Zoom control button handlers
    private void ZoomPreset_Click(object? sender, RoutedEventArgs e)
    {
        if (ViewModel?.SelectedTab == null) return;
        
        if (sender is Button button && button.Tag is string tagValue)
        {
            if (double.TryParse(tagValue, out double percentage))
            {
                ViewModel.SelectedTab.PreviewZoom = percentage / 100.0;
                System.Diagnostics.Debug.WriteLine($"🔍 Preset zoom applied: {percentage}%");
                
                // Close the flyout - find it in the visual tree
                var parent = button.Parent;
                while (parent != null && parent is not FlyoutPresenter)
                {
                    if (parent is Visual visual)
                    {
                        parent = visual.GetVisualParent();
                    }
                    else
                    {
                        break;
                    }
                }
                if (parent?.Parent is Popup popup)
                {
                    popup.Close();
                }
            }
        }
    }
    
    private void CustomZoom_Click(object? sender, RoutedEventArgs e)
    {
        if (ViewModel?.SelectedTab == null) return;
        
        if (sender is Button button)
        {
            // Find the NumericUpDown in the same parent
            var grid = button.Parent as Grid;
            if (grid != null)
            {
                var customInput = grid.Children
                    .OfType<NumericUpDown>()
                    .FirstOrDefault(n => n.Name == "CustomZoomInput");
                
                if (customInput != null && customInput.Value.HasValue)
                {
                    ViewModel.SelectedTab.PreviewZoom = (double)customInput.Value.Value / 100.0;
                    System.Diagnostics.Debug.WriteLine($"🔍 Custom zoom applied: {customInput.Value.Value}%");
                    
                    // Close the flyout
                    var parent = button.Parent;
                    while (parent != null && parent is not FlyoutPresenter)
                    {
                        if (parent is Visual visual)
                        {
                            parent = visual.GetVisualParent();
                        }
                        else
                        {
                            break;
                        }
                    }
                    if (parent?.Parent is Popup popup)
                    {
                        popup.Close();
                    }
                }
            }
        }
    }
}