using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace StudentClubsXml.Models;

public class SearchCriteria : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    private string? _name;
    private string? _faculty;
    private string? _department;
    private string? _type;
    private string? _course;
    private string? _day;
    private string? _leader;
    private string? _topic;

    public string? Name
    {
        get => _name;
        set => SetProperty(ref _name, value);
    }

    public string? Faculty
    {
        get => _faculty;
        set => SetProperty(ref _faculty, value);
    }

    public string? Department
    {
        get => _department;
        set => SetProperty(ref _department, value);
    }

    public string? Type
    {
        get => _type;
        set => SetProperty(ref _type, value);
    }

    public string? Course
    {
        get => _course;
        set => SetProperty(ref _course, value);
    }

    public string? Day
    {
        get => _day;
        set => SetProperty(ref _day, value);
    }

    public string? Leader
    {
        get => _leader;
        set => SetProperty(ref _leader, value);
    }

    public string? Topic
    {
        get => _topic;
        set => SetProperty(ref _topic, value);
    }

    public void Clear()
    {
        Name = null;
        Faculty = null;
        Department = null;
        Type = null;
        Course = null;
        Day = null;
        Leader = null;
        Topic = null;
    }

    public bool Matches(Club club)
    {
        return Contains(club.Name, Name)
            && Equal(club.Faculty, Faculty)
            && Equal(club.Department, Department)
            && Equal(club.Type, Type)
            && Equal(club.Course, Course)
            && Equal(club.Day, Day)
            && Contains(club.Leader, Leader)
            && Contains(club.Topic, Topic);
    }

    private static bool Equal(string? actual, string? wanted)
    {
        return string.IsNullOrWhiteSpace(wanted)
            || string.Equals(
                actual,
                wanted,
                StringComparison.OrdinalIgnoreCase);
    }

    private static bool Contains(string? actual, string? wanted)
    {
        return string.IsNullOrWhiteSpace(wanted)
            || (actual?.Contains(
                wanted,
                StringComparison.OrdinalIgnoreCase) ?? false);
    }

    private void SetProperty(
        ref string? field,
        string? value,
        [CallerMemberName] string? propertyName = null)
    {
        if (field == value)
            return;

        field = value;

        PropertyChanged?.Invoke(
            this,
            new PropertyChangedEventArgs(propertyName));
    }
}
