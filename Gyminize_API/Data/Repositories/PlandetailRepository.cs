using Gyminize_API.Data.Model;
using Microsoft.EntityFrameworkCore;

namespace Gyminize_API.Data.Repositories
{
    public class PlandetailRepository
    {

        private readonly EntityDatabaseContext _context;
        public PlandetailRepository(EntityDatabaseContext context)
        {
            _context = context;
        }
        public Plandetail? GetPlandetailById(int customerID)
        {
            var planDetail = _context.PlanDetailEntity
                  .Include(pd => pd.Plan) // Bao gồm đối tượng Plan
                  .Include(pd => pd.Workoutdetails) // Bao gồm danh sách Workoutdetails từ Plandetail
                      .ThenInclude(wd => wd.Typeworkout) // Bao gồm Typeworkout từ mỗi Workoutdetail
                      .ThenInclude(tw => tw.Exercisedetails) // Bao gồm danh sách Exercisedetails từ Typeworkout
                          .ThenInclude(ed => ed.Exercise) // Bao gồm đối tượng Exercise từ mỗi Exercisedetail
                  .Where(pd => pd.customer_id == customerID)
                  .FirstOrDefault(); // Kết thúc truy vấn với FirstOrDefault để trả về một Plandetail

            return planDetail;
        }
        public Plandetail AddPlandetail(int customerId, int plan)
        {
            try
            {
               var Plandetail = _context.PlanDetailEntity.FirstOrDefault(pl=>pl.customer_id == customerId);
                DateTime date = DateTime.Now;
                if (Plandetail != null && Plandetail.end_date > date)
                {
                    return null;
                }
                else
                {
                    DateTime start_day = DateTime.SpecifyKind(DateTime.Now.Date, DateTimeKind.Utc);
                    DateTime endDate = start_day.AddDays(56);

                    Plandetail plandetail = new Plandetail
                    {
                        customer_id = customerId,
                        plan_id = plan,
                        created_at = start_day,
                        start_date = start_day,
                        end_date = endDate
                    };
                    _context.PlanDetailEntity.Add(plandetail);
                    _context.SaveChanges();
                    return plandetail;
                }

                

            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}


