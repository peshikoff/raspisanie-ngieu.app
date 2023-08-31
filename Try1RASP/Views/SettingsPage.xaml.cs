using System.Diagnostics;
using Try1RASP;
using Try1RASP.Models;
using Try1RASP.Services;

namespace Try1RASP.Views;

public partial class SettingsPage : ContentPage
{
    readonly RestService restService = new();
    List<Groups> groups = new();

	public SettingsPage()
	{
		InitializeComponent();
		ContentPage_Loaded();
	}
    private async void ContentPage_Loaded()
    {
        try
        {
            groups = await restService.GetGroupsAsync();
            Choose_group_picker.ItemsSource = groups;
            Choose_group_picker.Title = Preferences.Get("group", "Выберите группу");
        }
        catch(Exception ex) 
        {
            Debug.Fail(ex.ToString());
        }



    }

    private void Choose_group_picker_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Preferences.Set("group", Choose_group_picker.Items[Choose_group_picker.SelectedIndex]);
        }
        catch (Exception ex)
        {
            Debug.Fail(ex.ToString());
        }
    }
}