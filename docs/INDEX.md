# MarkPad Documentation Index

Welcome to the MarkPad documentation. This index helps you find the information you need.

---

## 📚 Core Documentation

### Getting Started
- **[README.md](../README.md)** - Project overview, quick start, and feature summary
- **[FEATURES.md](FEATURES.md)** - Complete feature documentation with usage instructions

### Technical Documentation
- **[ARCHITECTURE.md](ARCHITECTURE.md)** - Detailed architecture, design patterns, and code organization
- **[TESTING_GUIDE.md](TESTING_GUIDE.md)** - Manual testing checklist for verification

---

## 🗂️ Documentation Structure

```
docs/
├── INDEX.md                 (this file)
├── ARCHITECTURE.md          Architecture & design
├── FEATURES.md              Feature documentation
├── TESTING_GUIDE.md         Testing procedures
├── UI_IMPROVEMENTS.md       UI design decisions
├── issues/                  Issue templates
│   └── ISSUE_TEMPLATE.md
└── archive/                 Historical documents
    ├── BUG_ANALYSIS.md
    ├── BUILD_FIX_SESSION.md
    ├── PROGRESS.md
    └── ... (other historical docs)
```

---

## 🎯 Quick Navigation

### For Users
- **Want to know what MarkPad can do?** → [FEATURES.md](FEATURES.md)
- **Need help using a feature?** → [FEATURES.md](FEATURES.md) (detailed usage)
- **Found a bug or issue?** → Check [issues/ISSUE_TEMPLATE.md](issues/ISSUE_TEMPLATE.md)

### For Developers
- **Understanding the codebase?** → [ARCHITECTURE.md](ARCHITECTURE.md)
- **Want to contribute?** → Start with [ARCHITECTURE.md](ARCHITECTURE.md)
- **Testing changes?** → [TESTING_GUIDE.md](TESTING_GUIDE.md)
- **Curious about design decisions?** → [UI_IMPROVEMENTS.md](UI_IMPROVEMENTS.md)

### For Project History
- **Development journey?** → [archive/](archive/) folder
- **Bug fixes history?** → [archive/BUG_ANALYSIS.md](archive/BUG_ANALYSIS.md)
- **Build issues solved?** → [archive/BUILD_FIX_SESSION.md](archive/BUILD_FIX_SESSION.md)

---

## 📖 Document Summaries

### README.md
- **Audience**: Everyone
- **Purpose**: First stop for anyone interested in MarkPad
- **Contains**: 
  - Feature overview
  - Quick start guide
  - Technology stack
  - Project status

### FEATURES.md
- **Audience**: Users and developers
- **Purpose**: Complete feature reference
- **Contains**:
  - All features with detailed descriptions
  - Usage instructions
  - Keyboard shortcuts
  - Tips and best practices
  - Known limitations

### ARCHITECTURE.md
- **Audience**: Developers
- **Purpose**: Technical deep-dive
- **Contains**:
  - Clean Architecture explanation
  - Layer structure and responsibilities
  - Design patterns used
  - Code organization
  - Extensibility points
  - SOLID compliance

### TESTING_GUIDE.md
- **Audience**: QA and developers
- **Purpose**: Ensure quality
- **Contains**:
  - Manual testing checklists
  - Test scenarios
  - Expected behaviors
  - Edge cases

### UI_IMPROVEMENTS.md
- **Audience**: Developers and designers
- **Purpose**: UI/UX design documentation
- **Contains**:
  - Theme system
  - Color schemes
  - Visual design decisions

---

## 🏗️ Architecture at a Glance

```
Presentation Layer (Avalonia UI)
  ↓ depends on
Application Layer (Use Cases & Interfaces)
  ↓ depends on          ↓ depends on
Domain Layer ←──── Infrastructure Layer
(Core Logic)           (External Implementations)
```

**Key Principle**: Dependencies point inward. Domain has no dependencies.

---

## 🔑 Key Concepts

### Clean Architecture
- **What**: Architectural pattern separating concerns into layers
- **Why**: Maintainability, testability, flexibility
- **Where**: [ARCHITECTURE.md](ARCHITECTURE.md#-architecture-overview)

### MVVM Pattern
- **What**: Model-View-ViewModel design pattern
- **Why**: Separation of UI and business logic
- **Where**: [ARCHITECTURE.md](ARCHITECTURE.md#4-presentation-layer-markpaddesktop)

### Multi-Document Interface
- **What**: Tab-based document management
- **Why**: Work on multiple files simultaneously
- **Where**: [FEATURES.md](FEATURES.md#-multi-document-interface)

---

## 📋 Common Tasks

### I want to...

**...understand what MarkPad does**  
→ Read [README.md](../README.md) first, then [FEATURES.md](FEATURES.md)

**...learn how to use a specific feature**  
→ Check [FEATURES.md](FEATURES.md) for detailed usage

**...understand the code structure**  
→ Start with [ARCHITECTURE.md](ARCHITECTURE.md)

**...add a new feature**  
→ Read [ARCHITECTURE.md](ARCHITECTURE.md) → Find extensibility points → Follow patterns

**...fix a bug**  
→ Review [ARCHITECTURE.md](ARCHITECTURE.md) → Identify affected layer → Fix → Test with [TESTING_GUIDE.md](TESTING_GUIDE.md)

**...test the application**  
→ Follow [TESTING_GUIDE.md](TESTING_GUIDE.md) checklist

---

## 🗺️ Document Relationships

```
README.md (Overview)
    ├──→ FEATURES.md (What can it do?)
    ├──→ ARCHITECTURE.md (How is it built?)
    └──→ TESTING_GUIDE.md (How to verify?)

ARCHITECTURE.md
    ├──→ Domain Layer explanation
    ├──→ Application Layer explanation
    ├──→ Infrastructure Layer explanation
    └──→ Presentation Layer explanation

FEATURES.md
    ├──→ Document Management
    ├──→ Multi-Document Interface
    ├──→ Editing Features
    └──→ Preview Features
```

---

## 🔄 Version History

| Version | Date | Major Changes |
|---------|------|---------------|
| 1.0.0 | 2025-10-14 | Initial release with multi-tab support |

---

## 💡 Tips for Navigation

1. **Start with README.md** for overview
2. **Use FEATURES.md** for "how-to" questions
3. **Reference ARCHITECTURE.md** for "why" and "how it works"
4. **Archive folder** contains historical context (optional reading)

---

## 📞 Getting Help

- **Feature questions**: Check [FEATURES.md](FEATURES.md)
- **Technical questions**: Review [ARCHITECTURE.md](ARCHITECTURE.md)
- **Something not working**: Verify with [TESTING_GUIDE.md](TESTING_GUIDE.md)
- **Historical context**: Browse [archive/](archive/) folder

---

**Documentation Last Updated:** 2025-10-14  
**MarkPad Version:** 1.0.0
