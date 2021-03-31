project =  --project GraphQL
PROJECT_DIR = GraphQL
# Launch the Database
up:
	cd ${PROJECT_DIR} && docker-compose --env-file .env up --no-recreate
# Run the C# project
run:
	cd ${PROJECT_DIR} && dotnet watch run  --project GraphQL
# `make db-migrate` will update to latest migration
# `make db-migrate 0` will delete all tables and revert back to a zero migration state
# `make db-migrate migration="20210104065856_InitialCreate"` will migration to specific migration by file name
db-migrate:
	dotnet ef database update $(migration) --context=ApplicationDbContext ${project}
# Drops the database
db-drop:
	dotnet ef database drop  --project GraphQL
# Creates a new migration file with the latest updates to entities
db-generate:
	dotnet ef migrations add $(migration) --context=ApplicationDbContext ${project}
# Creates an idempotent SQL upgrade script to the latest migration file
db-generate-script:
	dotnet ef migrations script -i --context=ApplicationDbContext -o upgradescript.sql
# This removes the last created migration so long as the migration hasn't been executed against the database
# If the migration has been executed, run make db-migrate migration="FILE_NAME" to revert back before running this command
db-ungenerate:
	dotnet ef migrations remove --context=ApplicationDbContext ${project}
build:
# Use this for validating that theres no warnings/errors before running migrations
	dotnet build
