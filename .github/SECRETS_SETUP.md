# 🔐 GitHub Secrets Setup Guide

## Required Secrets

To enable the automated license bot, you need to add the following secrets to your GitHub repository:

### 1. GMAIL_CREDENTIALS
**What**: OAuth 2.0 credentials for Gmail API access
**How to get**:
1. Go to [Google Cloud Console](https://console.cloud.google.com/)
2. Navigate to **APIs & Services → Credentials**
3. Create OAuth 2.0 Client ID (Desktop app type)
4. Download the JSON file
5. Copy the entire JSON content

**How to add**:
```bash
# GitHub Repository → Settings → Secrets and variables → Actions
# Click "New repository secret"
# Name: GMAIL_CREDENTIALS
# Value: Paste the entire JSON content from credentials.json
```

Example format (don't use these actual values):
```json
{
  "installed": {
    "client_id": "xxxxx.apps.googleusercontent.com",
    "project_id": "your-project-id",
    "auth_uri": "https://accounts.google.com/o/oauth2/auth",
    "token_uri": "https://oauth2.googleapis.com/token",
    "auth_provider_x509_cert_url": "https://www.googleapis.com/oauth2/v1/certs",
    "client_secret": "your-client-secret",
    "redirect_uris": ["http://localhost"]
  }
}
```

### 2. SHEET_ID
**What**: Google Sheets ID where orders are stored
**How to get**:
1. Open your Google Sheet
2. Copy the ID from the URL:
   ```
   https://docs.google.com/spreadsheets/d/[THIS_IS_YOUR_SHEET_ID]/edit
   ```

**How to add**:
```bash
# GitHub Repository → Settings → Secrets and variables → Actions
# Click "New repository secret"
# Name: SHEET_ID
# Value: Paste your Sheet ID (just the ID, not the full URL)
```

## Initial Setup Checklist

- [ ] Create Google Cloud Project
- [ ] Enable Gmail API
- [ ] Enable Google Sheets API
- [ ] Create OAuth 2.0 credentials
- [ ] Download credentials.json
- [ ] Run script locally once to authenticate
- [ ] Add GMAIL_CREDENTIALS to GitHub Secrets
- [ ] Add SHEET_ID to GitHub Secrets
- [ ] Test workflow manually

## Testing GitHub Actions

### Manual Run:
1. Go to your repository on GitHub
2. Click **Actions** tab
3. Select **License Bot Automation** workflow
4. Click **Run workflow** button
5. Check the logs for any errors

### Monitor Scheduled Runs:
- The bot runs every 15 minutes automatically
- Check **Actions** tab to see history
- Click on any run to view logs

## Troubleshooting

### "Authentication failed"
- Verify GMAIL_CREDENTIALS secret is correct JSON
- Run locally first to ensure credentials work
- Token might need refresh (delete cache and re-run)

### "Sheet not found"
- Verify SHEET_ID is correct
- Ensure the Google account has access to the sheet
- Sheet must be shared with the service account

### "Insufficient permissions"
- Enable Gmail API in Google Cloud Console
- Enable Google Sheets API
- Ensure OAuth scopes include:
  - gmail.readonly
  - gmail.modify
  - gmail.send
  - spreadsheets

### "Rate limit exceeded"
- Gmail API has daily quotas
- Reduce cron frequency if hitting limits
- Consider using service account instead

## Security Best Practices

✅ **DO**:
- Keep secrets in GitHub Secrets only
- Regularly rotate credentials
- Use least privilege principle
- Monitor Actions logs for suspicious activity

❌ **DON'T**:
- Commit credentials.json to repository
- Share credentials in plain text
- Use personal Gmail for production
- Ignore failed workflow notifications

## Advanced: Service Account (Optional)

For better security, consider using a Service Account instead of OAuth:

1. Create Service Account in Google Cloud Console
2. Download JSON key file
3. Share Google Sheet with service account email
4. Use service account credentials in GitHub Secret

Benefits:
- No manual authentication needed
- No token expiration issues
- Better for automation

## Support

If you encounter issues:
1. Check workflow logs in GitHub Actions
2. Verify all secrets are correctly set
3. Test credentials locally first
4. Review Google Cloud Console quotas
5. Check Gmail and Sheets API status

## Next Steps

After setup is complete:
1. ✅ Test with a real bKash payment email
2. ✅ Verify license email is sent
3. ✅ Check Google Sheet is updated
4. ✅ Monitor for a week to ensure stability
