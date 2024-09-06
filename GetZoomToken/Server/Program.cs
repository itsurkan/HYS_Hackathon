using System;
using System.Net;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        var listener = new HttpListener();
        var redirectUri = "http://localhost:5000/";
        listener.Prefixes.Add(redirectUri);

        listener.Start();
        Console.WriteLine($"Ожидаем редирект на {redirectUri}...");

        // Ожидаем запрос с кодом авторизации
        var context = await listener.GetContextAsync();
        var code = context.Request.QueryString["code"];

        Console.WriteLine($"Получен код авторизации: {code}");

        // Отправляем ответ в браузер
        var response = context.Response;
        var responseString = "<html><body>Авторизация прошла успешно. Можете вернуться в приложение.</body></html>";
        var buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
        response.ContentLength64 = buffer.Length;
        var output = response.OutputStream;
        await output.WriteAsync(buffer, 0, buffer.Length);
        output.Close();

        listener.Stop();

        // Далее используем полученный код для обмена на access_token
    }
}
