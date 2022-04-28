#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
#EXPOSE 80
#EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Presentation/MessageApp.API/MessageApp.API.csproj", "Presentation/MessageApp.API/"]
COPY ["Infrastructure/MessageApp.Presistence/MessageApp.Presistence.csproj", "Infrastructure/MessageApp.Presistence/"]
COPY ["Core/MessageApp.Application/MessageApp.Application.csproj", "Core/MessageApp.Application/"]
COPY ["Core/MessageApp.Domain/MessageApp.Domain.csproj", "Core/MessageApp.Domain/"]
COPY ["Infrastructure/MessageApp.Infrastructure/MessageApp.Infrastructure.csproj", "Infrastructure/MessageApp.Infrastructure/"]
RUN dotnet restore "Presentation/MessageApp.API/MessageApp.API.csproj"
COPY . .

WORKDIR "/src/Presentation/MessageApp.API"
RUN dotnet build "MessageApp.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MessageApp.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet", "MessageApp.API.dll"]
CMD ASPNETCORE_URLS=http://*:$PORT dotnet MessageApp.API.dll