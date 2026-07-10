using MediatR;

namespace AdminService.Application.Hotels.Commands;

/// <summary>
/// Eliminación lógica: deshabilita el hotel sin borrarlo de la base de datos.
/// </summary>
public record DeleteHotelCommand(Guid HotelId) : IRequest;
