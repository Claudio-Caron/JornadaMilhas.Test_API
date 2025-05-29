using JornadaMilhas.API.DTO.Auth;
using JornadaMilhas.Dados;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Testcontainers.MsSql;

namespace JornadaMilhas.Integration.Test.API;

public class JornadaMilhasWebApplicationFactory:WebApplicationFactory<Program>, IAsyncLifetime
{
    private string _connectionString = string.Empty;
    public JornadaMilhasContext Context { get; private set; }

    private MsSqlContainer _msSqlContainer = new MsSqlBuilder()
        .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
        .Build();

    private IServiceScope scope;


    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.RemoveAll(typeof(
                DbContextOptions<JornadaMilhasContext>));
            services.AddDbContext<JornadaMilhasContext>(options =>
            options
            .UseLazyLoadingProxies()
            .UseSqlServer
            (_connectionString));

        });
        base.ConfigureWebHost(builder);
    }
    public async Task<HttpClient> GetClientWithAccessTokenAsync()
    {
        var client = this.CreateClient();

        var user = new UserDTO
        {
            Email = "tester@email.com",
            Password = "Senha123@"
        };

        var response = await client.PostAsJsonAsync("/auth-login", user);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<UserTokenDTO>();

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result!.Token);

        return client;
    }

    public async Task InitializeAsync()
    {
        await _msSqlContainer.StartAsync();
        _connectionString = _msSqlContainer.GetConnectionString();
        this.scope = Services.CreateScope();
        Context =  scope.ServiceProvider.GetRequiredService<JornadaMilhasContext>();
    }

    async Task IAsyncLifetime.DisposeAsync()
    {
        await _msSqlContainer.DisposeAsync();
    }
}