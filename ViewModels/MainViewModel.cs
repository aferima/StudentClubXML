using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using StudentClubsXml.Models;
using StudentClubsXml.Services;

namespace StudentClubsXml.ViewModels;

public class MainViewModel : INotifyPropertyChanged
{
    private readonly IDialogService _dialogService;
    private readonly XmlSearchService _searchService;
    private readonly StrategyFactory _strategyFactory;
    private readonly XmlTransformer _transformer;
    private readonly ExitService _exitService;

    private string? _xmlPath;
    private string? _xslPath;
    private SearchType _selectedMethod = SearchType.Dom;
    private string _statusMessage = "Виберіть XML-файл.";
    private Dictionary<string, List<string>> _attributeValues = new();

    public event PropertyChangedEventHandler? PropertyChanged;

    public SearchCriteria Criteria { get; } = new();

    public ObservableCollection<Club> Results { get; } = new();

    public Dictionary<string, List<string>> AttributeValues
    {
        get => _attributeValues;
        private set
        {
            _attributeValues = value;
            OnPropertyChanged();
        }
    }

    public string StatusMessage
    {
        get => _statusMessage;
        private set
        {
            _statusMessage = value;
            OnPropertyChanged();
        }
    }

    public SearchType SelectedMethod
    {
        get => _selectedMethod;
        set
        {
            if (_selectedMethod == value)
                return;

            _selectedMethod = value;
            OnPropertyChanged();

            _searchService.SetStrategy(
                _strategyFactory.Create(value));

            Results.Clear();
            Criteria.Clear();

            if (_xmlPath != null)
            {
                try
                {
                    UpdateAttributeValues();
                    StatusMessage = $"Обрано метод: {value}.";
                }
                catch (Exception ex)
                {
                    AttributeValues = new();
                    StatusMessage = $"Помилка: {ex.Message}";
                }
            }
        }
    }

    public ICommand LoadFileCommand { get; }
    public ICommand SelectXslCommand { get; }
    public ICommand SearchCommand { get; }
    public ICommand TransformCommand { get; }
    public ICommand ClearCommand { get; }
    public ICommand ExitCommand { get; }

    public MainViewModel(
        IDialogService dialogService,
        XmlSearchService searchService,
        StrategyFactory strategyFactory,
        XmlTransformer transformer,
        ExitService exitService)
    {
        _dialogService = dialogService;
        _searchService = searchService;
        _strategyFactory = strategyFactory;
        _transformer = transformer;
        _exitService = exitService;

        _searchService.SetStrategy(
            _strategyFactory.Create(SearchType.Dom));

        LoadFileCommand = new Command(
            async () => await LoadFileAsync());

        SelectXslCommand = new Command(
            async () => await SelectXslAsync());

        SearchCommand = new Command(
            async () => await SearchAsync());

        TransformCommand = new Command(
            async () => await TransformAsync());

        ClearCommand = new Command(Clear);

        ExitCommand = new Command(
            async () => await _exitService.RequestExitAsync());
    }

    private async Task LoadFileAsync()
    {
        try
        {
            var path = await _dialogService.PickFileAsync("xml");

            if (path == null)
                return;

            XmlFileValidator.Validate(path);

            var newValues = ReadAttributeValues(path);

            _xmlPath = path;
            AttributeValues = newValues;

            Criteria.Clear();
            Results.Clear();

            StatusMessage = $"XML завантажено: {Path.GetFileName(path)}";
        }
        catch (Exception ex)
        {
            await _dialogService.ShowMessageAsync("Помилка завантаження XML", ex.Message);
        }
    }

    private async Task SelectXslAsync()
    {
        try
        {
            var path = await _dialogService.PickFileAsync("xsl");

            if (path == null)
                return;

            _xslPath = path;

            StatusMessage = $"XSL вибрано: {Path.GetFileName(path)}";
        }
        catch (Exception ex)
        {
            await _dialogService.ShowMessageAsync("Помилка вибору XSL", ex.Message);
        }
    }

    private void UpdateAttributeValues()
    {
        if (_xmlPath != null)
        {
            AttributeValues = ReadAttributeValues(_xmlPath);
        }
    }

    private Dictionary<string, List<string>> ReadAttributeValues(
        string path)
    {
        var values = new Dictionary<string, List<string>>();

        foreach (var attribute in new[]
        {
            "faculty",
            "department",
            "type",
            "course",
            "day"
        })
        {
            values[attribute] = _searchService.GetAttributeValues(path, attribute);
        }

        return values;
    }

    private async Task SearchAsync()
    {
        if (_xmlPath == null)
        {
            await _dialogService.ShowMessageAsync("Пошук", "Спочатку завантажте XML-файл.");

            return;
        }

        try
        {
            var clubs = _searchService.Search(_xmlPath, Criteria);

            Results.Clear();

            foreach (var club in clubs)
            {
                Results.Add(club);
            }

            if (Results.Count == 0)
            {
                StatusMessage = "Нічого не знайдено.";

                await _dialogService.ShowMessageAsync("Пошук", "Нічого не знайдено.");
            }
            else
            {
                StatusMessage = $"Знайдено гуртків: {Results.Count}";
            }
        }
        catch (Exception ex)
        {
            await _dialogService.ShowMessageAsync("Помилка пошуку", ex.Message);
        }
    }

    private async Task TransformAsync()
    {
        if (_xmlPath == null || _xslPath == null)
        {
            await _dialogService.ShowMessageAsync("Трансформація", "Спочатку виберіть XML та XSL-файли.");

            return;
        }

        try
        {
            string directory = Path.GetDirectoryName(_xmlPath) ?? Environment.CurrentDirectory;

            string fileName = Path.GetFileNameWithoutExtension(_xmlPath);

            string outputPath = Path.Combine(directory, $"{fileName}_{DateTime.Now:yyyyMMdd_HHmmss_fff}.html");

            _transformer.Transform(_xmlPath, _xslPath, outputPath, Criteria);

            StatusMessage = "HTML-файл успішно створено.";

            await _dialogService.ShowMessageAsync("Трансформація завершена", $"HTML-файл збережено:\n{outputPath}");
        }
        catch (Exception ex)
        {
            await _dialogService.ShowMessageAsync("Помилка трансформації", ex.Message);
        }
    }

    private void Clear()
    {
        Criteria.Clear();
        Results.Clear();

        StatusMessage ="Параметри пошуку та результати очищено.";
    }

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
