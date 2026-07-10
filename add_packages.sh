#!/bin/bash
set -e

# AdminService
dotnet add Src/AdminService/AdminService.Api/AdminService.Api.csproj package MediatR
dotnet add Src/AdminService/AdminService.Api/AdminService.Api.csproj package MassTransit.RabbitMQ
dotnet add Src/AdminService/AdminService.Api/AdminService.Api.csproj package Microsoft.EntityFrameworkCore.Design

dotnet add Src/AdminService/AdminService.Application/AdminService.Application.csproj package MediatR
dotnet add Src/AdminService/AdminService.Application/AdminService.Application.csproj package FluentValidation.DependencyInjectionExtensions

dotnet add Src/AdminService/AdminService.Infrastructure/AdminService.Infrastructure.csproj package Npgsql.EntityFrameworkCore.PostgreSQL

# BookingService
dotnet add Src/BookingService/BookingService.Api/BookingService.Api.csproj package MediatR
dotnet add Src/BookingService/BookingService.Api/BookingService.Api.csproj package MassTransit.RabbitMQ
dotnet add Src/BookingService/BookingService.Api/BookingService.Api.csproj package Microsoft.EntityFrameworkCore.Design

dotnet add Src/BookingService/BookingService.Application/BookingService.Application.csproj package MediatR
dotnet add Src/BookingService/BookingService.Application/BookingService.Application.csproj package FluentValidation.DependencyInjectionExtensions

dotnet add Src/BookingService/BookingService.Infrastructure/BookingService.Infrastructure.csproj package Npgsql.EntityFrameworkCore.PostgreSQL

# ApiGateway
dotnet add Src/ApiGateway/ApiGateway/ApiGateway.csproj package Yarp.ReverseProxy

echo "NuGet packages added successfully!"
