# MarkPad Architecture

This document describes the architectural design and principles behind MarkPad.

---

## 🏛️ Architecture Overview

MarkPad follows **Clean Architecture** principles with a focus on:
- **Separation of concerns**
- **Dependency inversion**
- **Testability**
- **Maintainability**

### Core Principles

1. **Domain-Centric**: Business logic is at the center
2. **Independent Layers**: Each layer can evolve independently
3. **Dependency Rule**: Dependencies point inward only
4. **SOLID Principles**: Applied throughout the codebase

---

## 📊 Layer Structure

```
┌─────────────────────────────────────────────────────────┐
│                    Presentation Layer                    │
│            (MarkPad.Desktop - Avalonia UI)              │
│  ┌──────────────┬──────────────┬──────────────┐        │
│  │  ViewModels  │   Services   │    Views     │        │
│  └──────────────┴──────────────┴──────────────┘        │
└─────────────────────┬───────────────────────────────────┘
                      │ Dependencies
┌─────────────────────▼───────────────────────────────────┐
│                  Application Layer                       │
│         (MarkPad.Application - Use Cases)               │
│  ┌──────────────┬──────────────┬──────────────┐        │
│  │  Interfaces  │   Services   │     DTOs     │        │
│  └──────────────┴──────────────┴──────────────┘        │
└─────────────────────┬───────────────────────────────────┘
                      │ Dependencies
        ┌─────────────┴─────────────┐
        │                           │
┌───────▼────────┐         ┌────────▼────────┐
│ Infrastructure │         │     Domain      │
│     Layer      │         │      Layer      │
│  (External)    │         │   (Core Logic)  │
└────────────────┘         └─────────────────┘
```

---

## 📦 Layer Details

### 1. Domain Layer (MarkPad.Domain)

**Purpose**: Core business logic and entities  
**Dependencies**: NONE - completely independent  
**Contents**:
- `Entities/Document.cs` - Core document entity
- Business rules
- Domain events (future)

#### Document Entity
```csharp
public class Document
{
    // State
    public string Content { get; set; }
    public FilePath? FilePath { get; }
    public bool HasUnsavedChanges { get; }
    
    // Behaviors
    public void Load(string path, string content)
    public void MarkAsNew()
    public void Save(string? path = null)
    public string GetDisplayName()
}
```

**Key Design Decisions**:
- Domain entity contains only business logic
- No UI concerns (no INotifyPropertyChanged)
- Immutable file path (use Save to change)
- Self-documenting methods

---

### 2. Application Layer (MarkPad.Application)

**Purpose**: Orchestrate business use cases  
**Dependencies**: Domain layer only  
**Contents**:
- `Interfaces/` - Service contracts
- `Services/` - Business service implementations

#### Key Interfaces

**IFileService** - File system operations
```csharp
Task<string> ReadTextAsync(string filePath)
Task WriteTextAsync(string filePath, string content)
```

**IDialogService** - File dialogs
```csharp
Task<string?> ShowOpenFileDialogAsync()
Task<string?> ShowSaveFileDialogAsync(string suggestedFileName)
```

**IMessageBoxService** - User notifications
```csharp
Task ShowErrorAsync(string title, string message)
Task<MessageBoxResult> ShowYesNoCancelAsync(string title, string message)
```

**IDocumentManagerService** - Multi-document management
```csharp
Document CreateNew()
Task<Document?> OpenAsync()
Task<Document?> OpenFileAsync(string filePath)
Task SaveAsync(Document document)
Task SaveAsAsync(Document document)
Task<bool> CloseDocumentAsync(Document document)
void SetActiveDocument(Document document)
```

#### Service Implementations

**DocumentManagerService**
- Manages document lifecycle
- Coordinates file operations
- Handles unsaved changes
- Provides active document tracking

**Key Design Decisions**:
- Interfaces define contracts, not implementations
- All cross-layer communication via interfaces
- Services coordinate between domain and infrastructure
- No direct infrastructure dependencies

---

### 3. Infrastructure Layer (MarkPad.Infrastructure)

**Purpose**: Implement external concerns  
**Dependencies**: Application (interfaces), Domain  
**Contents**:
- `Services/FileService.cs` - File I/O implementation

#### FileService Implementation
```csharp
public class FileService : IFileService
{
    public async Task<string> ReadTextAsync(string filePath)
    {
        // File.ReadAllTextAsync implementation
    }
    
    public async Task WriteTextAsync(string filePath, string content)
    {
        // File.WriteAllTextAsync implementation
    }
}
```

