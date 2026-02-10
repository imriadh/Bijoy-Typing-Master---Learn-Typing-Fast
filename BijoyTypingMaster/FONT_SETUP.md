# 📝 Font Installation Instructions

## Adding SutonnyMJ.ttf Font to Your Project

To use the Bijoy SutonnyMJ font in your application, follow these steps:

### Step 1: Obtain the Font File
- Download or locate your `SutonnyMJ.ttf` font file
- Make sure you have the legal rights to use this font in your application

### Step 2: Add Font to Project
1. Place the `SutonnyMJ.ttf` file in the following directory:
   ```
   BijoyTypingMaster/Resources/Fonts/
   ```

2. The font is already registered in `MauiProgram.cs`:
   ```csharp
   fonts.AddFont("SutonnyMJ.ttf", "SutonnyMJ");
   ```

### Step 3: Verify Font Configuration
The `.csproj` file is already configured to include all fonts:
```xml
<MauiFont Include="Resources\Fonts\*" />
```

### Step 4: Use the Font in XAML
You can now use the font in your XAML files:

```xml
<Label Text="আমি বাংলায় গান গাই" 
       FontFamily="SutonnyMJ"
       FontSize="24"/>
```

Or programmatically in C#:
```csharp
myLabel.FontFamily = "SutonnyMJ";
```

### Alternative Fonts
If you don't have SutonnyMJ.ttf, you can use other Bangla fonts:
- Kalpurush.ttf
- Nikosh.ttf
- SolaimanLipi.ttf

Just place them in `Resources/Fonts/` and register them in `MauiProgram.cs`.

### Troubleshooting
- **Font not showing**: Clean and rebuild the project
- **Font appears as boxes**: Ensure the font file is not corrupted
- **Wrong characters**: Verify you're using the correct font encoding

## Current Status
⚠️ **Action Required**: Please add your `SutonnyMJ.ttf` file to:
```
BijoyTypingMaster/Resources/Fonts/SutonnyMJ.ttf
```

Once added, the app will automatically use it for Bijoy layout typing practice.
