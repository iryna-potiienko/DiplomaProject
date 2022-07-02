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

#ENV ASPNETCORE_URLS	http://*:80;https://*:443
#ENV ASPNETCORE_HTTPS_PORT 443
#ENV ASPNETCORE_ENVIRONMENT Production
#ENV Env Production

COPY --from=build-env /app/out .
COPY /DiplomaProject /app/DiplomaProjectSrc

RUN dotnet tool install --global dotnet-ef
ENV PATH $PATH:/root/.dotnet/tools

RUN dotnet dev-certs https -ep ${HOME}/.aspnet/https/aspnetapp.pfx -p 0970729386  \
    && dotnet dev-certs https --trust 
    
COPY cert/aspnetapp.pfx /root/.aspnet/https/

ENTRYPOINT ["dotnet", "DiplomaProject.dll"]