using JornadaMilhas.Dados;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JornadaMilhas.Integration.Test.API
{
    public class OfertaViagem_GET:IClassFixture<JornadaMilhasContext>
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
            //Act
            //Assert
        }
    }
}
