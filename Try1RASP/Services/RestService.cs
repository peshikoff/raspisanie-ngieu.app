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

namespace Try1RASP.Services
{
    public class RestService
    {
        HttpClient _client;
        JsonSerializerOptions _serializerOptions;
        List<RaspisanieModel> raspisanie = new();
        List<RaspisanieModel> rasp = new();
        List<Groups> groups = new();
        List<Weeks> week = new();



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
            var group = Preferences.Get("group","");
            var day = "Понедельник";


            
                try
                {
                    HttpResponseMessage response = await _client.GetAsync("http://10.0.2.2:8765/api/Mobile/raspisanie/" + group + "/" + day, HttpCompletionOption.ResponseHeadersRead);
                    if (response.IsSuccessStatusCode)
                    {
                        string json1 = await response.Content.ReadAsStringAsync();
                        //raspisanie = await response.Content.ReadFromJsonAsync<List<RaspisanieModel>>();
                        rasp = JsonSerializer.Deserialize<List<RaspisanieModel>>(json1);

                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(@"\tERROR {0}", ex.Message);

                }
                finally
                {

                }
            
            return rasp;
        }

        public async Task<List<Groups>> GetGroupsAsync()
        {
            try
            {
                HttpResponseMessage response = await _client.GetAsync("http://10.0.2.2:8765/api/Support/getGroups", HttpCompletionOption.ResponseHeadersRead);
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
            finally
            {

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
