using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace WeatherApp;

class Data
{
    private static readonly HttpClient client = new HttpClient();

    public static async Task getdata()
    {
        Console.WriteLine("Weather Data:");
        string apiKey = "5d48ef08478f4bd2006695864a9e3434";
        string city = "London";
        string url = $"http://api.openweathermap.org/data/2.5/weather?q={city}&appid={apiKey}";

        try
        {
            HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            JObject weatherData = JObject.Parse(responseBody);

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