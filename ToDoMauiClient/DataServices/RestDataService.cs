using Android.OS;
using AndroidX.ConstraintLayout.Motion.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ToDoMauiClient.Models;

namespace ToDoMauiClient.DataServices;

public interface IRestDataService
{
    Task<List<ToDo>> GetAllToDosAsync();
    Task AddTodoAsync(ToDo toDo);
    Task UpdateToDoAsync(ToDo toDo);
    Task DeleteTodoAsync(int id);
}
public class RestDataService : IRestDataService
{
    private readonly HttpClient _httpClient;
    private readonly string _baseAddress;
    private readonly string _url;
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public RestDataService()
    {
        _httpClient = new HttpClient();
        _baseAddress = DeviceInfo.Platform == DevicePlatform.Android ?
            "http://10.0.2.2:5264" :
            "https://localhost:7264";
        _url = $"{_baseAddress}/api";

        _jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    }

    public Task AddTodoAsync(ToDo toDo)
    {
        throw new NotImplementedException();
    }

    public Task DeleteTodoAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<List<ToDo>> GetAllToDosAsync()
    {
        List<ToDo> todos = new List<ToDo>();

        if(Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
        {
            Console.WriteLine("----> No internet access...");
            return todos;
        }

        try
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"{_url}/todo");
            if(response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                todos = JsonSerializer.Deserialize<List<ToDo>>(content, _jsonSerializerOptions);
            }
            else
            {
                Console.WriteLine("----> Non Http 2xx response");
            }
        }
        catch(Exception ex)
        {
            Console.WriteLine($"Ooops exception: {ex.Message}");

        }

        return todos;

    }

    public Task UpdateToDoAsync(ToDo toDo)
    {
        throw new NotImplementedException();
    }
}
