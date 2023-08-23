using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Try1RASP.Models;
using System.Net.Http.Json;
using System.Text.Json.Nodes;
using Try1RASP.Views;
using Try1RASP.CustomControls;
using static Android.Telephony.CarrierConfigManager;

namespace Try1RASP.Services
{
    public class RestService
    {
        readonly HttpClient _client;
        readonly JsonSerializerOptions _serializerOptions;
        List<RaspisanieModel> rasp = new();
        List<Groups> groups = new();
        List<Weeks> week = new();
        readonly string raspisanieURI = "http://10.0.2.2:8765/api/Mobile/raspisanie/";
        readonly string changesURI =    "http://10.0.2.2:8765/api/Mobile/changes/";
        readonly string supportURI =    "http://10.0.2.2:8765/api/Support/";
        
        public RestService()
        {
            _client = new HttpClient();
            _serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
        }
        public async Task<List<RaspisanieModel>> GETraspisanieWithChanges()
        {
            var group         = Preferences.Get("group","");
            var day           = Preferences.Get("day", "");
            bool raspisanie   = Preferences.Get("raspisanie", false);
            bool changes      = Preferences.Get("changes", false);


            if (raspisanie == true & changes == true)
            {
                try
                {
                    //Запрос расписания с изменениями
                    HttpResponseMessage response = await _client
                        .GetAsync(raspisanieURI + group + "/" + day + "/2", HttpCompletionOption.ResponseHeadersRead);

                    if (response.IsSuccessStatusCode)
                    {
                        string json1 = await response.Content.ReadAsStringAsync();
                        rasp = JsonSerializer.Deserialize<List<RaspisanieModel>>(json1);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(@"\tERROR {0}", ex.Message);
                }
            }
            else if (raspisanie == true & changes == false)
            {
                try
                {
                    //Запрос расписания без изменений
                    
                    HttpResponseMessage response = await _client
                        .GetAsync(raspisanieURI + group + "/" + day + "/1", HttpCompletionOption.ResponseHeadersRead);
                    
                    if (response.IsSuccessStatusCode)
                    {
                        string json1 = await response.Content.ReadAsStringAsync();
                        rasp = JsonSerializer.Deserialize<List<RaspisanieModel>>(json1);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(@"\tERROR {0}", ex.Message);
                }
            }
            else if (raspisanie == false &changes == true)
            {
                try
                {
                    //Запрос изменений
                    HttpResponseMessage response = await _client
                        .GetAsync(changesURI + group + "/" + day, HttpCompletionOption.ResponseHeadersRead);

                    if (response.IsSuccessStatusCode)
                    {
                        string json1 = await response.Content.ReadAsStringAsync();
                        rasp = JsonSerializer.Deserialize<List<RaspisanieModel>>(json1);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(@"\tERROR {0}", ex.Message);
                }
            }
            else if (raspisanie == false & changes == false)
            {
                //DependencyService.Get<MessageAndroid>().ShortAlert("Выберите Расписание/Изменение или комбинацию");
            }


            return rasp;
        }
        public async Task<List<Groups>> GetGroupsAsync()
        {
            try
            {
                HttpResponseMessage response = await _client
                    .GetAsync(supportURI+"getGroups", HttpCompletionOption.ResponseHeadersRead);
                if (response.IsSuccessStatusCode)
                {
                    string json1 = await response.Content.ReadAsStringAsync();
                    groups = JsonSerializer.Deserialize<List<Groups>>(json1);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);

            }
            return groups;
        }
        public async Task<List<Weeks>> GetWeeksAsync()
        {
            try
            {
                HttpResponseMessage response = await _client.GetAsync("http://10.0.0.2:8765/api/Support/GetCurrentWeek", HttpCompletionOption.ResponseHeadersRead);
                if (response.IsSuccessStatusCode)
                {
                    string json1 = await response.Content.ReadAsStringAsync();
                    week = JsonSerializer.Deserialize<List<Weeks>>(json1);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }
            finally
            {

            }
            return week;
        }
    }
}
