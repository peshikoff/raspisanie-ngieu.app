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

namespace Try1RASP.Views;

public partial class MainPage : ContentPage
{
    RestService restService = new();
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
          rasp = await restService.GETraspisanieWithChanges();
          colView.ItemsSource = rasp;
          GetCurrentWeek();
        }
        catch (Exception ex)
        {
            Debug.Fail(ex.ToString()); // выведет ошибку в консоль студии
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
            CurrentWeek.Text = week.FirstOrDefault().ToString();
        }
        catch (Exception ex)
        {
            Debug.Fail(ex.ToString()); // выведет ошибку в консоль студии
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
}

