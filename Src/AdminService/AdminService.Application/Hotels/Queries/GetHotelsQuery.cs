using AdminService.Domain.Entities;
using MediatR;

namespace AdminService.Application.Hotels.Queries;

public record GetHotelsQuery() : IRequest<IEnumerable<Hotel>>;
