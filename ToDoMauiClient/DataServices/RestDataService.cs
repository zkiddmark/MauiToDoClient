using System.Text;
using System.Text.Json;
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
    //private readonly string _baseAddress;
    //private readonly string _url;
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public RestDataService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = DeviceInfo.Platform == DevicePlatform.Android ?
            new Uri("http://10.0.2.2:5264/api/") :
            new Uri("https://localhost:7264/api/");

        //_url = $"{_baseAddress}/api";

        _jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    }

    public async Task AddTodoAsync(ToDo toDo)
    {
        if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
        {
            Console.WriteLine("----> No internet access...");
            return;
        }

        try
        {
            string jsonToDo = JsonSerializer.Serialize<ToDo>(toDo, _jsonSerializerOptions);
            StringContent content = new(jsonToDo, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync($"todo", content);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Successfully created todo");
            }
            else
            {
                Console.WriteLine("----> Non Http 2xx response");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ooops exception: {ex.Message}");
        }

    }

    public async Task DeleteTodoAsync(int id)
    {
        if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
        {
            Console.WriteLine("----> No internet access...");
            return;
        }

        try
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync($"todo/{id}");

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Successfully deleted todo");
            }
            else
            {
                Console.WriteLine("----> Non Http 2xx response");
            }
        }
        catch (Exception ex)
        {

            Console.WriteLine($"Ooops exception: {ex.Message}");
        }

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
            HttpResponseMessage response = await _httpClient.GetAsync($"todo");
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

    public async Task UpdateToDoAsync(ToDo toDo)
    {
        if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
        {
            Console.WriteLine("----> No internet access...");
            return;
        }

        try
        {
            string jsonToDo = JsonSerializer.Serialize<ToDo>(toDo, _jsonSerializerOptions);
            StringContent content = new(jsonToDo, Encoding.UTF8, "application/json");
             
            HttpResponseMessage response = await _httpClient.PutAsync($"todo/{toDo.Id}", content);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Successfully updated todo");
            }
            else
            {
                Console.WriteLine("----> Non Http 2xx response");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ooops exception: {ex.Message}");
        }
    }
}
