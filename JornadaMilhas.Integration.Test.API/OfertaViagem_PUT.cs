using JornadaMilhas.Dominio.Entidades;
using JornadaMilhas.Dominio.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace JornadaMilhas.Integration.Test.API;

public class OfertaViagem_PUT:IClassFixture<JornadaMilhasWebApplicationFactory>
{
    private readonly JornadaMilhasWebApplicationFactory app;
    public OfertaViagem_PUT(JornadaMilhasWebApplicationFactory app)
    {
        this.app = app;
    }
    [Fact]
    public async Task Verifica_Atualizacao_OfertaViagem()
    {
        //Arrange
        var ofertaExistente = app.Context.OfertasViagem.FirstOrDefault();
        if (ofertaExistente is null)
        {
            ofertaExistente = new OfertaViagem
                (
                    new Rota("Origem", "Destino"),
                    new Periodo(DateTime.Parse("2025-03-03"),DateTime.Parse("2025-03-06")),
                    200
                );
            app.Context.Add(ofertaExistente);
            app.Context.SaveChanges();
        }

        ofertaExistente.Rota = new Rota("Origem2", "Destino2");
        ofertaExistente.Periodo = new Periodo(DateTime.Parse("2024-05-05"), DateTime.Parse("2024-05-08"));
        ofertaExistente.Preco = 100;

        var client = await app.GetClientWithAccessTokenAsync();


        //Act
        var result = await client.PutAsJsonAsync("/ofertas-viagem", ofertaExistente);

        //Assert
        Assert.NotNull(result);
        Assert.Equal(HttpStatusCode.NoContent, result.StatusCode);
        
    }
}
