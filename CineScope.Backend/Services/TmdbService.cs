using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

public class TmdbService
{
    private readonly HttpClient _http;
    private readonly string _apiKey;

    public TmdbService(HttpClient http, IConfiguration config)
    {
        _http = http;
        _apiKey = config["TMDB:ApiKey"];
    }

    public async Task<string?> GetPosterUrlAsync(string title)
    {
        var url = $"https://api.themoviedb.org/3/search/movie?api_key={_apiKey}&query={Uri.EscapeDataString(title)}";

        var response = await _http.GetStringAsync(url);
        var json = JObject.Parse(response);

        var first = json["results"]?.FirstOrDefault();
        if (first == null)
            return null;

        var posterPath = first["poster_path"]?.ToString();
        if (string.IsNullOrEmpty(posterPath))
            return null;

        return $"https://image.tmdb.org/t/p/w500{posterPath}";
    }
}
