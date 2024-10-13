﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
using Gyminize.Contracts.Services;
using Gyminize.Views;
using Gyminize.Models;
namespace Gyminize.ViewModels;
public partial class Guide1ViewModel : ObservableRecipient
{
    private readonly INavigationService _navigationService;
    public Guide1ViewModel(INavigationService navigationService)
    {
        _navigationService = navigationService;
        MaleCheckCommand = new RelayCommand<RoutedEventArgs?>(MaleSexCheck);
        FemaleCheckCommand = new RelayCommand<RoutedEventArgs?>(FemaleSexCheck);    
        AgeLostFocusCommand = new RelayCommand<RoutedEventArgs?>(OnAgeLostFocus);
        HeightLostFocusCommand = new RelayCommand<RoutedEventArgs?>(OnHeightLostFocus);
        WeightLostFocusCommand = new RelayCommand<RoutedEventArgs?>(OnWeightLostFocus);
        NavigateToGuidePage2Command = new RelayCommand(NavigateToGuidePage2);

        _maleCheckBox = new CheckBox();
        _femaleCheckBox = new CheckBox();
        _ageTextBox = new TextBox();
        _ageErrorTextBlock = new TextBlock();
        _heightTextBox = new TextBox();
        _heightErrorTextBlock = new TextBlock();
        _weightTextBox = new TextBox();
        _weightErrorTextBlock = new TextBlock();

        SelectedActivityLevel = new ComboBoxItem { Content = "Trung Bình ( 3 - 5 buổi/tuần )" };

    }

    public ICommand MaleCheckCommand { get; }
    public ICommand FemaleCheckCommand{ get;}
    public ICommand AgeLostFocusCommand { get; }
    public ICommand HeightLostFocusCommand { get; }
    public ICommand WeightLostFocusCommand { get; }
    public ICommand NavigateToGuidePage2Command
    {
        get;
    }

    private ComboBox _activityLevelComboBox;
    public ComboBox ActivityLevelComboBox
    {
        get => _activityLevelComboBox;
        set => SetProperty(ref _activityLevelComboBox, value);
    }

    private ComboBoxItem _selectedActivityLevel;
    public ComboBoxItem SelectedActivityLevel
    {
        get => _selectedActivityLevel;
        set => SetProperty(ref _selectedActivityLevel, value);
    }
    private CheckBox _maleCheckBox;
    public CheckBox MaleCheckBox
    {
        get => _maleCheckBox;
        set => SetProperty(ref _maleCheckBox, value);
    }

    private CheckBox _femaleCheckBox;
    public CheckBox FemaleCheckBox
    {
        get => _femaleCheckBox;
        set => SetProperty(ref _femaleCheckBox, value);
    }

    private TextBox _ageTextBox;
    public TextBox AgeTextBox
    {
        get => _ageTextBox;
        set => SetProperty(ref _ageTextBox, value);
    }

    private TextBlock _ageErrorTextBlock;
    public TextBlock AgeErrorTextBlock
    {
        get => _ageErrorTextBlock;
        set => SetProperty(ref _ageErrorTextBlock, value);
    }

    private TextBox _heightTextBox;
    public TextBox HeightTextBox
    {
        get => _heightTextBox;
        set => SetProperty(ref _heightTextBox, value);
    }

    private TextBlock _heightErrorTextBlock;
    public TextBlock HeightErrorTextBlock
    {
        get => _heightErrorTextBlock;
        set => SetProperty(ref _heightErrorTextBlock, value);
    }

    private TextBox _weightTextBox;
    public TextBox WeightTextBox
    {
        get => _weightTextBox;
        set => SetProperty(ref _weightTextBox, value);
    }

    private bool _isValid;
    public bool IsValid
    {
        get => _isValid;
        set => SetProperty(ref _isValid, value);
    }

    private TextBlock _weightErrorTextBlock;
    public TextBlock WeightErrorTextBlock
    {
        get => _weightErrorTextBlock;
        set => SetProperty(ref _weightErrorTextBlock, value);
    }

