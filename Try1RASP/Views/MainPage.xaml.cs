using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Json;
using System.Runtime.InteropServices;
using System.Text.Json;
using Try1RASP.Models;
using Try1RASP.Services;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;
using Android.Util;
using Try1RASP.CustomControls;
using Microsoft.Maui.Controls.PlatformConfiguration;
using Colors = Microsoft.Maui.Graphics.Colors;
using Android.App;
using CommunityToolkit.Maui.Alerts;

namespace Try1RASP.Views;

public partial class MainPage : ContentPage
{
    readonly RestService restService = new();
    List<RaspisanieModel> rasp = new();
    List<Weeks> week = new();
    public MainPage()
	{
        InitializeComponent();
        try
        {
            foreach (ToggleButton btn in Choose_day_HSL)
            {
                if (btn.Text.ToString() == Preferences.Get("day", ""))
                {
                    btn.IsToggled = true;
                    if (App.Current.Resources.TryGetValue("DarkOliveGreen", out var colorvalue))
                    {
                        btn.BackgroundColor = (Color)colorvalue;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Debug.Fail(ex.ToString()); // выведет ошибку в консоль студии
        }
        try
        {
            GetCurrentWeek();
        }
        catch (Exception ex)
        {
            Debug.Fail(ex.ToString()); // выведет ошибку в консоль студии
        }
    }
	public async void GetDataFromApi(object sender, EventArgs e)
	{
        try
        {
            if(changes_Tog_btn.IsToggled == false & raspisanie_Tog_btn.IsToggled == false)
            {
                var toast = Toast.Make("Выберите Расписание или Изменения", CommunityToolkit.Maui.Core.ToastDuration.Short, 14);
                await toast.Show();
            }
            else
            {
                rasp = await restService.GETraspisanieWithChanges();
                colView.ItemsSource = rasp;
                GetCurrentWeek();
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(@"\tERROR {0}", ex.Message); // выведет ошибку в консоль студии
        }
    }

    public static bool Vision(string item)
    {
        if (item == "")
        {
            return false;
        }
        else
        {
            return true;
        }        
    }
    public async void GetCurrentWeek()
    {
        try
        {
            week = await restService.GetWeeksAsync();
            CurrentWeek.Text = week.SingleOrDefault().Week.ToString();

        }
        catch (Exception ex)
        {
            Debug.WriteLine(@"\tERROR {0}", ex.Message);
        }
    }

    private void Day_Toggled(object sender, EventArgs e)
    {
        try
        {
            Color color = new();
            Color Primary = new();
            if (App.Current.Resources.TryGetValue("Primary", out var colorvalue))
            {
                Primary = (Color)colorvalue;
            }
            if (App.Current.Resources.TryGetValue("DarkOliveGreen", out colorvalue))
            {
                color = (Color)colorvalue;
            }

            foreach (ToggleButton btn in Choose_day_HSL)
            {
                try
                {
                    btn.BackgroundColor = Primary;
                    btn.IsToggled = false;
                }
                catch(Exception ex)
                {
                    Debug.Fail(ex.ToString());
                }
            }

            if (sender is ToggleButton)
            {
                ToggleButton btn = (ToggleButton)sender;
                btn.IsToggled = true;
                if (btn.IsToggled == true)
                {
                    btn.BackgroundColor = color;
                    Preferences.Set("day", btn.Text.ToString());
                }
            }
        }
        catch(Exception ex)
        {
            Debug.Fail(ex.ToString());
        }

        
    }

    private void changes_Tog_btn_Toggled(object sender, EventArgs e)
    {
        Color Secondary = new();
        Color Primary = new();

        try
        {
            if (App.Current.Resources.TryGetValue("Primary", out var colorvalue))
            {
                Primary = (Color)colorvalue;
            }
            if (App.Current.Resources.TryGetValue("DarkOliveGreen", out colorvalue))
            {
                Secondary = (Color)colorvalue;
            }
        }
        catch (Exception ex)
        {
            Debug.Fail(ex.ToString());
        }

        try
        {
            if(sender is ToggleButton)
            {
                if(changes_Tog_btn.IsToggled == true)
                {
                    changes_Tog_btn.BackgroundColor = Secondary;
                    Preferences.Set("changes", true);
                }
                else
                {
                    changes_Tog_btn.BackgroundColor = Primary;
                    changes_Tog_btn.IsToggled = false;
                    Preferences.Set("changes", false);
                }

            }
        }
        catch(Exception ex)
        {
            Debug.Fail(ex.ToString()); 
        }
    }

    private void raspisanie_Tog_btn_Toggled(object sender, EventArgs e)
    {
        Color Secondary = new();
        Color Primary = new();
        try
        {
            if (App.Current.Resources.TryGetValue("Primary", out var colorvalue))
            {
                Primary = (Color)colorvalue;
            }
            if (App.Current.Resources.TryGetValue("DarkOliveGreen", out colorvalue))
            {
                Secondary = (Color)colorvalue;
            }
        }
        catch (Exception ex)
        {
            Debug.Fail(ex.ToString());
        }
        try
        {
            if (sender is ToggleButton)
            {
                if (raspisanie_Tog_btn.IsToggled == true)
                {
                    raspisanie_Tog_btn.BackgroundColor = Secondary;
                    Preferences.Set("raspisanie", true);
                }
                else
                {
                    raspisanie_Tog_btn.BackgroundColor = Primary;
                    raspisanie_Tog_btn.IsToggled = false;
                    Preferences.Set("raspisanie", false);
                }
            }
        }
        catch (Exception ex)
        {
            Debug.Fail(ex.ToString());
        }
    }
}

