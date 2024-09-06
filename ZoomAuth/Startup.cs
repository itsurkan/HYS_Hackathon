public class Startup
{
    private static readonly string clientId = "oIcNUkH_QuWPUdivUkUvYw";
    private static readonly string clientSecret = "Ici1FoUNwZjoxJBvU8m5OQZ0EvilOB1X";
    private static readonly string redirectUri = "https://localhost:5080/api/zoom/oauth/callback";
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddHttpClient<ZoomService>(client =>
        {
            client.BaseAddress = new Uri("https://zoom.us/");
        });
        services.AddSingleton(new ZoomService(new HttpClient(), clientId, clientSecret, redirectUri));
        services.AddAuthorization();
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
    }


    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
