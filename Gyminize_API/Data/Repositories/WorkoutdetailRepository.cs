using Gyminize_API.Data.Model;


namespace Gyminize_API.Data.Repositories
{
    public class WorkoutdetailRepository
    {
        private readonly EntityDatabaseContext _context;
        public WorkoutdetailRepository(EntityDatabaseContext context)
        {
            _context = context;
        }
        public Workoutdetail UpdateDecription(int workoutdetail_id, string decription)
        {
            var check_workoutdetail = _context.WorkoutDetailEntity
                .Where(x => x.workoutdetail_id == workoutdetail_id)
                .FirstOrDefault();

            if (check_workoutdetail != null)
            {
                // Chuyển đổi date_workout về UTC nếu cần
                if (check_workoutdetail.date_workout.Kind == DateTimeKind.Unspecified)
                {
                    check_workoutdetail.date_workout = DateTime.SpecifyKind(
                        check_workoutdetail.date_workout, DateTimeKind.Utc
                    );
                }
                else if (check_workoutdetail.date_workout.Kind == DateTimeKind.Local)
                {
                    check_workoutdetail.date_workout = check_workoutdetail.date_workout.ToUniversalTime();
                }

                // Cập nhật description
                check_workoutdetail.description = decription;

                _context.WorkoutDetailEntity.Update(check_workoutdetail);
            }

            _context.SaveChanges();
            return check_workoutdetail;
        }

    }
}