    private void MaleSexCheck(RoutedEventArgs? e)
    {
        
        if (FemaleCheckBox.IsChecked == true)
        {
            FemaleCheckBox.IsChecked = false;
        }
        
    }
    private void FemaleSexCheck(RoutedEventArgs? e)
    {

        if (MaleCheckBox.IsChecked == true)
        {
            MaleCheckBox.IsChecked = false;
        }

    }

    private void OnAgeLostFocus(RoutedEventArgs? e)
    {
        if (int.TryParse(AgeTextBox.Text, out int age))
        {
            if (age < 16 || age > 70)
            {
                AgeErrorTextBlock.Visibility = Visibility.Visible;
                AgeErrorTextBlock.Text = "*Ứng dụng chỉ hỗ trợ đối tượng từ 16-70 tuổi";
                IsValid = false;
            }
            else
            {
                AgeErrorTextBlock.Visibility = Visibility.Collapsed;
                AgeErrorTextBlock.Text = "*OK";
                IsValid = true;
            }
        }
        else if (string.IsNullOrEmpty(AgeTextBox.Text))
        {
            AgeErrorTextBlock.Visibility = Visibility.Visible;
            AgeErrorTextBlock.Text = "*Vui lòng nhập một độ tuổi hợp lệ";
            IsValid = false;
        }
    }

    private void OnHeightLostFocus(RoutedEventArgs? e)
    {
        if (int.TryParse(HeightTextBox.Text, out int height))
        {
            if (height < 110 || height > 210)
            {
                HeightErrorTextBlock.Visibility = Visibility.Visible;
                HeightErrorTextBlock.Text = "*Chiều cao không được hỗ trợ";
                IsValid = false;
            }
            else
            {
                HeightErrorTextBlock.Visibility = Visibility.Collapsed;
                HeightErrorTextBlock.Text = "*OK";
                IsValid = false;
            }
        }
        else if (string.IsNullOrEmpty(HeightTextBox.Text))
        {
            HeightErrorTextBlock.Visibility = Visibility.Visible;
            HeightErrorTextBlock.Text = "*Vui lòng nhập một chiều cao hợp lệ";
        }
    }

    private void OnWeightLostFocus(RoutedEventArgs? e)
    {
        if (double.TryParse(WeightTextBox.Text, out var weight))
        {
            if (weight < 35 || weight > 250)
            {
                WeightErrorTextBlock.Visibility = Visibility.Visible;
                WeightErrorTextBlock.Text = "*Cân nặng không được hỗ trợ";
            }
            else
            {
                WeightErrorTextBlock.Visibility = Visibility.Collapsed;
                WeightErrorTextBlock.Text = "*OK";
            }
        }
        else if (string.IsNullOrEmpty(WeightTextBox.Text))
        {
            WeightErrorTextBlock.Visibility = Visibility.Visible;
            WeightErrorTextBlock.Text = "*Vui lòng nhập một cân nặng hợp lệ";
        }
    }

    private int GetSelectedActivityLevel()
    {
        return SelectedActivityLevel?.Content switch
        {
            "Hầu như không vận động" => 1,
            "Thấp ( 1 - 2 buổi/tuần )" => 2,
            "Trung Bình ( 3 - 5 buổi/tuần )" => 3,
            "Cao ( 6 - 7 buổi/tuần )" => 4,
            _ => 0
        };
    }

    private void NavigateToGuidePage2()
    {
        var customerInfo = new CustomerInfo
        {
            sex = MaleCheckBox.IsChecked == true ? 1 : 0,
            Age = int.Parse(AgeTextBox.Text),
            Weight = double.Parse(WeightTextBox.Text),
            Height = int.Parse(HeightTextBox.Text),
            ActivityLevel = GetSelectedActivityLevel(),
            BodyFat = 0

        };


        var pageKey = typeof(Guide2ViewModel).FullName;
        if (pageKey != null)
        {
            _navigationService.NavigateTo(pageKey,customerInfo);
        }
    }
}
