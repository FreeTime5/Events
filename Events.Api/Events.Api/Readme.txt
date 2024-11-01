
add a migration:

dotnet ef migrations add initial --project Events.DataAccess --startup-project Events.Api

remove a migration:

dotnet ef migrations remove --project Events.DataAccess --startup-project Events.Api

update database:

dotnet ef database update --project Events.DataAccess --startup-project Events.Api