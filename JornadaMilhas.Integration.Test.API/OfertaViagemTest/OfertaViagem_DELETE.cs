using JornadaMilhas.Dominio.Entidades;
using JornadaMilhas.Dominio.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace JornadaMilhas.Integration.Test.API.OfertaViagemTest;

public class OfertaViagem_DELETE : IClassFixture<JornadaMilhasWebApplicationFactory>
{
    private readonly JornadaMilhasWebApplicationFactory app;

    public OfertaViagem_DELETE(JornadaMilhasWebApplicationFactory app)
    {
        this.app = app;
    }
    [Fact]
    public async Task Deletar_OfertaViagem_PorId()
    {
        //Arrange
        var ofertaExistente = app.Context.OfertasViagem.FirstOrDefault();
        if (ofertaExistente is null)
        {
            ofertaExistente = new OfertaViagem
            (
                new Rota("Origem", "Destino"),
                new Periodo(DateTime.Parse("2025-03-03"), DateTime.Parse("2025-03-06")),
                206
            );
            app.Context.Add(ofertaExistente);
            app.Context.SaveChanges();
        }

        var client = await app.GetClientWithAccessTokenAsync();


        //Act
        var result = await client.DeleteAsync("/ofertas-viagem/" + ofertaExistente.Id);

        //Assert
        Assert.NotNull(result);

        Assert.Equal(HttpStatusCode.NoContent, result.StatusCode);
        //Assert.Equal(result.Preco, ofertaExistente.Preco, 0.001);
        //Assert.Equal(result.Periodo.DataInicial, ofertaExistente.Periodo.DataInicial);
        //Assert.Equal(result.Periodo.DataFinal, ofertaExistente.Periodo.DataFinal);
        //Assert.Equal(result.Rota.Origem, ofertaExistente.Rota.Origem);
        //Assert.Equal(result.Rota.Destino, ofertaExistente.Rota.Destino);
    }
}
