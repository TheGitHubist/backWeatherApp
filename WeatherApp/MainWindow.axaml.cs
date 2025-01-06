using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;
using Avalonia.Interactivity;
using System.Diagnostics;

namespace WeatherApp
{
    public partial class MainWindow   : Window
    {
        private TextBlock WeatherLabel;
        private TextBlock TemperatureLabel;
        private TextBlock HumidityLabel;
        private Button FetchWeatherButton;

        private TextBlock CityName;

        private TextBlock Lat_long;

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

            Lat_long = this.FindControl<TextBlock>("Lat_longa");
            CityName = this.FindControl<TextBlock>("CityNamea");
            WeatherLabel = this.FindControl<TextBlock>("WeatherLabela");
            TemperatureLabel = this.FindControl<TextBlock>("TemperatureLabela");
            HumidityLabel = this.FindControl<TextBlock>("HumidityLabela");
            FetchWeatherButton = this.FindControl<Button>("FetchWeatherButtona");

            FetchWeatherButton.Click += Button_Click;
        }

        private async void Button_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {   
            string citynfo = InputTextBox.Text;
            File.WriteAllText("kjbv.txt", citynfo);
            //string weatherInfo = await GetWeatherDataAsync(citynfo);
            //DisplayWeather(weatherInfo);
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
            string longitude = weatherData.coord.lon;
            string latitude = weatherData.coord.lat;
            CityName.Text = $"city : {city}";
            Lat_long.Text = $"lat/long : {latitude}/{longitude}";
            WeatherLabel.Text = $"Weather: {description}";
            TemperatureLabel.Text = $"Temperature: {temperature}°C";
            HumidityLabel.Text = $"Humidity: {humidity}%";
        }
    }
}
