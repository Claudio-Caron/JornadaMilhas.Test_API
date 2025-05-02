using JornadaMilhas.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace JornadaMilhas.Integration.Test.API.RotaTest;

public class Rota_DELETE:IClassFixture<JornadaMilhasWebApplicationFactory>
{
    private readonly JornadaMilhasWebApplicationFactory factory;

    public Rota_DELETE(JornadaMilhasWebApplicationFactory factory)
    {
        this.factory = factory;
    }
    [Fact]
    public async Task Delete_Rota_PorId()
    {
        //Arrage
        var rotaRecuperada = factory.Context.Rota.FirstOrDefault(); 
        if (rotaRecuperada is null)
        {
            rotaRecuperada = new Rota("Origem", "Destino");
            factory.Context.Rota.Add(rotaRecuperada);
            factory.Context.SaveChanges();
        }

        var client = await factory.GetClientWithAccessTokenAsync();
        //Act
        var result = await client.DeleteAsync("/rota-viagem/"+ rotaRecuperada.Id);

        //Assert
        Assert.NotNull(result);
        Assert.Equal(HttpStatusCode.NoContent, result.StatusCode);
    }
}
