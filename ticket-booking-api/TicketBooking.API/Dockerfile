FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /source
COPY . .
RUN dotnet restore "./TicketBooking.API.csproj" --disable-parallel
RUN dotnet publish "./TicketBooking.API.csproj" -c release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/sdk:7.0
WORKDIR /app
COPY --from=build /app ./
EXPOSE 3001
CMD ["dotnet", "TicketBooking.API.dll"]