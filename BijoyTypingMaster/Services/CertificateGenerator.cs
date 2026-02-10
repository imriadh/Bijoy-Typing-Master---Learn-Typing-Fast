using BijoyTypingMaster.Models;

namespace BijoyTypingMaster.Services;

/// <summary>
/// Generates typing certificates for users
/// </summary>
public class CertificateGenerator
{
    /// <summary>
    /// Generate certificate data for display/export
    /// </summary>
    public CertificateData GenerateCertificate(string userName, double wpm, double accuracy, DateTime date)
    {
        return new CertificateData
        {
            UserName = userName,
            WPM = wpm,
            Accuracy = accuracy,
            Date = date,
            CertificateNumber = GenerateCertificateNumber(),
            Rating = GetRating(wpm, accuracy),
            Message = GetCertificateMessage(wpm, accuracy)
        };
    }

    /// <summary>
    /// Generate certificate from speed test result
    /// </summary>
    public CertificateData GenerateCertificate(string userName, SpeedTestResult result)
    {
        return GenerateCertificate(userName, result.WPM, result.Accuracy, result.Date);
    }

    /// <summary>
    /// Generate certificate from user progress
    /// </summary>
    public CertificateData GenerateCertificate(string userName, UserProgress progress)
    {
        return GenerateCertificate(userName, progress.WPM, progress.Accuracy, progress.Date);
    }

    /// <summary>
    /// Generate unique certificate number
    /// </summary>
    private string GenerateCertificateNumber()
    {
        var date = DateTime.Now;
        var random = new Random();
        return $"BTM-{date:yyyy}{date:MM}{date:dd}-{random.Next(1000, 9999)}";
    }

    /// <summary>
    /// Get performance rating
    /// </summary>
    private string GetRating(double wpm, double accuracy)
    {
        if (accuracy < 80) return "Needs Practice";
        if (wpm < 20) return "Beginner";
        if (wpm < 40) return "Intermediate";
        if (wpm < 60) return "Advanced";
        if (wpm < 80) return "Expert";
        return "Master Typist";
    }

    /// <summary>
    /// Get certificate message based on performance
    /// </summary>
    private string GetCertificateMessage(double wpm, double accuracy)
    {
        if (wpm >= 80 && accuracy >= 95)
            return "Outstanding Performance! You've achieved master level typing skills.";
        if (wpm >= 60 && accuracy >= 90)
            return "Excellent Work! Your typing skills are at an expert level.";
        if (wpm >= 40 && accuracy >= 85)
            return "Great Progress! You've reached advanced typing proficiency.";
        if (wpm >= 20 && accuracy >= 80)
            return "Good Job! You've completed intermediate typing training.";
        
        return "Congratulations on completing your typing practice!";
    }

    /// <summary>
    /// Export certificate as formatted text
    /// </summary>
    public string ExportAsText(CertificateData certificate)
    {
        return $@"
╔════════════════════════════════════════════════════════════╗
║                 TYPING PROFICIENCY CERTIFICATE              ║
║                    Bijoy Typing Master                      ║
╚════════════════════════════════════════════════════════════╝

                        CERTIFICATE OF ACHIEVEMENT

This is to certify that

                    {certificate.UserName}

has successfully demonstrated typing proficiency with the following results:

    📊 Typing Speed (WPM):     {certificate.WPM:F2}
    ✅ Accuracy:               {certificate.Accuracy:F2}%
    🏆 Rating:                 {certificate.Rating}

{certificate.Message}

Certificate Number: {certificate.CertificateNumber}
Date: {certificate.Date:MMMM dd, yyyy}

═══════════════════════════════════════════════════════════════
              Bijoy Typing Master - Learn Typing Fast
═══════════════════════════════════════════════════════════════
";
    }

