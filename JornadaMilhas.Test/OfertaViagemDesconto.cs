using JornadaMilhas.Dominio.Entidades;
using JornadaMilhas.Dominio.ValueObjects;


namespace JornadaMilhas.Test;

public class OfertaViagemDesconto
{
    [Fact]
    public void RetornaPrecoAtualizadoQuandoAplicadoDesconto()
    {
        //arrange
        Rota rota = new Rota("OrigemA", "DestinoA");
        Periodo periodo = new(new DateTime(2025, 05, 01), new DateTime(2025, 05, 10));
        double precoOriginal = 125.00;
        double desconto = 20.00;
        double precoComDesconto = precoOriginal-desconto;
        OfertaViagem oferta = new OfertaViagem(rota, periodo, precoOriginal);

        //act
        oferta.Desconto = desconto;

        //assert
        Assert.Equal(precoComDesconto, oferta.Preco);
    }
    [Fact]
    public void RetornaDescontoMaximoQuandoValorDescontoMaiorQueDescontoMaximo()
    {
        //arrange
        Rota rota = new Rota("OrigemA", "DestinoA");
        Periodo periodo = new(new DateTime(2025, 05, 01), new DateTime(2025, 05, 10));
        double precoOriginal = 100.00;
        double desconto = 120.00;
        double precoComDesconto = 30.00;
        OfertaViagem oferta = new OfertaViagem(rota, periodo, precoOriginal);

        //act
        oferta.Desconto = desconto;

        //assert
        Assert.Equal(precoComDesconto, oferta.Preco, 0.001);
    }
    [Fact]
    public void ReturnPrecoOriginalQuandoDescontoNegativo()
    {
        //arrange
        Rota rota = new Rota("OrigemA", "DestinoA");
        Periodo periodo = new(new DateTime(2025, 05, 01), new DateTime(2025, 05, 10));
        double precoOriginal = 100.00;
        double desconto = -54.00;
        double precoComDesconto = 100.00;
        OfertaViagem oferta = new OfertaViagem(rota, periodo, precoOriginal);

        //act
        oferta.Desconto = desconto;

        //assert
        Assert.Equal(precoComDesconto, oferta.Preco, 0.001);
    }
}
