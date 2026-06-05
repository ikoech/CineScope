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

    // ⭐ Fetch main poster (used in Create/Edit)
    public async Task<string?> GetPosterUrlAsync(string title)
    {
        var url =
            $"https://api.themoviedb.org/3/search/movie?api_key={_apiKey}&query={Uri.EscapeDataString(title)}";

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

    // ⭐ Fetch extra posters + backdrops (fast & easy version)
    public async Task<List<string>> GetExtraImagesAsync(string title)
    {
        var results = new List<string>();

        // 1. Search movie
        var searchUrl =
            $"https://api.themoviedb.org/3/search/movie?api_key={_apiKey}&query={Uri.EscapeDataString(title)}";

        var searchResponse = await _http.GetStringAsync(searchUrl);
        var searchJson = JObject.Parse(searchResponse);

        var first = searchJson["results"]?.FirstOrDefault();
        if (first == null)
            return results;

        int movieId = (int)first["id"];

        // 2. Fetch images
        var imagesUrl =
            $"https://api.themoviedb.org/3/movie/{movieId}/images?api_key={_apiKey}";

        var imagesResponse = await _http.GetStringAsync(imagesUrl);
        var imagesJson = JObject.Parse(imagesResponse);

        // Posters (limit to 6)
        foreach (var p in imagesJson["posters"].Take(6))
            results.Add("https://image.tmdb.org/t/p/w500" + p["file_path"]);

        // Backdrops (limit to 6)
        foreach (var b in imagesJson["backdrops"].Take(6))
            results.Add("https://image.tmdb.org/t/p/w780" + b["file_path"]);

        return results;
    }
}
