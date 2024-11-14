using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gyminize.Helpers;
public class WorkoutDetail : INotifyPropertyChanged
{
    private bool _isSelected;
    public int TypeworkoutId
    {
        get; set;
    }   // ID của loại bài tập
    public DateTime DateWorkout
    {
        get; set;
    } // Ngày thực hiện bài tập

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
    public bool IsCurrentDay => DateWorkout.Date == DateTime.Now.Date;

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    // New property to hold the list of ExerciseDetail objects
    public List<ExerciseDetail> ExerciseDetails
    {
        get; set;
    }

    public static List<WorkoutDetail> GetSampleWorkoutDetails()
    {
        var allExercises = ExerciseDetail.GetCompleteSampleExerciseDetails();

        var workoutDetails = new List<WorkoutDetail>
            {
                new WorkoutDetail { TypeworkoutId = 1, DateWorkout = new DateTime(2024, 11, 12) },
                new WorkoutDetail { TypeworkoutId = 2, DateWorkout = new DateTime(2024, 11, 14) },
                new WorkoutDetail { TypeworkoutId = 3, DateWorkout = new DateTime(2024, 11, 16) },
                new WorkoutDetail { TypeworkoutId = 4, DateWorkout = new DateTime(2024, 11, 18) },
                new WorkoutDetail { TypeworkoutId = 1, DateWorkout = new DateTime(2024, 11, 20) },
                new WorkoutDetail { TypeworkoutId = 2, DateWorkout = new DateTime(2024, 11, 22) },
                new WorkoutDetail { TypeworkoutId = 3, DateWorkout = new DateTime(2024, 11, 24) },
                new WorkoutDetail { TypeworkoutId = 4, DateWorkout = new DateTime(2024, 11, 26) },
                new WorkoutDetail { TypeworkoutId = 1, DateWorkout = new DateTime(2024, 11, 28) },
                new WorkoutDetail { TypeworkoutId = 2, DateWorkout = new DateTime(2024, 11, 30) },
                new WorkoutDetail { TypeworkoutId = 3, DateWorkout = new DateTime(2024, 12, 2) },
                new WorkoutDetail { TypeworkoutId = 4, DateWorkout = new DateTime(2024, 12, 4) },
                new WorkoutDetail { TypeworkoutId = 1, DateWorkout = new DateTime(2024, 12, 6) },
                new WorkoutDetail { TypeworkoutId = 2, DateWorkout = new DateTime(2024, 12, 8) },
                new WorkoutDetail { TypeworkoutId = 3, DateWorkout = new DateTime(2024, 12, 10) },
                new WorkoutDetail { TypeworkoutId = 4, DateWorkout = new DateTime(2024, 12, 12) },
                new WorkoutDetail { TypeworkoutId = 1, DateWorkout = new DateTime(2024, 12, 14) },
                new WorkoutDetail { TypeworkoutId = 2, DateWorkout = new DateTime(2024, 12, 16) },
                new WorkoutDetail { TypeworkoutId = 3, DateWorkout = new DateTime(2024, 12, 18) },
                new WorkoutDetail { TypeworkoutId = 4, DateWorkout = new DateTime(2024, 12, 20) },
                new WorkoutDetail { TypeworkoutId = 1, DateWorkout = new DateTime(2024, 12, 22) },
                new WorkoutDetail { TypeworkoutId = 2, DateWorkout = new DateTime(2024, 12, 24) },
                new WorkoutDetail { TypeworkoutId = 3, DateWorkout = new DateTime(2024, 12, 26) },
                new WorkoutDetail { TypeworkoutId = 4, DateWorkout = new DateTime(2024, 12, 28) },
                new WorkoutDetail { TypeworkoutId = 1, DateWorkout = new DateTime(2024, 12, 30) },
                new WorkoutDetail { TypeworkoutId = 2, DateWorkout = new DateTime(2025, 1, 1) },
                new WorkoutDetail { TypeworkoutId = 3, DateWorkout = new DateTime(2025, 1, 3) },
                new WorkoutDetail { TypeworkoutId = 4, DateWorkout = new DateTime(2025, 1, 5) },
                new WorkoutDetail { TypeworkoutId = 1, DateWorkout = new DateTime(2025, 1, 7) }
            };

        // Populate ExerciseDetails for each WorkoutDetail
        foreach (var workoutDetail in workoutDetails)
        {
            workoutDetail.ExerciseDetails = allExercises
                .Where(exercise => exercise.TypeworkoutId == workoutDetail.TypeworkoutId)
                .ToList();
        }

        return workoutDetails;
    }
}

