using Bogus;
using JornadaMilhas.Dominio.Entidades;
using JornadaMilhas.Dominio.ValueObjects;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JornadaMilhas.Integration.Test.API.DataBuilders
{
    internal class OfertaViagemDataBuilder:Faker<OfertaViagem>
    {
        public double PrecoMinimo { get; set; } = 1;
        public double PrecoMaximo { get; set; } = 100;
        public Rota Rota { get; set; }
        public Periodo Periodo { get; set; }
        public OfertaViagemDataBuilder()
        {
            CustomInstantiator(f =>
            {
                Rota rota = Rota ?? new RotaDataBuilder().Build();
                Periodo periodo = Periodo ?? new PeriodoDataBuilder().Build();
                double preco = f.Random.Double(PrecoMinimo, PrecoMaximo);
                return new OfertaViagem(rota, periodo, preco);
            });
        }
    }
}
