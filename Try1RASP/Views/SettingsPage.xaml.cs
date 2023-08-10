using System.Diagnostics;
using Try1RASP;
using Try1RASP.Models;
using Try1RASP.Services;

namespace Try1RASP.Views;

public partial class SettingsPage : ContentPage
{
    RestService restService = new RestService();
    List<Groups> groups = new();

	public SettingsPage()
	{
		InitializeComponent();
		ContentPage_Loaded();
	}
    private async void ContentPage_Loaded()
    {
		groups = await restService.GetGroupsAsync();
		Choose_group_picker.ItemsSource = groups;

    }

    private void Choose_group_picker_SelectedIndexChanged(object sender, EventArgs e)
    {
        Preferences.Set("group", Choose_group_picker.Items[Choose_group_picker.SelectedIndex]);
    }
}