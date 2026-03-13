
PROJECT Guidelines

Instructions for the final year project
You must develop a web application on a topic of your choice.
This application must consist of:

    A client application
    A web API
    A database


Technology
[Frontend] Web application
Framework: React or NextJS (Other JS frameworks possible on request)
Language: JavaScript / TypeScript
[Backend] RestFul Web API Server + Database
Framework: ASP.Net API
Language: C#
Database:
 • Relational (Ado, Dapper, EFCore): MSSQL, PostgreSQL, MariaDB
 • NoSQL: MongoDB, CosmoDb, etc. 

Before you begin

    Summarise your project and the features you wish to develop.
    Choose the different technologies for your final year project.
    Confirm your choices with your trainer.


Technical constraints

    Development by feature.
    Use of Git is mandatory with an online repository (Github, GitLab, etc.).
        One repository for the client application (front-end).
    One repository for the API server (back-end).
    Links must be sent to the trainer (public or private).
    Perform an analysis.
    Create an I/O diagram of the database.
    Create (at minimum) the UML ‘Use Case’ diagram.
    Set up a Trello board (or similar).

PROJECT PLAN

# Theme
- Platform for organising gaming activities (board games, analogue games)

# Name
- Project Analogify

## Technology

    Front-end: React (JS)
    Back-end: ASP.Net API
    Database: MSSQL (EF Core)

## Features

### User account
- Default filter preference: Board games / Analogue games
- Encoding of game style mastery (nice UI)
- Player profile (bio, level, etc.)

### Event management
- Event creation (location, game type, date, number of players, etc.)
- Option to manage events with others (moderators)
- Public visibility (not logged in)
- Registration request (subject to moderator approval)
- FAQ system (user questions and moderator responses)
- (Bonus) Create maps with events
- (Bonus) Real-time chat [Potentially temporary]
- Rating system
- Review after the event (if you participated)
- Player ratings (1 to 5 stars)
- Admin actions
- Reporting the event (rename, delete)
- Reporting inappropriate behaviour (ban?)