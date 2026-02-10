using BijoyTypingMaster.Models;
using BijoyTypingMaster.Services;
using System.Collections.ObjectModel;

namespace BijoyTypingMaster.Views;

public partial class CustomPracticeWindow : ContentPage
{
    private readonly CustomTextManager _customTextManager;
    private readonly XPManager _xpManager;
    private ObservableCollection<CustomTextViewModel> _savedTexts = new();

    public CustomPracticeWindow(CustomTextManager customTextManager, XPManager xpManager)
    {
        InitializeComponent();
        _customTextManager = customTextManager;
        _xpManager = xpManager;

        SavedTextsCollection.ItemsSource = _savedTexts;

        LoadSavedTexts();
    }

    private async void LoadSavedTexts()
    {
        try
        {
            var texts = await _customTextManager.GetSavedTextsAsync();
            _savedTexts.Clear();
            
            foreach (var text in texts)
            {
                _savedTexts.Add(new CustomTextViewModel(text));
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to load saved texts: {ex.Message}", "OK");
        }
    }

    private void OnTextChanged(object sender, TextChangedEventArgs e)
    {
        int count = e.NewTextValue?.Length ?? 0;
        CharCountLabel.Text = $"{count} characters";

        if (count < 50)
        {
            CharCountLabel.TextColor = Color.FromArgb("#ef4444");
        }
        else
        {
            CharCountLabel.TextColor = Color.FromArgb("#10b981");
        }
    }

    private async void OnImportClicked(object sender, EventArgs e)
    {
        try
        {
            var result = await FilePicker.PickAsync(new PickOptions
            {
                FileTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.WinUI, new[] { ".txt" } }
                }),
                PickerTitle = "Select text file"
            });

            if (result != null)
            {
                var (success, message, text) = await _customTextManager.ImportFromFileAsync(result.FullPath);
                
                if (success)
                {
                    TextEditor.Text = text;
                    TitleEntry.Text = Path.GetFileNameWithoutExtension(result.FileName);
                    await DisplayAlert("Success", message, "OK");
                }
                else
                {
                    await DisplayAlert("Import Failed", message, "OK");
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to import file: {ex.Message}", "OK");
        }
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        string title = TitleEntry.Text?.Trim() ?? "";
        string text = TextEditor.Text?.Trim() ?? "";

        var validation = _customTextManager.ValidateText(text);
        if (!validation.valid)
        {
            await DisplayAlert("Validation Error", validation.message, "OK");
            return;
        }

        var (success, message, id) = await _customTextManager.SaveCustomTextAsync(title, text);
        
        if (success)
        {
            await DisplayAlert("Success", message, "OK");
            TitleEntry.Text = "";
            TextEditor.Text = "";
            LoadSavedTexts();
        }
        else
        {
            await DisplayAlert("Save Failed", message, "OK");
        }
    }

    private async void OnPracticeClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is int id)
        {
            var texts = await _customTextManager.GetSavedTextsAsync();
            var selectedText = texts.FirstOrDefault(t => t.Id == id);

            if (selectedText != null)
            {
                // Navigate to practice window with custom text
                var practiceWindow = new CustomTextPracticeWindow(selectedText, _customTextManager, _xpManager);
                await Navigation.PushAsync(practiceWindow);

                // Refresh list when returning
                practiceWindow.Disappearing += (s, args) => LoadSavedTexts();
            }
        }
    }

    private async void OnDeleteClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is int id)
        {
            bool confirm = await DisplayAlert(
                "Confirm Delete",
                "Are you sure you want to delete this saved text?",
                "Delete",
                "Cancel"
            );

            if (confirm)
            {
                bool success = await _customTextManager.DeleteTextAsync(id);
                if (success)
                {
                    await DisplayAlert("Success", "Text deleted successfully", "OK");
                    LoadSavedTexts();
                }
                else
                {
                    await DisplayAlert("Error", "Failed to delete text", "OK");
                }
            }
        }
    }
}

// ViewModel for saved texts
public class CustomTextViewModel
{
    private readonly CustomPracticeSession _session;

    public CustomTextViewModel(CustomPracticeSession session)
    {
        _session = session;
    }

    public int Id => _session.Id;
    public string Title => _session.Title;
    public string InfoText => $"{_session.GetWordCount()} words • {_session.GetCharacterCount()} characters";
    public string StatsText => _session.GetStatsString();
}
