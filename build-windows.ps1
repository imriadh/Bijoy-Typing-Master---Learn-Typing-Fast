# Bijoy Typing Master - Windows Build Script (Portable Edition)
# Run this on Windows with: .\build-windows.ps1

Write-Host "╔══════════════════════════════════════════════════════════════╗" -ForegroundColor Cyan
Write-Host "║  Bijoy Typing Master - Build Script                         ║" -ForegroundColor Cyan
Write-Host "║  Creating Portable Edition (No installation needed)         ║" -ForegroundColor Cyan
Write-Host "╚══════════════════════════════════════════════════════════════╝" -ForegroundColor Cyan
Write-Host ""

# Check if .NET SDK is installed
Write-Host "[1/6] Checking .NET SDK..." -ForegroundColor Yellow
$dotnetVersion = & dotnet --version 2>$null
if ($LASTEXITCODE -ne 0) {
    Write-Host "❌ .NET SDK not found!" -ForegroundColor Red
    Write-Host ""
    Write-Host "Please install .NET 6 or .NET 8 SDK from:" -ForegroundColor Yellow
    Write-Host "https://dotnet.microsoft.com/download" -ForegroundColor Cyan
    Write-Host ""
    Write-Host "Or use winget: winget install Microsoft.DotNet.SDK.8" -ForegroundColor Cyan
    exit 1
}
Write-Host "✅ .NET SDK $dotnetVersion found" -ForegroundColor Green

# Check MAUI workload
Write-Host ""
Write-Host "[2/6] Checking MAUI workload..." -ForegroundColor Yellow
$mauiInstalled = & dotnet workload list | Select-String "maui"
if (-not $mauiInstalled) {
    Write-Host "⚠️  MAUI workload not installed. Installing now..." -ForegroundColor Yellow
    dotnet workload install maui
    if ($LASTEXITCODE -ne 0) {
        Write-Host "❌ Failed to install MAUI workload" -ForegroundColor Red
        exit 1
    }
    Write-Host "✅ MAUI workload installed" -ForegroundColor Green
} else {
    Write-Host "✅ MAUI workload already installed" -ForegroundColor Green
}

# Navigate to project directory
Write-Host ""
Write-Host "[3/6] Preparing build..." -ForegroundColor Yellow
Set-Location BijoyTypingMaster

# Restore packages
Write-Host ""
Write-Host "[4/6] Restoring NuGet packages..." -ForegroundColor Yellow
dotnet restore BijoyTypingMaster.csproj
if ($LASTEXITCODE -ne 0) {
    Write-Host "❌ Package restore failed" -ForegroundColor Red
    exit 1
}
Write-Host "✅ Packages restored" -ForegroundColor Green

# Publish self-contained
Write-Host ""
Write-Host "[5/6] Building portable app (this may take 5-10 minutes)..." -ForegroundColor Yellow
Write-Host "    Creating self-contained package with .NET runtime..." -ForegroundColor Gray

dotnet publish BijoyTypingMaster.csproj `
    -f net6.0-windows10.0.19041.0 `
    -c Release `
    -r win10-x64 `
    --self-contained true `
    -p:PublishSingleFile=false `
    -o ../BuildOutput

if ($LASTEXITCODE -ne 0) {
    Write-Host "❌ Build failed!" -ForegroundColor Red
    Write-Host ""
    Write-Host "Common issues:" -ForegroundColor Yellow
    Write-Host "1. Make sure you're running on Windows 10/11" -ForegroundColor Gray
    Write-Host "2. Ensure .NET 6+ SDK is installed" -ForegroundColor Gray
    Write-Host "3. Try running as Administrator" -ForegroundColor Gray
    exit 1
}

Set-Location ..
Write-Host "✅ Build completed successfully!" -ForegroundColor Green

# Create README
Write-Host ""
Write-Host "[6/6] Creating package..." -ForegroundColor Yellow

