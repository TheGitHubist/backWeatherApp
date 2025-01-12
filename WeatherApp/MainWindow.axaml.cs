using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;
using Avalonia.Interactivity;
using System.Collections.Generic;
using Avalonia.Layout;

namespace WeatherApp
{
    public partial class MainWindow : Window
    {
        private TextBlock WeatherLabel;
        private TextBlock TemperatureLabel;
        private TextBlock HumidityLabel;
        private TextBlock CityName;
        private TextBlock Lat_long;
        private TextBox cs;
        private StackPanel ForecastPanel;
        private StackPanel para;
        private TextBlock SavedCityLabel;
        private TextBox SaveCityTextBox;
        private Button SaveCityButton;
        private Image WeatherIcon;

        private static readonly string apiKey = File.ReadAllText("api.config");

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            LoadSavedCityAndDisplayWeather();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            Lat_long = this.FindControl<TextBlock>("Lat_longa");
            CityName = this.FindControl<TextBlock>("CityNamea");
            WeatherLabel = this.FindControl<TextBlock>("WeatherLabela");
            TemperatureLabel = this.FindControl<TextBlock>("TemperatureLabela");
            HumidityLabel = this.FindControl<TextBlock>("HumidityLabela");
            cs = this.FindControl<TextBox>("csa");
            ForecastPanel = this.FindControl<StackPanel>("ForecastPanela");
            para = this.FindControl<StackPanel>("paraa");
            WeatherIcon = this.FindControl<Image>("WeatherIcona");
        }

        private void paramter(object sender, RoutedEventArgs args)
        {
            if (para.Children.Count > 0)
            {
                para.Children.Clear();
            }
            else
            {
                SavedCityLabel = new TextBlock
                {
                    Text = "No city saved yet.",
                    Margin = new Thickness(10)
                };
                para.Children.Add(SavedCityLabel);

                SaveCityTextBox = new TextBox
                {
                    Margin = new Thickness(10),
                    Width = 200
                };
                para.Children.Add(SaveCityTextBox);

                SaveCityButton = new Button
                {
                    Content = "Save City",
                    Margin = new Thickness(10)
                };
                SaveCityButton.Click += SaveCityButton_Click;
                para.Children.Add(SaveCityButton);

                DisplaySavedCity();
            }
        }

        private void SaveCityButton_Click(object sender, RoutedEventArgs args)
        {
            string cityToSave = SaveCityTextBox.Text;
            if (!string.IsNullOrEmpty(cityToSave))
            {
                File.WriteAllText("saved_city.txt", cityToSave);
                DisplaySavedCity();
            }
        }

        private void DisplaySavedCity()
        {
            if (File.Exists("saved_city.txt"))
            {
                string savedCity = File.ReadAllText("saved_city.txt");
                SavedCityLabel.Text = $"Saved City: {savedCity}";
            }
        }

        private async void LoadSavedCityAndDisplayWeather()
        {
            if (File.Exists("saved_city.txt"))
            {
                string savedCity = File.ReadAllText("saved_city.txt");

                string currentWeatherInfo = await GetWeatherDataAsync(savedCity);
                DisplayWeather(currentWeatherInfo, savedCity);

                string forecastInfo = await GetWeatherForecastAsync(savedCity);
                DisplayForecast(forecastInfo);
            }
        }

        private async void Button_Click(object sender, RoutedEventArgs args)
        {
            var citynfo = cs.Text;
            string currentWeatherInfo = await GetWeatherDataAsync(citynfo);
            DisplayWeather(currentWeatherInfo, citynfo);

            string forecastInfo = await GetWeatherForecastAsync(citynfo);
            DisplayForecast(forecastInfo);
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

        private async Task<string> GetWeatherForecastAsync(string cityName)
        {
            string url = $"https://api.openweathermap.org/data/2.5/forecast?q={cityName}&appid={apiKey}&units=metric";

            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetStringAsync(url);
                return response;
            }
        }

        private void DisplayWeather(string data, string city)
        {
            dynamic weatherData = JsonConvert.DeserializeObject(data);
            string temperature = weatherData.main.temp;
            string humidity = weatherData.main.humidity;
            string description = weatherData.weather[0].description;
            string longitude = weatherData.coord.lon;
            string latitude = weatherData.coord.lat;
            CityName.Text = $"City: {city}";
            Lat_long.Text = $"Lat/Long: {latitude}/{longitude}";
            WeatherLabel.Text = $"Weather: {description}";
            TemperatureLabel.Text = $"Temperature: {temperature}°C";
            HumidityLabel.Text = $"Humidity: {humidity}%";

            SetWeatherIcon(description);
        }

        private void SetWeatherIcon(string description)
        {
            string iconName = GetIconName(description);
            string iconPath = $"C:/Users/axelp/Desktop/School/CS/backWeatherApp/WeatherApp/img/{iconName}.png";
            if (File.Exists(iconPath))
            {
                WeatherIcon.Source = new Avalonia.Media.Imaging.Bitmap(iconPath);
            }
            else
            {
                WeatherIcon.Source = null;
            }
        }

        private void DisplayForecast(string data)
        {
            dynamic forecastData = JsonConvert.DeserializeObject(data);
            ForecastPanel.Children.Clear();

            var dailyForecasts = new Dictionary<string, List<dynamic>>();

            foreach (var item in forecastData.list)
            {
                string date = DateTime.Parse(item.dt_txt.ToString()).ToString("yyyy-MM-dd");

                if (!dailyForecasts.ContainsKey(date))
                {
                    dailyForecasts[date] = new List<dynamic>();
                }

                dailyForecasts[date].Add(item);
            }

            foreach (var day in dailyForecasts)
            {
                string date = day.Key;
                var forecasts = day.Value;

                double avgTemp = forecasts.Average(f => (double)f.main.temp);
                double minTemp = forecasts.Min(f => (double)f.main.temp);
                double maxTemp = forecasts.Max(f => (double)f.main.temp);
                string description = forecasts.GroupBy(f => (string)f.weather[0].description)
                                              .OrderByDescending(g => g.Count())
                                              .First().Key;

                var forecastStackPanel = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(10) };
                string iconName = GetIconName(description);
                string iconPath = $"C:/Users/axelp/Desktop/School/CS/backWeatherApp/WeatherApp/img/{iconName}.png";
                var weatherIcon = new Image
                {
                    Width = 10,
                    Height = 10
                };

                if (File.Exists(iconPath))
                {
                    weatherIcon.Source = new Avalonia.Media.Imaging.Bitmap(iconPath);
                }
                else
                {
                    weatherIcon.Source = null;
                }

                forecastStackPanel.Children.Add(weatherIcon);

                var textBlock = new TextBlock
                {
                    Text = $"{date}: Avg Temp: {avgTemp:F1}°C, Min Temp: {minTemp:F1}°C, Max Temp: {maxTemp:F1}°C, {description}",
                    Margin = new Thickness(10, 0, 0, 0)
                };

                forecastStackPanel.Children.Add(textBlock);

                ForecastPanel.Children.Add(forecastStackPanel);
            }
        }

        private string GetIconName(string description)
        {
            return description.ToLower() switch
            {
                "clear sky" => "sunny",
                "few clouds" => "partly_cloudy",
                "scattered clouds" => "cloudy",
                "broken clouds" => "cloudy",
                "shower rain" => "rainy",
                "rain" => "rainy",
                "thunderstorm" => "stormy",
                "snow" => "snowy",
                "mist" => "mist",
                _ => "default",
            };
        }
    }
}