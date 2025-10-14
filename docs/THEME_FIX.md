# Theme Support Fix

## Problem Identified

The Light theme had issues where certain parts of the application were not properly respecting the theme switch. Specifically:
- LiveMarkdown preview area
- AvaloniaEdit text editor
- Potentially inconsistent colors across components

## Root Cause

Two third-party components have their own color schemes that don't automatically adapt to Avalonia's theme system:

1. **LiveMarkdown.Avalonia** - Uses its own color resource keys
2. **AvaloniaEdit** - Has its own styling system

## Solution Implemented

### 1. LiveMarkdown Theme Override

**File**: `src/MarkPad.Desktop/Styles/LiveMarkdownTheme.axaml`

Created theme-aware overrides for LiveMarkdown's color keys:

**Light Theme Colors**:
- BorderColor: `#E1E1E1`
- ForegroundColor: `#1A1A1A` (dark text)
- CardBackgroundColor: `#F5F5F5`
- SecondaryCardBackgroundColor: `#FAFAFA`

**Dark Theme Colors**:
- BorderColor: `#3E3E42`
- ForegroundColor: `#E4E4E4` (light text)
- CardBackgroundColor: `#252526`
- SecondaryCardBackgroundColor: `#1E1E1E`

### 2. AvaloniaEdit Theme Override

**File**: `src/MarkPad.Desktop/Styles/AvaloniaEditTheme.axaml`

Created theme-specific styles for the text editor:

**Light Theme**:
- Editor background: `#FAFAFA`
- Text color: `#1A1A1A`
- Line number margin: `#F5F5F5`

**Dark Theme**:
- Editor background: `#1E1E1E`
- Text color: `#E4E4E4`
- Line number margin: `#252526`

### 3. Style Loading Order

**File**: `src/MarkPad.Desktop/App.axaml`

Updated to include overrides in correct order:

```xml
<Application.Styles>
    <FluentTheme />
    <!-- 1. Load AvaloniaEdit base theme -->
    <StyleInclude Source="avares://AvaloniaEdit/Themes/Fluent/AvaloniaEdit.xaml"/>
    <!-- 2. Override AvaloniaEdit colors -->
    <StyleInclude Source="/Styles/AvaloniaEditTheme.axaml"/>
    <!-- 3. Load LiveMarkdown base theme -->
    <StyleInclude Source="avares://LiveMarkdown.Avalonia/Styles.axaml"/>
    <!-- 4. Override LiveMarkdown colors -->
    <StyleInclude Source="/Styles/LiveMarkdownTheme.axaml"/>
    <!-- 5. Load app-specific styles -->
    <StyleInclude Source="/Styles/AppStyles.axaml"/>
</Application.Styles>
```

**Order is critical**: Base themes → Overrides → App styles

### 4. Removed Duplicate References

**File**: `src/MarkPad.Desktop/MainWindow.axaml`

Removed duplicate AvaloniaEdit theme reference (now centralized in App.axaml).

---

## Testing Instructions

### Manual Test

1. **Build**: `dotnet build` ✅ (Succeeded with 0 warnings)
2. **Run**: Launch MarkPad
3. **Switch to Light Theme**: View → Theme → Light
4. **Verify**:
   - ☐ Editor background is light
   - ☐ Editor text is dark
   - ☐ Preview background is light  
   - ☐ Preview text is dark
   - ☐ All UI elements have good contrast
   - ☐ No dark spots in light theme

5. **Switch to Dark Theme**: View → Theme → Dark
6. **Verify**:
   - ☐ Editor background is dark
   - ☐ Editor text is light
   - ☐ Preview background is dark
   - ☐ Preview text is light
   - ☐ All UI elements have good contrast
   - ☐ No light spots in dark theme

7. **Switch to System Default**: View → Theme → System Default
8. **Verify**: Follows OS theme setting

---

## Technical Details

### Theme Variant Selectors

Avalonia supports theme-aware styling using selectors:

