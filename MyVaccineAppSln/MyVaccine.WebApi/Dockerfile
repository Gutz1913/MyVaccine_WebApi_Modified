# Use the official image as a parent image
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app

# Use SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["MyVaccine.WebApi/MyVaccine.WebApi.csproj", "MyVaccine.WebApi/"]
RUN dotnet restore "MyVaccine.WebApi/MyVaccine.WebApi.csproj"
COPY . .
WORKDIR "/src/MyVaccine.WebApi"
RUN dotnet build "MyVaccine.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MyVaccine.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MyVaccine.WebApi.dll"]