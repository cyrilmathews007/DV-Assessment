FROM mcr.microsoft.com/dotnet/sdk:6.0 as build
WORKDIR /app
EXPOSE 80

COPY DeliVeggie.sln DeliVeggie.sln
COPY Gateway/Gateway.csproj Gateway/Gateway.csproj
COPY Microservice/Microservice.csproj Microservice/Microservice.csproj
COPY Messages/Messages.csproj Messages/Messages.csproj
COPY Domain/Domain.csproj Domain/Domain.csproj
COPY Microservices.Test/Microservices.Test.csproj Microservices.Test/Microservices.Test.csproj

RUN dotnet restore DeliVeggie.sln

COPY Microservice Microservice
COPY Messages Messages
COPY Domain Domain

WORKDIR /app/Microservice
RUN dotnet publish -c Release -o /app/out

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT [ "dotnet", "Microservice.dll" ]

