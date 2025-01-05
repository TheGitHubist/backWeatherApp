using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;

namespace WeatherApp
{
    public partial class MainWindow   : Window
    {
        private TextBlock WeatherLabel;
        private TextBlock TemperatureLabel;
        private TextBlock HumidityLabel;
        private Button FetchWeatherButton;

        private static readonly string apiKey = File.ReadAllText("api.config");  
        private static readonly string city = "London"; 

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            WeatherLabel = this.FindControl<TextBlock>("WeatherLabela");
            TemperatureLabel = this.FindControl<TextBlock>("TemperatureLabela");
            HumidityLabel = this.FindControl<TextBlock>("HumidityLabela");
            FetchWeatherButton = this.FindControl<Button>("FetchWeatherButtona");

            FetchWeatherButton.Click += FetchWeatherButton_Click;
        }

        private async void FetchWeatherButton_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            string weatherInfo = await GetWeatherDataAsync(city);
            DisplayWeather(weatherInfo);
        }

        private async Task<string> GetWeatherDataAsync(string cityName)
        {
            string url = $"https://api.openweathermap.org/data/2.5/weather?q={cityName}&appid={apiKey}&units=metric";  

            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetStringAsync(url);
                return response;
            }
        }

        private void DisplayWeather(string data)
        {
            dynamic weatherData = JsonConvert.DeserializeObject(data);

            string temperature = weatherData.main.temp;
            string humidity = weatherData.main.humidity;
            string description = weatherData.weather[0].description;

            WeatherLabel.Text = $"Weather: {description}";
            TemperatureLabel.Text = $"Temperature: {temperature}°C";
            HumidityLabel.Text = $"Humidity: {humidity}%";
        }
    }
}
