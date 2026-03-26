# Use the SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore as distinct layers
COPY ["vita-care.csproj", "./"]
RUN dotnet restore "vita-care.csproj"

# Copy everything else and build
COPY . .
RUN dotnet build "vita-care.csproj" -c Release -o /app/build

# Publish the app
FROM build AS publish
RUN dotnet publish "vita-care.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Use the runtime image to run the app
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Expose the API port (default is 80 or 8080 in .NET 8)
EXPOSE 8080

ENTRYPOINT ["dotnet", "vita-care.dll"]
