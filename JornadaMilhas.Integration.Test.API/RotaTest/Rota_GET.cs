using JornadaMilhas.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace JornadaMilhas.Integration.Test.API.RotaTest;

public class Rota_GET:IClassFixture<JornadaMilhasWebApplicationFactory>
{
    private readonly JornadaMilhasWebApplicationFactory app;

    public Rota_GET(JornadaMilhasWebApplicationFactory app)
    {
        this.app = app;
    }
    [Fact]
    public async Task Recupera_Rota()
    {
        //Arrange
        Rota rotaExistente = app.Context.Rota.FirstOrDefault();
        if (rotaExistente is null)
        {
            rotaExistente = new Rota("Origem", "Destino");
            app.Context.Add(rotaExistente);
            app.Context.SaveChanges();
        }

        HttpClient client = await app.GetClientWithAccessTokenAsync();

        //Act
        HttpResponseMessage result = await client.GetAsync("/rota-viagem");
        //Assert

        Assert.NotNull(result);
        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
    }
}
