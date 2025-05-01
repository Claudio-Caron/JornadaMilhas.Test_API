using JornadaMilhas.Dominio.Entidades;
using JornadaMilhas.Dominio.ValueObjects;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace JornadaMilhas.Integration.Test.API;
 
public class OfertaViagem_POST:IClassFixture<JornadaMilhasWebApplicationFactory>
{
    private JornadaMilhasWebApplicationFactory app;

    public OfertaViagem_POST(JornadaMilhasWebApplicationFactory app)
    {
        this.app = app;
    }

    [Fact]
    public async Task Cadastra_OfertaViagemAutenticado()
    {
        //Arrange
        using var client = await app.GetClientWithAccessTokenAsync();

        var OfertaViagem = new OfertaViagem()
        {
            Preco = 100,
            Rota = new Rota("Origem", "Destino"),
            Periodo = new Periodo(DateTime.Parse("2024-03-03"), DateTime.Parse("2024-03-06"))
        };


        //Act
        var response = await client.PostAsJsonAsync("/ofertas-viagem", OfertaViagem);
        //Assert
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
    [Fact]
    public async Task Cadastra_OfertaViagemNaoAutenticado()
    {
        //Arrange
        using var client =  app.CreateClient();

        var OfertaViagem = new OfertaViagem()
        {
            Preco = 100,
            Rota = new Rota("Origem", "Destino"),
            Periodo = new Periodo(DateTime.Parse("2024-03-03"), DateTime.Parse("2024-03-06"))
        };


        //Act
        var response = await client.PostAsJsonAsync("/ofertas-viagem", OfertaViagem);
        //Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}
