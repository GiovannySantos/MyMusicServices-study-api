namespace MyMusicServicesApi.Models;

public class LoginRequest
{
    public LoginRequest(string username, string password)
    {
        Username = username;
        Password = password;
    }

    public string Username { get; set; }
    public string Password { get; set; }
}
public class LoginResponse
{
    public LoginResponse(string token)
    {
        Token = token;
    }
    
    public string Token { get; set; }
}