namespace ImageGallery.Web.App.Services;

public class AuthStateService
{
    public string? AccessToken { get; private set; }
    public bool IsAuthenticated => !string.IsNullOrEmpty(AccessToken);

    public event Action? OnAuthStateChanged;

    public void SetToken(string? token)
    {
        AccessToken = token;
        OnAuthStateChanged?.Invoke();
    }

    public void Logout()
    {
        SetToken(null);
    }
}
