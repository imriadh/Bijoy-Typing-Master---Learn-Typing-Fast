# 📐 Architecture & Design Document

## System Overview

**Bijoy Typing Master** is a desktop application built with a modern layered architecture separating concerns between UI, business logic, data access, and external automation.

## Architecture Diagram

```
┌─────────────────────────────────────────────────────────────┐
│                        USER INTERFACE                        │
│  (XAML Pages: MainPage, PracticeWindow, PaymentWindow)      │
└────────────────┬────────────────────────────────────────────┘
                 │
┌────────────────▼────────────────────────────────────────────┐
│                     BUSINESS LOGIC LAYER                     │
│  ┌──────────────┐  ┌──────────────┐  ┌─────────────────┐  │
│  │TypingEngine  │  │LicenseManager│  │  IKeyboardLayout│  │
│  │- ProcessKey  │  │- IsValid()   │  │  - BijoyLayout  │  │
│  │- CalcWPM     │  │- ValidateKey │  │  - UnicodeLayout│  │
│  └──────────────┘  └──────────────┘  └─────────────────┘  │
└────────────────┬────────────────────────────────────────────┘
                 │
┌────────────────▼────────────────────────────────────────────┐
│                      DATA ACCESS LAYER                       │
│  ┌──────────────────────────────────────────────────────┐  │
│  │           DatabaseManager (SQLite)                    │  │
│  │  - Lessons Table      - UserProgress Table            │  │
│  └──────────────────────────────────────────────────────┘  │
└─────────────────────────────────────────────────────────────┘
                 │
┌────────────────▼────────────────────────────────────────────┐
│                    HARDWARE & STORAGE                        │
│  ┌─────────────┐  ┌──────────────┐  ┌─────────────────┐   │
│  │ HardwareId  │  │typing_master │  │  license.dat    │   │
│  │(CPU+Mobo ID)│  │     .db      │  │                 │   │
│  └─────────────┘  └──────────────┘  └─────────────────┘   │
└─────────────────────────────────────────────────────────────┘

                    EXTERNAL SYSTEM
┌─────────────────────────────────────────────────────────────┐
│              PYTHON AUTOMATION BOT                           │
│  (Gmail API + Google Sheets + License Generation)           │
│  ┌──────────┐  ┌───────────┐  ┌─────────────────────────┐ │
│  │ Monitor  │→ │ Validate  │→ │  Send License Email     │ │
│  │ bKash    │  │ Payment   │  │  (GitHub Actions 15min) │ │
│  └──────────┘  └───────────┘  └─────────────────────────┘ │
└─────────────────────────────────────────────────────────────┘
```

## Component Design

### 1. Presentation Layer (Views)

#### MainPage
- **Purpose**: Home screen with navigation
- **Responsibilities**:
  - Display menu options
  - Navigate to practice modes
  - Access settings

#### PracticeWindow
- **Purpose**: Main typing practice interface
- **Responsibilities**:
  - Display lesson text
  - Capture keyboard input
  - Show real-time WPM/Accuracy
  - Highlight correct/wrong characters
  - Save progress to database

#### PaymentWindow
- **Purpose**: License purchase and activation
- **Responsibilities**:
  - Display Machine ID
  - Show trial status
  - Open payment form
  - Validate and activate license keys

#### VirtualKeyboard (Control)
- **Purpose**: Visual keyboard representation
- **Responsibilities**:
  - Display QWERTY layout
  - Highlight pressed keys
  - Visual feedback during typing

### 2. Business Logic Layer (Services)

#### TypingEngine
```csharp
public class TypingEngine
{
    - IKeyboardLayout _currentLayout
    - Stopwatch _timer
    - int _correctCharacters
    - int _totalCharacters
    
    + void SetLayout(string layoutType)
    + void StartSession(string targetText)
    + string ProcessKeyPress(string key)
    + (double wpm, double accuracy) EndSession()
    - void CalculateMetrics()
}
```

**Key Algorithms**:
- **WPM Calculation**: `(totalCharacters / 5) / minutes`
- **Accuracy**: `(correctCharacters / totalCharacters) * 100`

#### Keyboard Layouts

**IKeyboardLayout** (Interface)
```csharp
public interface IKeyboardLayout
{
    string ProcessKey(string key, string buffer);
    string LayoutName { get; }
    bool RequiresBuffer(string key);
}
```

**BijoyLayout** (Implementation)
- Complex state machine for conjuncts
- Handles `&` symbol for Juktakkhor
- Pre-defined conjunct dictionary
- Buffer management for multi-key sequences

**UnicodeLayout** (Implementation)
- Simple key-to-character mapping
- No buffer required
- Direct Unicode output

#### LicenseManager
```csharp
public class LicenseManager
{
    - const int TRIAL_DAYS = 7
    
    + bool IsValid()
    + int GetRemainingTrialDays()
    + bool ValidateKey(string key)
    + bool ActivateLicense(string key)
    + string GetMachineId()
}
```

**License Key Algorithm**:
```
1. Get Machine ID: "A1B2-C3D4-E5F6-G7H8"
2. Remove dashes: "A1B2C3D4E5F6G7H8"
3. Reverse string: "8H7G6F5E4D3C2B1A"
4. Add month: "8H7G6F5E4D3C2B1A02"
5. Hash MD5: Take first 8 chars
6. Combine: reversed + hash
7. Format: XXXX-XXXX-XXXX-XXXX
```

