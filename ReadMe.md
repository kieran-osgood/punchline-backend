# Get Started
Before running the application there are a number of steps required to get the environment setup.
It is assumed you will already have installed the .NET 6 development SDK, and ensure you the dotnet cli too installed (if `dotnet -h` responds anything other than "command not found" you're good to go).
Additionally docker and docker-compose are required to run this project.

## Pre-Requisites
You will need to download the app service.json and included it with the project in order to run the project (Firebase Admin SDK will expect this in the Startup.cs).
## Installation
Running `make up` will run docker-compose to setup the environment (database) - this should always be running before starting the dotnet application.
We are not using appsettings for storing sensitive secret, instead opt for `user-secrets`, accessible via the dotnet cli tool.

You will be required to setup environment variables for:
- GOOGLE_APPLICATION_CREDENTIALS 
    - `➜ dotnet user-secrets set "GOOGLE_APPLICATION_CREDENTIALS" "/Users/<USER>/path/to/repostiory/firebase-google-service.json"`
- ConnectionStrings:DefaultConnection
    - `➜ dotnet user-secrets set "ConnectionStrings:DefaultConnection" "User ID=;Password=;Host=;Port=5432;Database=postgres;Pooling=true;Minimum Pool Size=0;Maximum Pool Size=100;Connection Lifetime=0;"`
    - (docker-compose will take the variables from the .env file in the root directory - update these as you see fit)
