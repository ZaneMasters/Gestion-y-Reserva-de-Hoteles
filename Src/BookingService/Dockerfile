FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["Src/BookingService/BookingService.Api/BookingService.Api.csproj", "BookingService.Api/"]
COPY ["Src/BookingService/BookingService.Domain/BookingService.Domain.csproj", "BookingService.Domain/"]
COPY ["Src/BookingService/BookingService.Application/BookingService.Application.csproj", "BookingService.Application/"]
COPY ["Src/BookingService/BookingService.Infrastructure/BookingService.Infrastructure.csproj", "BookingService.Infrastructure/"]
RUN dotnet restore "BookingService.Api/BookingService.Api.csproj"
COPY Src/BookingService/ .
WORKDIR "/src/BookingService.Api"
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
EXPOSE 8080
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BookingService.Api.dll"]
