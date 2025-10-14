# MarkPad Features

Complete feature documentation for MarkPad Markdown Editor.

---

## 📑 Table of Contents

1. [Document Management](#document-management)
2. [Multi-Document Interface](#multi-document-interface)
3. [Editing Features](#editing-features)
4. [Preview Features](#preview-features)
5. [User Interface](#user-interface)
6. [Keyboard Shortcuts](#keyboard-shortcuts)

---

## 📂 Document Management

### File Operations

#### New Document
- **Access**: File → New or `Ctrl+N`
- **Behavior**: Creates a new untitled document in a new tab
- **Notes**: Previous unsaved changes are preserved in their respective tabs

#### Open Document
- **Access**: File → Open or `Ctrl+O`
- **Supported formats**: `.md`, `.markdown`
- **Behavior**: Opens file in a new tab
- **Alternative**: Drag & drop files from File Explorer

#### Save Document
- **Access**: File → Save or `Ctrl+S`
- **Behavior**: 
  - If file has path: Saves to existing location
  - If new file: Opens Save As dialog
- **Auto-detection**: Tracks unsaved changes with `*` indicator in tab

#### Save As
- **Access**: File → Save As or `Ctrl+Shift+S`
- **Behavior**: Always prompts for new file location
- **Use case**: Create a copy or rename document

#### Close Tab
- **Access**: 
  - File → Close Tab
  - `Ctrl+W`
  - Click × button on tab
- **Safety**: Prompts to save if document has unsaved changes
- **Auto-create**: If last tab is closed, creates new empty document

---

## 🗂️ Multi-Document Interface

### Tab Management

#### Multiple Documents
- **Capacity**: Unlimited number of open tabs
- **Switching**: Click tab header to switch documents
- **Indicator**: Asterisk (*) shows unsaved changes
- **State**: Each tab maintains independent:
  - Content
  - Editing position
  - Preview zoom level
  - Statistics (word/char count)

#### Drag & Drop
- **Supported files**: `.md` and `.markdown`
- **Multiple files**: Can drop several files at once
- **Behavior**: Each file opens in new tab
- **Drop zones**: Anywhere on the main window
- **Visual feedback**: Copy cursor shows valid drop

#### Tab Display
- **Title format**: `[*]filename.md` (* = unsaved)
- **Close button**: × on each tab for easy closing
- **Active indicator**: Visual highlight on selected tab

---

## ✏️ Editing Features

### Text Formatting

#### Bold
- **Syntax**: `**text**`
- **Shortcut**: `Ctrl+B`
- **Button**: **B** on toolbar
- **Toggle**: Works on selection or inserts markers

#### Italic
- **Syntax**: `*text*`
- **Shortcut**: `Ctrl+I`
- **Button**: *I* on toolbar
- **Toggle**: Works on selection or inserts markers

#### Strikethrough
- **Syntax**: `~~text~~`
- **Button**: ~~S~~ on toolbar
- **Toggle**: Works on selection or inserts markers

### Headings

#### Heading Levels
- **H1**: `# Heading` - Main heading
- **H2**: `## Heading` - Subheading
- **H3**: `### Heading` - Sub-subheading
- **Buttons**: H₁, H₂, H₃ on toolbar
- **Behavior**: Inserts syntax at line start

### Lists

#### Bullet List
- **Syntax**: `- Item`
- **Button**: • on toolbar
- **Behavior**: Adds marker at line start

#### Numbered List
- **Syntax**: `1. Item`
- **Button**: 1. on toolbar
- **Behavior**: Adds numbered marker

#### Task List
- **Syntax**: `- [ ] Task`
- **Button**: ☑ on toolbar
- **Interactive**: Can check/uncheck in preview

### Links and Media

#### Hyperlinks
- **Syntax**: `[text](url)`
- **Shortcut**: `Ctrl+K`
- **Button**: 🔗 on toolbar
- **Template**: Inserts with example URL

#### Images
- **Syntax**: `![alt](url)`
- **Button**: 🖼️ on toolbar
- **Support**: URLs and local file paths

### Code

#### Inline Code
- **Syntax**: `` `code` ``
- **Button**: ` on toolbar
- **Use**: For inline code snippets

#### Code Blocks
- **Syntax**: ``` language ... ```
- **Button**: Code block button
- **Features**:
  - Syntax highlighting in preview
  - Copy button in preview
  - Language specification support

---

## 👁️ Preview Features

### Real-Time Rendering
- **Engine**: LiveMarkdown.Avalonia powered by Markdig
- **Update**: Instant as you type
- **Support**: Full GitHub Flavored Markdown (GFM)

### Markdown Elements

#### Supported Syntax
- Headers (H1-H6)
- Bold, Italic, Strikethrough
- Ordered and unordered lists
- Task lists with checkboxes
- Links and images
- Code blocks with syntax highlighting
- Inline code
- Blockquotes
- Tables
- Horizontal rules

### Code Block Features
- **Syntax Highlighting**: Multiple language support
- **Copy Button**: Built-in button to copy code
- **Language Display**: Shows language name
- **Line Wrapping**: Handles long lines properly

### Preview Controls

#### Zoom
- **Controls**: `Ctrl + Mouse Wheel`
- **Range**: 50% to 300%
- **Display**: Shows zoom percentage
- **Per-tab**: Each document has independent zoom

#### Split View
- **Layout**: Editor (left) | Preview (right)
- **Splitter**: Draggable divider between panels
- **Resize**: Adjust relative sizes to preference
- **Synchronized**: Preview updates with editor

---

## 🎨 User Interface

### Theme Support

#### Available Themes
- **System Default**: Follows OS theme settings
- **Light Theme**: Bright, high-contrast
- **Dark Theme**: Easy on eyes, modern
- **Access**: View → Theme menu

#### Theme Colors
Each theme provides consistent:
- Background colors
- Text colors
- Border colors
- Toolbar colors
- Editor background
- Syntax highlighting colors

### Status Bar

#### Information Display
- **Word Count**: Real-time word counting
- **Character Count**: Real-time character counting
- **Status Messages**: Operation feedback
- **Per-tab**: Statistics for active document

### Window Layout

#### Main Areas
1. **Menu Bar**: Top - File and View menus
2. **Toolbar**: Below menu - Formatting buttons
3. **Tab Bar**: Document tabs
4. **Editor Panel**: Left side with line numbers
5. **Preview Panel**: Right side with markdown rendering
6. **Status Bar**: Bottom with statistics

---

## ⌨️ Keyboard Shortcuts

### File Operations
| Action | Shortcut |
|--------|----------|
| New Document | `Ctrl+N` |
| Open Document | `Ctrl+O` |
| Save Document | `Ctrl+S` |
| Save As | `Ctrl+Shift+S` |
| Close Tab | `Ctrl+W` |

### Formatting
| Action | Shortcut |
|--------|----------|
| Bold | `Ctrl+B` |
| Italic | `Ctrl+I` |
| Insert Link | `Ctrl+K` |

### Preview
| Action | Shortcut |
|--------|----------|
| Zoom In/Out | `Ctrl+Mouse Wheel` |

### Navigation
| Action | Method |
|--------|--------|
| Switch Tabs | Click tab header |
| Focus Editor | Click in editor |
| Focus Preview | Click in preview |

---

## 🔧 Technical Features

### Performance
- **Real-time updates**: Optimized for live editing
- **Large files**: Handles documents efficiently
- **Memory**: Per-tab state management

### File Handling
- **Encoding**: UTF-8 with BOM detection
- **Line endings**: Preserves original (CRLF/LF)
- **Autosave**: Not implemented (manual save only)

### Safety Features
- **Unsaved changes**: Detection and prompts
- **Cancel support**: All dialogs cancelable
- **Error handling**: Graceful error messages
- **Last tab protection**: Auto-creates new tab

---

## 💡 Usage Tips

### Efficient Workflow
1. **Use tabs**: Keep related documents open
2. **Drag & drop**: Fastest way to open files
3. **Keyboard shortcuts**: Speed up common operations
4. **Preview zoom**: Adjust for readability

### Best Practices
- **Save frequently**: No auto-save yet
- **Use formatting toolbar**: Visual formatting aid
- **Check preview**: Verify rendering
- **Theme choice**: Use dark for extended editing

### Common Workflows

#### Writing a Blog Post
1. `Ctrl+N` - New document
2. Type and format using toolbar
3. Check preview for final look
4. `Ctrl+S` - Save
5. Export Markdown to your blog

#### Multiple Documents
1. Open several related files
2. Switch tabs while referencing
3. Copy/paste between documents
4. Save all before closing

#### Code Documentation
1. Use code blocks with language
2. Preview shows syntax highlighting
3. Test copy button functionality
4. Format with headers for sections

---

## 🐛 Known Limitations

### Current Version
- No auto-save functionality
- No find/replace feature
- No export to HTML/PDF
- No spell checker
- Code block copy button has minimal UI feedback
- Tab reordering not supported

### Future Improvements
See README.md for planned enhancements.

---

## ❓ FAQ

### Q: Can I open multiple instances?
**A:** Yes, each instance is independent.

### Q: What happens if I close with unsaved changes?
**A:** You'll be prompted to save for each unsaved document.

### Q: Can I drag tabs to reorder?
**A:** Not yet, but it's on the roadmap.

### Q: Does it support LaTeX/Math?
**A:** Not currently, markdown only.

### Q: Can I customize themes?
**A:** Not through UI yet, but styles can be modified in code.

---

## 📞 Support

For issues or questions:
- Check documentation in `docs/`
- Review architecture in `docs/ARCHITECTURE.md`
- Examine code structure for customization

---

**Last Updated:** 2025-10-14  
**MarkPad Version:** 1.0.0
