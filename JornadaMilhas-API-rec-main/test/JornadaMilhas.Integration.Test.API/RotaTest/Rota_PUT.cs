using JornadaMilhas.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace JornadaMilhas.Integration.Test.API.RotaTest;

public class Rota_PUT:IClassFixture<JornadaMilhasWebApplicationFactory>
{
    private readonly JornadaMilhasWebApplicationFactory factory;

    public Rota_PUT(JornadaMilhasWebApplicationFactory factory)
    {
        this.factory = factory;
    }
    [Fact]
    public async Task Atualiza_Rota_PorId()
    {
        //Arrange
        var rotaRecuperada = factory.Context.Rota.FirstOrDefault();
        if (rotaRecuperada is null)
        {
            rotaRecuperada = new Rota("Origem", "Destino");
            factory.Context.Rota.Add(rotaRecuperada);
            factory.Context.SaveChanges();
        }
        rotaRecuperada.Origem = "nova Andradina";
        rotaRecuperada.Destino = "Piracicaba";

        var client = await factory.GetClientWithAccessTokenAsync();


        //Act
        var result = await client.PutAsJsonAsync("/rota-viagem/", rotaRecuperada);
        //Assert
        Assert.NotNull(result);
        Assert.Equal(HttpStatusCode.NoContent, result.StatusCode);

    }

    
}