#### HardwareId
```csharp
public static class HardwareId
{
    + static string GetMachineId()
    - static string GetProcessorId()      // WMI: Win32_Processor
    - static string GetMotherboardSerial() // WMI: Win32_BaseBoard
    - static string GenerateHash(string)   // SHA256
    - static string FormatMachineId(string)
}
```

### 3. Data Access Layer

#### DatabaseManager
```csharp
public class DatabaseManager
{
    - string _dbPath
    - string _connectionString
    
    + void InitializeDatabase()
    + List<Lesson> GetLessonsByType(string type)
    + Lesson? GetLessonById(int id)
    + void SaveProgress(UserProgress progress)
    + List<UserProgress> GetProgressHistory(int limit)
}
```

**Database Schema**:

```sql
-- Lessons Table
CREATE TABLE Lessons (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Title TEXT NOT NULL,
    Difficulty TEXT NOT NULL,
    TextContent TEXT NOT NULL,
    Type TEXT NOT NULL
);

-- UserProgress Table
CREATE TABLE UserProgress (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Date TEXT NOT NULL,
    WPM REAL NOT NULL,
    Accuracy REAL NOT NULL,
    LessonId INTEGER NOT NULL,
    FOREIGN KEY(LessonId) REFERENCES Lessons(Id)
);
```

### 4. External Automation System

#### Python License Bot

**Flow Diagram**:
```
Start
  ↓
Authenticate with Google (OAuth2)
  ↓
Search Gmail for "Bkash: Received"
  ↓
Extract TrxID and Amount
  ↓
Amount == 200? → No → Mark as read → End
  ↓ Yes
Search Google Sheet for TrxID
  ↓
Found && Status != "Delivered"? → No → End
  ↓ Yes
Generate License Key (MachineID)
  ↓
Send Email with Key
  ↓
Update Sheet: Status = "Delivered"
  ↓
Mark Gmail as read
  ↓
End
```

**Components**:
1. **Gmail API**: Email monitoring and manipulation
2. **Sheets API**: Order database
3. **Email Sender**: MIME multipart email generation
4. **Key Generator**: Implements same algorithm as C# app

## Data Flow

### Typing Session Flow
```
1. User selects layout (Bijoy/Unicode)
2. TypingEngine.SetLayout(layoutType)
3. DatabaseManager.GetLessonsByType(type)
4. Display lesson in PracticeWindow
5. User starts typing
6. For each key:
   a. TypingEngine.ProcessKeyPress(key)
   b. Layout.ProcessKey(key, buffer)
   c. Compare with target character
   d. Update WPM & Accuracy
   e. Highlight keys on VirtualKeyboard
7. Session complete
8. TypingEngine.EndSession()
9. DatabaseManager.SaveProgress(progress)
```

### License Activation Flow
```
User Side:
1. App generates HardwareId
2. Display Machine ID in PaymentWindow
3. User copies ID
4. User submits form with ID + payment
5. User receives email with key
6. User enters key in app
7. LicenseManager.ValidateKey(key)
8. If valid → Save to license.dat
9. App unlocked

Bot Side (Parallel):
1. Gmail receives bKash notification
2. Bot extracts TrxID
3. Bot searches Sheet for TrxID
4. Bot gets MachineID from Sheet
5. Bot generates License Key
6. Bot sends email to user
7. Bot updates Sheet
```

## Security Considerations

### Machine ID Generation
- Combines CPU ID + Motherboard Serial
- SHA256 hashing prevents reverse engineering
- Unique per physical machine
- Survives OS reinstall

### License Validation
- Keys are machine-specific
- Month-based component prevents key sharing
- MD5 hash adds complexity
- Keys stored encrypted (Base64)

### Trial System
- Install date stored locally
- Tamper detection via file integrity
- No server validation required

## Performance Optimization

1. **Database**: 
   - Indexed primary keys
   - Batch inserts for sample data
   - Connection pooling

2. **Typing Engine**:
   - Single shared instance
   - Stopwatch for accurate timing
   - Lazy evaluation of metrics

3. **UI**:
   - Async/await for database operations
   - Virtual keyboard updates on demand
   - Debounced text updates

## Scalability

### Current Limitations
- Single user per machine
- Local database only
- Email-based delivery (15 min delay)

### Future Enhancements
- Cloud sync for progress
- Real-time license validation
- Instant key delivery via webhook
- Multi-language support
- Web-based admin panel

## Testing Strategy

### Unit Tests (Recommended)
```csharp
[TestClass]
public class TypingEngineTests
{
    [TestMethod]
    public void ProcessKeyPress_ValidKey_ReturnsCharacter() { }
    
    [TestMethod]
    public void CalculateWPM_AfterOneMinute_ReturnsCorrectValue() { }
}
```

### Integration Tests
- Database CRUD operations
- License key validation
- Payment email parsing

### Manual Testing
- Typing accuracy on different layouts
- Trial expiration behavior
- Key activation success/failure

## Deployment

### Desktop App
```bash
# Publish as single-file executable
dotnet publish -c Release -r win-x64 --self-contained true /p:PublishSingleFile=true

# Output: BijoyTypingMaster.exe (single file, ~100MB)
```

### Python Bot
- Deploy to GitHub Actions (free)
- Runs every 15 minutes
- No server costs

## Maintenance

### Regular Tasks
- Monitor GitHub Actions logs
- Check Gmail API quotas
- Update sample lessons
- Review user feedback

### Updates
- App updates via GitHub Releases
- No auto-update mechanism (manual download)
- Version number in About dialog

---

**Version**: 1.0  
**Last Updated**: February 2026  
**Architecture Style**: Layered + Event-Driven
