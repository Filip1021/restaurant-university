# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy project files
COPY ["src/RestaurantReservation.Host/RestaurantReservation.Host.csproj", "src/RestaurantReservation.Host/"]
COPY ["src/RestaurantReservation.BL/RestaurantReservation.BL.csproj", "src/RestaurantReservation.BL/"]
COPY ["src/RestaurantReservation.DAL/RestaurantReservation.DAL.csproj", "src/RestaurantReservation.DAL/"]

# Restore dependencies
RUN dotnet restore "src/RestaurantReservation.Host/RestaurantReservation.Host.csproj"

# Copy source code
COPY . .

# Build
RUN dotnet build "src/RestaurantReservation.Host/RestaurantReservation.Host.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "src/RestaurantReservation.Host/RestaurantReservation.Host.csproj" -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=publish /app/publish .

EXPOSE 5127
ENTRYPOINT ["dotnet", "RestaurantReservation.Host.dll"]
