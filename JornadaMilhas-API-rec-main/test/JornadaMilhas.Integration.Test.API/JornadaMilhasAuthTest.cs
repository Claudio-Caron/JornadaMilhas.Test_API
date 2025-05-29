


using JornadaMilhas.API.DTO.Auth;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System.Net;
using System.Net.Http.Json;

namespace JornadaMilhas.Integration.Test.API;

public class JornadaMilhasAuthTest : IClassFixture<JornadaMilhasWebApplicationFactory>
{
    private JornadaMilhasWebApplicationFactory _app;
    public JornadaMilhasAuthTest(JornadaMilhasWebApplicationFactory app)
    {
        _app = app;
    }
    [Fact]
    public async Task POST_Efetua_Login_Com_Sucesso()
    {
        //Arrange
        
        var user = new UserDTO { Email = "tester@email.com", Password = "Senha123@" };
        
        using var client = _app.CreateClient();
        //Act
        var resultado = await client.PostAsJsonAsync("/auth-login", user);

        //Assert
        Assert.Equal(HttpStatusCode.OK, resultado.StatusCode);
    }
    [Fact]
    public async Task POST_Efetua_Login_Com_Falha()
    {
        //Arrange
        var user = new UserDTO() { Email=  "Email@gmail.com", Password = "@Senha1234" };
        

        using var client = _app.CreateClient();
        //Act
        var cadastro = await client.PostAsJsonAsync("/auth-login", user);

        //Assert
        Assert.Equal(HttpStatusCode.BadRequest, cadastro.StatusCode);
    }
}
