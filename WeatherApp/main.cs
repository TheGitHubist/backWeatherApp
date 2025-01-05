using System;

namespace WeatherApp;

class MainC {
    public static void Main(string[] args) {
        Console.WriteLine("Hello World!");
        Console.WriteLine("Welcome to the Weather App");
        Data.getdata().Wait();
    }
}