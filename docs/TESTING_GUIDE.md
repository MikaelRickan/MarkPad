# Functional Testing Guide for MarkPad

**Purpose**: Verify all implemented features work as designed  
**Status**: Build succeeds - Ready to test functionality  
**Date**: October 13, 2025

---

## 🎯 Testing Priorities

### Priority 1: CRITICAL (Must Work)
These features are essential for basic functionality:

1. **Application Launch**
   - [ ] Application starts without crashing
   - [ ] Main window appears
   - [ ] UI elements are visible

2. **Text Editing**
   - [ ] Can type in editor pane
   - [ ] Text appears correctly
   - [ ] Cursor moves properly
   - [ ] Can select text
   - [ ] Can copy/paste

3. **File Operations**
   - [ ] New Document (Ctrl+N) creates blank document
   - [ ] Open (Ctrl+O) shows file dialog
   - [ ] Can open .md files
   - [ ] Save (Ctrl+S) works for existing files
   - [ ] Save As works for new files

4. **Markdown Preview**
   - [ ] Preview pane shows something
   - [ ] Updates when typing
   - [ ] Renders basic markdown (headers, bold, italic)

### Priority 2: IMPORTANT (Should Work)
These features enhance usability:

1. **Formatting Toolbar**
   - [ ] Bold button (Ctrl+B)
   - [ ] Italic button (Ctrl+I)
   - [ ] Strikethrough
   - [ ] Heading buttons (H1, H2, H3)
   - [ ] List buttons (bullet, numbered, task)
   - [ ] Link button (Ctrl+K)
   - [ ] Image button

2. **Unsaved Changes**
   - [ ] * appears in title when modified
   - [ ] Confirmation dialog when closing unsaved
   - [ ] Confirmation dialog when opening new file with unsaved changes

3. **Status Bar**
   - [ ] Word count updates
   - [ ] Character count updates
   - [ ] Shows "Ready" status

### Priority 3: NICE TO HAVE (Can Wait)
These features polish the experience:

1. **Theme Switching**
   - [ ] Theme menu exists
   - [ ] Can switch between themes
   - [ ] Theme persists

2. **Window Behavior**
   - [ ] Window title updates with filename
   - [ ] Window can be resized
   - [ ] Splitter can adjust panes

3. **Advanced Markdown**
   - [ ] Code blocks with syntax highlighting
   - [ ] Tables render correctly
   - [ ] Links are clickable
   - [ ] Images display

---

## 📝 Test Scenarios

### Scenario 1: New User First Run

**Goal**: Verify clean first-time experience

**Steps**:
1. Launch MarkPad
2. Verify blank document appears
3. Type some text
4. Verify preview updates
5. Use Save As to save file
6. Verify saved correctly
7. Close application

**Expected Results**:
- ✅ Application starts
- ✅ Can type immediately
- ✅ Preview shows formatted text
- ✅ Save As dialog appears
- ✅ File is created on disk
- ✅ Application closes cleanly

### Scenario 2: Edit Existing File

**Goal**: Verify file loading and editing

**Steps**:
1. Create a test .md file externally
2. Launch MarkPad
3. Use Open to load the file
4. Verify content appears in editor
5. Verify preview renders correctly
6. Make changes
7. Save the file
8. Verify changes persist

**Expected Results**:
- ✅ File dialog shows .md files
- ✅ Content loads in editor
- ✅ Preview shows formatted content
- ✅ Can edit the content
- ✅ Save updates the file
- ✅ Re-opening shows changes

### Scenario 3: Formatting Toolbar

**Goal**: Verify toolbar buttons work

**Steps**:
1. Type some text
2. Select the text
3. Click Bold button
4. Verify **text** appears
5. Verify preview shows bold
6. Try each toolbar button
7. Verify preview updates

**Expected Results**:
- ✅ Selection gets wrapped with markdown
- ✅ Preview shows formatting
- ✅ All buttons insert correct syntax
- ✅ Keyboard shortcuts work

### Scenario 4: Unsaved Changes Warning

**Goal**: Verify data loss prevention

