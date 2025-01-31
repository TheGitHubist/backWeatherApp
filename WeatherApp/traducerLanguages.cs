using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Translator {
    public class Translator {
        private const string baseUrl = "http://api.mymemory.translated.net";
        private HttpClient httpClient;
        public async Task<string> TranslateAsync(string text, string sourceLanguage, string destinationLanguage) {
            string url = $"{baseUrl}/get?q={Uri.EscapeDataString(text)}&langpair={sourceLanguage}|{destinationLanguage}";
            HttpResponseMessage response = await httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();
            var translationResult = JsonConvert.DeserializeObject<TranslationResponse>(responseBody);

            if (translationResult.ResponseStatus == 200) {
                return translationResult.TranslatedText;
            }
            return string.Empty;
        }
        public Translator() {
            httpClient = new HttpClient();
        }
    }
    public class TranslationResponse {
        [JsonProperty("responseStatus")]
        public int ResponseStatus { get; set; }

        [JsonProperty("reponseData")]
        public TranslationData ResponseData { get; set; }
        public string TranslatedText => ResponseData?.TranslatedText;
    }

    public class TranslationData {
        [JsonProperty("translatedText")]
        public string TranslatedText { get; set; }
    }
}