using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Gyminize.Models; 

public class Workoutdetail : INotifyPropertyChanged
{
    public int workoutdetail_id
    {
        get; set;
    }
    public int typeworkout_id
    {
        get; set;
    }
    public int plan_id
    {
        get; set;
    }
    public DateTime date_workout
    {
        get; set;
    }
    public string description
    {
        get; set;
    }
    public Typeworkout? Typeworkout
    {
        get; set;
    }
    private bool _isSelected;
    public bool IsSelected
    {
        get => _isSelected;
        set
        {
            if (_isSelected != value)
            {
                _isSelected = value;
                OnPropertyChanged(nameof(IsSelected));
            }
        }
    }
    public bool IsCurrentDay => date_workout.Date == DateTime.Now.Date;

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
