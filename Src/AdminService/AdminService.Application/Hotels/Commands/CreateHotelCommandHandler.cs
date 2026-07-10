using AdminService.Domain.Entities;
using AdminService.Domain.Interfaces;
using MediatR;

namespace AdminService.Application.Hotels.Commands;

public class CreateHotelCommandHandler : IRequestHandler<CreateHotelCommand, Guid>
{
    private readonly IHotelRepository _hotelRepository;

    public CreateHotelCommandHandler(IHotelRepository hotelRepository)
    {
        _hotelRepository = hotelRepository;
    }

    public async Task<Guid> Handle(CreateHotelCommand request, CancellationToken cancellationToken)
    {
        var hotel = new Hotel(request.Name, request.City, request.Address, request.Description);
        
        await _hotelRepository.AddAsync(hotel);
        await _hotelRepository.SaveChangesAsync();

        return hotel.Id;
    }
}
