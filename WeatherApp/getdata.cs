using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.IO;

namespace WeatherApp;

class Data
{
    private static readonly HttpClient client = new HttpClient();

    public static async Task getdata()
    {
        Console.WriteLine("Weather Data:");
        string apiKey = File.ReadAllText("api.config"); 
        string city = "London";
        string url = $"http://api.openweathermap.org/data/2.5/weather?q={city}&appid={apiKey}";

        try
        {
            HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            JObject weatherData = JObject.Parse(responseBody);

            File.WriteAllText("dataresult.json", weatherData.ToString());
            Console.WriteLine("Weather Data:");
            Console.WriteLine(weatherData.ToString());
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine("\nException Caught!");
            Console.WriteLine("Message :{0} ", e.Message);
        }
    }
}