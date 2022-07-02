# syntax=docker/dockerfile:1
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY DiplomaProject/*.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY /DiplomaProject ./
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/sdk:6.0
WORKDIR /app

COPY --from=build-env /app/out .
COPY /DiplomaProject /app/DiplomaProjectSrc

RUN dotnet tool install --global dotnet-ef
ENV PATH $PATH:/root/.dotnet/tools

COPY cert/aspnetapp.pfx ${HOME}/.aspnet/https/

#ENTRYPOINT ["dotnet", "DiplomaProject.dll"]

CMD ["sleep", "10000"]