﻿#pragma checksum "J:\Study\WindowPrograming\Gyminize_WinUi_App\Gyminize\Views\NutritionsPage.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "FFBF0081488EC84EA726E5BAA10F9BE13813EB12FB6319F9BA35B717E66BDB35"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Gyminize.Views
{
    partial class NutritionsPage : 
        global::Microsoft.UI.Xaml.Controls.Page, 
        global::Microsoft.UI.Xaml.Markup.IComponentConnector
    {

        /// <summary>
        /// Connect()
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.UI.Xaml.Markup.Compiler"," 3.0.0.2409")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 2: // Views\NutritionsPage.xaml line 9
                {
                    this.ContentArea = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.Grid>(target);
                }
                break;
            case 3: // Views\NutritionsPage.xaml line 309
                {
                    this.FoodLibrary = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.ListView>(target);
                }
                break;
            case 5: // Views\NutritionsPage.xaml line 351
                {
                    global::Microsoft.UI.Xaml.Controls.Button element5 = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.Button>(target);
                    ((global::Microsoft.UI.Xaml.Controls.Button)element5).Click += this.AddButton_Click;
                }
                break;
            case 6: // Views\NutritionsPage.xaml line 214
                {
                    this.SnackItemsControl = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.ItemsControl>(target);
                }
                break;
            case 8: // Views\NutritionsPage.xaml line 251
                {
                    global::Microsoft.UI.Xaml.Controls.Button element8 = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.Button>(target);
                    ((global::Microsoft.UI.Xaml.Controls.Button)element8).Click += this.DelButton_Click;
                }
                break;
            case 9: // Views\NutritionsPage.xaml line 158
                {
                    this.DinnerItemsControl = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.ItemsControl>(target);
                }
                break;
            case 11: // Views\NutritionsPage.xaml line 193
                {
                    global::Microsoft.UI.Xaml.Controls.Button element11 = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.Button>(target);
                    ((global::Microsoft.UI.Xaml.Controls.Button)element11).Click += this.DelButton_Click;
                }
                break;
            case 12: // Views\NutritionsPage.xaml line 100
                {
                    this.LunchItemsControl = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.ItemsControl>(target);
                }
                break;
            case 14: // Views\NutritionsPage.xaml line 137
                {
                    global::Microsoft.UI.Xaml.Controls.Button element14 = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.Button>(target);
                    ((global::Microsoft.UI.Xaml.Controls.Button)element14).Click += this.DelButton_Click;
                }
                break;
            case 15: // Views\NutritionsPage.xaml line 42
                {
                    this.BreakfastItemsControl = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.ItemsControl>(target);
                }
                break;
            case 17: // Views\NutritionsPage.xaml line 79
                {
                    global::Microsoft.UI.Xaml.Controls.Button element17 = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.Button>(target);
                    ((global::Microsoft.UI.Xaml.Controls.Button)element17).Click += this.DelButton_Click;
                }
                break;
            default:
                break;
            }
            this._contentLoaded = true;
        }


        /// <summary>
        /// GetBindingConnector(int connectionId, object target)
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.UI.Xaml.Markup.Compiler"," 3.0.0.2409")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::Microsoft.UI.Xaml.Markup.IComponentConnector GetBindingConnector(int connectionId, object target)
        {
            global::Microsoft.UI.Xaml.Markup.IComponentConnector returnValue = null;
            return returnValue;
        }
    }
}

