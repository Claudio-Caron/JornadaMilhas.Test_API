using JornadaMilhas.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace JornadaMilhas.Integration.Test.API.RotaTest;

public class Rota_POST:IClassFixture<JornadaMilhasWebApplicationFactory>
{
    private readonly JornadaMilhasWebApplicationFactory app;

    public Rota_POST(JornadaMilhasWebApplicationFactory app)
    {
        this.app = app;
    }
    [Fact]
    public async Task Verifica_Cadastro_Rota()
    {
        //Arrange
        var rota = new Rota("Origem", "destino");

        var client = await app.GetClientWithAccessTokenAsync();

        //Act
        var result = await client.PostAsJsonAsync("/rota-viagem/", rota);
        //Assert
        Assert.NotNull(result);
        Assert.Equal(HttpStatusCode.Created, result.StatusCode);
    }
}
