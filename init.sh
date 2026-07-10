#!/bin/bash
set -e

# Create solution
dotnet new sln -n AdminHotels

mkdir -p Src/AdminService
cd Src/AdminService
dotnet new webapi -n AdminService.Api
dotnet new classlib -n AdminService.Domain
dotnet new classlib -n AdminService.Application
dotnet new classlib -n AdminService.Infrastructure
cd ../..

mkdir -p Src/BookingService
cd Src/BookingService
dotnet new webapi -n BookingService.Api
dotnet new classlib -n BookingService.Domain
dotnet new classlib -n BookingService.Application
dotnet new classlib -n BookingService.Infrastructure
cd ../..

mkdir -p Src/ApiGateway
cd Src/ApiGateway
dotnet new webapi -n ApiGateway
cd ../..

# Add all projects to solution
dotnet sln AdminHotels.slnx add Src/AdminService/AdminService.Api/AdminService.Api.csproj
dotnet sln AdminHotels.slnx add Src/AdminService/AdminService.Domain/AdminService.Domain.csproj
dotnet sln AdminHotels.slnx add Src/AdminService/AdminService.Application/AdminService.Application.csproj
dotnet sln AdminHotels.slnx add Src/AdminService/AdminService.Infrastructure/AdminService.Infrastructure.csproj

dotnet sln AdminHotels.slnx add Src/BookingService/BookingService.Api/BookingService.Api.csproj
dotnet sln AdminHotels.slnx add Src/BookingService/BookingService.Domain/BookingService.Domain.csproj
dotnet sln AdminHotels.slnx add Src/BookingService/BookingService.Application/BookingService.Application.csproj
dotnet sln AdminHotels.slnx add Src/BookingService/BookingService.Infrastructure/BookingService.Infrastructure.csproj

dotnet sln AdminHotels.slnx add Src/ApiGateway/ApiGateway/ApiGateway.csproj

# Add references (Domain -> Application -> Infrastructure -> Api)
cd Src/AdminService
dotnet add AdminService.Application/AdminService.Application.csproj reference AdminService.Domain/AdminService.Domain.csproj
dotnet add AdminService.Infrastructure/AdminService.Infrastructure.csproj reference AdminService.Application/AdminService.Application.csproj
dotnet add AdminService.Api/AdminService.Api.csproj reference AdminService.Infrastructure/AdminService.Infrastructure.csproj
cd ../..

cd Src/BookingService
dotnet add BookingService.Application/BookingService.Application.csproj reference BookingService.Domain/BookingService.Domain.csproj
dotnet add BookingService.Infrastructure/BookingService.Infrastructure.csproj reference BookingService.Application/BookingService.Application.csproj
dotnet add BookingService.Api/BookingService.Api.csproj reference BookingService.Infrastructure/BookingService.Infrastructure.csproj
cd ../..

echo "Project structure generated successfully!"
