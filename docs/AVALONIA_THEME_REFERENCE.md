# Avalonia Theme Switching - Complete Reference

## Critical Finding: Proper Theme Implementation in Avalonia 11+

**Date Discovered**: 2025-10-14  
**Context**: MarkPad project - Light theme showing as completely dark

---

## The Problem

When implementing theme switching in Avalonia applications, a common issue is that custom colors don't switch properly when `RequestedThemeVariant` changes. Symptoms include:
- Transparent backgrounds when switching themes
- All UI staying dark in light theme (or vice versa)
- Third-party components not respecting theme changes

---

## The Solution: Proper ThemeDictionaries

### ✅ Correct Implementation

```xml
<Application xmlns="https://github.com/avaloniaui"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
             x:Class="YourApp.App"
             RequestedThemeVariant="Default">

    <Application.Styles>
        <FluentTheme />
        <!-- Other style includes -->
    </Application.Styles>
    
    <Application.Resources>
        <ResourceDictionary>
            <ResourceDictionary.ThemeDictionaries>
                <!-- Light Theme -->
                <ResourceDictionary x:Key='Light'>
                    <!-- Define Colors -->
                    <Color x:Key="AppBackgroundColor">#FFFFFF</Color>
                    <Color x:Key="AppTextColor">#1A1A1A</Color>
                    
                    <!-- Define Brushes (CRITICAL!) -->
                    <SolidColorBrush x:Key="BackgroundBrush" Color="{DynamicResource AppBackgroundColor}"/>
                    <SolidColorBrush x:Key="TextBrush" Color="{DynamicResource AppTextColor}"/>
                </ResourceDictionary>
                
                <!-- Dark Theme -->
                <ResourceDictionary x:Key='Dark'>
                    <!-- Define Colors -->
                    <Color x:Key="AppBackgroundColor">#1E1E1E</Color>
                    <Color x:Key="AppTextColor">#E4E4E4</Color>
                    
                    <!-- Define Brushes (CRITICAL!) -->
                    <SolidColorBrush x:Key="BackgroundBrush" Color="{DynamicResource AppBackgroundColor}"/>
                    <SolidColorBrush x:Key="TextBrush" Color="{DynamicResource AppTextColor}"/>
                </ResourceDictionary>
                
                <!-- Default Theme (System) -->
                <ResourceDictionary x:Key='Default'>
                    <!-- Usually same as Light -->
                    <Color x:Key="AppBackgroundColor">#FFFFFF</Color>
                    <Color x:Key="AppTextColor">#1A1A1A</Color>
                    
                    <SolidColorBrush x:Key="BackgroundBrush" Color="{DynamicResource AppBackgroundColor}"/>
                    <SolidColorBrush x:Key="TextBrush" Color="{DynamicResource AppTextColor}"/>
                </ResourceDictionary>
            </ResourceDictionary.ThemeDictionaries>
        </ResourceDictionary>
    </Application.Resources>
</Application>
```

### ❌ Common Mistakes

#### Mistake 1: Missing Brush Resources
```xml
<!-- WRONG - Only defining colors -->
<ResourceDictionary x:Key='Light'>
    <Color x:Key="AppBackgroundColor">#FFFFFF</Color>
</ResourceDictionary>

<!-- RIGHT - Must include brushes too -->
<ResourceDictionary x:Key='Light'>
    <Color x:Key="AppBackgroundColor">#FFFFFF</Color>
    <SolidColorBrush x:Key="BackgroundBrush" Color="{DynamicResource AppBackgroundColor}"/>
</ResourceDictionary>
```

#### Mistake 2: Wrong Location
```xml
<!-- WRONG - In Application.Styles -->
<Application.Styles>
    <Style Selector=":is(Control)">
        <Style.Resources>
            <Color x:Key="AppBackgroundColor">#FFFFFF</Color>
        </Style.Resources>
    </Style>
</Application.Styles>

<!-- RIGHT - In Application.Resources.ThemeDictionaries -->
<Application.Resources>
    <ResourceDictionary>
        <ResourceDictionary.ThemeDictionaries>
            <ResourceDictionary x:Key='Light'>
                <Color x:Key="AppBackgroundColor">#FFFFFF</Color>
            </ResourceDictionary>
        </ResourceDictionary.ThemeDictionaries>
    </ResourceDictionary>
</Application.Resources>
```

#### Mistake 3: Using FluentTheme System Colors
```xml
<!-- WRONG - These don't switch reliably -->
<SolidColorBrush x:Key="BackgroundBrush" Color="{DynamicResource SystemChromeLowColor}"/>

<!-- RIGHT - Define your own colors in ThemeDictionaries -->
<ResourceDictionary x:Key='Light'>
    <Color x:Key="AppBackgroundColor">#FFFFFF</Color>
    <SolidColorBrush x:Key="BackgroundBrush" Color="{DynamicResource AppBackgroundColor}"/>
</ResourceDictionary>
```

---

## Theme Switching in Code

### In MainWindow.axaml.cs or ViewModel

