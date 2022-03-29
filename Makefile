project =  --project GraphQL
PROJECT_DIR = GraphQL

# ------------- Run project for development -----------
up:
# Launch the Database
	cd ${PROJECT_DIR} && docker-compose --env-file .env up --no-recreate --remove-orphans
run:
# Run the C# project
	cd ${PROJECT_DIR} && dotnet watch run  --project GraphQL
	
# ------------- Database -----------
db-migrate:
# `make db-migrate` will update to latest migration
# `make db-migrate 0` will delete all tables and revert back to a zero migration state
# `make db-migrate migration="20210104065856_InitialCreate"` will migration to specific migration by file name
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
	
# ------------- SOPS -----------
decrypt-firebase:
	AWS_PROFILE=iamadmin-punchline sops -d GraphQL/encrypted.firebase-admin-sdk.json > GraphQL/firebase-admin-sdk.json
encrypt-firebase:
	AWS_PROFILE=iamadmin-punchline sops -e GraphQL/firebase-admin-sdk.json > GraphQL/encrypted.firebase-admin-sdk.json
decrypt-prod:
	AWS_PROFILE=iamadmin-punchline sops -d GraphQL/encrypted.appsettings.Production.json > GraphQL/appsettings.Production.json
encrypt-prod:
	AWS_PROFILE=iamadmin-punchline sops -e GraphQL/appsettings.Production.json > GraphQL/encrypted.appsettings.Production.json

# ------------- Lambda Deployment -----------
deploy:
	cd ${PROJECT_DIR} && dotnet lambda deploy-serverless
