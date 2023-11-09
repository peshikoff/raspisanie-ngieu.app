using System.Diagnostics;
using System.Text.Json;
using Try1RASP.Models;

namespace Try1RASP.Services
{
    public class RestService
    {
        readonly HttpClient _client;
        readonly JsonSerializerOptions _serializerOptions;
        public RestService()
        {
            _client = new HttpClient();
            _serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
        }
        List<RaspisanieModel> rasp = new();
        List<Groups> groups = new();
        List<Weeks> week = new();
        List<Teachers> teachers = new();

        readonly string raspisanieURI = "http://10.0.2.2:8765/api/Mobile/raspisanie/";
        readonly string changesURI =    "http://10.0.2.2:8765/api/Mobile/changes/";
        readonly string supportURI =    "http://10.0.2.2:8765/api/Support/";

        


        public async Task<List<RaspisanieModel>> GETraspisanieWithChanges()
        {
            Stopwatch stopwatch = new();
            stopwatch.Start();
            var teacher       = Preferences.Get("teacher", null);
            var group         = Preferences.Get("group","Преподаватели");
            var day           = Preferences.Get("day", null);
            bool raspisanie   = Preferences.Get("raspisanie", false);
            bool changes      = Preferences.Get("changes", false);
            List<RaspisanieModel> rasp = new();
            
            if (group !=null & teacher==null)
            {
                try
                {
                    if(raspisanie==true & changes==true)
                    {
                        //Запрос расписания с изменениями raspisanie_changes_events
                        HttpResponseMessage response = await _client
                            .GetAsync(raspisanieURI+group+"/"+day+"/group_all",HttpCompletionOption.ResponseHeadersRead);

                        if (response.IsSuccessStatusCode)
                        {
                            string json1 = await response.Content.ReadAsStringAsync();
                            rasp = JsonSerializer.Deserialize<List<RaspisanieModel>>(json1);
                        }
                    }
                    else if(raspisanie==false & changes == true)
                    {
                        try
                        {
                            //Запрос изменений changes/15С/Четверг/group
                            HttpResponseMessage response = await _client
                                .GetAsync(changesURI + group + "/" + day+"/group", HttpCompletionOption.ResponseHeadersRead);

                            if (response.IsSuccessStatusCode)
                            {
                                string json1 = await response.Content.ReadAsStringAsync();
                                rasp = JsonSerializer.Deserialize<List<RaspisanieModel>>(json1);
                            }
                        }
                        catch (Exception ex) { Debug.WriteLine(@"\tERROR {0}", ex.Message); }
                    }
                    else if(raspisanie==true & changes== false)
                    {
                        try
                        {
                            //Запрос расписания raspisanie/15С/Четверг/group
                            HttpResponseMessage response = await _client
                                .GetAsync(raspisanieURI + group + "/" + day + "/group", HttpCompletionOption.ResponseHeadersRead);

                            if (response.IsSuccessStatusCode)
                            {
                                string json1 = await response.Content.ReadAsStringAsync();
                                rasp = JsonSerializer.Deserialize<List<RaspisanieModel>>(json1);
                            }

                        }
                        catch (Exception ex) { Debug.WriteLine(@"\tERROR {0}", ex.Message); }
                    }
                }
                catch (Exception ex) { Debug.WriteLine(@"\tERROR {0}", ex.Message); }
            }
            else if (group=="Преподаватели" & teacher !=null)
            {
                try
                {
                    if (raspisanie == true & changes == true)
                    {
                        //Запрос расписания с изменениями api/Mobile/raspisanie/61b5f61b-d182-4bce-be1c-fb641933b738/Пятница/teacher_all
                        HttpResponseMessage response = await _client
                            .GetAsync(raspisanieURI + teacher + "/" + day + "/teacher_all",HttpCompletionOption.ResponseHeadersRead);

                        if (response.IsSuccessStatusCode)
                        {
                            string json1 = await response.Content.ReadAsStringAsync();
                            rasp = JsonSerializer.Deserialize<List<RaspisanieModel>>(json1);
                        }
                    }
                    else if (raspisanie == false & changes == true)
                    {
                        try
                        {
                            //Запрос изменений /61b5f61b-d182-4bce-be1c-fb641933b738/Пятница/teacher
                            string link = changesURI + teacher + "/" + day + "/teacher";
                            HttpResponseMessage response = await _client
                                .GetAsync(link, HttpCompletionOption.ResponseHeadersRead);

                            if (response.IsSuccessStatusCode)
                            {
                                string json1 = await response.Content.ReadAsStringAsync();
                                rasp = JsonSerializer.Deserialize<List<RaspisanieModel>>(json1);
                            }
                        }
                        catch (Exception ex) { Debug.WriteLine(@"\tERROR {0}", ex.Message); }
                    }
                    else if (raspisanie == true & changes == false)
                    {
                        try
                        {
                            //Запрос расписания /61b5f61b-d182-4bce-be1c-fb641933b738/Четверг/teacher

                            HttpResponseMessage response = await _client.
                            GetAsync(raspisanieURI + teacher + "/" + day + "/teacher", HttpCompletionOption.ResponseHeadersRead);

                            if (response.IsSuccessStatusCode)
                            {
                                string json1 = await response.Content.ReadAsStringAsync();
                                rasp = JsonSerializer.Deserialize<List<RaspisanieModel>>(json1);
                            }
                        }
                        catch (Exception ex) { Debug.WriteLine(@"\tERROR {0}", ex.Message); }
                    }
                }
                catch (Exception ex) { Debug.WriteLine(@"\tERROR {0}", ex.Message); }
            }

            if(group!="Преподаватели")
            {
                rasp.ForEach((rasp) =>
                {
                    rasp.Group = null;

                });
            }
            stopwatch.Stop();
            Console.WriteLine("Время вывода расписания"+stopwatch.ElapsedMilliseconds);
            return rasp;
        }

        public async Task<List<Groups>> GetGroupsAsync()
        {
            Stopwatch stopwatch = new();
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
            stopwatch.Stop();
            Console.WriteLine("Время выполнения на список групп "+stopwatch.ElapsedMilliseconds);
            return groups;
        }
        public async Task<List<Weeks>> GetWeeksAsync()
        {
            Stopwatch stopwatch = new();
            stopwatch.Start();
            try
            {
                HttpResponseMessage response = await _client
                    .GetAsync(supportURI+"GetCurrentWeek", HttpCompletionOption.ResponseHeadersRead);
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
            stopwatch.Stop();
            Console.WriteLine("Время выполнения на текущую неделю "+stopwatch.ElapsedMilliseconds);
            return week;
        }
        public async Task<List<Teachers>> GetTeachersAsync()
        {
            Stopwatch stopwatch = new();
            stopwatch.Start();
            try
            {
                HttpResponseMessage response = await _client
                    .GetAsync(supportURI + "GetTeachers", HttpCompletionOption.ResponseHeadersRead);
                if (response.IsSuccessStatusCode)
                {
                    string json1 = await response.Content.ReadAsStringAsync();
                    teachers = JsonSerializer.Deserialize<List<Teachers>>(json1);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }
            finally
            {

            }
            stopwatch.Stop();
            Console.WriteLine("Время выполнения на список учителей "+stopwatch.ElapsedMilliseconds);
            return teachers;
        }

    }
}
