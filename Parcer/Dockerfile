#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Parcer/Parser.csproj", "Parcer/"]
COPY ["Parser.DataAccess/Parser.DataAccess.csproj", "Parser.DataAccess/"]
COPY ["Parser.Serviсes/Parser.Serviсes.csproj", "Parser.Serviсes/"]
COPY ["Parser.DataAccess.SqlServer/Parser.DataAccess.SqlServer.csproj", "Parser.DataAccess.SqlServer/"]
COPY ["Parser.Services.Logic/Parser.Services.Logic.csproj", "Parser.Services.Logic/"]
RUN dotnet restore "Parcer/Parser.csproj"
COPY . .
WORKDIR "/src/Parcer"
RUN dotnet build "Parser.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Parser.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet", "Parser.dll"]
CMD ASPNETCORE_URLS=http://*:$PORT dotnet Parser.dll