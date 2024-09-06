using System.Text;

class Program
{
    private static readonly string clientId = "3upUbFDdTlW_fBvH3eQXsQ";
    private static readonly string clientSecret = "yzDuR8Cp6sFz32khdKruAqP4kNbno0vy";
    private static readonly string redirectUri = "http://localhost:5000/";

    static async Task Main(string[] args)
    {
        var authorizationCode = "heyIq76dO77Or_L6-61QVu8ypdVNu544g";

        var authHeaderValue = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{clientId}:{clientSecret}"));

        using (var client = new HttpClient())
        {
            client.DefaultRequestHeaders.Add("Authorization", $"Basic {authHeaderValue}");

            var content = new StringContent(
                $"grant_type=authorization_code&code={authorizationCode}&redirect_uri={redirectUri}",
                Encoding.UTF8, "application/x-www-form-urlencoded");

            var response = await client.PostAsync("https://zoom.us/oauth/token", content);
            var responseContent = await response.Content.ReadAsStringAsync();

            Console.WriteLine("Response:");
            Console.WriteLine(responseContent);
        }
    }
}
