using System.ComponentModel;

namespace ToDoMauiClient.Models;

public class ToDo : INotifyPropertyChanged
{
    int _id;
    public int Id 
    { 
        get => _id; 
        set
        {
            if (_id == value)
                return;
            _id = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Id)));
        }
    }

    string _toDoName;
    public string ToDoName 
    { 
        get => _toDoName;
        set
        {
            if (_toDoName == value)
                return;

            _toDoName = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ToDoName)));
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;
}
