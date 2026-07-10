using AdminService.Domain.Interfaces;
using MediatR;

namespace AdminService.Application.Hotels.Commands;

public class UpdateHotelCommandHandler : IRequestHandler<UpdateHotelCommand>
{
    private readonly IHotelRepository _hotelRepository;

    public UpdateHotelCommandHandler(IHotelRepository hotelRepository)
    {
        _hotelRepository = hotelRepository;
    }

    public async Task Handle(UpdateHotelCommand request, CancellationToken cancellationToken)
    {
        var hotel = await _hotelRepository.GetByIdAsync(request.HotelId)
            ?? throw new KeyNotFoundException($"Hotel with Id {request.HotelId} not found.");

        hotel.Update(request.Name, request.City, request.Address, request.Description);

        await _hotelRepository.UpdateAsync(hotel);
        await _hotelRepository.SaveChangesAsync();
    }
}
