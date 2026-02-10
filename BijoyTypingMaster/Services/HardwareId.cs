using System.Management;
using System.Security.Cryptography;
using System.Text;

namespace BijoyTypingMaster.Services;

/// <summary>
/// Generate unique hardware-based Machine ID
/// Uses Processor ID and Motherboard Serial Number
/// </summary>
public class HardwareId
{
    /// <summary>
    /// Get the unique machine ID (e.g., "A1B2-C3D4-E5F6-G7H8")
    /// </summary>
    public static string GetMachineId()
    {
        try
        {
            string processorId = GetProcessorId();
            string motherboardSerial = GetMotherboardSerial();
            
            // Combine and hash
            string combined = processorId + motherboardSerial;
            string hash = GenerateHash(combined);
            
            // Format as XXXX-XXXX-XXXX-XXXX
            return FormatMachineId(hash);
        }
        catch (Exception ex)
        {
            // Fallback to a machine-specific but less secure ID
            Console.WriteLine($"Error generating hardware ID: {ex.Message}");
            return GenerateFallbackId();
        }
    }

    /// <summary>
    /// Get Processor ID using WMI (Windows Management Instrumentation)
    /// </summary>
    private static string GetProcessorId()
    {
        try
        {
            string cpuInfo = "";
            ManagementClass mc = new ManagementClass("Win32_Processor");
            ManagementObjectCollection moc = mc.GetInstances();

            foreach (ManagementObject mo in moc)
            {
                cpuInfo = mo.Properties["ProcessorId"].Value.ToString();
                break;
            }

            return cpuInfo;
        }
        catch
        {
            return Environment.MachineName;
        }
    }

    /// <summary>
    /// Get Motherboard Serial Number
    /// </summary>
    private static string GetMotherboardSerial()
    {
        try
        {
            string serial = "";
            ManagementObjectSearcher searcher = 
                new ManagementObjectSearcher("SELECT * FROM Win32_BaseBoard");

            foreach (ManagementObject mo in searcher.Get())
            {
                serial = mo["SerialNumber"]?.ToString() ?? "";
                break;
            }

            return serial;
        }
        catch
        {
            return Environment.UserName;
        }
    }

    /// <summary>
    /// Generate SHA256 hash from the combined hardware info
    /// </summary>
    private static string GenerateHash(string input)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
            
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("X2"));
            }
            
            return builder.ToString();
        }
    }

    /// <summary>
    /// Format hash as XXXX-XXXX-XXXX-XXXX
    /// </summary>
    private static string FormatMachineId(string hash)
    {
        // Take first 16 characters and format
        if (hash.Length >= 16)
        {
            return $"{hash.Substring(0, 4)}-{hash.Substring(4, 4)}-{hash.Substring(8, 4)}-{hash.Substring(12, 4)}";
        }
        
        return hash.PadRight(19, '0');
    }

    /// <summary>
    /// Generate a fallback ID if hardware access fails
    /// </summary>
    private static string GenerateFallbackId()
    {
        string fallback = Environment.MachineName + Environment.UserName + Environment.OSVersion.ToString();
        string hash = GenerateHash(fallback);
        return FormatMachineId(hash);
    }

    /// <summary>
    /// Verify if two machine IDs match
    /// </summary>
    public static bool VerifyMachineId(string storedId)
    {
        string currentId = GetMachineId();
        return currentId.Equals(storedId, StringComparison.OrdinalIgnoreCase);
    }
}
