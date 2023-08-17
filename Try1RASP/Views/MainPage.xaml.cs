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

        GetCurrentWeek();

    }
	public async void GetDataFromApi(object sender, EventArgs e)
	{
        try
        {
          rasp = await restService.GETraspisanieWithChanges();
        }
        catch (Exception ex)
        {
            Debug.Fail(ex.ToString()); // выведет ошибку в консоль студии
        }
        colView.ItemsSource = rasp;
        GetCurrentWeek();

    }

    public static bool Vision(string item)
    {
        if(item == "")
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
        }
        catch (Exception ex)
        {
            Debug.Fail(ex.ToString()); // выведет ошибку в консоль студии
        }
        CurrentWeek.Text = week.ToString();
    }

    private void Day_Toggled(object sender, EventArgs e)
    {
        Color color = new();
        Color Primary = new();
        if (App.Current.Resources.TryGetValue("Primary", out var colorvalue))
        {

            Primary = (Color)colorvalue;

        };

        if (App.Current.Resources.TryGetValue("DarkOliveGreen", out colorvalue))
        {

            color = (Color)colorvalue;

        }

        Monday.BackgroundColor = Primary;
        Monday.IsToggled = false;
        Tuesday.BackgroundColor = Primary;
        Tuesday.IsToggled = false;
        Wednesday.BackgroundColor = Primary;
        Wednesday.IsToggled = false;
        Thursday.BackgroundColor = Primary;
        Thursday.IsToggled = false; 
        Friday.BackgroundColor = Primary;
        Friday.IsToggled = false; 
        Saturday.BackgroundColor = Primary;
        Saturday.IsToggled = false; 
        Sunday.BackgroundColor  = Primary;
        Sunday.IsToggled = false;

        if (sender is ToggleButton)
        {
            ToggleButton btn = (ToggleButton)sender;
            btn.IsToggled=true;
            if (btn.IsToggled==true)
            {

                btn.BackgroundColor = color;
                Preferences.Set("day", btn.Text.ToString());
            }
        }
    }
}