**Key Design Decisions**:
- Implements interfaces from Application layer
- Contains all external dependencies
- Could be swapped without changing business logic
- Testable via mocking

---

### 4. Presentation Layer (MarkPad.Desktop)

**Purpose**: Avalonia UI and user interaction  
**Dependencies**: Application, Infrastructure  
**Pattern**: MVVM (Model-View-ViewModel)  
**Contents**:
- `ViewModels/` - View models
- `Views/` - AXAML files
- `Services/` - UI-specific services
- `Styles/` - Application styling

#### Key ViewModels

**MainViewModel**
- Manages tab collection
- Coordinates document operations
- Exposes window-level state
```csharp
public class MainViewModel
{
    ObservableCollection<DocumentTabViewModel> Tabs
    DocumentTabViewModel? SelectedTab
    string WindowTitle
    string StatusMessage
    // ... methods for file ops
}
```

**DocumentTabViewModel**
- Represents single document tab
- Manages editor state
- Preview zoom level
- Word/character counts
```csharp
public class DocumentTabViewModel
{
    Document Document
    TextDocument EditorDocument        // AvaloniaEdit
    ObservableStringBuilder MarkdownBuilder  // LiveMarkdown
    string TabHeader
    double PreviewZoom
    // ... formatting methods
}
```

#### UI-Specific Services

**AvaloniaDialogService**
- Implements IDialogService using Avalonia's StorageProvider
- Native file dialogs
```csharp
public class AvaloniaDialogService : IDialogService
{
    private readonly IStorageProvider _storageProvider;
    
    public async Task<string?> ShowOpenFileDialogAsync()
    {
        // Avalonia file picker implementation
    }
}
```

**AvaloniaMessageBoxService**
- Implements IMessageBoxService using MessageBox.Avalonia
- Modal dialogs with Yes/No/Cancel options

**Key Design Decisions**:
- ViewModels have no direct UI dependencies
- ViewModels communicate via properties and commands
- Services injected via dependency injection
- Clear separation between ViewModel and View

---

## 🔄 Data Flow

### Opening a File
```
1. User clicks "Open" (MainWindow.axaml)
   ↓
2. Event handler calls ViewModel.OpenDocument() (MainViewModel)
   ↓
3. ViewModel calls DocumentManagerService.OpenAsync()
   ↓
4. Service uses IDialogService to get file path
   ↓
5. Service uses IFileService to read file
   ↓
6. Service creates new Document entity
   ↓
7. ViewModel creates DocumentTabViewModel
   ↓
8. Tab added to Tabs collection
   ↓
9. UI updates via data binding
```

### Saving Changes
```
1. User presses Ctrl+S
   ↓
2. MainWindow routes to ViewModel.SaveDocument()
   ↓
3. ViewModel calls DocumentManagerService.SaveAsync(document)
   ↓
4. If no path: Service calls IDialogService for path
   ↓
5. Service calls IFileService.WriteTextAsync()
   ↓
6. Document.Save() updates internal state
   ↓
7. PropertyChanged events update UI
```

---

## 🎯 Dependency Injection

### Service Registration (App.axaml.cs)

```csharp
var services = new ServiceCollection();

// Infrastructure services
services.AddSingleton<IFileService, FileService>();

// Presentation services (require Avalonia types)
services.AddSingleton<IDialogService>(
    new AvaloniaDialogService(mainWindow.StorageProvider));
services.AddSingleton<IMessageBoxService>(
    new AvaloniaMessageBoxService(mainWindow));

// Application services
services.AddSingleton<IDocumentManagerService, DocumentManagerService>();

var serviceProvider = services.BuildServiceProvider();

// Create ViewModel with dependencies
var viewModel = new MainViewModel(
    serviceProvider.GetRequiredService<IDocumentManagerService>());
```

**Benefits**:
- Loose coupling
- Easy testing (mock services)
- Clear dependencies
- Flexible implementations

---

## 🧪 Testability

### Unit Testing Strategy

**Domain Layer**
- Pure logic testing
- No mocking required
- Fast execution
```csharp
[Test]
public void Document_GetDisplayName_ReturnsUntitled_WhenNoPath()
{
    var doc = new Document();
    Assert.Equal("Untitled", doc.GetDisplayName());
}
```

