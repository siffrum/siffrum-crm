# -----------------------------
# Build stage
# -----------------------------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY Components ./Components
COPY UI ./UI
COPY ReferenceDlls ./ReferenceDlls

# Fix Linux case-sensitive folder name mismatch for BAL
RUN if [ -d "Components/siffrum.Web.PAYROLL.BAL" ] && [ ! -d "Components/Siffrum.Web.Payroll.BAL" ]; then \
      mv "Components/siffrum.Web.PAYROLL.BAL" "Components/Siffrum.Web.Payroll.BAL"; \
    fi

# Disable Windows-only post-build .bat Exec (breaks on Linux sh)
RUN set -eux; \
    f="Components/Siffrum.Web.Payroll.ServiceModels/Siffrum.Web.Payroll.ServiceModels.csproj"; \
    if [ -f "$f" ] && grep -q "DevCopyToTargetFolders.bat" "$f"; then \
      sed -i 's#<Exec Command="#<Exec Condition="'"'"'$(OS)'"'"' == '"'"'Windows_NT'"'"'" Command="#' "$f"; \
    fi

RUN dotnet restore UI/Siffrum.Web.Payroll.API/Siffrum.Web.Payroll.API.csproj

RUN dotnet publish UI/Siffrum.Web.Payroll.API/Siffrum.Web.Payroll.API.csproj \
    -c Release \
    -o /app/publish \
    --no-restore

# -----------------------------
# Runtime stage (net6.0)
# -----------------------------
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app

COPY --from=build /app/publish ./

ENV ASPNETCORE_URLS=http://0.0.0.0:6000
EXPOSE 6000

ENTRYPOINT ["dotnet", "Siffrum.Web.Payroll.API.dll"]
