# MarkPad

**A modern, feature-rich Markdown editor built with Avalonia UI and Clean Architecture**

![.NET](https://img.shields.io/badge/.NET-8.0-blue)
![Platform](https://img.shields.io/badge/platform-Windows%20%7C%20macOS%20%7C%20Linux-lightgrey)
![Status](https://img.shields.io/badge/status-active-success)

---

## 📋 Overview

MarkPad is a professional Markdown editor designed for writers, developers, and anyone who works with Markdown daily. Built with Clean Architecture principles and modern UI design, it offers a seamless editing experience with real-time preview and comprehensive formatting tools.

### ✨ Key Features

#### **Core Functionality**
- 📝 **Multi-Document Tabs** - Work on multiple files simultaneously with easy tab switching
- 🔄 **Real-Time Preview** - Live Markdown rendering as you type using LiveMarkdown.Avalonia
- 💾 **Smart File Management** - New, Open, Save, Save As with unsaved changes protection
- 🎯 **Drag & Drop** - Drop .md files directly into the editor to open them

#### **Editing Tools**
- 🎨 **Rich Formatting Toolbar** - Quick access to Bold, Italic, Strikethrough, Headers, Lists
- ⌨️ **Keyboard Shortcuts** - Full keyboard support for all operations
- 📊 **Live Statistics** - Real-time word and character count per document
- 📋 **Code Block Copy** - Built-in copy buttons for code blocks in preview

#### **User Experience**
- 🌓 **Theme Support** - System, Light, and Dark themes
- 🔍 **Preview Zoom** - Ctrl+Mouse Wheel zoom (50%-300%)
- 📐 **Split View** - Editor and preview side-by-side with resizable splitter
- 🎯 **Clean UI** - Modern Fluent Design with thoughtful interactions

---

## 🚀 Quick Start

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) or later
- Windows 10/11, macOS 10.14+, or modern Linux distribution

### Building & Running

```bash
# Clone or navigate to the project
cd MarkPad

# Restore dependencies
dotnet restore

# Build the solution
dotnet build

# Run the application
dotnet run --project src/MarkPad.Desktop/MarkPad.Desktop.csproj
```

---

## 📁 Project Structure

The project follows Clean Architecture principles with clear separation of concerns:

```
MarkPad/
├── src/
│   ├── MarkPad.Domain/           # Core business logic & entities
│   │   └── Entities/             # Document domain model
│   ├── MarkPad.Application/      # Use cases & interfaces
│   │   ├── Interfaces/           # Service contracts
│   │   └── Services/             # Business services
│   ├── MarkPad.Infrastructure/   # External implementations
│   │   └── Services/             # File system, dialogs
│   └── MarkPad.Desktop/          # Avalonia UI application
│       ├── ViewModels/           # MVVM view models
│       ├── Services/             # UI-specific services
│       └── Styles/               # UI themes & styles
└── docs/                         # Project documentation
    ├── ARCHITECTURE.md           # Architecture overview
    ├── FEATURES.md               # Detailed feature list
    └── archive/                  # Historical documents
```

### Architecture Layers

**Domain** → Pure business logic, no dependencies  
**Application** → Use cases, coordinates domain & infrastructure  
**Infrastructure** → External concerns (files, dialogs, etc.)  
**Presentation** → Avalonia UI with MVVM pattern

---

## 🛠️ Technology Stack

- **UI Framework:** [Avalonia UI](https://avaloniaui.net/) 11.2.7
- **Language:** C# 12 (.NET 8)
- **Architecture:** Clean Architecture with SOLID principles
- **Pattern:** MVVM (Model-View-ViewModel)
- **Markdown Engine:** [Markdig](https://github.com/xoofx/markdig)
- **Markdown Renderer:** [LiveMarkdown.Avalonia](https://github.com/DearVa/LiveMarkdown.Avalonia) 1.0.0
- **Text Editor:** [AvaloniaEdit](https://github.com/AvaloniaUI/AvaloniaEdit) 11.2.0
- **Dependency Injection:** Microsoft.Extensions.DependencyInjection

---

## 📖 Usage

### File Operations

- **New Document**: `Ctrl+N` or File → New
- **Open File**: `Ctrl+O` or File → Open (also supports drag & drop)
- **Save**: `Ctrl+S` or File → Save
- **Save As**: `Ctrl+Shift+S` or File → Save As
- **Close Tab**: `Ctrl+W` or click the × on tab or File → Close Tab

### Formatting

- **Bold**: `Ctrl+B` or click **B** button - `**text**`
- **Italic**: `Ctrl+I` or click *I* button - `*text*`
- **Strikethrough**: Click ~~S~~ button - `~~text~~`
- **Headings**: Click H₁, H₂, H₃ buttons - `# Heading`
- **Lists**: Bullet (•), Numbered (1.), Task (☑) buttons
- **Links**: `Ctrl+K` - `[text](url)`
- **Code**: Inline `` ` `` or block ``` buttons

### Preview Controls

- **Zoom In/Out**: `Ctrl+Mouse Wheel` over preview
- **Resize Panels**: Drag the splitter between editor and preview
- **Theme Switch**: View → Theme → System/Light/Dark

### Multi-Document Workflow

1. Open multiple files via File → Open or drag & drop
2. Switch between tabs by clicking on tab headers
3. Each tab maintains its own:
   - Content and editing state
   - Preview zoom level
   - Unsaved changes status
   - Word/character count

---

## 🏗️ Architecture Highlights

### Clean Architecture Benefits

- **Testability**: Core logic independent of UI and infrastructure
- **Maintainability**: Clear separation of concerns
- **Flexibility**: Easy to swap implementations (e.g., different file systems)
- **Scalability**: New features can be added without breaking existing code

### Key Design Patterns

- **Repository Pattern**: File operations abstracted through interfaces
- **Service Layer**: Business logic encapsulated in domain services
- **MVVM**: Clean separation of UI and business logic
- **Dependency Injection**: Loose coupling and easy testing

---

## 📚 Documentation

- **[ARCHITECTURE.md](docs/ARCHITECTURE.md)** - Detailed architecture documentation
- **[FEATURES.md](docs/FEATURES.md)** - Complete feature list and usage
- **[TESTING_GUIDE.md](docs/TESTING_GUIDE.md)** - Manual testing checklist

Historical development documents are in `docs/archive/`.

---

## 🎯 Current Status

✅ **Stable & Feature-Complete for Core Use Cases**

### Working Features
- ✅ Multi-document tab interface
- ✅ File operations (New, Open, Save, Save As, Close)
- ✅ Drag & drop file opening
- ✅ Real-time Markdown preview with syntax highlighting
- ✅ Rich text formatting toolbar
- ✅ Keyboard shortcuts
- ✅ Theme switching (System, Light, Dark)
- ✅ Preview zoom
- ✅ Live statistics (words, characters)
- ✅ Unsaved changes detection
- ✅ Code block copy functionality

---

## 🔮 Future Enhancements

Potential features for future versions:

- Recent files list
- Find and replace
- Export to HTML/PDF
- Auto-save functionality
- Tab reordering
- Custom themes
- Extended Markdown syntax (diagrams, math equations)
- Spell checker
- Version control integration

---

## 🤝 Contributing

This project follows Clean Architecture and SOLID principles. Contributions are welcome!

### Development Guidelines

1. Maintain separation of concerns across layers
2. Keep Domain layer free of external dependencies
3. Use interfaces for all cross-layer dependencies
4. Follow MVVM pattern in presentation layer
5. Write clear, self-documenting code

---

## 📄 License

[License to be determined]

---

## 🙏 Acknowledgments

Built with:
- [Avalonia UI](https://avaloniaui.net/) - Cross-platform UI framework
- [Markdig](https://github.com/xoofx/markdig) - Markdown parsing
- [LiveMarkdown.Avalonia](https://github.com/DearVa/LiveMarkdown.Avalonia) - Real-time rendering
- [AvaloniaEdit](https://github.com/AvaloniaUI/AvaloniaEdit) - Text editor component

---

**Last Updated:** 2025-10-14  
**Version:** 1.0.0  
**Status:** 🟢 Stable and actively maintained
