FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

ARG CONTEXT=Category

COPY src/nuget.config .
COPY src/Shared/EShop.Contracts/EShop.Contracts.csproj Shared/EShop.Contracts/
COPY src/${CONTEXT}/${CONTEXT}.Domain/${CONTEXT}.Domain.csproj ${CONTEXT}/${CONTEXT}.Domain/
COPY src/${CONTEXT}/${CONTEXT}.DomainServices/${CONTEXT}.DomainServices.csproj ${CONTEXT}/${CONTEXT}.DomainServices/
COPY src/${CONTEXT}/${CONTEXT}.InternalContracts/${CONTEXT}.InternalContracts.csproj ${CONTEXT}/${CONTEXT}.InternalContracts/
COPY src/${CONTEXT}/${CONTEXT}.Application/${CONTEXT}.Application.csproj ${CONTEXT}/${CONTEXT}.Application/
COPY src/${CONTEXT}/${CONTEXT}.Storage/${CONTEXT}.Storage.csproj ${CONTEXT}/${CONTEXT}.Storage/
COPY src/${CONTEXT}/${CONTEXT}.Api/${CONTEXT}.Api.csproj ${CONTEXT}/${CONTEXT}.Api/

RUN dotnet restore ${CONTEXT}/${CONTEXT}.Api/${CONTEXT}.Api.csproj

COPY src/Shared/ Shared/
COPY src/${CONTEXT}/ ${CONTEXT}/

RUN dotnet publish ${CONTEXT}/${CONTEXT}.Api/${CONTEXT}.Api.csproj -c Release -o /app/publish --no-restore

FROM base AS final
ARG DEPLOYMENT_PROFILE=Single
ENV ASPNETCORE_ENVIRONMENT=${DEPLOYMENT_PROFILE}
WORKDIR /app
COPY --from=build /app/publish .

ARG CONTEXT=Category
ENV ENTRYPOINT_DLL="${CONTEXT}.Api.dll"
ENTRYPOINT ["sh", "-c", "dotnet $ENTRYPOINT_DLL"]
