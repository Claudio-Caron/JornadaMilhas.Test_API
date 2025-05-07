

using JornadaMilhas.Dominio.Entidades;
using JornadaMilhas.Dominio.ValueObjects;

namespace JornadaMilhas.Test;

public class OfertaViagemConstrutor
{
    [Theory]
    [InlineData("", null, "2024-01-01", "2024-01-02", 0, false)]
    [InlineData("OrigemTeste", "DestinoTeste", "2024-02-01", "2024-02-05", 100, true)]
    [InlineData(null, "São Paulo", "2024-01-01", "2024-01-02", -1, false)]
    [InlineData("Vitória", "São Paulo", "2024-01-01", "2024-01-01", 0, false)]
    [InlineData("Rio de Janeiro", "São Paulo", "2024-01-01", "2024-01-02", -500, false)]
    //código do teste omitido

    public void ReturnIsValidAccordingToTheInputData(string origem, string destino, string dataInicial,
        string dataFinal, double preco, bool validacao)
    {
        //cenario - arrange
        Rota rota = new(origem, destino);
        Periodo periodo = new(DateTime.Parse(dataInicial), DateTime.Parse(dataFinal));
         

        //ação (do teste) - action
        OfertaViagem oferta = new OfertaViagem(rota, periodo, preco);

        //Validação - assert
        Assert.Equal(validacao, oferta.EhValido);

    }
    [Fact]
    public void ReturnRouteErrorMessageOrInvalidPeriodWhenNullRoute()
    {
        Rota rota = null;
        Periodo periodo = new(new DateTime(2024, 2, 1), new DateTime(2024, 2, 5));
        double preco = 100;
        bool validacao = true;

        OfertaViagem oferta = new OfertaViagem(rota, periodo, preco);

        Assert.Contains("A oferta de viagem não possui rota ou período válidos.", oferta.Erros.Sumario);
        Assert.False(oferta.EhValido);

    }
    [Fact]
    public void ReturnDateErrorMessageWhenFinalDateLessInitialDate()
    {
        Rota rota = new("OrigemTeste", "DestinoTeste");
        DateTime dataInicial = new DateTime(2024, 2, 5);
        DateTime dataFinal = new DateTime(2024, 2, 1);
        Periodo periodo = new(dataInicial, dataFinal);
        double preco = 100;
        bool validacao = true;

        OfertaViagem oferta = new(rota, periodo, preco);

        Assert.Contains("Erro: Data de ida não pode ser maior que a data de volta.", oferta.Erros.Sumario);
        Assert.NotEqual(dataInicial, dataFinal);
        Assert.False(oferta.EhValido);
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(-250)]
    public void ReturnErrorMessageWhenPrecoLessThanZero(double preco)
    {
        //arrange
        Rota rota = new Rota("Origem1", "Destino1");
        Periodo periodo = new Periodo(new DateTime(2024, 8, 20), new DateTime(2024, 8, 30));

        //act
        OfertaViagem oferta = new OfertaViagem(rota, periodo, preco);

        //assert
        Assert.Contains("O preço da oferta de viagem deve ser maior que zero.", oferta.Erros.Sumario);
    }
    [Fact]
    public void RetornaTresErrosDeValidacaoQuandoRotaPeriodoEPRecoSaoInvalidos()
    {
        int quantidadeEsperada = 3;
        Rota rota = null;
        Periodo periodo = new Periodo(new DateTime(2025, 6, 1), new DateTime(2025, 5, 10));
        double preco = -100;
        

        OfertaViagem oferta = new(rota, periodo, preco);

        Assert.Equal(quantidadeEsperada, oferta.Erros.Count());
    }
    //[Theory]
    //[InlineData("Black in back")]
    //public void InicializaNomeCorretamenteQuandoCadastraNovaMusica(string nome)
    //{

    //    var musica = new Mus(nome);

    //    Assert.Equal(nome, musica.Nome);



    //}
    //[Theory]
    //[InlineData(0)]
    //public void RightInicializedNameWhenPostMusic(int id)
    //{

    //    var musica = new Mus("Crawling");
    //    musica.Id = id;

    //    Assert.Equal(id, musica.Id);

    //}
    //[Fact]
    //public void TestandoToStringMusica()
    //{
    //    var random = new Random();
    //    int id = random.Next(30);
    //    string nome = "Something in the way";

    //    var musica = new Mus(nome);
    //    musica.Id = id;

    //    Assert.Equal(@$"Id: {id} Nome: {nome}", musica.ToString());

    //}

    //[Fact]
    //public void RetornaNuloQuandoAnoLancamentoNegativoOuNeutro()
    //{
    //    string nomeArtista = "Djavan";
    //    var fakerMusica = new Faker<Mus>()
    //        .CustomInstantiator(f =>
    //        {
    //            return new Mus(f.Lorem.Word());
    //        })
    //        .RuleFor(f => f.AnoLancamento, f => f.Random.Int(-1, 0))
    //        .RuleFor(f => f.Id, f => f.IndexFaker)
    //        .RuleFor(f => f.Artista, nomeArtista);

        

    //    Mus musica = fakerMusica.Generate();


        
    //    Assert.Null(musica.AnoLancamento);

    //}
    //[Fact]
    //public void RetornaArtistaDesconhecidoQuandoArtistaVazio()
    //{
    //    string artista = "Artista Desconhecido";
    //    var fakerMusica = new Faker<Mus>()
    //        .CustomInstantiator(f =>
    //        {
    //            return new Mus(f.Lorem.Word());
    //        })
    //        .RuleFor(f => f.AnoLancamento, f => f.Date.Past(50).Year)
    //        .RuleFor(f => f.Id, f => f.IndexFaker);


    //    Mus musica = fakerMusica.Generate();

    //    Assert.Equal(musica.Artista, artista);
    //}


}