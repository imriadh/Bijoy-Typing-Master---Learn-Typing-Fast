# 🤖 License Automation Bot Setup Guide

## Overview
This Python script automates the license key delivery process by:
1. Monitoring Gmail for bKash payment notifications
2. Matching transactions with Google Sheets orders
3. Generating license keys
4. Sending automated emails with license keys

## Prerequisites

### 1. Google Cloud Project Setup
1. Go to [Google Cloud Console](https://console.cloud.google.com/)
2. Create a new project or select existing one
3. Enable the following APIs:
   - Gmail API
   - Google Sheets API

### 2. Create OAuth 2.0 Credentials

#### Step-by-Step:
1. In Google Cloud Console, go to **APIs & Services > Credentials**
2. Click **+ CREATE CREDENTIALS** → **OAuth client ID**
3. Select **Application type**: Desktop app
4. Name it: "Bijoy License Bot"
5. Click **Create**
6. Download the `credentials.json` file
7. Place it in the `/automation/` folder

### 3. Google Sheets Setup

#### Create Your Order Sheet:
1. Create a new Google Sheet
2. Name it "Bijoy License Orders"
3. Set up columns (in Sheet1):
   ```
   A: Timestamp
   B: Email
   C: TrxID
   D: MachineID
   E: Status
   F: LicenseKey
   ```

4. Create a Google Form that submits to this sheet with fields:
   - Email
   - Transaction ID (TrxID)
   - Machine ID
   - Payment Amount

5. Get the Sheet ID from the URL:
   ```
   https://docs.google.com/spreadsheets/d/[SHEET_ID]/edit
   ```

### 4. Gmail Configuration

The bot will monitor your Gmail for emails with:
- Subject containing: **"Bkash: Received"**
- Body containing:
  - TrxID: XXXXXXXXXX
  - Amount: 200

Make sure your bKash account sends email notifications to this Gmail.

## Installation

### Local Testing

```bash
cd automation

# Install dependencies
pip install -r requirements.txt

# Run the script (first time will open browser for OAuth)
python automation_script.py
```

### First Run Authentication
1. Script will open browser
2. Sign in to your Gmail account
3. Grant permissions (Gmail & Sheets access)
4. Token will be saved as `token.pickle`

## Configuration

### Environment Variables
Set these in your environment or `.env` file:

```bash
export SHEET_ID="your_google_sheet_id_here"
```

## How It Works

### Workflow:
```
1. Check Gmail for unread "Bkash: Received" emails
   ↓
2. Extract TrxID and Amount from email
   ↓
3. Verify Amount = 200 BDT
   ↓
4. Search Google Sheet for matching TrxID
   ↓
5. Check if Status != "Delivered"
   ↓
6. Generate License Key from MachineID
   ↓
7. Send email with License Key
   ↓
8. Update Sheet: Status = "Delivered"
   ↓
9. Mark Gmail as read
```

### License Key Algorithm:
```python
# Reverse the Machine ID
machine_id = "A1B2-C3D4-E5F6-G7H8"
reversed = "8H7G-6F5E-4D3C-2B1A"

# Add month-based hash
month = current_month  # e.g., 02 for February
hash = MD5(reversed + month)[:8]

# Final key
license_key = reversed + hash
# Format: XXXX-XXXX-XXXX-XXXX
```

## Testing

### Manual Test:
1. Send a test email to yourself with subject "Bkash: Received"
2. Include in body:
   ```
   TrxID: TEST123456
   Amount: 200
   ```
3. Add an entry to Google Sheet with TrxID: TEST123456
4. Run the script

### Debug Mode:
```python
# In automation_script.py, add print statements
print(f"Debug: {payment_info}")
```

## Troubleshooting

### Common Issues:

**"credentials.json not found"**
- Download from Google Cloud Console
- Place in `/automation/` folder

**"Insufficient permissions"**
- Delete `token.pickle`
- Re-run script to re-authenticate

**"Sheet not found"**
- Verify SHEET_ID environment variable
- Make sure your Google account has access to the sheet

**"No emails found"**
- Check Gmail search query
- Verify email subject matches "Bkash: Received"

## Security Notes

⚠️ **Important**:
- Never commit `credentials.json` or `token.pickle` to GitHub
- Add them to `.gitignore`
- Use GitHub Secrets for CI/CD
- Keep SHEET_ID private

## Next Steps

After local testing works:
1. Set up GitHub Actions (see `../.github/workflows/license_bot.yml`)
2. Add secrets to GitHub repository
3. Bot will run automatically every 15 minutes

## Support

For issues or questions:
- Check logs in GitHub Actions
- Verify Gmail API quotas
- Ensure bKash emails are forwarded correctly