**Steps**:
1. Create new document
2. Type some text
3. Verify * appears in title
4. Try to close window
5. Verify warning dialog
6. Choose "Cancel"
7. Verify window stays open
8. Try Open command
9. Verify warning dialog

**Expected Results**:
- ✅ Title shows * for unsaved
- ✅ Warning appears on close
- ✅ Can cancel close operation
- ✅ Warning appears on file operations
- ✅ Can save or discard changes

### Scenario 5: Markdown Rendering

**Goal**: Verify preview quality

**Test Content**:
```markdown
# Heading 1
## Heading 2
### Heading 3

**Bold text** and *italic text* and ~~strikethrough~~

- Bullet list item 1
- Bullet list item 2

1. Numbered item 1
2. Numbered item 2

[Link text](https://example.com)

`inline code`

```
code block
multi-line
```

> Blockquote
```

**Steps**:
1. Paste test content in editor
2. Verify preview renders correctly
3. Check all elements display

**Expected Results**:
- ✅ Headings have different sizes
- ✅ Bold/italic/strike work
- ✅ Lists are formatted
- ✅ Links are styled
- ✅ Code blocks have background
- ✅ Blockquotes are indented

---

## 🐛 Bug Tracking Template

When you find a bug, document it like this:

```markdown
### Bug #X: [Short Description]

**Severity**: Critical / Major / Minor
**Steps to Reproduce**:
1. Step one
2. Step two
3. Step three

**Expected**: What should happen
**Actual**: What actually happens
**Error Messages**: Any errors shown
**Workaround**: If any exists

**Status**: Open / In Progress / Fixed
```

---

## ✅ Testing Checklist

Copy this checklist to track your testing:

### Core Functionality
- [ ] Application launches
- [ ] Can type in editor
- [ ] Preview updates in real-time
- [ ] Can create new document
- [ ] Can open existing file
- [ ] Can save file
- [ ] Can save as new file
- [ ] Unsaved changes warning works

### Formatting
- [ ] Bold (Ctrl+B)
- [ ] Italic (Ctrl+I)
- [ ] Strikethrough
- [ ] H1 button
- [ ] H2 button
- [ ] H3 button
- [ ] Bullet list
- [ ] Numbered list
- [ ] Task list
- [ ] Link (Ctrl+K)
- [ ] Image

### UI Elements
- [ ] Menu bar accessible
- [ ] Toolbar buttons click
- [ ] Status bar updates
- [ ] Theme switching (if tested)
- [ ] Window resize works
- [ ] Pane splitter works

### Markdown Rendering
- [ ] Headers render
- [ ] Bold/italic/strike render
- [ ] Lists render
- [ ] Links render
- [ ] Code blocks render
- [ ] Blockquotes render
- [ ] Tables render (if supported)

### Error Handling
- [ ] Invalid file path handled
- [ ] Read-only file handled
- [ ] Disk full handled (if testable)
- [ ] Invalid markdown handled

---

## 📊 Test Results Format

After testing, create a summary document:

```markdown
# Test Results - [Date]

## Summary
- Tests Passed: X / Y
- Tests Failed: X / Y
- Critical Bugs: X
- Major Bugs: X
- Minor Issues: X

## Pass/Fail by Priority

### Priority 1 (Critical)
✅ Application Launch
✅ Text Editing
❌ File Operations (Bug #1)
✅ Markdown Preview

### Priority 2 (Important)
✅ Formatting Toolbar
❌ Unsaved Changes (Bug #2)
✅ Status Bar

### Priority 3 (Nice to Have)
✅ Theme Switching
✅ Window Behavior
⚠️ Advanced Markdown (partial)

## Bugs Found
[Link to bug reports]

## Recommendations
[What to fix first]
```

---

## 🚀 Quick Start Testing

**5-Minute Smoke Test**:

1. Launch app → Should open
2. Type "# Hello" → Preview should show heading
3. Click Bold → Should wrap **
4. Save As → Should create file
5. Close and reopen → Should load file

If all 5 steps work, proceed with full testing.  
If any fail, report the bug before continuing.

---

**Prepared by**: AI Development Agent (Claude)  
**Build Version**: Step 5 Complete  
**Last Updated**: October 13, 2025
