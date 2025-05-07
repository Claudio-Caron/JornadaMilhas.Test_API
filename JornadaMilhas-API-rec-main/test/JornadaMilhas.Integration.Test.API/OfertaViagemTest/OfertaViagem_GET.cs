using JornadaMilhas.Dados;
using JornadaMilhas.Dominio.Entidades;
using JornadaMilhas.Dominio.ValueObjects;
using JornadaMilhas.Integration.Test.API.DataBuilders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace JornadaMilhas.Integration.Test.API.OfertaViagemTest;

public class OfertaViagem_GET : IClassFixture<JornadaMilhasWebApplicationFactory>
{
    private readonly JornadaMilhasWebApplicationFactory app;

    public OfertaViagem_GET(JornadaMilhasWebApplicationFactory app)
    {
        this.app = app;
    }
    [Fact]
    public async Task RecuperaOfertaViagemPorId()
    {
        //Arrange
        var ofertaExistente = app.Context.OfertasViagem.FirstOrDefault();
        if (ofertaExistente is null)
        {
            ofertaExistente = new OfertaViagem()
            {
                Preco = 100,
                Rota = new Rota("Origem", "Destino"),
                Periodo = new Periodo(DateTime.Parse("2024-03-03"), DateTime.Parse("2024-03-06"))
            };
            app.Context.OfertasViagem.Add(ofertaExistente);
            app.Context.SaveChanges();
        }
        var client = await app.GetClientWithAccessTokenAsync();


        //Act
        var response = await client.GetFromJsonAsync<OfertaViagem>("/ofertas-viagem/" + ofertaExistente.Id);

        //Assert
        Assert.NotNull(response);
        Assert.Equal(response.Preco, ofertaExistente.Preco, 0.001);
        Assert.Equal(response.Rota.Origem, ofertaExistente.Rota.Origem);
        Assert.Equal(response.Rota.Destino, ofertaExistente.Rota.Destino);

    }
    [Fact]
    public async Task Get_OfertasViagem_Por_Pagina()
    {
        //Arrange
        //if (app.Context.OfertasViagem.Count() < 80)
        //{

        //}
        int pagina = 1;
        int tamanhoPorPagina = 80;

        var listaOfertas = new OfertaViagemDataBuilder().Generate(tamanhoPorPagina);
        app.Context.OfertasViagem.AddRange(listaOfertas);
        app.Context.SaveChanges();

        
        var client = await app.GetClientWithAccessTokenAsync();
        //Act
        var response = await client.GetFromJsonAsync<ICollection<OfertaViagem>>
            ($"/ofertas-viagem?pagina={pagina}&tamanhoPorPagina={tamanhoPorPagina}");

        //Assert
        Assert.True(response != null);
        Assert.Equal(tamanhoPorPagina, response.Count());
    }
    [Fact]
    public async Task Get_OfertasViagem_Ultima_Pagina()
    {
        //Arrange
        int pagina = 4;
        int tamanhoPorPagina = 25;

        app.Context.Database.ExecuteSqlRaw("DELETE FROM OfertasViagem");
        var listaOfertas = new OfertaViagemDataBuilder().Generate(80);
        app.Context.OfertasViagem.AddRange(listaOfertas);
        app.Context.SaveChanges();

        HttpClient client = await app.GetClientWithAccessTokenAsync();

        //Act
        var response = await client.GetFromJsonAsync<IEnumerable<OfertaViagem>>
            ($"/ofertas-viagem?pagina={pagina}&tamanhoPorPagina={tamanhoPorPagina}");
        //Assert
        Assert.True (response != null);
        Assert.Equal(5, response.Count());
        
    }
    [Fact]
    public async Task Get_OfertasViagem_Pagina_Inexistente()
    {
        //Arrange
        int pagina = 5;
        int tamanhoPorPagina = 25;

        app.Context.Database.ExecuteSqlRaw("DELETE FROM OfertasViagem");
        var listaOfertas = new OfertaViagemDataBuilder().Generate(80);
        app.Context.OfertasViagem.AddRange(listaOfertas);
        app.Context.SaveChanges();

        HttpClient client = await app.GetClientWithAccessTokenAsync();

        //Act
        var response = await client.GetFromJsonAsync<IEnumerable<OfertaViagem>>
            ($"/ofertas-viagem?pagina={pagina}&tamanhoPorPagina={tamanhoPorPagina}");
        //Assert
        Assert.True(response != null);
        Assert.Equal(0, response.Count());

    }
    [Fact]
    public async Task Get_OfertasViagem_Pagina_Com_Valor_Negativo()
    {
        //Arrange
        app.Context.Database.ExecuteSqlRaw("DELETE FROM OfertasViagem");
        int pagina = -5;
        int tamanhoPorPagina = 25;

        var listaOfertas = new OfertaViagemDataBuilder().Generate(80);
        app.Context.OfertasViagem.AddRange(listaOfertas);
        app.Context.SaveChanges();

        var client = await app.GetClientWithAccessTokenAsync();
        //Act + Assert
        await Assert.ThrowsAsync<HttpRequestException>(async()=>{
            var response = await client.GetFromJsonAsync<IEnumerable<OfertaViagem>>
            ($"/ofertas-viagem?pagina={pagina}&tamanhoPorPagina={tamanhoPorPagina}");
        });

        
    }
}
