#!/usr/bin/env python3
"""
Bijoy Typing Master - License Automation Bot
Automatically processes payments and sends license keys via email
"""

import os
import base64
import re
from datetime import datetime
from email.mime.text import MIMEText
from email.mime.multipart import MIMEMultipart

from google.oauth2.credentials import Credentials
from google_auth_oauthlib.flow import InstalledAppFlow
from google.auth.transport.requests import Request
from googleapiclient.discovery import build
from googleapiclient.errors import HttpError
import pickle

# Gmail API Scopes
SCOPES = [
    'https://www.googleapis.com/auth/gmail.readonly',
    'https://www.googleapis.com/auth/gmail.modify',
    'https://www.googleapis.com/auth/gmail.send',
    'https://www.googleapis.com/auth/spreadsheets'
]

# Configuration (Set via environment variables or GitHub Secrets)
SPREADSHEET_ID = os.getenv('SHEET_ID', '')  # Google Sheets ID
EXPECTED_AMOUNT = "200"  # Payment amount in BDT


class LicenseBot:
    def __init__(self):
        self.gmail_service = None
        self.sheets_service = None
        self.credentials = None
        
    def authenticate(self):
        """Authenticate with Google APIs using OAuth2"""
        creds = None
        
        # Token file stores the user's access and refresh tokens
        if os.path.exists('token.pickle'):
            with open('token.pickle', 'rb') as token:
                creds = pickle.load(token)
        
        # If there are no (valid) credentials available, let the user log in
        if not creds or not creds.valid:
            if creds and creds.expired and creds.refresh_token:
                creds.refresh(Request())
            else:
                # Use credentials.json from GitHub Secrets
                if os.path.exists('credentials.json'):
                    flow = InstalledAppFlow.from_client_secrets_file(
                        'credentials.json', SCOPES)
                    creds = flow.run_local_server(port=0)
                else:
                    print("ERROR: credentials.json not found!")
                    return False
            
            # Save the credentials for the next run
            with open('token.pickle', 'wb') as token:
                pickle.dump(creds, token)
        
        self.credentials = creds
        self.gmail_service = build('gmail', 'v1', credentials=creds)
        self.sheets_service = build('sheets', 'v4', credentials=creds)
        
        print("✓ Authentication successful!")
        return True
    
    def get_unread_payment_emails(self):
        """Search Gmail for unread payment notification emails"""
        try:
            # Search for unread emails with "Bkash: Received" in subject
            query = 'subject:"Bkash: Received" is:unread'
            
            results = self.gmail_service.users().messages().list(
                userId='me', q=query, maxResults=10
            ).execute()
            
            messages = results.get('messages', [])
            print(f"Found {len(messages)} unread payment emails")
            
            return messages
        
        except HttpError as error:
            print(f"Error fetching emails: {error}")
            return []
    
    def extract_payment_info(self, message_id):
        """Extract Transaction ID and Amount from email"""
        try:
            message = self.gmail_service.users().messages().get(
                userId='me', id=message_id, format='full'
            ).execute()
            
            # Get email body
            if 'parts' in message['payload']:
                parts = message['payload']['parts']
                data = parts[0]['body'].get('data', '')
            else:
                data = message['payload']['body'].get('data', '')
            
            # Decode base64
            body = base64.urlsafe_b64decode(data).decode('utf-8')
            
            # Extract TrxID (format: TrxID: XXXXXXXXXX)
            trx_match = re.search(r'TrxID[:\s]+([A-Z0-9]+)', body, re.IGNORECASE)
            trx_id = trx_match.group(1) if trx_match else None
            
            # Extract Amount (format: Amount: 200 or Tk 200)
            amount_match = re.search(r'(?:Amount|Tk)[:\s]+(\d+)', body, re.IGNORECASE)
            amount = amount_match.group(1) if amount_match else None
            
            print(f"Extracted - TrxID: {trx_id}, Amount: {amount}")
            
            return {
                'message_id': message_id,
                'trx_id': trx_id,
                'amount': amount
            }
        
        except HttpError as error:
            print(f"Error reading message: {error}")
            return None
    
    def find_order_in_sheet(self, trx_id):
        """Search Google Sheet for matching TrxID"""
        try:
            # Read data from Google Sheets
            range_name = 'Sheet1!A:E'  # Adjust sheet name if needed
            result = self.sheets_service.spreadsheets().values().get(
                spreadsheetId=SPREADSHEET_ID,
                range=range_name
            ).execute()
            
            rows = result.get('values', [])
            
            # Find row with matching TrxID
            for i, row in enumerate(rows):
                if len(row) >= 4 and row[2] == trx_id:  # Column C = TrxID
                    status = row[4] if len(row) > 4 else ""
                    
                    if status.lower() == 'delivered':
                        print(f"Order already delivered for TrxID: {trx_id}")
                        return None
                    
                    return {
                        'row': i + 1,
                        'timestamp': row[0],
                        'email': row[1],
                        'trx_id': row[2],
                        'machine_id': row[3],
                        'status': status
                    }
            
            print(f"No matching order found for TrxID: {trx_id}")
            return None
        
        except HttpError as error:
            print(f"Error reading sheet: {error}")
            return None
    
    def generate_license_key(self, machine_id):
        """
        Generate license key using the algorithm:
        Reverse MachineID + Current Month Number + Hash
        """
        import hashlib
        
        # Remove dashes and reverse
        clean_id = machine_id.replace("-", "")
        reversed_id = clean_id[::-1]
        
        # Add month
        month = datetime.now().month
        combined = reversed_id + str(month).zfill(2)
        
        # Create hash
        hash_obj = hashlib.md5(combined.encode())
        hash_suffix = hash_obj.hexdigest()[:8].upper()
        
        # Combine
        license_key = (reversed_id + hash_suffix).upper()
        
        # Format as XXXX-XXXX-XXXX-XXXX
        formatted = '-'.join([license_key[i:i+4] for i in range(0, 16, 4)])
        
        print(f"Generated license key: {formatted}")
        return formatted
    
    def send_license_email(self, to_email, license_key, machine_id):
        """Send license key via email"""
        try:
            message = MIMEMultipart('alternative')
            message['To'] = to_email
            message['From'] = 'me'
            message['Subject'] = 'Your Bijoy Typing Master License Key 🎉'
            
            # Email body
            html_body = f"""
            <html>
            <body style="font-family: Arial, sans-serif; padding: 20px;">
                <h2 style="color: #512BD4;">Thank You for Your Purchase!</h2>
                <p>Your <strong>Bijoy Typing Master</strong> license has been activated.</p>
                
                <div style="background-color: #f0f0f0; padding: 20px; border-radius: 10px; margin: 20px 0;">
                    <h3>Your License Information:</h3>
                    <p><strong>Machine ID:</strong> <code>{machine_id}</code></p>
                    <p><strong>License Key:</strong> <code style="font-size: 18px; color: #512BD4;">{license_key}</code></p>
                </div>
                
                <h3>How to Activate:</h3>
                <ol>
                    <li>Open Bijoy Typing Master application</li>
                    <li>Go to "Settings & License" section</li>
                    <li>Enter your license key exactly as shown above</li>
                    <li>Click "Activate License"</li>
                </ol>
                
                <p style="color: #666; font-size: 12px; margin-top: 30px;">
                    If you have any issues, please reply to this email or contact support.
                </p>
                
                <p>Happy typing! 🚀</p>
            </body>
            </html>
            """
            
            text_body = f"""
Thank You for Your Purchase!

Your Bijoy Typing Master license has been activated.

Machine ID: {machine_id}
License Key: {license_key}

How to Activate:
1. Open Bijoy Typing Master application
2. Go to "Settings & License" section
3. Enter your license key
4. Click "Activate License"

Happy typing!
            """
            
            part1 = MIMEText(text_body, 'plain')
            part2 = MIMEText(html_body, 'html')
            message.attach(part1)
            message.attach(part2)
            
            # Encode and send
            raw = base64.urlsafe_b64encode(message.as_bytes()).decode()
            send_message = {'raw': raw}
            
            self.gmail_service.users().messages().send(
                userId='me', body=send_message
            ).execute()
            
            print(f"✓ License email sent to {to_email}")
            return True
        
        except HttpError as error:
            print(f"Error sending email: {error}")
            return False
    
    def mark_email_as_read(self, message_id):
        """Mark Gmail message as read"""
        try:
            self.gmail_service.users().messages().modify(
                userId='me',
                id=message_id,
                body={'removeLabelIds': ['UNREAD']}
            ).execute()
            
            print(f"✓ Marked email as read")
        except HttpError as error:
            print(f"Error marking email as read: {error}")
    
    def update_sheet_status(self, row_number, license_key):
        """Update Google Sheet with Delivered status and license key"""
        try:
            # Update Status (Column E) and License Key (Column F)
            range_name = f'Sheet1!E{row_number}:F{row_number}'
            values = [['Delivered', license_key]]
            body = {'values': values}
            
            self.sheets_service.spreadsheets().values().update(
                spreadsheetId=SPREADSHEET_ID,
                range=range_name,
                valueInputOption='RAW',
                body=body
            ).execute()
            
            print(f"✓ Updated sheet row {row_number} with status: Delivered")
        
        except HttpError as error:
            print(f"Error updating sheet: {error}")
    
    def process_payments(self):
        """Main processing loop"""
        print("\n" + "="*50)
        print("🤖 Bijoy Typing Master License Bot")
        print("="*50 + "\n")
        
        # Get unread payment emails
        messages = self.get_unread_payment_emails()
        
        if not messages:
            print("No new payment emails found.")
            return
        
        processed_count = 0
        
        for message in messages:
            message_id = message['id']
            print(f"\n--- Processing Email ID: {message_id} ---")
            
            # Extract payment info
            payment_info = self.extract_payment_info(message_id)
            
            if not payment_info or not payment_info['trx_id']:
                print("Could not extract payment info, skipping...")
                continue
            
            # Check if amount matches
            if payment_info['amount'] != EXPECTED_AMOUNT:
                print(f"Amount mismatch: {payment_info['amount']} != {EXPECTED_AMOUNT}")
                self.mark_email_as_read(message_id)
                continue
            
            # Find order in Google Sheets
            order = self.find_order_in_sheet(payment_info['trx_id'])
            
            if not order:
                print("No matching order found or already delivered")
                self.mark_email_as_read(message_id)
                continue
            
            # Generate license key
            license_key = self.generate_license_key(order['machine_id'])
            
            # Send email with license
            if self.send_license_email(order['email'], license_key, order['machine_id']):
                # Update sheet
                self.update_sheet_status(order['row'], license_key)
                
                # Mark Gmail as read
                self.mark_email_as_read(message_id)
                
                processed_count += 1
                print(f"✓ Successfully processed order for {order['email']}")
            else:
                print(f"Failed to send email to {order['email']}")
        
        print(f"\n{'='*50}")
        print(f"✓ Processed {processed_count} orders successfully!")
        print(f"{'='*50}\n")


def main():
    """Main entry point"""
    bot = LicenseBot()
    
    # Authenticate
    if not bot.authenticate():
        print("Authentication failed!")
        return
    
    # Process payments
    bot.process_payments()


if __name__ == "__main__":
    main()
