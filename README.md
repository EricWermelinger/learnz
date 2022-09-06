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

## Features
### Dashboard (09/06/2022 not implemented yet)
On the Dashboard page, the user will see his recent activities.

### Together
On the Together page, user can interract with each other in a 1:1 mode. To interract with another user, the users need to connect first. This can either be done by sending a connection request similar to Instagram on the Connect subpage or by swiping similar to Tinder on the Swip subpage.

### Group
On the Group page, users can interract with each other on a Group level or also like a class. A Group can be created. As a user, I can only add already connected other users to it. In the Group section, users can then upload files in the File subpage or have a chat in the Chat subpage.

### Create
On the Create page, users can create a Set, which will be the base for the Learn, Challenge and Test pages. This means, a Set created here can then be learned, challenged or tested in the other pages. In addition, user can set the policy and visibility of a Set.

### Learn
On the Learn page, users can learn a Set from the Create page in a similar way as in Quizlet. User can see their last results, their progression and see which Questions were especially hard for them.

### Challenge
On the Challenge page, users can challenge each other to check who knows a Set from the Create page better. This page works similar to Kahoot. There is a Game Leader, who takes the control of the Game and multiple players who can join the Game.

### Test
On the Test page, users can do tests, automatically created from a Set from the Create page. This works similar as Moodle Tests. A Test can either be private or linked with a Group. When linking a Test to a Group, e.g. when a Teacher creates a Test for a Class, it is the behaviour, that the Test Creator can't participate in the Test. Instead, he can change the settings of a Test (which Questions are visible, how much points they wort...) and manually adjust the automatical correction.

### Draw (09/06/2022 not fully tested / fixed yet)
On the Draw page, user can do drawings with free-hand drawing, lines and text, similar to the OneNote functionality. Users either can do private drawings or link them with a Group. When a Drawing is linked to a Group, the Policy can either be set to Only Self Editable, i.e. the other users just have read-only access or to public. In public Mode, users can collaborate with each other on the same drawing.

### Settings
On the Settings page, users can change their Profile, their Password, their Profileimage and so on.