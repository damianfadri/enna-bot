#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Enna.Bot/Enna.Bot.csproj", "src/Enna.Bot/"]
COPY ["src/Enna.Bot.Infrastructure.Mssql/Enna.Bot.Infrastructure.Mssql.csproj", "src/Enna.Bot.Infrastructure.Mssql/"]
COPY ["src/Enna.Discord.Domain/Enna.Discord.Domain.csproj", "src/Enna.Discord.Domain/"]
COPY ["src/Enna.Streamers.Domain/Enna.Streamers.Domain.csproj", "src/Enna.Streamers.Domain/"]
COPY ["src/Enna.Core.Domain/Enna.Core.Domain.csproj", "src/Enna.Core.Domain/"]
COPY ["src/Enna.Bot.Infrastructure/Enna.Bot.Infrastructure.csproj", "src/Enna.Bot.Infrastructure/"]
COPY ["src/Enna.Core.Application/Enna.Core.Application.csproj", "src/Enna.Core.Application/"]
COPY ["src/Enna.Discord.Application/Enna.Discord.Application.csproj", "src/Enna.Discord.Application/"]
COPY ["src/Enna.Discord.Application.Contracts/Enna.Discord.Application.Contracts.csproj", "src/Enna.Discord.Application.Contracts/"]
COPY ["src/Enna.Streamers.Application/Enna.Streamers.Application.csproj", "src/Enna.Streamers.Application/"]
COPY ["src/Enna.Streamers.Application.Contracts/Enna.Streamers.Application.Contracts.csproj", "src/Enna.Streamers.Application.Contracts/"]
RUN dotnet restore "src/Enna.Bot/Enna.Bot.csproj"
COPY . .
WORKDIR "/src/src/Enna.Bot"
RUN dotnet build "Enna.Bot.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Enna.Bot.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Enna.Bot.dll"]