using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gyminize.Contracts.Services;

namespace Gyminize.Services;

/// <summary>
/// Lớp này chịu trách nhiệm lấy thời gian hiện tại (hỗ trợ mock test).
/// </summary>
public class DateTimeProvider : IDateTimeProvider
{
    public DateTime Now => DateTime.Now;
    public DateTime UtcNow => DateTime.UtcNow;
}
