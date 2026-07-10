#!/bin/bash
set -e

# Create Test projects
mkdir -p Src/AdminService/AdminService.UnitTests
cd Src/AdminService/AdminService.UnitTests
dotnet new xunit
dotnet add package Moq
dotnet add package FluentAssertions
dotnet add reference ../AdminService.Application/AdminService.Application.csproj
dotnet add reference ../AdminService.Domain/AdminService.Domain.csproj
cd ../../..

mkdir -p Src/BookingService/BookingService.UnitTests
cd Src/BookingService/BookingService.UnitTests
dotnet new xunit
dotnet add package Moq
dotnet add package FluentAssertions
dotnet add package MassTransit
dotnet add reference ../BookingService.Application/BookingService.Application.csproj
dotnet add reference ../BookingService.Domain/BookingService.Domain.csproj
cd ../../..

# Add to solution
dotnet sln AdminHotels.slnx add Src/AdminService/AdminService.UnitTests/AdminService.UnitTests.csproj
dotnet sln AdminHotels.slnx add Src/BookingService/BookingService.UnitTests/BookingService.UnitTests.csproj

echo "Test projects generated and configured successfully!"