$readme = @"
╔══════════════════════════════════════════════════════════════╗
║  Bijoy Typing Master - Portable Edition                     ║
║  No Installation Required!                                   ║
╚══════════════════════════════════════════════════════════════╝

📥 QUICK START (3 Steps):

1. DOWNLOAD FONT (Required for Bangla text):
   https://www.omicronlab.com/download/fonts/SutonnyMJ.ttf
   OR: https://github.com/omicronlab/fonts/raw/main/SutonnyMJ.ttf

2. COPY FONT to this folder:
   Resources\Fonts\SutonnyMJ.ttf

3. RUN THE APP:
   Double-click: BijoyTypingMaster.exe

✅ No .NET installation needed - everything is included!
✅ No admin rights required  
✅ Fully portable - works from USB drive

📊 SYSTEM REQUIREMENTS:
- Windows 10 (1809+) or Windows 11
- 200 MB free space
- No other software needed!

🎮 FEATURES:
✅ Phase 1: 6 core typing features (100%)
   • Structured lessons (Bijoy & Unicode)
   • Speed tests with WPM tracking
   • Statistics dashboard
   • Settings panel
   • Certificate generator
   • Finger position guide

✅ Phase 2: 5 gamification features (71%)
   • XP & Leveling System (50 levels)
   • Daily Challenges (4 types)
   • Achievement System (26 achievements)
   • Custom text practice
   • Full XP integration

⚙️ DATABASE:
All your progress saves to:
%LOCALAPPDATA%\BijoyTypingMaster\BijoyTypingMaster.db

To reset progress, delete that database file.

🆘 TROUBLESHOOTING:
• Bangla shows as □□□ → Missing SutonnyMJ.ttf font
• Won't start → Run as Administrator once
• Blocked by Windows → Right-click .exe → Properties → Unblock
• Error on first run → Wait 2-3 seconds for database initialization

📧 SUPPORT: https://github.com/imriadh/Bijoy-Typing-Master---Learn-Typing-Fast

Generated: $(Get-Date -Format 'yyyy-MM-dd HH:mm:ss')
Build: Self-Contained Portable Edition (Built on your machine)
"@

$readme | Out-File -FilePath BuildOutput/README.txt -Encoding UTF8
Write-Host "✅ README created" -ForegroundColor Green

# Create ZIP
Write-Host ""
Write-Host "Creating ZIP package..." -ForegroundColor Yellow
$zipPath = "BijoyTypingMaster-Portable.zip"
if (Test-Path $zipPath) {
    Remove-Item $zipPath
}
Compress-Archive -Path BuildOutput/* -DestinationPath $zipPath
Write-Host "✅ ZIP package created: $zipPath" -ForegroundColor Green

# Show completion message
Write-Host ""
Write-Host "╔══════════════════════════════════════════════════════════════╗" -ForegroundColor Green
Write-Host "║  🎉 BUILD SUCCESSFUL!                                        ║" -ForegroundColor Green
Write-Host "╚══════════════════════════════════════════════════════════════╝" -ForegroundColor Green
Write-Host ""
Write-Host "📦 Portable Package: BijoyTypingMaster-Portable.zip" -ForegroundColor Cyan
Write-Host "📁 Build Output: BuildOutput/" -ForegroundColor Cyan
Write-Host ""
Write-Host "Next Steps:" -ForegroundColor Yellow
Write-Host "1. Extract the ZIP to any folder" -ForegroundColor White
Write-Host "2. Download SutonnyMJ.ttf font (see README.txt)" -ForegroundColor White
Write-Host "3. Run BijoyTypingMaster.exe" -ForegroundColor White
Write-Host ""
Write-Host "⚠️  IMPORTANT: Do not forget to add the Bangla font!" -ForegroundColor Yellow
Write-Host ""

# Ask if user wants to open the folder
$response = Read-Host "Open build folder now? (y/n)"
if ($response -eq "y" -or $response -eq "Y") {
    Start-Process BuildOutput
}
