# Stage 1: Build the application
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app

# Copiar el archivo .csproj desde la carpeta específica
COPY *.csproj ./

# Restaurar las dependencias
RUN dotnet restore

# Copiar el resto del código y construir la aplicación
COPY . ./

#COPY firebase_config /app/
#ENV GOOGLE_APPLICATION_CREDENTIALS="/app/firebase_config/app-whatsapp-aspnetcore-firebase-adminsdk-owj7t-b5eb481bbb.json"

RUN dotnet publish -c Release -o /app/publish

# Stage 2: Run the application
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime
WORKDIR /app

# Copiar los archivos publicados desde el stage anterior
COPY --from=build /app/publish .

# Exponer el puerto en el que corre la aplicación
EXPOSE 80

# Configurar el comando de inicio
ENTRYPOINT ["dotnet", "ASP.NetCore-WhatsApp-1.dll"]