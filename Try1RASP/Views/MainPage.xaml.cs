using CommunityToolkit.Maui.Alerts;
using System.Diagnostics;
using Try1RASP.CustomControls;
using Try1RASP.Models;
using Try1RASP.Services;

namespace Try1RASP.Views;

public partial class MainPage : ContentPage
{
    readonly RestService restService = new();
    List<RaspisanieModel> rasp = new();
    List<Weeks> week = new();
    readonly List<RaspisanieModel> plug = new()
    {
        new RaspisanieModel
        {
            Day="",
            Number=0,
            Time="",
            Lesson="НЕ НАЙДЕНО",
            Type="",
            FIO="",
            Room=""
        }
    };
    public MainPage()
    {
        InitializeComponent();
        ContentPage_LoadedAsync();

    }

    private async Task ContentPage_LoadedAsync()
    {
        Stopwatch stopwatch = new();
        stopwatch.Start();
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
            Preferences.Set("raspisanie", false);
            Preferences.Set("changes", false);
            GetCurrentWeek();
            Btn_refresh.Text = "Запросить";

            if (Preferences.Get("group", null) == null | Preferences.Get("group", null) != "Преподаватели")
            {
                var toast = Toast.Make("Перейдите в настройки и выберите группу", CommunityToolkit.Maui.Core.ToastDuration.Short, 14);
                await toast.Show();
            }
        }
        catch (Exception ex)
        {
            Debug.Fail(ex.ToString()); // выведет ошибку в консоль студии
        }

        stopwatch.Stop();
        Console.WriteLine("Время загрузки контента на страницу" + stopwatch.ElapsedMilliseconds);

    }

    public async void GetDataFromApi(object sender, EventArgs e)
	{
        Stopwatch stopwatch = new();
        stopwatch.Start();
        try
        {
            Btn_refresh.Text = "Обновить";
            if (changes_Tog_btn.IsToggled == false & raspisanie_Tog_btn.IsToggled == false)
            {
                var toast = Toast.Make("Выберите Расписание и/или Изменения", CommunityToolkit.Maui.Core.ToastDuration.Short, 14);
                await toast.Show();
            }
            else if (Preferences.Get("group", null) == null & Preferences.Get("teachers", null) != null)
            {
                var toast = Toast.Make("Вы не выбрали группу", CommunityToolkit.Maui.Core.ToastDuration.Short, 14);
                await toast.Show();
            }
            else if (Preferences.Get("day",null)==null)
            {
                var toast = Toast.Make("Вы не выбрали день", CommunityToolkit.Maui.Core.ToastDuration.Short, 14);
                await toast.Show();
            }
            else if (Preferences.Get("group", null) == null & Preferences.Get("teachers", null) != null)
            {
                var toast = Toast.Make("Вы не выбрали ФИО преподавателя", CommunityToolkit.Maui.Core.ToastDuration.Short, 14);
                await toast.Show();
            }
            else
            {
                rasp = await restService.GETraspisanieWithChanges();
                Task.WaitAny(Task.FromResult(rasp));
                if(rasp.Count>0)
                {
                    colView.ItemsSource = rasp;
                   
                    GetCurrentWeek();
                }
                else
                {
                    colView.ItemsSource = plug;
                    GetCurrentWeek();
                }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(@"\tERROR {0}", ex.Message); // выведет ошибку в консоль студии
        }

        stopwatch.Stop();
        Console.WriteLine("Время выполнения метода на проверку условий" + stopwatch.ElapsedMilliseconds);

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
        Stopwatch stopwatch = new();
        stopwatch.Start();
        try
        {
            week = await restService.GetWeeksAsync();
            CurrentWeek.Text = week.SingleOrDefault().Week.ToString();

        }
        catch (Exception ex)
        {
            Debug.WriteLine(@"\tERROR {0}", ex.Message);
        }

        stopwatch.Stop();
        Console.WriteLine("Время запроса текущей недели " + stopwatch.ElapsedMilliseconds);

    }

    private void Day_Toggled(object sender, EventArgs e)
    {
        Stopwatch stopwatch = new();
        stopwatch.Start();
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

            if (sender is ToggleButton )
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

        stopwatch.Stop();
        Console.WriteLine("Время смены дня " + stopwatch.ElapsedMilliseconds);

    }

    private void changes_Tog_btn_Toggled(object sender, EventArgs e)
    {
        Stopwatch stopwatch = new();
        stopwatch.Start();

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

        stopwatch.Stop();
        Console.WriteLine("Время нажатия на кнопку с изменениями" + stopwatch.ElapsedMilliseconds);

    }

    private void raspisanie_Tog_btn_Toggled(object sender, EventArgs e)
    {
        Stopwatch stopwatch = new();
        stopwatch.Start();
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
        stopwatch.Stop();
        Console.WriteLine("Время нажатия на кнопку с расписанием"+stopwatch.ElapsedMilliseconds);
    }
}