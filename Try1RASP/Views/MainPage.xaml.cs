using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Json;
using System.Text.Json;
using Try1RASP.Models;
using Try1RASP.Services;

namespace Try1RASP.Views;

public partial class MainPage : ContentPage
{
    RestService restService = new RestService();
    List<RaspisanieModel> rasp = new();
    public MainPage()
	{

        InitializeComponent();
	}
	public async void GetDataFromApi(object sender, EventArgs e)
	{
        try
        {
          rasp = await restService.RefreshDataAsync();
        }
        catch (Exception ex)
        {
            Debug.Fail(ex.ToString()); // выведет ошибку в консоль студии
        }
        colView.ItemsSource = rasp;

    }    

}

