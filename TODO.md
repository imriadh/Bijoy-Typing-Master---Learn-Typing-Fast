# ✅ TODO Checklist - Action Items

## 🎯 Before You Can Use the App

### 1️⃣ Add Font File (REQUIRED)
- [ ] Obtain `SutonnyMJ.ttf` font file
- [ ] Place it in: `BijoyTypingMaster/Resources/Fonts/SutonnyMJ.ttf`
- [ ] Alternative: Use any Bangla Unicode font (Kalpurush, Nikosh, etc.)

### 2️⃣ Update Payment URL (REQUIRED)
- [ ] Create a Google Form for payment collection
- [ ] Form fields should include:
  - Email address
  - Transaction ID (TrxID)
  - Machine ID
  - Payment confirmation
- [ ] Copy form URL (e.g., `https://forms.gle/xxxxx`)
- [ ] Update in file: `BijoyTypingMaster/Views/PaymentWindow.xaml.cs`
  ```csharp
  Line 8: private const string PAYMENT_FORM_URL = "YOUR_URL_HERE";
  ```

### 3️⃣ Set Up Google Cloud (for automation)
- [ ] Go to [Google Cloud Console](https://console.cloud.google.com/)
- [ ] Create new project
- [ ] Enable Gmail API
- [ ] Enable Google Sheets API
- [ ] Create OAuth 2.0 credentials (Desktop app)
- [ ] Download `credentials.json`
- [ ] Place in: `automation/credentials.json`
- [ ] **Important**: Add `credentials.json` to `.gitignore` (already done!)

### 4️⃣ Create Google Sheet for Orders
- [ ] Create new Google Sheet named "Bijoy License Orders"
- [ ] Add columns (in Sheet1):
  - A: Timestamp
  - B: Email
  - C: TrxID
  - D: MachineID
  - E: Status
  - F: LicenseKey
- [ ] Link your Google Form to this sheet
- [ ] Copy Sheet ID from URL
- [ ] Save for GitHub Secrets

### 5️⃣ Test Python Bot Locally (OPTIONAL)
- [ ] cd into `automation/` folder
- [ ] Install Python dependencies: `pip install -r requirements.txt`
- [ ] Set environment variable: `export SHEET_ID=your_sheet_id`
- [ ] Run script: `python automation_script.py`
- [ ] First run will open browser for Google OAuth
- [ ] Authorize access to Gmail and Sheets
- [ ] Test with a sample payment email

### 6️⃣ Configure GitHub Secrets
- [ ] Go to your repository on GitHub
- [ ] Navigate to: **Settings → Secrets and variables → Actions**
- [ ] Add secret: `GMAIL_CREDENTIALS`
  - Value: Paste entire content of `credentials.json`
- [ ] Add secret: `SHEET_ID`
  - Value: Your Google Sheets ID (from URL)

### 7️⃣ Test GitHub Actions Workflow
- [ ] Go to **Actions** tab on GitHub
- [ ] Select **License Bot Automation**
- [ ] Click **Run workflow** manually
- [ ] Check logs for any errors
- [ ] Verify bot connects to Gmail and Sheets

### 8️⃣ Test on Windows (REQUIRED)
- [ ] Clone repository to Windows machine
- [ ] Install **Visual Studio 2022** with **.NET MAUI workload**
- [ ] Open `BijoyTypingMaster.csproj`
- [ ] Restore NuGet packages
- [ ] Build solution (Ctrl+Shift+B)
- [ ] Run application (F5)

### 9️⃣ Manual Testing Checklist
- [ ] App launches without errors
- [ ] Main menu displays correctly
- [ ] "Practice Bijoy" opens practice window
- [ ] "Practice Unicode" opens practice window
- [ ] Can type and see real-time WPM/Accuracy
- [ ] Lesson text displays in correct font
- [ ] Progress is saved to database
- [ ] Trial countdown shows correct days
- [ ] Machine ID displays in Payment window
- [ ] "Copy Machine ID" button works
- [ ] "Buy Now" opens your Google Form
- [ ] Can enter and activate a license key

### 🔟 Generate Test License Key
To test license activation without waiting for payment:

- [ ] Get your Machine ID from the app
- [ ] Use this Python code to generate a test key:
  ```python
  import hashlib
  from datetime import datetime
  
  machine_id = "XXXX-XXXX-XXXX-XXXX"  # Replace with your Machine ID
  clean_id = machine_id.replace("-", "")
  reversed_id = clean_id[::-1]
  month = datetime.now().month
  combined = reversed_id + str(month).zfill(2)
  hash_obj = hashlib.md5(combined.encode())
  hash_suffix = hash_obj.hexdigest()[:8].upper()
  license_key = (reversed_id + hash_suffix).upper()
  formatted = '-'.join([license_key[i:i+4] for i in range(0, 16, 4)])
  print(f"Test License Key: {formatted}")
  ```
- [ ] Enter this key in the app to test activation

---

## 🚀 Optional Enhancements

### UI Improvements
- [ ] Add app icon (Resources/AppIcon/appicon.svg)
- [ ] Create splash screen
- [ ] Add more sample lessons
- [ ] Improve color scheme
- [ ] Add animations

### Features
- [ ] Statistics dashboard (graphs)
- [ ] Lesson progress tracker
- [ ] Achievement system
- [ ] Settings page (theme, font size)
- [ ] Export progress to CSV

### Localization
- [ ] Add English/Bangla language toggle
- [ ] Translate all UI text
- [ ] Multi-language documentation

### Marketing
- [ ] Create demo video
- [ ] Write blog post
- [ ] Social media graphics
- [ ] User testimonials

---

## 📝 Pre-Launch Checklist

### Code Quality
- [ ] All TODOs resolved
- [ ] No placeholder code
- [ ] Debug logging removed
- [ ] Error handling added
- [ ] Code commented

### Testing
- [ ] All features tested on Windows
- [ ] Payment flow tested end-to-end
- [ ] License activation tested
- [ ] Trial expiration tested
- [ ] Edge cases covered

### Documentation
- [ ] README updated with screenshots
- [ ] Contact email added
- [ ] Support instructions clear
- [ ] Installation guide complete

### Deployment
- [ ] Build release configuration
- [ ] Create installer (MSI/MSIX)
- [ ] Code signing certificate (optional)
- [ ] Version number set
- [ ] GitHub release created

### Legal
- [ ] Privacy policy (if collecting data)
- [ ] Terms of service
- [ ] License agreement
- [ ] Font usage rights confirmed

---

## 🎉 Launch Day Checklist

- [ ] GitHub Release published
- [ ] Download link tested
- [ ] Installer works on clean Windows
- [ ] Payment form is live
- [ ] Google Sheet monitoring works
- [ ] Bot is running on GitHub Actions
- [ ] Support email ready
- [ ] Social media announcement ready
- [ ] Backup of codebase taken

---

## 📊 Post-Launch Monitoring

### Daily (First Week)
- [ ] Check GitHub Actions logs
- [ ] Monitor payment emails
- [ ] Respond to support requests
- [ ] Fix critical bugs

### Weekly
- [ ] Review user feedback
- [ ] Update documentation
- [ ] Add more lessons
- [ ] Plan feature updates

### Monthly
- [ ] Review analytics
- [ ] Plan new features
- [ ] Update dependencies
- [ ] Security audit

---

## 🆘 Troubleshooting Reference

### App Won't Build
1. Verify .NET 6 SDK installed
2. Restore NuGet packages
3. Clean and rebuild
4. Check for .csproj errors

### Font Not Showing
1. Confirm font file in correct location
2. Check font name matches in XAML
3. Rebuild project
4. Clear cache

### License Not Validating
1. Check Machine ID matches
2. Verify key format (XXXX-XXXX-XXXX-XXXX)
3. Test key generation algorithm
4. Check for typos

### Bot Not Running
1. Check GitHub Actions logs
2. Verify secrets are set
3. Test credentials locally
4. Check API quotas

---

## 📞 Support Contacts

- **GitHub Issues**: [Report a bug](https://github.com/imriadh/Bijoy-Typing-Master---Learn-Typing-Fast/issues)
- **Discussions**: [Ask questions](https://github.com/imriadh/Bijoy-Typing-Master---Learn-Typing-Fast/discussions)
- **Email**: *(Add your email)*

---

**Last Updated**: February 10, 2026  
**Status**: Ready for testing on Windows  
**Priority**: Complete items 1-8 first!

---

*Good luck with your Bijoy Typing Master launch! 🚀*
