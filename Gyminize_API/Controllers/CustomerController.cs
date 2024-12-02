using Gyminize_API.Data.Models;
using Gyminize_API.Data.Repositories;
using Microsoft.AspNetCore.Mvc;
// Import các thành phần của ASP.NET Core MVC.
// ControllerBase và các thuộc tính như [HttpGet], [HttpPost], [HttpPut], [HttpDelete]
// đều thuộc về thư viện này.
namespace Gyminize_API.Controllers
{
    /// \class CustomerController
    /// \brief Một controller để xử lý các yêu cầu API liên quan đến khách hàng.
    ///
    /// Controller này cung cấp các endpoint để lấy, tạo và cập nhật thông tin khách hàng.
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly CustomerRepository _customerRepository;

        /// \brief Constructor cho CustomerController.
        /// \param customerRepository Một instance của CustomerRepository để tương tác với tầng dữ liệu.
        public CustomerController(CustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        /// \brief Lấy thông tin khách hàng theo tên đăng nhập.
        /// \param username Tên đăng nhập của khách hàng.
        /// \return Một IActionResult chứa dữ liệu khách hàng.
        [HttpGet("get/username/{username}")]
        public IActionResult GetCustomerByUserName(string username)
        {
            var customer = _customerRepository.GetCustomerByUsername(username);
            return Ok(customer);
        }

        /// \brief Lấy thông tin khách hàng theo ID.
        /// \param id ID của khách hàng.
        /// \return Một IActionResult chứa dữ liệu khách hàng.
        [HttpGet("get/{id:int}")]
        public IActionResult GetCustomerByID(int id)
        {
            var customer = _customerRepository.GetCustomerById(id);
            return Ok(customer);
        }

        /// \brief Lấy thông tin khách hàng theo email.
        /// \param email Email của khách hàng.
        /// \return Một IActionResult chứa dữ liệu khách hàng.
        [HttpGet("get/email/{email}")]
        public IActionResult GetCustomerByEmail(string email)
        {
            var customer = _customerRepository.GetCustomerByEmail(email);
            return Ok(customer);
        }

        /// \brief Tạo mới một khách hàng.
        /// \param customer Đối tượng khách hàng cần tạo.
        /// \return Một IActionResult chứa dữ liệu khách hàng mới được tạo.
        [HttpPost("create")]
        public IActionResult AddCustomer(Customer customer)
        {
            var newCustomer = _customerRepository.addCustomer(customer);
            return Ok(newCustomer);
        }

        /// \brief Cập nhật thông tin khách hàng.
        /// \param username Tên đăng nhập của khách hàng cần cập nhật.
        /// \param customer Đối tượng khách hàng đã được cập nhật.
        /// \return Một IActionResult chứa dữ liệu khách hàng đã được cập nhật.
        [HttpPut("update/{username}")]
        public IActionResult UpdateCustomer(string username, Customer customer)
        {
            var updateCustomer = _customerRepository.updateCustomer(username, customer);
            return Ok(updateCustomer);
        }

        /// \brief Cập nhật mật khẩu của khách hàng.
        /// \param username Tên đăng nhập của khách hàng.
        /// \param password Mật khẩu mới.
        /// \return Một IActionResult chứa dữ liệu khách hàng đã được cập nhật.
        [HttpPut("update/{username}/password/{password}")]
        public IActionResult UpdatePassword(string username, string password)
        {
            var updatePassword = _customerRepository.updatePassworkByUser(username, password);
            return Ok(updatePassword);
        }
    }
}