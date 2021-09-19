project =  --project GraphQL
PROJECT_DIR = GraphQL
# Launch the Database
up:
	cd ${PROJECT_DIR} && docker-compose --env-file .env up --no-recreate --remove-orphans
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
decrypt-firebase:
	AWS_PROFILE=iamadmin-punchline sops -d GraphQL/encrypted.firebase-admin-sdk.json > GraphQL/firebase-admin-sdk.json
encrypt-firebase:
	AWS_PROFILE=iamadmin-punchline sops -e GraphQL/firebase-admin-sdk.json > GraphQL/encrypted.firebase-admin-sdk.json
decrypt-prod:
	AWS_PROFILE=iamadmin-punchline sops -d GraphQL/encrypted.appsettings.Production.json > GraphQL/appsettings.Production.json
encrypt-prod:
	AWS_PROFILE=iamadmin-punchline sops -e GraphQL/appsettings.Production.json > GraphQL/encrypted.appsettings.Production.json
build:
	docker build -t punchline-backend-image . --no-cache                                                    
run:
	docker run -it --rm -p 5000:80 --name punchline-backend-container punchline-backend-image
run-ecs:
## Requires authentication # ➜ docker login -u AWS --password $(AWS_PROFILE=iamadmin-punchline aws ecr get-login-password --region us-east-1) 345637428723.dkr.ecr.us-east-1.amazonaws.com/punchline-backend
	docker run -it --rm -p 8080:80 --name punchline-backend-container 345637428723.dkr.ecr.us-east-1.amazonaws.com/punchline-backend:52e8df9dc8f71973d3db0cd5d863f0dda45a73f5 