```xml
<!-- Targets Light theme -->
<Style Selector=":is(Application)[RequestedThemeVariant=Light]">
    <!-- Light theme overrides -->
</Style>

<!-- Targets Dark theme -->
<Style Selector=":is(Application)[RequestedThemeVariant=Dark]">
    <!-- Dark theme overrides -->
</Style>

<!-- Targets System Default -->
<Style Selector=":is(Application)[RequestedThemeVariant=Default]">
    <!-- Fallback (usually Light) -->
</Style>
```

### Color Resource Keys

**Application Colors** (App.axaml):
- AppPrimaryColor
- AppBackgroundColor
- AppSurfaceColor
- AppBorderColor
- AppTextPrimaryColor
- AppTextSecondaryColor
- AppToolbarBackgroundColor
- AppEditorBackgroundColor

**Dynamic Brushes** (AppStyles.axaml):
- PrimaryBrush
- BackgroundBrush
- SurfaceBrush
- BorderBrush
- TextPrimaryBrush
- TextSecondaryBrush
- ToolbarBackgroundBrush
- EditorBackgroundBrush

### Third-Party Component Keys

**LiveMarkdown.Avalonia**:
- BorderColor
- ForegroundColor
- CardBackgroundColor
- SecondaryCardBackgroundColor

**AvaloniaEdit**:
- Uses selectors to target TextArea and line number margins

---

## Color Palette Reference

### Light Theme
| Element | Color | Usage |
|---------|-------|-------|
| Primary | `#0078D4` | Accent color |
| Background | `#FFFFFF` | Main background |
| Surface | `#F9F9F9` | Cards, panels |
| Border | `#E1E1E1` | Dividers, borders |
| Text Primary | `#1A1A1A` | Main text |
| Text Secondary | `#666666` | Labels, hints |
| Toolbar | `#F5F5F5` | Toolbar background |
| Editor | `#FAFAFA` | Editor background |

### Dark Theme
| Element | Color | Usage |
|---------|-------|-------|
| Primary | `#4CC2FF` | Accent color |
| Background | `#1E1E1E` | Main background |
| Surface | `#252526` | Cards, panels |
| Border | `#3E3E42` | Dividers, borders |
| Text Primary | `#E4E4E4` | Main text |
| Text Secondary | `#A0A0A0` | Labels, hints |
| Toolbar | `#2D2D30` | Toolbar background |
| Editor | `#1E1E1E` | Editor background |

---

## Files Changed

1. ✅ **Created**: `Styles/LiveMarkdownTheme.axaml`
2. ✅ **Created**: `Styles/AvaloniaEditTheme.axaml`
3. ✅ **Modified**: `App.axaml` (style loading order)
4. ✅ **Modified**: `MainWindow.axaml` (removed duplicate)

---

## Potential Issues & Solutions

### If Light Theme Still Has Dark Spots

**Check**: Ensure style loading order is correct in App.axaml  
**Solution**: Overrides must come AFTER base themes

### If Colors Don't Update When Switching

**Check**: Are colors using DynamicResource?  
**Solution**: Change StaticResource to DynamicResource

### If Preview Doesn't Match Theme

**Check**: LiveMarkdownTheme.axaml is loaded  
**Solution**: Verify StyleInclude path is correct

### If Editor Doesn't Match Theme

**Check**: AvaloniaEditTheme.axaml is loaded  
**Solution**: Verify StyleInclude path and selectors

---

## Next Steps

1. **Test Both Themes**: Verify Light and Dark themes both look good
2. **Report Issues**: If specific elements still don't match, identify them
3. **Fine-tune Colors**: Adjust if contrast isn't sufficient
4. **Document**: Update user-facing docs if needed

---

## Build Status

✅ **Build Successful**: 0 errors, 0 warnings  
✅ **Files Created**: 2 new theme files  
✅ **Files Modified**: 2 existing files  
⏳ **Requires Testing**: Manual theme switching verification

---

**Fix Applied**: 2025-10-14  
**Status**: Ready for testing
