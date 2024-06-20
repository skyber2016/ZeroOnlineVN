# Sử dụng một ảnh .NET Core 8.0 SDK để xây dựng ứng dụng
FROM mcr.microsoft.com/dotnet/core/sdk:8.0 AS build
WORKDIR /app

# Sao chép toàn bộ mã nguồn ứng dụng và thực hiện xây dựng
COPY . .
RUN dotnet publish -c Release -o out

# Sử dụng ảnh ASP.NET Core 8.0 Runtime để chạy ứng dụng
FROM mcr.microsoft.com/dotnet/core/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out ./

# Khởi chạy ứng dụng khi container được khởi động
ENTRYPOINT ["dotnet", "ProxyServer.dll"]