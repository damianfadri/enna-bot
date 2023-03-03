docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=Adminxyz22#" -p 1433:1433 -d mcr.microsoft.com/mssql/server:2022-latest

do 
{
    sqlcmd -S tcp:127.0.0.1,1433 -U sa -P "Adminxyz22#" -Q "SELECT @@VERSION" 2>$null
    Start-Sleep -Seconds 1
} 
while ($LastExitCode -eq 1)

dotnet ef database update --project .\src\Enna.Bot.Infrastructure.Mssql\Enna.Bot.Infrastructure.Mssql.csproj --startup-project .\src\Enna.Bot\Enna.Bot.csproj --context StreamerContext
dotnet ef database update --project .\src\Enna.Bot.Infrastructure.Mssql\Enna.Bot.Infrastructure.Mssql.csproj --startup-project .\src\Enna.Bot\Enna.Bot.csproj --context TenantContext