```csharp
// Switch to Light theme
this.RequestedThemeVariant = ThemeVariant.Light;

// Switch to Dark theme
this.RequestedThemeVariant = ThemeVariant.Dark;

// Use system theme
this.RequestedThemeVariant = ThemeVariant.Default;
```

### Menu Item Example (XAML)

```xml
<MenuItem Header="View">
    <MenuItem Header="Theme">
        <MenuItem Header="System" Click="ThemeSystem_Click"/>
        <MenuItem Header="Light" Click="ThemeLight_Click"/>
        <MenuItem Header="Dark" Click="ThemeDark_Click"/>
    </MenuItem>
</MenuItem>
```

```csharp
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
```

---

## Using Theme Resources in UI

### In MainWindow.axaml

```xml
<Window Background="{DynamicResource BackgroundBrush}">
    <Grid>
        <TextBlock Text="Hello" 
                   Foreground="{DynamicResource TextBrush}"/>
        
        <Border Background="{DynamicResource SurfaceBrush}"
                BorderBrush="{DynamicResource BorderBrush}"/>
    </Grid>
</Window>
```

**CRITICAL**: Use `DynamicResource`, NOT `StaticResource`, for theme-aware properties!

---

## Third-Party Component Integration

### LiveMarkdown.Avalonia Example

LiveMarkdown requires specific color keys. Add these to **each** ThemeDictionary:

```xml
<ResourceDictionary x:Key='Light'>
    <!-- Your app colors -->
    <Color x:Key="AppBackgroundColor">#FFFFFF</Color>
    
    <!-- LiveMarkdown required colors -->
    <Color x:Key="BorderColor">#E1E1E1</Color>
    <Color x:Key="ForegroundColor">#1A1A1A</Color>
    <Color x:Key="CardBackgroundColor">#F5F5F5</Color>
    <Color x:Key="SecondaryCardBackgroundColor">#FAFAFA</Color>
</ResourceDictionary>

<ResourceDictionary x:Key='Dark'>
    <!-- Your app colors -->
    <Color x:Key="AppBackgroundColor">#1E1E1E</Color>
    
    <!-- LiveMarkdown required colors -->
    <Color x:Key="BorderColor">#3E3E42</Color>
    <Color x:Key="ForegroundColor">#E4E4E4</Color>
    <Color x:Key="CardBackgroundColor">#252526</Color>
    <Color x:Key="SecondaryCardBackgroundColor">#1E1E1E</Color>
</ResourceDictionary>
```

---

## Complete Color Palette Example

### Light Theme Colors
```csharp
Primary:        #0078D4  // Accent color
Background:     #FFFFFF  // Main background
Surface:        #F9F9F9  // Cards, panels
Border:         #E1E1E1  // Dividers, borders
TextPrimary:    #1A1A1A  // Main text
TextSecondary:  #666666  // Labels, hints
Toolbar:        #F5F5F5  // Toolbar background
Editor:         #FAFAFA  // Editor background
```

### Dark Theme Colors
```csharp
Primary:        #4CC2FF  // Accent color
Background:     #1E1E1E  // Main background
Surface:        #252526  // Cards, panels
Border:         #3E3E42  // Dividers, borders
TextPrimary:    #E4E4E4  // Main text
TextSecondary:  #A0A0A0  // Labels, hints
Toolbar:        #2D2D30  // Toolbar background
Editor:         #1E1E1E  // Editor background
```

---

## Debugging Theme Issues

### Problem: Transparent Backgrounds
**Cause**: Missing SolidColorBrush definitions in ThemeDictionaries  
**Solution**: Add brushes to each theme dictionary

### Problem: Theme Doesn't Switch
**Cause**: Using StaticResource instead of DynamicResource  
**Solution**: Change all theme-aware bindings to DynamicResource

### Problem: Third-Party Component Wrong Colors
**Cause**: Component has its own color keys not in ThemeDictionaries  
**Solution**: Research component's required color keys and add them

### Problem: Some Controls Stay Dark/Light
**Cause**: Controls using hardcoded colors or wrong resource keys  
**Solution**: Audit XAML for hardcoded colors, ensure DynamicResource usage

---

## Key Principles

1. **Always use ThemeDictionaries** for theme-dependent colors
2. **Define both Colors AND Brushes** in each dictionary
3. **Use DynamicResource** for all theme-aware properties
4. **Include all three keys**: `Light`, `Dark`, `Default`
5. **Research third-party components** for required color keys
6. **Test both themes** thoroughly after implementation

---

## MarkPad Implementation

See `src/MarkPad.Desktop/App.axaml` for complete working example with:
- Full Light/Dark/Default theme dictionaries
- LiveMarkdown.Avalonia integration
- Custom app colors and brushes
- Proper resource structure

---

## References

- [Avalonia Theme Variants Documentation](https://docs.avaloniaui.net/docs/guides/styles-and-resources/how-to-use-theme-variants)
- [Avalonia FluentTheme Documentation](https://docs.avaloniaui.net/docs/basics/user-interface/styling/themes/fluent)
- MarkPad `docs/THEME_FIX.md` - Detailed fix documentation

---

**Status**: ✅ Verified working in MarkPad (2025-10-14)  
**Avalonia Version**: 11.2.7  
**Framework**: .NET 8.0