    /// <summary>
    /// Get certificate as HTML for better formatting
    /// </summary>
    public string ExportAsHtml(CertificateData certificate)
    {
        return $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='UTF-8'>
    <style>
        body {{
            font-family: 'Georgia', serif;
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            padding: 40px;
            margin: 0;
        }}
        .certificate {{
            background: white;
            max-width: 800px;
            margin: 0 auto;
            padding: 60px;
            border: 15px solid #FFD700;
            box-shadow: 0 0 30px rgba(0,0,0,0.3);
        }}
        .header {{
            text-align: center;
            color: #512BD4;
            margin-bottom: 30px;
        }}
        .title {{
            font-size: 42px;
            font-weight: bold;
            margin: 20px 0;
        }}
        .subtitle {{
            font-size: 24px;
            color: #666;
        }}
        .recipient {{
            text-align: center;
            font-size: 36px;
            color: #2c3e50;
            margin: 40px 0;
            font-weight: bold;
            border-bottom: 2px solid #333;
            padding-bottom: 10px;
        }}
        .stats {{
            display: flex;
            justify-content: space-around;
            margin: 40px 0;
        }}
        .stat {{
            text-align: center;
        }}
        .stat-value {{
            font-size: 32px;
            color: #512BD4;
            font-weight: bold;
        }}
        .stat-label {{
            font-size: 16px;
            color: #666;
            margin-top: 5px;
        }}
        .message {{
            text-align: center;
            font-size: 18px;
            color: #555;
            margin: 30px 0;
            font-style: italic;
        }}
        .footer {{
            text-align: center;
            margin-top: 50px;
            padding-top: 20px;
            border-top: 2px solid #ddd;
            color: #888;
        }}
        .cert-number {{
            font-family: 'Courier New', monospace;
            font-size: 14px;
        }}
    </style>
</head>
<body>
    <div class='certificate'>
        <div class='header'>
            <div class='title'>🎯 TYPING PROFICIENCY CERTIFICATE</div>
            <div class='subtitle'>Bijoy Typing Master</div>
        </div>
        
        <div style='text-align: center; font-size: 24px; margin: 30px 0;'>
            CERTIFICATE OF ACHIEVEMENT
        </div>
        
        <div style='text-align: center; font-size: 18px; color: #666;'>
            This is to certify that
        </div>
        
        <div class='recipient'>{certificate.UserName}</div>
        
        <div style='text-align: center; font-size: 16px; color: #666; margin: 20px 0;'>
            has successfully demonstrated typing proficiency with the following results:
        </div>
        
        <div class='stats'>
            <div class='stat'>
                <div class='stat-value'>{certificate.WPM:F1}</div>
                <div class='stat-label'>Words Per Minute</div>
            </div>
            <div class='stat'>
                <div class='stat-value'>{certificate.Accuracy:F1}%</div>
                <div class='stat-label'>Accuracy</div>
            </div>
            <div class='stat'>
                <div class='stat-value'>{GetStars(certificate.WPM, certificate.Accuracy)}</div>
                <div class='stat-label'>Rating</div>
            </div>
        </div>
        
        <div style='text-align: center; font-size: 22px; color: #512BD4; font-weight: bold; margin: 20px 0;'>
            {certificate.Rating}
        </div>
        
        <div class='message'>
            {certificate.Message}
        </div>
        
        <div class='footer'>
            <div class='cert-number'>Certificate No: {certificate.CertificateNumber}</div>
            <div style='margin-top: 10px;'>{certificate.Date:MMMM dd, yyyy}</div>
            <div style='margin-top: 20px; font-size: 14px;'>
                Bijoy Typing Master - Learn Typing Fast
            </div>
        </div>
    </div>
</body>
</html>
";
    }

    private string GetStars(double wpm, double accuracy)
    {
        double score = (wpm * accuracy) / 100.0;
        int stars = score < 20 ? 1 : score < 40 ? 2 : score < 60 ? 3 : score < 80 ? 4 : 5;
        return new string('⭐', stars);
    }
}

/// <summary>
/// Certificate data container
/// </summary>
public class CertificateData
{
    public string UserName { get; set; } = string.Empty;
    public double WPM { get; set; }
    public double Accuracy { get; set; }
    public DateTime Date { get; set; }
    public string CertificateNumber { get; set; } = string.Empty;
    public string Rating { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}
