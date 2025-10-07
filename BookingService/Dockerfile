FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["BookingService/BookingService.csproj", "BookingService/"]
COPY ["BookingService.BLL/BookingService.BLL.csproj", "BookingService.BLL/"]
COPY ["BookingService.Common/BookingService.Common.csproj", "BookingService.Common/"]
COPY ["BookingService.DAL/BookingService.DAL.csproj", "BookingService.DAL/"]

RUN dotnet restore "BookingService/BookingService.csproj"

COPY . .

WORKDIR "/src/BookingService"
RUN dotnet build "BookingService.csproj" -c Release -o /app/build
RUN dotnet publish "BookingService.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

RUN apt-get update && apt-get install -y \
    wget \
    && rm -rf /var/lib/apt/lists/*
COPY --from=build /app/publish .

EXPOSE 5009
EXPOSE 7156

ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:5009;https://+:7156

ENTRYPOINT ["dotnet", "BookingService.dll"]