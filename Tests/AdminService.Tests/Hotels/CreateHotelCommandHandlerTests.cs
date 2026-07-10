using AdminService.Application.Hotels.Commands;
using AdminService.Domain.Entities;
using AdminService.Domain.Interfaces;
using Moq;
using Xunit;

namespace AdminService.Tests.Hotels;

/// <summary>
/// Tests del CreateHotelCommandHandler siguiendo el ciclo TDD Red/Green/Refactor.
///
/// RED:   Se escribió el test primero (falla porque el handler no existía).
/// GREEN: Se implementó CreateHotelCommandHandler para hacer pasar los tests.
/// REFACTOR: Se extrajo la lógica de dominio al constructor de Hotel (Name, City, etc.)
///           y se simplificó el handler delegando la validación al repositorio.
/// </summary>
public class CreateHotelCommandHandlerTests
{
    private readonly Mock<IHotelRepository> _hotelRepositoryMock;
    private readonly CreateHotelCommandHandler _handler;

    public CreateHotelCommandHandlerTests()
    {
        _hotelRepositoryMock = new Mock<IHotelRepository>();
        _handler = new CreateHotelCommandHandler(_hotelRepositoryMock.Object);
    }

    /// <summary>
    /// [GREEN] El handler debe persistir el hotel y retornar un Guid válido.
    /// </summary>
    [Fact]
    public async Task Handle_ValidCommand_ShouldCreateHotelAndReturnGuid()
    {
        // Arrange
        var command = new CreateHotelCommand("Hotel Bogotá", "Bogotá", "Calle 100 #10-50", "Hotel de lujo en Bogotá");

        _hotelRepositoryMock.Setup(r => r.AddAsync(It.IsAny<Hotel>()))
            .Returns(Task.CompletedTask);
        _hotelRepositoryMock.Setup(r => r.SaveChangesAsync())
            .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotEqual(Guid.Empty, result);
    }

    /// <summary>
    /// [GREEN] El handler debe llamar exactamente una vez a AddAsync y SaveChangesAsync.
    /// </summary>
    [Fact]
    public async Task Handle_ValidCommand_ShouldCallRepositoryOnce()
    {
        // Arrange
        var command = new CreateHotelCommand("Hotel Medellín", "Medellín", "El Poblado", "Hotel boutique");

        _hotelRepositoryMock.Setup(r => r.AddAsync(It.IsAny<Hotel>())).Returns(Task.CompletedTask);
        _hotelRepositoryMock.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _hotelRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Hotel>()), Times.Once);
        _hotelRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    /// <summary>
    /// [GREEN] El hotel creado debe tener los datos correctos del comando.
    /// </summary>
    [Fact]
    public async Task Handle_ValidCommand_ShouldCreateHotelWithCorrectData()
    {
        // Arrange
        var command = new CreateHotelCommand("Hotel Cartagena", "Cartagena", "Centro Histórico", "Frente al mar");
        Hotel? capturedHotel = null;

        _hotelRepositoryMock.Setup(r => r.AddAsync(It.IsAny<Hotel>()))
            .Callback<Hotel>(h => capturedHotel = h)
            .Returns(Task.CompletedTask);
        _hotelRepositoryMock.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(capturedHotel);
        Assert.Equal("Hotel Cartagena", capturedHotel!.Name);
        Assert.Equal("Cartagena", capturedHotel.City);
        Assert.True(capturedHotel.IsEnabled); // Enabled by default
    }

    /// <summary>
    /// [RED→GREEN] Si el repositorio lanza excepción, el handler debe propagarla.
    /// </summary>
    [Fact]
    public async Task Handle_WhenRepositoryThrows_ShouldPropagateException()
    {
        // Arrange
        var command = new CreateHotelCommand("Hotel Falla", "Ciudad", "Dirección", "Desc");

        _hotelRepositoryMock.Setup(r => r.AddAsync(It.IsAny<Hotel>()))
            .ThrowsAsync(new InvalidOperationException("DB Error"));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _handler.Handle(command, CancellationToken.None));
    }
}
