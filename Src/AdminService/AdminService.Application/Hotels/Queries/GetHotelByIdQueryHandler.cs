using AdminService.Domain.Entities;
using AdminService.Domain.Interfaces;
using MediatR;

namespace AdminService.Application.Hotels.Queries;

public class GetHotelByIdQueryHandler : IRequestHandler<GetHotelByIdQuery, Hotel?>
{
    private readonly IHotelRepository _hotelRepository;

    public GetHotelByIdQueryHandler(IHotelRepository hotelRepository)
    {
        _hotelRepository = hotelRepository;
    }

    public async Task<Hotel?> Handle(GetHotelByIdQuery request, CancellationToken cancellationToken)
    {
        return await _hotelRepository.GetByIdAsync(request.HotelId);
    }
}
