using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gyminize.Helpers;
public class ExerciseDetail
{
    public int TypeworkoutId
    {
        get; set;
    }          // ID của ngày tập
    public string WorkoutDayType
    {
        get; set;
    }      // Loại ngày tập (Push/Pull/Leg/Fullbody)
    public string WorkoutDescription
    {
        get; set;
    }  // Mô tả của ngày tập (ví dụ: "Ngày tập cơ ngực và vai")
    public string ExerciseName
    {
        get; set;
    }        // Tên bài tập
    public string ExerciseDescription
    {
        get; set;
    } // Mô tả bài tập
    public string LinkVideo
    {
        get; set;
    }           // Link video hướng dẫn
    public int Reps
    {
        get; set;
    }                   // Số lần lặp lại
    public int WorkoutSets
    {
        get; set;
    }            // Số set của bài tập

    public static List<ExerciseDetail> GetCompleteSampleExerciseDetails()
    {
        return new List<ExerciseDetail>
    {
        // Ngày Push
        new ExerciseDetail
        {
            TypeworkoutId = 1,
            WorkoutDayType = "Push",
            WorkoutDescription = "Ngày tập cơ ngực và vai",
            ExerciseName = "Bench Press",
            ExerciseDescription = "Bài tập ngực với tạ đòn",
            LinkVideo = "https://example.com/benchpress",
            Reps = 10,
            WorkoutSets = 3
        },
        new ExerciseDetail
        {
            TypeworkoutId = 1,
            WorkoutDayType = "Push",
            WorkoutDescription = "Ngày tập cơ ngực và vai",
            ExerciseName = "Shoulder Press",
            ExerciseDescription = "Bài tập vai với tạ đòn",
            LinkVideo = "https://example.com/shoulderpress",
            Reps = 10,
            WorkoutSets = 3
        },
        new ExerciseDetail
        {
            TypeworkoutId = 1,
            WorkoutDayType = "Push",
            WorkoutDescription = "Ngày tập cơ ngực và vai",
            ExerciseName = "Chest Fly",
            ExerciseDescription = "Bài tập ngực với máy",
            LinkVideo = "https://example.com/chestfly",
            Reps = 12,
            WorkoutSets = 3
        },
        new ExerciseDetail
        {
            TypeworkoutId = 1,
            WorkoutDayType = "Push",
            WorkoutDescription = "Ngày tập cơ ngực và vai",
            ExerciseName = "Tricep Dips",
            ExerciseDescription = "Bài tập tay sau",
            LinkVideo = "https://example.com/tricepdips",
            Reps = 12,
            WorkoutSets = 3
        },
        new ExerciseDetail
        {
            TypeworkoutId = 1,
            WorkoutDayType = "Push",
            WorkoutDescription = "Ngày tập cơ ngực và vai",
            ExerciseName = "Push Ups",
            ExerciseDescription = "Chống đẩy",
            LinkVideo = "https://example.com/pushups",
            Reps = 15,
            WorkoutSets = 3
        },

        // Ngày Pull
        new ExerciseDetail
        {
            TypeworkoutId = 2,
            WorkoutDayType = "Pull",
            WorkoutDescription = "Ngày tập cơ lưng và tay",
            ExerciseName = "Pull Ups",
            ExerciseDescription = "Bài tập kéo xà",
            LinkVideo = "https://example.com/pullups",
            Reps = 8,
            WorkoutSets = 3
        },
        new ExerciseDetail
        {
            TypeworkoutId = 2,
            WorkoutDayType = "Pull",
            WorkoutDescription = "Ngày tập cơ lưng và tay",
            ExerciseName = "Barbell Row",
            ExerciseDescription = "Bài tập lưng với tạ đòn",
            LinkVideo = "https://example.com/barbellrow",
            Reps = 10,
            WorkoutSets = 3
        },
        new ExerciseDetail
        {
            TypeworkoutId = 2,
            WorkoutDayType = "Pull",
            WorkoutDescription = "Ngày tập cơ lưng và tay",
            ExerciseName = "Lat Pulldown",
            ExerciseDescription = "Bài tập lưng với máy kéo",
            LinkVideo = "https://example.com/latpulldown",
            Reps = 12,
            WorkoutSets = 3
        },
        new ExerciseDetail
        {
            TypeworkoutId = 2,
            WorkoutDayType = "Pull",
            WorkoutDescription = "Ngày tập cơ lưng và tay",
            ExerciseName = "Dumbbell Row",
            ExerciseDescription = "Bài tập lưng với tạ tay",
            LinkVideo = "https://example.com/dumbbellrow",
            Reps = 10,
            WorkoutSets = 3
        },
        new ExerciseDetail
        {
            TypeworkoutId = 2,
            WorkoutDayType = "Pull",
            WorkoutDescription = "Ngày tập cơ lưng và tay",
            ExerciseName = "Bicep Curl",
            ExerciseDescription = "Bài tập tay trước với tạ",
            LinkVideo = "https://example.com/bicepcurl",
            Reps = 12,
            WorkoutSets = 3
        },

        // Ngày Leg
        new ExerciseDetail
        {
            TypeworkoutId = 3,
            WorkoutDayType = "Leg",
            WorkoutDescription = "Ngày tập chân và mông",
            ExerciseName = "Squats",
            ExerciseDescription = "Bài tập chân với tạ đòn",
            LinkVideo = "https://example.com/squats",
            Reps = 12,
            WorkoutSets = 3
        },
        new ExerciseDetail
        {
            TypeworkoutId = 3,
            WorkoutDayType = "Leg",
            WorkoutDescription = "Ngày tập chân và mông",
            ExerciseName = "Leg Press",
            ExerciseDescription = "Bài tập đẩy chân",
            LinkVideo = "https://example.com/legpress",
            Reps = 12,
            WorkoutSets = 3
        },
        new ExerciseDetail
        {
            TypeworkoutId = 3,
            WorkoutDayType = "Leg",
            WorkoutDescription = "Ngày tập chân và mông",
            ExerciseName = "Lunges",
            ExerciseDescription = "Bài tập bước chân",
            LinkVideo = "https://example.com/lunges",
            Reps = 12,
            WorkoutSets = 3
        },
        new ExerciseDetail
        {
            TypeworkoutId = 3,
            WorkoutDayType = "Leg",
            WorkoutDescription = "Ngày tập chân và mông",
            ExerciseName = "Leg Curls",
            ExerciseDescription = "Bài tập cơ chân sau",
            LinkVideo = "https://example.com/legcurls",
            Reps = 12,
            WorkoutSets = 3
        },

        // Ngày Fullbody
        new ExerciseDetail
        {
            TypeworkoutId = 4,
            WorkoutDayType = "Fullbody",
            WorkoutDescription = "Ngày tập toàn thân",
            ExerciseName = "Deadlift",
            ExerciseDescription = "Bài tập kéo tạ từ sàn",
            LinkVideo = "https://example.com/deadlift",
            Reps = 10,
            WorkoutSets = 3
        },
        new ExerciseDetail
        {
            TypeworkoutId = 4,
            WorkoutDayType = "Fullbody",
            WorkoutDescription = "Ngày tập toàn thân",
            ExerciseName = "Burpees",
            ExerciseDescription = "Bài tập nhảy kết hợp hít đất",
            LinkVideo = "https://example.com/burpees",
            Reps = 15,
            WorkoutSets = 3
        },
        new ExerciseDetail
        {
            TypeworkoutId = 4,
            WorkoutDayType = "Fullbody",
            WorkoutDescription = "Ngày tập toàn thân",
            ExerciseName = "Clean and Press",
            ExerciseDescription = "Bài tập nâng tạ lên vai và đẩy qua đầu",
            LinkVideo = "https://example.com/cleanandpress",
            Reps = 8,
            WorkoutSets = 3
        },
        new ExerciseDetail
        {
            TypeworkoutId = 4,
            WorkoutDayType = "Fullbody",
            WorkoutDescription = "Ngày tập toàn thân",
            ExerciseName = "Kettlebell Swing",
            ExerciseDescription = "Bài tập vung tạ tay",
            LinkVideo = "https://example.com/kettlebellswing",
            Reps = 12,
            WorkoutSets = 3
        },
        new ExerciseDetail
        {
            TypeworkoutId = 4,
            WorkoutDayType = "Fullbody",
            WorkoutDescription = "Ngày tập toàn thân",
            ExerciseName = "Mountain Climbers",
            ExerciseDescription = "Bài tập leo núi tại chỗ",
            LinkVideo = "https://example.com/mountainclimbers",
            Reps = 20,
            WorkoutSets = 3
        }
    };
    }
}



