using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace WeatherApp
{
    public class MainWindow   : Window
    {
        private TextBlock WeatherLabel;
        private TextBlock TemperatureLabel;
        private TextBlock HumidityLabel;
        private Button FetchWeatherButton;

        private static readonly string apiKey = "YOUR_API_KEY";  // Replace with your OpenWeatherMap API key
        private static readonly string city = "London"; // Change to any city of your choice

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            WeatherLabel = this.FindControl<TextBlock>("WeatherLabel");
            TemperatureLabel = this.FindControl<TextBlock>("TemperatureLabel");
            HumidityLabel = this.FindControl<TextBlock>("HumidityLabel");
            FetchWeatherButton = this.FindControl<Button>("FetchWeatherButton");

            FetchWeatherButton.Click += FetchWeatherButton_Click;
        }

        private async void FetchWeatherButton_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            string weatherInfo = await GetWeatherDataAsync(city);
            DisplayWeather(weatherInfo);
        }

        private async Task<string> GetWeatherDataAsync(string cityName)
        {
            string url = $"https://api.openweathermap.org/data/2.5/weather?q={cityName}&appid={apiKey}&units=metric";  // Use metric for Celsius

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
