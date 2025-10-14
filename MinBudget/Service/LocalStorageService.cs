using System.Text.Json;
using Microsoft.JSInterop;

namespace MinBudget.Service
{
    // Service för localStorage
    public class LocalStorageService
    {
        private readonly IJSRuntime _js;
        public LocalStorageService(IJSRuntime js)
        {
            _js = js;
        }

        // Spara en lista till localStorage
        public async Task SaveListAsync<T>(string key, List<T> list)
        {
            var json = JsonSerializer.Serialize(list);
            await _js.InvokeVoidAsync("localStorage.setItem", key, json);
        }

        // Läs en lista från localStorage
        public async Task<List<T>> ReadListAsync<T>(string key)
        {
            var json = await _js.InvokeAsync<string>("localStorage.getItem", key);
            if (!string.IsNullOrEmpty(json))
            {
                try
                {
                    return JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>();
                }
                catch
                {
                    return new List<T>();
                }
            }
            return new List<T>();
        }
    }
}

