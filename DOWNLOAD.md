# 📥 Download Pre-Built App (No Installation Required!)

## ⚡ PORTABLE VERSION - Just Like an APK! 🎉

**No .NET installation needed! No admin rights required! Works from USB drive!**

This is a **self-contained package** with everything included - just download, add font, and run!

---

## 🚀 Quick Start (3 Steps)

### Step 1: Download the App (5-10 minutes)
1. Go to: **https://github.com/imriadh/Bijoy-Typing-Master---Learn-Typing-Fast/actions**
2. Click the latest **"Build Windows App"** with green ✅
3. Scroll to **"Artifacts"** section
4. Download **"BijoyTypingMaster-Portable-NoInstall.zip"** (~150-250 MB)

### Step 2: Extract & Add Font (1 minute)
1. Extract the ZIP to any folder (e.g., `C:\BijoyTypingMaster\`)
2. Download font: **https://www.omicronlab.com/download/fonts/SutonnyMJ.ttf**
3. Copy font to: `Resources\Fonts\SutonnyMJ.ttf` (inside extracted folder)

### Step 3: Run! (0 seconds)
Double-click **`BijoyTypingMaster.exe`** - that's it! 🎉

---

## ✅ What's Included

Everything you need is in the ZIP:
- ✅ BijoyTypingMaster.exe (main app)
- ✅ .NET 6 Runtime (no separate install needed!)
- ✅ All dependencies bundled
- ✅ Resources folder structure
- ✅ README.txt with instructions

**Total size**: ~150-250 MB (similar to a large mobile app)

---

## 📱 Just Like Android APK!

| Android APK | Windows Portable App |
|-------------|---------------------|
| Download .apk file | Download .zip file |
| No Google Play needed | No Microsoft Store needed |
| Just install & run | Just extract & run |
| ~50-200 MB | ~150-250 MB |
| Works offline | Works offline |

---

## 🎯 System Requirements

### Minimum:
- **OS**: Windows 10 (version 1809+) or Windows 11
- **RAM**: 2 GB
- **Storage**: 500 MB free space
- **Internet**: Only for download (runs offline after)

### ✅ NOT Required:
- ❌ No Visual Studio
- ❌ No .NET SDK installation
- ❌ No MAUI workload
- ❌ No admin rights (runs as regular user)
- ❌ No dependencies to manage

---

## 🎨 Font Setup (CRITICAL!)

**The app requires a Bangla font to display text properly!**

### Download Font:
- **SutonnyMJ.ttf**: https://www.omicronlab.com/download/fonts/SutonnyMJ.ttf
- **Alternative**: Kalpurush.ttf or Nikosh.ttf (any Bangla Unicode font)

### Install Font:
1. After extracting the app, locate the folder containing `BijoyTypingMaster.exe`
2. Look for `Resources\Fonts\` subfolder
3. Copy `SutonnyMJ.ttf` into `Resources\Fonts\`

**Without the font, Bangla text will show as boxes (□) or question marks (?)**

---

## 🚀 First Run

When you launch the app for the first time:

1. **Database Creation**: SQLite database created in `%LOCALAPPDATA%\BijoyTypingMaster\`
2. **User Profile**: Created with Level 1, 0 XP
3. **Daily Challenge**: Today's challenge generated automatically
4. **Achievements**: All 26 achievements locked and ready to unlock

### Expected Window:
```
╔══════════════════════════════════════╗
║  Level 1 | 0/100 XP | 🔥 Streak: 0   ║
╠══════════════════════════════════════╣
║   Welcome to Bijoy Typing Master!    ║
║                                      ║
║   [📝 Practice Bijoy]                ║
║   [📝 Practice Unicode]              ║
║   [⚡ Speed Test]                    ║
║   [📅 Daily Challenge]               ║
║   [🏆 Achievements]                  ║
║   [✍️ Custom Practice]              ║
╚══════════════════════════════════════╝
```

---

## 🧪 Quick Test (30 seconds)

1. ✅ Click **"Practice Bijoy"** → lesson loads
2. ✅ Type the displayed text (even random keys work for testing)
3. ✅ Click **"Finish"** → alert shows **"+25 XP Earned!"**
4. ✅ Return to home → XP bar updated to **"25/100 XP"**
5. ✅ Click **"🏆 Achievements"** → see 26 achievements (some may have unlocked!)

---

## 📊 System Requirements

### Minimum:
- **OS**: Windows 10 version 1809 (build 17763) or later
- **RAM**: 2 GB
- **Storage**: 200 MB free space
- **.NET Runtime**: Included in the build (no separate installation needed)

### Recommended:
- **OS**: Windows 10 21H2 or Windows 11
- **RAM**: 4 GB+
- **Storage**: 500 MB free space
- **Display**: 1280x720 or higher

---

## 🗂️ File Structure

```
BijoyTypingMaster-Windows/
├── BijoyTypingMaster.exe          # Main executable
├── BijoyTypingMaster.dll          # Application library
├── Resources/
│   └── Fonts/
│       └── SutonnyMJ.ttf         # ⚠️ ADD THIS MANUALLY
└── ... (runtime libraries)

User Data (Created at runtime):
%LOCALAPPDATA%\BijoyTypingMaster\
└── BijoyTypingMaster.db          # SQLite database
```

---

## 🆘 Troubleshooting

### Issue: "Cannot run on Linux/Mac"
**Solution**: This is a Windows-only app. Use GitHub Actions to build, then test on Windows.

### Issue: "Bangla text shows as □□□"
**Solution**: 
1. Download `SutonnyMJ.ttf`
2. Place in `Resources\Fonts\` folder next to the .exe
3. Restart the app

### Issue: "App doesn't start"
**Solution**:
1. Ensure you have Windows 10 (1809+) or Windows 11
2. Try running as Administrator (right-click → Run as administrator)
3. Check Windows Defender didn't block it (Settings → Virus & threat protection → Protection history)

### Issue: "Database error"
**Solution**: 
1. Close the app
2. Delete: `%LOCALAPPDATA%\BijoyTypingMaster\BijoyTypingMaster.db`
3. Restart the app (database will recreate)

### Issue: "Missing DLL errors"
**Solution**: Download the entire artifact folder, not just the .exe file. All files are required.

---

## 🎯 What's Included

### Phase 1 Features (6/6) ✅
- **Structured Lessons**: 30 progressive lessons for Bijoy & Unicode
- **Speed Tests**: 1-5 minute tests with WPM/accuracy tracking
- **Statistics Dashboard**: Charts, progress graphs, performance history
- **Settings Panel**: Customize font size, theme, keyboard visibility
- **Certificate Generator**: Generate printable certificates
- **Finger Position Guide**: Real-time finger placement hints

### Phase 2 Features (5/7) ✅
- **XP & Leveling System**: 50 levels, earn XP from all activities
- **Daily Challenges**: 4 challenge types, streak bonuses
- **Achievement System**: 26 achievements across 6 categories
- **Custom Text Practice**: Create/save/import custom typing texts
- **Full Integration**: All features award XP and unlock achievements

---

## 📞 Support

**Issues**: https://github.com/imriadh/Bijoy-Typing-Master---Learn-Typing-Fast/issues

When reporting issues, include:
- Windows version
- Error message (screenshot if possible)
- Steps to reproduce

---

## 🔄 Auto-Updates

The app automatically rebuilds on every GitHub push. To get the latest version:

1. Go to: https://github.com/imriadh/Bijoy-Typing-Master---Learn-Typing-Fast/actions
2. Find the newest successful build (green ✅)
3. Download the latest artifact
4. Replace your old files

---

**Generated**: February 10, 2026  
**Build**: Automated via GitHub Actions  
**License**: Open Source  
**Platform**: Windows 10/11 Only
