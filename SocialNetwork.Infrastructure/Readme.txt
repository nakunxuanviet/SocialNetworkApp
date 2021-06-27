Add-Migration Init -Context ApplicationDbContext -o Persistence\Migrations
Remove-migration
Update-Database

Drop-Database (IMPORTANT!!!)