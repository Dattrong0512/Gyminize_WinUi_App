using Gyminize_API.Data.Models;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.EntityFrameworkCore;
namespace Gyminize_API.Data
{
    public class EntityDatabaseContext : DbContext//Một lớp của EntityFramwork để tương tác với csdl
    {
        //DbContextOptions chứa cấu hình cần thiết để kết nối với csdl, ở đây ngữ cảnh là EntityDatabaseContext
        public EntityDatabaseContext(DbContextOptions<EntityDatabaseContext> options) : base(options)//Dùng để truyền các cấu hình đã được cấu hình lúc khởi tạo,
                                                                                                     //cho lớp cha là DBcontextOptions
                                                                                                     //Các cấu hình này đã được tiêm vào qua DI
        {

        }
        public DbSet<Customer> CustomerEntity
        {
            get; set;
        }

    }
}
