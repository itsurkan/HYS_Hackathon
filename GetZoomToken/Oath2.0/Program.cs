using System.Text;

class Program
{
    private static readonly string clientId = "1";
    private static readonly string clientSecret = "2";
    private static readonly string redirectUri = "http://localhost:5000/";

    static async Task Main(string[] args)
    {
        var authorizationCode = "AUTHORIZATION_CODE"; 

        var authHeaderValue = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{clientId}:{clientSecret}"));

        using (var client = new HttpClient())
        {
            client.DefaultRequestHeaders.Add("Authorization", $"Basic {authHeaderValue}");

            var content = new StringContent($"grant_type=authorization_code&code={authorizationCode}&redirect_uri={redirectUri}", Encoding.UTF8, "application/x-www-form-urlencoded");

            var response = await client.PostAsync("https://zoom.us/oauth/token", content);
            var responseContent = await response.Content.ReadAsStringAsync();

            Console.WriteLine(responseContent);
        }
    }
}
