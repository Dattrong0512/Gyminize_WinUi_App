﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gyminize.Models;

namespace Gyminize.Contracts.Services;
public interface IDialogService
{
    Task<(string? selectedMeal, int Quantity)> ShowMealSelectionDialogAsync();
    Task<bool> ShowFullExerciseWorkoutDialogAsync(List<Exercisedetail> workouts);

    Task ShowExerciseVideoDialogAsync(Exercise exercise);

    Task<bool> ShowVerificationDialogAsync(string email, string code);

    Task<(string email, string username)> ShowUsernameInputDialog();

    Task<string> ShowResetPasswordDialogAsync();

    Task ShowErrorDialogAsync(string errorMessage);

    Task ShowProductDialogWithSupplierAsync(Product product);

    Task ShowCheckoutDialogAsync(string customerName, string address, decimal totalProductPrice);
}
