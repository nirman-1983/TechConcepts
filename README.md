# Deployment guide

## Scripts

```
dotnet ef migrations add InitialCreate
--project ProductCatalog.Infrastructure
--startup-project ProductCatalog.API
--output-dir Data\SQLServer-Migrations
```

## Permissions
### Service Principle for DB Deployment
1. Contributor access on Subscription (or else azure/login@v2 won't work)
2. 'SQL Security Manager' access on SQL Server instance (for adding/ removing firewall rule)
3. Access rights on SQL Database (for EF Core migrations)
```
CREATE USER [service-principle-name] FROM EXTERNAL PROVIDER;
EXEC sp_addrolemember 'db_ddladmin', [service-principle-name];
EXEC sp_addrolemember 'db_datawriter', [service-principle-name];
EXEC sp_addrolemember 'db_datareader', [service-principle-name];
```
## GitHub multiple accounts on same machine