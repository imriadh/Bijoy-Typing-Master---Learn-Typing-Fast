# 🔧 Windows Build Checklist

## ✅ Pre-Build Verification (Completed on Linux Dev Container)

### Code Structure
- ✅ All namespaces correct (`BijoyTypingMaster.*`)
- ✅ All file paths valid
- ✅ No syntax errors detected
- ✅ All XAML namespace declarations present
- ✅ DI registrations complete in MauiProgram.cs

### Project Files
- ✅ `.csproj` file valid (net6.0-windows10.0.19041.0)
- ✅ All NuGet packages specified:
  - System.Data.SQLite.Core 1.0.118
  - System.Management 6.0.0
  - Microsoft.Extensions.Logging.Debug 6.0.0
- ✅ Target framework: Windows 10 (19041+)

### File Inventory
**Total Files: 46**
- Models: 10 files
- Services: 15 files
- Views: 11 XAML + 11 .cs files
- Controls: 3 XAML + 3 .cs files
- Core: 4 files (App, Shell, Program, .csproj)

### Database Schema
- ✅ 10 tables defined in DatabaseManager.InitializeDatabase()
  - UserProgress (Phase 1)
  - LicenseActivations (Phase 1)
  - Settings (Phase 1)
  - SpeedTestResults (Phase 1)
  - UserProfile (Phase 2)
  - XPHistory (Phase 2)
  - DailyChallenges (Phase 2)
  - UserAchievements (Phase 2)
  - CustomTexts (Phase 2)
  - BookmarkedLessons (Phase 2)

### Dependencies Verified
- ✅ All service dependencies injected correctly
- ✅ All view constructors match DI registrations
- ✅ Navigation paths correct

---

## 🪟 Windows Build Instructions

### Step 1: Clone Repository
```bash
git clone https://github.com/imriadh/Bijoy-Typing-Master---Learn-Typing-Fast.git
cd Bijoy-Typing-Master---Learn-Typing-Fast/BijoyTypingMaster
```

### Step 2: Open in Visual Studio 2022
1. Open `BijoyTypingMaster.csproj` in Visual Studio 2022
2. Ensure ".NET MAUI" workload is installed
3. Wait for NuGet package restore

### Step 3: Restore NuGet Packages
```bash
dotnet restore
```

Expected packages:
- Microsoft.Extensions.Logging.Debug 6.0.0
- System.Data.SQLite.Core 1.0.118
- System.Management 6.0.0

### Step 4: Add Bangla Font (CRITICAL!)
**See FONT_SETUP.md for detailed instructions**

1. Download `SutonnyMJ.ttf` font file
2. Place in: `BijoyTypingMaster/Resources/Fonts/`
3. Font is already registered in MauiProgram.cs:
   ```csharp
   fonts.AddFont("SutonnyMJ.ttf", "SutonnyMJ");
   ```

**WITHOUT THIS FONT, BANGLA TEXT WILL NOT DISPLAY!**

### Step 5: Build Project
```bash
# Debug build
dotnet build -f net6.0-windows10.0.19041.0

# Release build
dotnet build -f net6.0-windows10.0.19041.0 -c Release
```

### Step 6: Run Application
**Option 1: Visual Studio**
- Press F5 or click "Start Debugging"
- Select "Windows Machine" target

**Option 2: Command Line**
```bash
dotnet run -f net6.0-windows10.0.19041.0
```

---

## 🧪 Testing Checklist

### Phase 1 Features (6/6)
- [ ] **Structured Lessons**: Load Bijoy/Unicode lessons, track progress
- [ ] **Speed Test**: 1-5 min tests, star ratings, save to database
- [ ] **Statistics Dashboard**: View WPM/accuracy charts, progress history
- [ ] **Settings Panel**: Change font size, theme, keyboard visibility, finger guide
- [ ] **Certificate Generator**: Generate certificates from speed test results
- [ ] **Finger Position Guide**: Shows correct finger for each key

### Phase 2 Features (5/7)
- [ ] **XP & Level System**: 
  - XP bar visible on MainPage
  - Complete lesson → awards 25 XP
  - Complete speed test → awards 30+ XP (performance bonus)
  - Daily login → awards 10 XP + streak bonuses
  - Level up notifications with rewards
  
- [ ] **Daily Challenges**:
  - New challenge generated each day (4 types)
  - Complete challenge → 100-200 XP
  - Challenge history tracking
  - Streak tracking (consecutive days)
  
- [ ] **Achievement System**:
  - 26 predefined achievements across 6 categories
  - Automatic unlock on completing requirements
  - Progress tracking for locked achievements
  - XP rewards (50-2000 XP based on tier)
  
- [ ] **Custom Text Practice**:
  - Create custom typing practice texts
  - Import from .txt files
  - Track best WPM/accuracy per text
  - Award 15 XP per completion
  
