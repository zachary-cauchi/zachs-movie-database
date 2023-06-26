
Adding new migrations:
```powershell
Add-Migration MigrationNameHere
```

Updating the database:
```powershell
Update-Database -Connection "Host=localhost:5432;Username=****;Password=****;Database=postgres"
```