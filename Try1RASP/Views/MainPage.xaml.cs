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

using static Try1RASP.App;

using System.Drawing;

namespace Try1RASP.Views;

public partial class MainPage : ContentPage
{
    RestService restService = new();
    List<RaspisanieModel> rasp = new();
    List<Weeks> week = new();
    public MainPage()
	{

        InitializeComponent();
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

    private void Day_Clicked(object sender, EventArgs e)
    {
        Monday.BackgroundColor = (Microsoft.Maui.Graphics.Color)Application.Current.Resources["AlcoRubin"];

        if(sender is Button)
        {
            Button btn = (Button)sender;
            btn.BackgroundColor = Colors.Orange;
        }
    }
}

