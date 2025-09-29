using Diamond.Share.Models;

namespace Diamond.WebUI.Services
{
    public class UserState
    {

        private readonly HttpClient _http;
        public User? CurrentUser { get; private set; }
        public bool IsLoaded { get; private set; }

        private event Action? OnChange;

        public UserState(HttpClient http)
        {
            _http = http;
        }

        private void Notify() => OnChange?.Invoke();
        private string baseUrl = "http://localhost:5218/api/user/me";

        public async Task LoadUserAsync()
        {
            try
            {
                var resp = await _http.GetAsync(baseUrl);
                if (resp.IsSuccessStatusCode)
                {
                    CurrentUser = await resp.Content.ReadFromJsonAsync<User>();
                }
                else
                {
                    CurrentUser = null;
                }
            }
            catch
            {
                CurrentUser = null;
            }
            finally
            {
                IsLoaded = true;
                Notify();
            }
        }

        public void SetUser(User user)
        {
            CurrentUser = user;
            IsLoaded = true;
            Notify();
        }

        public void clear()
        {
            CurrentUser = null;
            IsLoaded = true;
            Notify();
        }
    }
}
