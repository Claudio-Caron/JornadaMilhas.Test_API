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

namespace JornadaMilhas.Integration.Test.API
{
    public class JornadaMilhasWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {
        public JornadaMilhasContext Context { get; private set; }

        private IServiceScope scope;

        private readonly MsSqlContainer _mssqlContainer = new MsSqlBuilder()
     .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
     .Build();



        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.RemoveAll(typeof(DbContextOptions<JornadaMilhasContext>));
                services.AddDbContext<JornadaMilhasContext>(options =>
           options
           .UseLazyLoadingProxies()
           .UseSqlServer(_mssqlContainer.GetConnectionString()));
            });

            base.ConfigureWebHost(builder);
        }

        public async Task<HttpClient> GetClientWithAccessTokenAsync()
        {
            var client = this.CreateClient();
            var user = new UserDTO { Email = "tester@email.com", Password = "Senha123@" };
            var resultado = await client.PostAsJsonAsync("/auth-login", user);

            resultado.EnsureSuccessStatusCode();

            var result = await resultado.Content.ReadFromJsonAsync<UserTokenDTO>();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result!.Token);

            return client;
        }

        public async Task InitializeAsync()
        {
            await _mssqlContainer.StartAsync();

            var maxTentativas = 10;
            var delay = TimeSpan.FromSeconds(2);

            for (int i = 0; i < maxTentativas; i++)
            {
                try
                {
                    this.scope = Services.CreateScope();
                    Context = scope.ServiceProvider.GetRequiredService<JornadaMilhasContext>();
                    await Context.Database.CanConnectAsync();
                    break;
                }
                catch
                {
                    await Task.Delay(delay);
                }
            }
            //this.scope = Services.CreateScope();
            //Context = scope.ServiceProvider.GetRequiredService<JornadaMilhasContext>();

        }

        async Task IAsyncLifetime.DisposeAsync()
        {
            if (scope is not null) scope.Dispose();
            await _mssqlContainer.DisposeAsync();
        }

    }
}