**Application Layer**
- Mock infrastructure interfaces
- Test service coordination
```csharp
[Test]
public async Task DocumentManager_OpenAsync_ReturnsDocument()
{
    var mockDialog = new Mock<IDialogService>();
    mockDialog.Setup(x => x.ShowOpenFileDialogAsync())
        .ReturnsAsync("/path/file.md");
    
    var mockFile = new Mock<IFileService>();
    mockFile.Setup(x => x.ReadTextAsync(It.IsAny<string>()))
        .ReturnsAsync("# Content");
    
    var service = new DocumentManagerService(
        mockFile.Object, mockDialog.Object, ...);
    
    var doc = await service.OpenAsync();
    Assert.NotNull(doc);
}
```

**Presentation Layer**
- Test ViewModel logic
- Mock application services
- Verify state changes

---

## 🔒 Design Patterns Used

### Repository Pattern
- `IFileService` abstracts file system
- Could swap with database, cloud storage, etc.

### Service Layer Pattern
- Business logic in services
- Coordinates domain and infrastructure
- Examples: `DocumentManagerService`

### MVVM Pattern
- ViewModels expose state to Views
- Commands handle user actions
- Data binding for automatic updates

### Dependency Injection
- Constructor injection throughout
- Interface-based dependencies
- Service locator in App startup

### Observer Pattern
- `INotifyPropertyChanged` in ViewModels
- Reactive updates in UI
- Event-driven state changes

---

## 📐 Design Decisions & Rationale

### Why Clean Architecture?
**Decision**: Use Clean Architecture over traditional layered architecture  
**Rationale**:
- Business logic independence
- Better testability
- Future-proof design
- Clear boundaries

### Why Avalonia over WPF?
**Decision**: Use Avalonia UI instead of WPF  
**Rationale**:
- Cross-platform support (Windows, macOS, Linux)
- Modern UI capabilities
- Active development
- Similar to WPF for migration ease

### Why MVVM?
**Decision**: MVVM instead of MVC or MVP  
**Rationale**:
- Natural fit for data binding
- Clear separation of concerns
- Testable ViewModels
- Standard pattern for XAML frameworks

### Why LiveMarkdown.Avalonia?
**Decision**: LiveMarkdown over Markdown.Avalonia  
**Rationale**:
- Real-time performance
- Designed for streaming updates
- Modern implementation
- Active development

### Document vs DocumentViewModel Split
**Decision**: Separate domain Document from DocumentTabViewModel  
**Rationale**:
- Domain stays pure
- UI concerns in ViewModel
- Testable business logic
- Clear boundaries

---

## 🚀 Extensibility Points

### Adding New File Formats
1. Extend `IFileService` with format-specific methods
2. Implement in `FileService`
3. Update Document entity if needed

### Adding Export Features
1. Create `IExportService` interface in Application
2. Implement in Infrastructure (HTML, PDF, etc.)
3. Expose via ViewModel
4. Add UI in MainWindow

### Adding Auto-Save
1. Add timer to MainViewModel
2. Call SaveAsync periodically
3. Track last save time in DocumentTabViewModel
4. Add user preference for interval

### Adding Spell Check
1. Create `ISpellCheckService` interface
2. Implement using external library
3. Integrate in editor via AvaloniaEdit
4. Show squiggles in editor

---

## 📚 Code Organization Best Practices

### Naming Conventions
- **Interfaces**: `IServiceName`
- **ViewModels**: `NameViewModel`
- **Services**: `NameService`
- **Events**: `OnEventName`

### File Structure
- One class per file
- Match filename to class name
- Group by feature/concern
- Keep related code together

### Comments
- XML documentation on public APIs
- Inline comments for complex logic
- Self-documenting code preferred
- Architecture decisions documented

---

## 🔍 Code Quality Metrics

### SOLID Compliance
- ✅ **S**ingle Responsibility - Each class has one job
- ✅ **O**pen/Closed - Extensible without modification
- ✅ **L**iskov Substitution - Interfaces properly implemented
- ✅ **I**nterface Segregation - Focused interfaces
- ✅ **D**ependency Inversion - Depend on abstractions

### Complexity
- Low cyclomatic complexity
- Shallow inheritance hierarchies
- Minimal coupling
- High cohesion

---

## 📖 Further Reading

- [Clean Architecture (Robert C. Martin)](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
- [SOLID Principles](https://en.wikipedia.org/wiki/SOLID)
- [MVVM Pattern](https://docs.avaloniaui.net/docs/concepts/the-mvvm-pattern/)
- [Avalonia Documentation](https://docs.avaloniaui.net/)

---

**Last Updated:** 2025-10-14  
**MarkPad Version:** 1.0.0
