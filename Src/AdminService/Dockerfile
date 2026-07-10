FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["Src/AdminService/AdminService.Api/AdminService.Api.csproj", "AdminService.Api/"]
COPY ["Src/AdminService/AdminService.Domain/AdminService.Domain.csproj", "AdminService.Domain/"]
COPY ["Src/AdminService/AdminService.Application/AdminService.Application.csproj", "AdminService.Application/"]
COPY ["Src/AdminService/AdminService.Infrastructure/AdminService.Infrastructure.csproj", "AdminService.Infrastructure/"]
RUN dotnet restore "AdminService.Api/AdminService.Api.csproj"
COPY Src/AdminService/ .
WORKDIR "/src/AdminService.Api"
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
EXPOSE 8080
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AdminService.Api.dll"]
