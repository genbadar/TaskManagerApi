# TaskManagerApi

A RESTful Task Manager API built with **ASP.NET Core**, **Entity Framework Core**, and **PostgreSQL**. Supports full CRUD for tasks and categories with Swagger documentation.

---

## Tech Stack

| Layer     | Technology             |
| --------- | ---------------------- |
| Framework | ASP.NET Core (.NET 10) |
| ORM       | Entity Framework Core  |
| Database  | PostgreSQL             |
| API Docs  | Swagger / Swashbuckle  |
| Language  | C#                     |

---

## Features

- Create, read, update, and delete tasks
- Organize tasks into categories
- Mark tasks as complete
- Filter tasks by category
- Interactive API docs via Swagger UI

---

## Project Structure

```
TaskManagerApi/
├── Controllers/
│   ├── TasksController.cs
│   └── CategoriesController.cs
├── Data/
│   └── AppDbContext.cs
├── Models/
│   ├── TaskItem.cs
│   └── Category.cs
├── Migrations/
├── appsettings.json
└── Program.cs
```

---

## Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [PostgreSQL](https://www.postgresql.org/download/)
- [EF Core CLI](https://learn.microsoft.com/en-us/ef/core/cli/dotnet)

```bash
dotnet tool install --global dotnet-ef
```

### 1. Clone the repository

```bash
git clone https://github.com/genbadar/TaskManagerApi.git
cd TaskManagerApi
```

### 2. Configure the database

Create a PostgreSQL database:

```sql
CREATE DATABASE taskmanagerdb;
```

Update the connection string in `appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=taskmanagerdb;Username=postgres;Password=YOUR_PASSWORD"
}
```

### 3. Apply migrations

```bash
dotnet ef database update
```

### 4. Run the API

```bash
dotnet run --launch-profile https
```

### 5. Open Swagger UI

```
https://localhost:7158/swagger
```

---

## API Endpoints

### Tasks

| Method   | Endpoint                   | Description           |
| -------- | -------------------------- | --------------------- |
| `GET`    | `/api/tasks`               | Get all tasks         |
| `GET`    | `/api/tasks/{id}`          | Get task by ID        |
| `POST`   | `/api/tasks`               | Create a new task     |
| `PUT`    | `/api/tasks/{id}`          | Update a task         |
| `PATCH`  | `/api/tasks/{id}/complete` | Mark task as complete |
| `DELETE` | `/api/tasks/{id}`          | Delete a task         |

### Categories

| Method   | Endpoint               | Description                         |
| -------- | ---------------------- | ----------------------------------- |
| `GET`    | `/api/categories`      | Get all categories                  |
| `GET`    | `/api/categories/{id}` | Get category by ID (includes tasks) |
| `POST`   | `/api/categories`      | Create a new category               |
| `PUT`    | `/api/categories/{id}` | Update a category                   |
| `DELETE` | `/api/categories/{id}` | Delete a category                   |

---

## Example Requests

### Create a category

```json
POST /api/categories
{
  "name": "Work",
  "color": "#4A90E2"
}
```

### Create a task

```json
POST /api/tasks
{
  "title": "Finish project report",
  "description": "Complete the Q2 summary",
  "dueDate": "2026-06-01T00:00:00Z",
  "categoryId": 1
}
```

### Mark a task complete

```http
PATCH /api/tasks/1/complete
```

---

## EF Core Commands Reference

```bash
# Create a new migration after model changes
dotnet ef migrations add <MigrationName>

# Apply migrations to the database
dotnet ef database update

# List all migrations
dotnet ef migrations list

# Undo the last migration (if not yet applied)
dotnet ef migrations remove

# Roll back to a specific migration
dotnet ef database update <MigrationName>
```

---

## License

This project is open source and available under the [MIT License](LICENSE).
