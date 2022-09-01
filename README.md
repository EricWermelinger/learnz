# learnz
Summerprojektli 2022 - Partial Work for the IDPA (Interdisciplinary Practical Work) at Berufsschule BBBaden.

## How to get started
### The following tools should be installed.
- NodeJS
- SSMS
- Visual Studio

### Start the Frontend
- Execute npm install within ../learnz/Frontend.
- Execute ng serve within ../learnz/Frontend.
> This will start the Frontend by default on port 4200. (http://localhost:4200/)

### Start the Backend
- Adjust within the appsettings.json file the DefaultConnection to your connection string.
- Start Project learnz within Visual Studio.
> This will start the Backend by default on port 7039. (API: https://localhost:7039/api/, Websockets: https://localhost:7039/ws/)

### The project should now run. To explore it:
- Open http://localhost:4200/
- SignUp to create a new user.
- Note that to see most of the features in action, it requires you to set up multiple users and let them interact with each other.
- The easiest way to get a connection between the user is letting them connect through the together > swipe page: /app/together-swipe.