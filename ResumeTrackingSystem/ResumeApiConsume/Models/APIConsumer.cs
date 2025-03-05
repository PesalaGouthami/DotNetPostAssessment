using System.Net.Http.Headers;
using System.Text.Json;

namespace ResumeApiConsume.Models
{
    public class APIConsumer
    {
        static string baseUrl = "http://localhost:5190/api/Home";
        public static List<Employee> GetEmps()
        {
                var lstEmps = new List<Employee>();
                using (var http = new HttpClient())
                {
                    http.BaseAddress = new Uri(baseUrl);
                   // http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var response = http.GetStringAsync("");
                    response.Wait();
                    if (response.IsCompletedSuccessfully)
                    {
                        var data = response.Result;
                        lstEmps = JsonSerializer.Deserialize<List<Employee>>(data);
                    }
                    else
                    {
                        throw new Exception(response.Exception.Message);
                    }
                }
                return lstEmps;
            }

        public static string AddEmp(Employee emp)
        {
            using (var http = new HttpClient())
            {
                http.BaseAddress = new Uri(baseUrl);
               // http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var task = http.PostAsJsonAsync<Employee>("", emp);
                task.Wait();
                if (task.IsCompletedSuccessfully)
                {
                    var response = task.Result;
                    if (response.IsSuccessStatusCode)
                    {
                        var responseRead = response.Content.ReadAsStringAsync();
                        responseRead.Wait();
                        var msg = responseRead.Result;
                        return msg;
                    }
                    else
                    {
                        return "could not insert record";
                    }
                }
                else
                {
                    return "Request Failed";
                }
            }
        }
        public static Employee GetEmpById(int id)
        {
            var lstEmps = new Employee();
            //call the API GetAll
            using (var http = new HttpClient())
            {
                http.BaseAddress = new Uri(baseUrl);
                var response = http.GetStringAsync($"{id}");
                response.Wait();
                if (response.IsCompletedSuccessfully)
                {
                    var data = response.Result;
                    lstEmps = JsonSerializer.Deserialize<Employee>(data);
                    return lstEmps;
                }
                else
                {
                    throw new Exception(response.Exception.Message);
                }
            }
        }
        public static bool UpdateEmp(Employee emp)
        {
            using (var http = new HttpClient())
            {
                http.BaseAddress = new Uri(baseUrl);
                var task = http.PutAsJsonAsync<Employee>($"UpdateEmp/{emp.EmployeeId}", emp);
                task.Wait();
                if (task.IsCompletedSuccessfully)
                {
                    var response = task.Result;
                    if (response.IsSuccessStatusCode)
                    {
                        var responseRead = response.Content.ReadAsStringAsync();
                        responseRead.Wait();
                        var msg = responseRead.Result;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }
        public static bool Delete(int id)
        {
            //call the API GetAll
            using (var http = new HttpClient())
            {
                http.BaseAddress = new Uri(baseUrl);
                var response = http.DeleteAsync($"{id}");
                response.Wait();
                if (response.IsCompletedSuccessfully)
                {
                    //var data = response.Result;
                    //JsonSerializer.Deserialize<Employee>(data);
                    return true;
                }
                else
                {
                    throw new Exception(response.Exception.Message);
                }
            }
            return false; ;
        }
    }
}
