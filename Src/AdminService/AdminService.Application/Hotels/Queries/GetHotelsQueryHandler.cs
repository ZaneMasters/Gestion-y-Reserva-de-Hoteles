using AdminService.Domain.Entities;
using AdminService.Domain.Interfaces;
using MediatR;

namespace AdminService.Application.Hotels.Queries;

public class GetHotelsQueryHandler : IRequestHandler<GetHotelsQuery, IEnumerable<Hotel>>
{
    private readonly IHotelRepository _hotelRepository;

    public GetHotelsQueryHandler(IHotelRepository hotelRepository)
    {
        _hotelRepository = hotelRepository;
    }

    public async Task<IEnumerable<Hotel>> Handle(GetHotelsQuery request, CancellationToken cancellationToken)
    {
        return await _hotelRepository.GetAllAsync();
    }
}
