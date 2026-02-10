# 🎯 Bijoy Typing Master - Learn Typing Fast

A professional cross-platform desktop application for learning Bengali typing using Bijoy and Unicode layouts. Built with .NET MAUI (C# .NET 6) and featuring automated license delivery through Python.

[![.NET MAUI](https://img.shields.io/badge/.NET_MAUI-6.0-512BD4?logo=.net)](https://dotnet.microsoft.com/apps/maui)
[![Python](https://img.shields.io/badge/Python-3.9+-3776AB?logo=python&logoColor=white)](https://python.org)
[![License](https://img.shields.io/badge/License-Commercial-orange)](LICENSE)

## ✨ Features

### 🎓 Learning Features
- **Dual Layout Support**: Practice both Bijoy and Unicode Bengali typing
- **Complex Juktakkhor (Conjuncts)**: Handles advanced Bijoy rules and character combinations
- **Real-time Metrics**: Live WPM (Words Per Minute) and accuracy tracking
- **Progress Tracking**: Save and review your typing history
- **Virtual Keyboard**: Visual on-screen keyboard with key highlighting
- **Graded Lessons**: Beginner to Advanced difficulty levels

### 🔐 Licensing & Monetization
- **7-Day Free Trial**: Full feature access for new users
- **Hardware-Based Licensing**: Unique Machine ID generation using CPU & Motherboard
- **Automated License Delivery**: Python bot processes payments and sends keys via email
- **Secure Validation**: Mathematical key generation algorithm
- **200 BDT Pricing**: Affordable one-time payment

### 🤖 Automation
- **Gmail Integration**: Monitors bKash payment notifications
- **Google Sheets**: Tracks orders and delivery status
- **Auto-Email**: Sends license keys within minutes of payment
- **GitHub Actions**: Runs every 15 minutes (free cloud hosting)

## 📸 Screenshots

*(Add your screenshots here once the app is running on Windows)*

## 🏗️ Project Structure

```
Bijoy-Typing-Master---Learn-Typing-Fast/
├── BijoyTypingMaster/              # Main .NET MAUI Application
│   ├── Models/                     # Data models (Lesson, UserProgress)
│   ├── Services/                   # Business logic
│   │   ├── DatabaseManager.cs      # SQLite database handler
│   │   ├── TypingEngine.cs         # Core typing logic & metrics
│   │   ├── BijoyLayout.cs          # Bijoy keyboard implementation
│   │   ├── UnicodeLayout.cs        # Unicode keyboard implementation
│   │   ├── HardwareId.cs           # Machine ID generation
│   │   └── LicenseManager.cs       # License validation & trial
│   ├── Views/                      # XAML UI pages
│   │   ├── MainPage.xaml           # Home screen
│   │   ├── PracticeWindow.xaml     # Typing practice interface
│   │   └── PaymentWindow.xaml      # License purchase & activation
│   ├── Controls/                   # Custom UI controls
│   │   └── VirtualKeyboard.xaml    # On-screen keyboard
│   └── Resources/                  # Fonts, images, styles
│       └── Fonts/                  # Place SutonnyMJ.ttf here
│
├── automation/                     # Python License Bot
│   ├── automation_script.py        # Main bot script
│   ├── requirements.txt            # Python dependencies
│   └── README.md                   # Bot setup guide
│
└── .github/
    └── workflows/
        └── license_bot.yml         # GitHub Actions configuration
```

## 🚀 Getting Started

### Prerequisites

#### For Development:
- **Windows 10/11** (MAUI desktop support)
- **.NET 6 SDK** or later
- **Visual Studio 2022** with MAUI workload
- **Python 3.9+** (for automation bot)

#### For Users:
- **Windows 10/11** (x64)
- No additional runtime needed (self-contained deployment)

### Installation

#### Option 1: For Developers

```bash
# Clone the repository
git clone https://github.com/imriadh/Bijoy-Typing-Master---Learn-Typing-Fast.git
cd Bijoy-Typing-Master---Learn-Typing-Fast

# Open in Visual Studio 2022
# File → Open → Project/Solution
# Select: BijoyTypingMaster/BijoyTypingMaster.csproj

# Add the Bijoy font (required)
# Download SutonnyMJ.ttf and place it in:
# BijoyTypingMaster/Resources/Fonts/SutonnyMJ.ttf

# Build and Run
# Press F5 in Visual Studio
```

See [FONT_SETUP.md](BijoyTypingMaster/FONT_SETUP.md) for font installation details.

#### Option 2: For End Users

1. Download the latest release from [Releases](https://github.com/imriadh/Bijoy-Typing-Master---Learn-Typing-Fast/releases)
2. Extract and run `BijoyTypingMaster.exe`
3. Start your 7-day free trial!

## 🎮 How to Use

### Starting a Practice Session

1. **Launch the app** and select your layout:
   - **Bijoy**: For traditional SutonnyMJ font typing
   - **Unicode**: For modern Bengali Unicode

2. **Practice Window**:
   - Target text displays at the top
   - Type using your keyboard
   - Watch real-time WPM and accuracy
   - Green = correct, Red = wrong, Yellow = current

3. **Complete Lesson**:
   - Click "Finish & Save" when done
   - View your stats
   - Progress is saved to database

### Purchasing a License

1. Go to **Settings & License**
2. Copy your **Machine ID**
3. Click **Buy Now (200 BDT)**
4. Fill the Google Form with your Machine ID
5. Send 200 BDT via bKash
6. Receive license key via email (within 24 hours)
7. Enter key in the app and click **Activate**

## 🛠️ Technology Stack

### Frontend (Desktop App)
- **.NET MAUI 6.0**: Cross-platform UI framework
- **C#**: Application logic
- **XAML**: UI markup
- **SQLite**: Local database for lessons and progress

### Backend (Automation)
- **Python 3.9**: Automation scripting
- **Gmail API**: Email monitoring and sending
- **Google Sheets API**: Order management
- **GitHub Actions**: Free cloud hosting (runs every 15 minutes)

## 🔐 Licensing System

### How It Works

```
User installs app
    ↓
Machine ID generated (CPU + Motherboard hash)
    ↓
7-day trial begins (saved locally)
    ↓
After 7 days → Payment required
    ↓
User purchases (bKash 200 BDT)
    ↓
Python bot detects payment email
    ↓
Generates license key: Reverse(MachineID) + Hash
    ↓
Emails key to user
    ↓
User activates in app → Lifetime access
```

### Setting Up Automation

See detailed guides:
- [Automation Setup](automation/README.md)
- [GitHub Secrets](/.github/SECRETS_SETUP.md)
- [Google API Setup](automation/README.md#google-cloud-project-setup)

## 📊 Database Schema

### Lessons Table
| Column | Type | Description |
|--------|------|-------------|
| Id | INTEGER | Primary key |
| Title | TEXT | Lesson name |
| Difficulty | TEXT | Beginner/Intermediate/Advanced |
| TextContent | TEXT | Practice text content |
| Type | TEXT | 'Bijoy' or 'Unicode' |

### UserProgress Table
| Column | Type | Description |
|--------|------|-------------|
| Id | INTEGER | Primary key |
| Date | TEXT | Session date/time |
| WPM | REAL | Words per minute |
| Accuracy | REAL | Percentage (0-100) |
| LessonId | INTEGER | Foreign key to Lessons |

## 🤝 Contributing

We welcome contributions! Here's how:

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

### Development Guidelines
- Follow C# coding conventions
- Add XML documentation to public methods
- Test on Windows before submitting PR
- Update README if adding new features

## 🐛 Troubleshooting

### Common Issues

**Q: App won't start on Windows**
- Ensure Windows 10/11 x64
- Install [.NET 6 Runtime](https://dotnet.microsoft.com/download/dotnet/6.0)

**Q: Font shows as boxes**
- Add `SutonnyMJ.ttf` to `Resources/Fonts/`
- Rebuild the project

**Q: Trial shows 0 days but I just installed**
- Delete `license.dat` from `%LOCALAPPDATA%/BijoyTypingMaster/`
- Restart the app

**Q: License key not working**
- Ensure no extra spaces in key
- Verify Machine ID matches
- Key is case-insensitive

**Q: Automation bot not running**
- Check GitHub Actions logs
- Verify secrets are set correctly
- Ensure Gmail API is enabled

## 📝 License & Usage

This is **commercial software**. 

- ✅ Free 7-day trial for all users
- ✅ Source code available for learning
- ❌ Redistribution requires permission
- ❌ Commercial use of code requires license

## 🙏 Acknowledgments

- **Bijoy**: Original Bengali keyboard layout
- **SutonnyMJ Font**: Traditional Bijoy font
- **Microsoft**: .NET MAUI framework
- **Google**: Gmail and Sheets APIs

## 📧 Contact & Support

- **Developer**: imriadh
- **Email**: *(Add your contact email)*
- **Issues**: [GitHub Issues](https://github.com/imriadh/Bijoy-Typing-Master---Learn-Typing-Fast/issues)
- **Discussions**: [GitHub Discussions](https://github.com/imriadh/Bijoy-Typing-Master---Learn-Typing-Fast/discussions)

## 🚀 Roadmap

- [ ] Add more lessons and difficulty levels
- [ ] Implement statistics dashboard
- [ ] Add sound effects and animations
- [ ] Support for Linux (via MAUI)
- [ ] Mobile version (Android/iOS)
- [ ] Multiplayer typing races
- [ ] Custom lesson creator

## ⭐ Star History

If you find this project helpful, please give it a star! ⭐

---

**Made with ❤️ in Bangladesh 🇧🇩**

*Learn Bangla typing the right way - fast, efficient, and professional!*