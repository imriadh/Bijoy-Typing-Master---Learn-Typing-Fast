using System.Security.Cryptography;
using System.Text;

namespace BijoyTypingMaster.Services;

/// <summary>
/// Manage licensing: 7-day trial and license key validation
/// </summary>
public class LicenseManager
{
    private const string SETTINGS_FILE = "license.dat";
    private const int TRIAL_DAYS = 7;
    private readonly string _settingsPath;

    public LicenseManager()
    {
        var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        var appFolder = Path.Combine(appDataPath, "BijoyTypingMaster");
        
        if (!Directory.Exists(appFolder))
        {
            Directory.CreateDirectory(appFolder);
        }
        
        _settingsPath = Path.Combine(appFolder, SETTINGS_FILE);
    }

    /// <summary>
    /// Check if the license is valid (either in trial period or has valid key)
    /// </summary>
    public bool IsValid()
    {
        var settings = LoadSettings();

        // Check if premium
        if (settings.IsPremium && !string.IsNullOrEmpty(settings.LicenseKey))
        {
            // Validate the license key
            if (ValidateKey(settings.LicenseKey))
            {
                return true;
            }
        }

        // Check trial period
        if (settings.InstallDate == DateTime.MinValue)
        {
            // First run - set install date
            settings.InstallDate = DateTime.Now;
            SaveSettings(settings);
            return true;
        }

        // Check if trial expired
        TimeSpan trialPeriod = DateTime.Now - settings.InstallDate;
        return trialPeriod.TotalDays <= TRIAL_DAYS;
    }

    /// <summary>
    /// Get remaining trial days
    /// </summary>
    public int GetRemainingTrialDays()
    {
        var settings = LoadSettings();
        
        if (settings.IsPremium)
        {
            return -1; // Premium user
        }

        if (settings.InstallDate == DateTime.MinValue)
        {
            return TRIAL_DAYS;
        }

        TimeSpan elapsed = DateTime.Now - settings.InstallDate;
        int remaining = TRIAL_DAYS - (int)elapsed.TotalDays;
        
        return Math.Max(0, remaining);
    }

    /// <summary>
    /// Validate a license key against the current machine ID
    /// Algorithm: Reverse MachineID + Append current month number
    /// </summary>
    public bool ValidateKey(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            return false;
        }

        try
        {
            string machineId = HardwareId.GetMachineId();
            string expectedKey = GenerateLicenseKey(machineId);
            
            return key.Equals(expectedKey, StringComparison.OrdinalIgnoreCase);
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Generate a license key for a given machine ID
    /// Algorithm: Reverse the MachineID and add a hash suffix
    /// </summary>
    public static string GenerateLicenseKey(string machineId)
    {
        // Remove dashes and reverse
        string cleanId = machineId.Replace("-", "");
        char[] charArray = cleanId.ToCharArray();
        Array.Reverse(charArray);
        string reversed = new string(charArray);

        // Add month-based hash
        int month = DateTime.Now.Month;
        string combined = reversed + month.ToString("D2");

        // Create a hash for additional security
        using (MD5 md5 = MD5.Create())
        {
            byte[] bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(combined));
            string hash = BitConverter.ToString(bytes).Replace("-", "").Substring(0, 8);
            
            // Format: REVERSED + HASH
            string licenseKey = reversed + hash;
            
            // Format as XXXX-XXXX-XXXX-XXXX
            return FormatLicenseKey(licenseKey);
        }
    }

    /// <summary>
    /// Format license key as XXXX-XXXX-XXXX-XXXX
    /// </summary>
    private static string FormatLicenseKey(string key)
    {
        if (key.Length < 16)
        {
            key = key.PadRight(16, '0');
        }

        return $"{key.Substring(0, 4)}-{key.Substring(4, 4)}-{key.Substring(8, 4)}-{key.Substring(12, 4)}";
    }

    /// <summary>
    /// Activate premium with a license key
    /// </summary>
    public bool ActivateLicense(string licenseKey)
    {
        if (ValidateKey(licenseKey))
        {
            var settings = LoadSettings();
            settings.IsPremium = true;
            settings.LicenseKey = licenseKey;
            settings.ActivationDate = DateTime.Now;
            SaveSettings(settings);
            
            return true;
        }
        
        return false;
    }

    /// <summary>
    /// Get the current machine ID for display
    /// </summary>
    public string GetMachineId()
    {
        return HardwareId.GetMachineId();
    }

    /// <summary>
    /// Check if user has premium
    /// </summary>
    public bool IsPremium()
    {
        var settings = LoadSettings();
        return settings.IsPremium && ValidateKey(settings.LicenseKey);
    }

    #region Settings Management

    private class LicenseSettings
    {
        public DateTime InstallDate { get; set; }
        public bool IsPremium { get; set; }
        public string LicenseKey { get; set; } = "";
        public DateTime ActivationDate { get; set; }
    }

    private LicenseSettings LoadSettings()
    {
        try
        {
            if (File.Exists(_settingsPath))
            {
                string encrypted = File.ReadAllText(_settingsPath);
                string json = DecryptString(encrypted);
                return System.Text.Json.JsonSerializer.Deserialize<LicenseSettings>(json) 
                    ?? new LicenseSettings();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading settings: {ex.Message}");
        }

        return new LicenseSettings();
    }

    private void SaveSettings(LicenseSettings settings)
    {
        try
        {
            string json = System.Text.Json.JsonSerializer.Serialize(settings);
            string encrypted = EncryptString(json);
            File.WriteAllText(_settingsPath, encrypted);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving settings: {ex.Message}");
        }
    }

    private string EncryptString(string plainText)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(plainText);
        return Convert.ToBase64String(bytes);
    }

    private string DecryptString(string encryptedText)
    {
        byte[] bytes = Convert.FromBase64String(encryptedText);
        return Encoding.UTF8.GetString(bytes);
    }

    #endregion
}
