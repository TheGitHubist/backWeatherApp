using System;
using Translator;

naemspace main {
    public static class Program {
        public static void Main(string[] args) {
            TranslationString translation = "Sentence I will traduce"
            UserSingleton.CurrentUser.Locale = "nl-NL";
            Console.WriteLine(translation); 
        }
    }
}