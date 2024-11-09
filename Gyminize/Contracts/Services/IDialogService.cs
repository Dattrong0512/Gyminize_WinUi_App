using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gyminize.Contracts.Services;
public interface IDialogService
{
    Task<(string? selectedMeal, int Quantity)> ShowMealSelectionDialogAsync();
}