- [ ] **XP Integration**:
  - All typing activities award XP
  - Profile stats update (lessons/tests completed, practice time)
  - Achievements unlock automatically after sessions
  - Level-up/achievement alerts display correctly

### Database Testing
- [ ] SQLite database creates on first run (`BijoyTypingMaster.db`)
- [ ] All 10 tables created successfully
- [ ] User profile creates with default values
- [ ] Progress saves after lessons
- [ ] Speed test results save
- [ ] Settings persist across app restarts
- [ ] Custom texts save/load correctly
- [ ] Daily challenges generate at midnight
- [ ] Achievements unlock and persist

### UI Testing
- [ ] All buttons clickable and navigate correctly
- [ ] XP bar displays level/progress correctly
- [ ] Bangla text displays correctly (not boxes/?)
- [ ] Typing input captures keystrokes
- [ ] Stats update in real-time during practice
- [ ] Alerts/dialogs show properly
- [ ] CollectionViews populate with data
- [ ] ScrollViews scroll smoothly

### Error Scenarios
- [ ] App handles missing font gracefully
- [ ] Database errors logged and don't crash app
- [ ] Navigation handles back button correctly
- [ ] Empty states display helpful messages
- [ ] Invalid custom text input shows validation errors

---

## 📝 Expected First Run Behavior

1. **App Launch**: MainPage loads with XP bar showing Level 1, 0 XP
2. **Database Init**: SQLite database created in app data folder
3. **User Profile**: Default profile created with:
   - Level: 1
   - XP: 0
   - Streak: 0
   - Join Date: Current date
4. **Daily Challenge**: First challenge generated for today
5. **Achievements**: All 26 achievements created as "locked"

---

## 🚨 Known Issues & Limitations

### Font Display
- **Issue**: Bangla text may show as boxes/question marks
- **Solution**: Ensure SutonnyMJ.ttf is in Resources/Fonts/
- **Alternative**: Try Kalpurush.ttf or Nikosh.ttf

### Database Location
- **Path**: `%LOCALAPPDATA%\BijoyTypingMaster\BijoyTypingMaster.db`
- **Note**: Database persists between runs
- **Reset**: Delete database file to start fresh

### Performance
- **First Run**: May take 2-3 seconds to initialize database
- **Subsequent Runs**: Should load in <1 second

---

## 📊 Build Artifacts

### Debug Build Output
```
BijoyTypingMaster/bin/Debug/net6.0-windows10.0.19041.0/win10-x64/
├── BijoyTypingMaster.exe
├── BijoyTypingMaster.dll
├── BijoyTypingMaster.db (created on first run)
└── Resources/
    └── Fonts/
        └── SutonnyMJ.ttf
```

### Release Build Output
```
BijoyTypingMaster/bin/Release/net6.0-windows10.0.19041.0/win10-x64/
├── BijoyTypingMaster.exe (optimized)
├── BijoyTypingMaster.dll
└── ... (same structure)
```

---

## 🎯 Success Criteria

Build is successful if:
- ✅ Application launches without errors
- ✅ MainPage displays with XP bar
- ✅ All navigation buttons work
- ✅ Bangla text displays correctly
- ✅ Lessons load and typing works
- ✅ Speed test runs and saves results
- ✅ XP awards after completing activities
- ✅ Achievements unlock correctly
- ✅ Daily challenge generates
- ✅ Database persists data
- ✅ Settings save and load

---

## 🆘 Troubleshooting

### Build Errors

**Error: "Target framework not found"**
```bash
# Install .NET 6 SDK
winget install Microsoft.DotNet.SDK.6
```

**Error: "MAUI workload not installed"**
```bash
dotnet workload install maui
```

**Error: "NuGet package restore failed"**
```bash
# Clear NuGet cache
dotnet nuget locals all --clear
dotnet restore
```

### Runtime Errors

**Error: "Database file not found"**
- Database creates automatically on first run
- Check app has write permissions to LocalAppData

**Error: "Font not rendering"**
- Verify SutonnyMJ.ttf exists in Resources/Fonts/
- Check font is registered in MauiProgram.cs
- Try clean rebuild

**Error: "Service not registered"**
- Check MauiProgram.cs for missing DI registrations
- All 10 services should be registered as Singleton
- All 10 views should be registered as Transient

---

## 📞 Support

**Repository**: https://github.com/imriadh/Bijoy-Typing-Master---Learn-Typing-Fast
**Issues**: Open GitHub issue with:
- Build logs
- Error messages
- Windows version
- Visual Studio version

---

**Generated**: February 10, 2026
**Phase**: 1 (100%) + Phase 2 (71%)
**Status**: ✅ Ready for Windows Build
