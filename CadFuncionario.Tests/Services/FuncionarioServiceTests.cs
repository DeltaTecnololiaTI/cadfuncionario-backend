using Xunit;
using Moq;
using System;
using System.Threading.Tasks;
using CadFuncionario.Application.Services;
using CadFuncionario.Application.DTOs;
using CadFuncionario.Infra.Interfaces;
using CadFuncionario.Domain.Entities;
using System.Collections.Generic;

public class FuncionarioServiceTests
{
    private readonly Mock<IFuncionarioRepository> _funcionarioRepositoryMock;
    private readonly Mock<ICargoRepository> _cargoRepositoryMock;
    private readonly Mock<ITelefoneRepository> _telefoneRepositoryMock; 
    private readonly FuncionarioService _funcionarioService;

    public FuncionarioServiceTests()
    {
        _funcionarioRepositoryMock = new Mock<IFuncionarioRepository>();
        _cargoRepositoryMock = new Mock<ICargoRepository>();
        _telefoneRepositoryMock = new Mock<ITelefoneRepository>(); 
    _funcionarioService = new FuncionarioService(
        _funcionarioRepositoryMock.Object, 
        _cargoRepositoryMock.Object,
        _telefoneRepositoryMock.Object );
    }

    [Fact]
    public async Task CriarFuncionario_DeveLancarExcecao_SeMenorDeIdade()
    {
        // Arrange
        var funcionarioDTO = new FuncionarioDTO
        {
            Nome = "João",
            DataNascimento = DateTime.UtcNow.AddYears(-17) // Menor de 18 anos
        };

        // Act & Assert
        var ex = await Assert.ThrowsAsync<Exception>(() => _funcionarioService.CriarFuncionarioAsync(funcionarioDTO));
        Assert.Equal("O funcionário deve ter pelo menos 18 anos para ser cadastrado.", ex.Message);
    }
    [Fact]
    public async Task CriarFuncionarioAsync_DeveCriarFuncionarioComSucesso()
    {
        // Arrange
        var funcionarioDTO = new FuncionarioDTO
        {
            Nome = "João",
            DataNascimento = DateTime.UtcNow.AddYears(-20), // 20 anos (válido)
            CargoId = 1,
            Email = "joao@email.com",
            SenhaHash = "SenhaForte123"
        };

        var cargo = new Cargo { CargoId = 1, Nome = "Funcionário" };
        _cargoRepositoryMock.Setup(x => x.ObterPorIdAsync(1)).ReturnsAsync(cargo);
        _funcionarioRepositoryMock.Setup(x => x.AdicionarAsync(It.IsAny<Funcionario>()))
                                  .ReturnsAsync(new Funcionario { FuncionarioId = 1 });

        // Act
        var resultado = await _funcionarioService.CriarFuncionarioAsync(funcionarioDTO);

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal(1, resultado.FuncionarioId);
    }
    [Fact]
    public async Task CriarFuncionarioAsync_DeveLancarExcecao_SeFuncionarioForMenorDeIdade()
    {
        // Arrange
        var funcionarioDTO = new FuncionarioDTO
        {
            Nome = "Pedro",
            DataNascimento = DateTime.UtcNow.AddYears(-16), // 16 anos (inválido)
            CargoId = 1
        };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() =>
            _funcionarioService.CriarFuncionarioAsync(funcionarioDTO));

        Assert.Equal("O funcionário deve ter pelo menos 18 anos para ser cadastrado.", exception.Message);
    }
    [Fact]
    public async Task CriarFuncionarioAsync_DeveLancarExcecao_SeCargoNaoExistir()
    {
        // Arrange
        var funcionarioDTO = new FuncionarioDTO { Nome = "Carlos", CargoId = 99 }; // CargoId inválido

        _cargoRepositoryMock.Setup(x => x.ObterPorIdAsync(99)).ReturnsAsync((Cargo?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() =>
            _funcionarioService.CriarFuncionarioAsync(funcionarioDTO));

        Assert.Equal("Cargo inválido.", exception.Message);
    }

    [Fact]
    public async Task AtualizarFuncionarioAsync_DeveAtualizarComSucesso()
    {
        // Arrange
        var funcionarioDTO = new FuncionarioDTO
        {
            FuncionarioId = 1,
            Nome = "Ana",
            CargoId = 2
        };

        var funcionarioExistente = new Funcionario { FuncionarioId = 1, Nome = "Ana Antiga", CargoId = 1 };

        _funcionarioRepositoryMock.Setup(x => x.ObterPorIdAsync(1)).ReturnsAsync(funcionarioExistente);
        _funcionarioRepositoryMock.Setup(x => x.AtualizarAsync(It.IsAny<Funcionario>()))
                                  .ReturnsAsync(new Funcionario { FuncionarioId = 1, Nome = "Ana" });

        // Act
        var resultado = await _funcionarioService.AtualizarFuncionarioAsync(funcionarioDTO);

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal("Ana", resultado.Nome);
    }
    [Fact]
    public async Task DeletarFuncionarioAsync_DeveDeletarComSucesso()
    {

        // Arrange
        _funcionarioRepositoryMock.Setup(x => x.DeletarAsync(1)).ReturnsAsync(true);

        // Act
        var resultado = await _funcionarioService.DeletarFuncionarioAsync(1);

        // Assert
        Assert.True(resultado);
    }
    [Fact]
    public async Task ObterFuncionarioPorIdAsync_DeveRetornarFuncionarioSeExistir()
    {
        // Arrange
        var funcionario = new Funcionario { FuncionarioId = 1, Nome = "José" };

        _funcionarioRepositoryMock.Setup(x => x.ObterPorIdAsync(1)).ReturnsAsync(funcionario);

        // Act
        var resultado = await _funcionarioService.ObterFuncionarioPorIdAsync(1);

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal(1, resultado.FuncionarioId);
    }
    [Fact]
    public async Task ObterTodosFuncionariosAsync_DeveRetornarListaFuncionarios()
    {
        // Arrange
        var funcionarios = new List<Funcionario>
    {
        new Funcionario { FuncionarioId = 1, Nome = "Maria" },
        new Funcionario { FuncionarioId = 2, Nome = "Carlos" }
    };

        _funcionarioRepositoryMock.Setup(x => x.ObterTodosAsync()).ReturnsAsync(funcionarios);

        // Act
        var resultado = await _funcionarioService.ObterTodosFuncionariosAsync();

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal(2, resultado.Count());
    }


}
