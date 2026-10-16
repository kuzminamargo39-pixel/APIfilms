# APIfilms Project

## GitHub Repository
https://github.com/kuzminamargo39-pixel/APIfilms.git

## Project Overview
This repository contains my submission for the TD. The goal of this project was to build a clean, reliable, and user-friendly full-stack solution. The work is split into three main parts:
- **APIfilms:** A RESTful ASP.NET Core Web API with PostgreSQL integration using Entity Framework Core. I structured the backend with clear separation of concerns, utilizing Data Transfer Objects (DTOs), AutoMapper for clean mapping, and repository/controller patterns.
- **ClientBlazorFilms:** A Blazor WebAssembly frontend that interacts with the API through HTTP calls.
- **APIfilmsTests:** A unit testing project using Moq and xUnit to test API controller behavior and validation logic independently from the database.

## Key Improvements & Features
Beyond the core requirements, I paid special attention to user experience and overall code quality:
- **French Language Support:** In accordance with the project requirements, all UI messages, validation notices, alerts, and feedback states are fully translated and displayed in French.
- **Enhanced UI/UX:** I redesigned the default Blazor interface to make it more intuitive, clean, and visually engaging for movie management and navigation.
- **Error Handling & Feedback:** Clear status and error messages are provided when interacting with the API (loading states, successful additions/updates, and error handling).

## How to Run the Project

### Database Configuration
Make sure PostgreSQL is running locally and verify your connection string in `APIfilms/appsettings.json`.

Apply migrations to create the database schema:
dotnet ef database update --project APIfilms

### Launching the Application
To run both the backend API and the Blazor client at the same time:

**In Visual Studio:**
1. Right-click the Solution and select **Configure Startup Projects**.
2. Select **Multiple startup projects**.
3. Set **APIfilms** to **Start** and **ClientBlazorFilms** to **Start**.
4. Press `Ctrl + F5`.

**In Terminal:**
1. Run the API in the first terminal:
dotnet run --project APIfilms

2. Run the Blazor client in a second terminal:
dotnet run --project ClientBlazorFilms

### Running Tests
To run all unit tests:
dotnet test APIfilmsTests

## Author
Margarita Kuzmina
