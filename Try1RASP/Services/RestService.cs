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

namespace Try1RASP.Services
{
    public class RestService
    {
        HttpClient _client;
        JsonSerializerOptions _serializerOptions;
        List<RaspisanieModel> raspisanie = new();
        List<RaspisanieModel> rasp = new();



        public RestService()
        {
            _client = new HttpClient();
            _serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
        }
        public async Task<List<RaspisanieModel>> RefreshDataAsync()
        {
            var group = "15С";
            var day = "Четверг";

            try
            {
                HttpResponseMessage response =  await _client.GetAsync("http://10.0.2.2:8765/api/Mobile/" + group + "/" + day, HttpCompletionOption.ResponseHeadersRead);
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
    }
}
