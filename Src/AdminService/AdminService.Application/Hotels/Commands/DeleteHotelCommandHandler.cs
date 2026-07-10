using AdminService.Domain.Interfaces;
using MediatR;

namespace AdminService.Application.Hotels.Commands;

/// <summary>
/// Eliminación lógica: marca el hotel como deshabilitado en lugar de eliminarlo físicamente.
/// Esto preserva la integridad referencial con las reservas existentes.
/// </summary>
public class DeleteHotelCommandHandler : IRequestHandler<DeleteHotelCommand>
{
    private readonly IHotelRepository _hotelRepository;

    public DeleteHotelCommandHandler(IHotelRepository hotelRepository)
    {
        _hotelRepository = hotelRepository;
    }

    public async Task Handle(DeleteHotelCommand request, CancellationToken cancellationToken)
    {
        var hotel = await _hotelRepository.GetByIdAsync(request.HotelId)
            ?? throw new KeyNotFoundException($"Hotel with Id {request.HotelId} not found.");

        // Logical delete: disable instead of removing from DB
        hotel.ChangeStatus(false);

        await _hotelRepository.UpdateAsync(hotel);
        await _hotelRepository.SaveChangesAsync();
    }
}
