using AdminService.Domain.Interfaces;
using MediatR;

namespace AdminService.Application.Hotels.Commands;

public class ChangeHotelStatusCommandHandler : IRequestHandler<ChangeHotelStatusCommand>
{
    private readonly IHotelRepository _hotelRepository;

    public ChangeHotelStatusCommandHandler(IHotelRepository hotelRepository)
    {
        _hotelRepository = hotelRepository;
    }

    public async Task Handle(ChangeHotelStatusCommand request, CancellationToken cancellationToken)
    {
        var hotel = await _hotelRepository.GetByIdAsync(request.HotelId)
            ?? throw new KeyNotFoundException($"Hotel with Id {request.HotelId} not found.");

        hotel.ChangeStatus(request.IsEnabled);

        await _hotelRepository.UpdateAsync(hotel);
        await _hotelRepository.SaveChangesAsync();
    }
}
