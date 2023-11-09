using System.Diagnostics;
using Try1RASP.Models;
using Try1RASP.Services;

namespace Try1RASP.Views;

public partial class SettingsPage : ContentPage
{
    readonly RestService restService = new();
    List<Groups> groups = new();
    List<Teachers> teachers = new();

    public SettingsPage()
    {
        InitializeComponent();
        ContentPage_Loaded();
        
    }
    private async void ContentPage_Loaded()
    {
        Stopwatch stopwatch = new();
        stopwatch.Start();
        try
        {
            groups = await restService.GetGroupsAsync();
            Choose_group_picker.ItemsSource = groups;

            Choose_teacher_picker.Title = Preferences.Get("teacher_fio", "Выберите ФИО");
            if(Preferences.Get("group",null)=="Преподаватели")
            {
                You_teacher.IsVisible = true;
                Choose_teacher_picker.IsVisible = true;
            }
            else
            {
                You_teacher.IsVisible = false;
                Choose_teacher_picker.IsVisible = false;
            }
            
            teachers = await restService.GetTeachersAsync();
            Choose_teacher_picker.ItemsSource = teachers;

            Choose_group_picker.Title = Preferences.Get("group", "Выберите группу");
        }
        catch (Exception ex)
        {
            Debug.Fail(ex.ToString());
        }
        stopwatch.Stop();
        Console.WriteLine("Время загрузки контента на страницу настроек " + stopwatch.ElapsedMilliseconds);
    }

    private void Choose_group_picker_SelectedIndexChanged(object sender, EventArgs e)
    {
        Stopwatch stopwatch = new();
        stopwatch.Start();
        try
        {
            if (Choose_group_picker.Items[Choose_group_picker.SelectedIndex].ToString() == "Преподаватели")
            {
                You_teacher.IsVisible = true;
                Choose_teacher_picker.IsVisible = true;

                Preferences.Set("group", Choose_group_picker.Items[Choose_group_picker.SelectedIndex].ToString());
                Preferences.Set("teacher", null);
                Preferences.Set("teacger_fio", null);

                Choose_teacher_picker.Title = Preferences.Get("teacher_fio","Выберите ФИО");
                Choose_group_picker.Title = Choose_group_picker.Items[Choose_group_picker.SelectedIndex].ToString();
            }
            else
            {

                Preferences.Set("group", Choose_group_picker.Items[Choose_group_picker.SelectedIndex].ToString());
                
                Preferences.Set("teacher_fio",null);
                Preferences.Set("teacher", null);

                You_teacher.IsVisible= false;
                Choose_teacher_picker.IsVisible = false;
                
            }
        }
        catch (Exception ex)
        {
            Debug.Fail(ex.ToString());
        }
        stopwatch.Stop();
        Console.WriteLine("Время выполнения при смене группы " + stopwatch.ElapsedMilliseconds);
    }
    private void Choose_teacher_picker_SelectedIndexChanged(object sender, EventArgs e)
    {
        Stopwatch stopwatch = new();
        stopwatch.Start();
        try
        {
            Preferences.Set("teacher", teachers.Find(item => item.FIO == Choose_teacher_picker.Items[Choose_teacher_picker.SelectedIndex]).guid.ToString());
            Preferences.Set("teacher_fio",teachers.Find(item => item.FIO == Choose_teacher_picker.Items[Choose_teacher_picker.SelectedIndex]).FIO.ToString());
            Choose_teacher_picker.Title = Preferences.Get("teacher_fio","Выберите ФИО");
            Preferences.Set("group",null);
        }
        catch (Exception ex)
        {
            Debug.Fail(ex.ToString());
        }

        stopwatch.Stop();
        Console.WriteLine("Время выполнения при смене ФИО " + stopwatch.ElapsedMilliseconds);
    }
}