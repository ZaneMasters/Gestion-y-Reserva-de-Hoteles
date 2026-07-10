using AdminService.Domain.Entities;
using MediatR;

namespace AdminService.Application.Hotels.Queries;

public record GetHotelByIdQuery(Guid HotelId) : IRequest<Hotel?>;
