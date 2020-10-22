# downtime_reporter
Use sql server. You can create sqlserver on docker by using that command.
docker run -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=yourStrong(!)Password' --restart=always -p 1433:1433 -d --name docker_mssql2017 mcr.microsoft.com/mssql/server:2017-latest
