# Project Management Application

A comprehensive ASP.NET Core MVC application for managing projects, tasks, and team collaboration.

## Features

- **User Authentication**: Register, login, and user profile management
- **Project Management**: Create, edit, view, and manage projects
- **Task Management**: Create tasks, assign to team members, track status
- **Team Collaboration**: Add team members to projects, manage permissions
- **Time Tracking**: Log work hours for tasks
- **Comments**: Add comments to tasks for effective communication

## Technology Stack

- **Framework**: ASP.NET Core MVC
- **Database**: PostgreSQL
- **ORM**: Entity Framework Core
- **Authentication**: Cookie-based authentication
- **Frontend**: Bootstrap, jQuery

## Project Structure

The application follows a layered architecture:

- **Controllers**: Handle HTTP requests and responses
- **Services**: Implement business logic
- **Repositories**: Manage data access operations
- **Models**: Define domain entities
- **DTOs**: Transfer data between layers
- **Views**: Present data to users

## Key Components

### Models

- **User**: Represents system users
- **Project**: Contains project details, status, dates
- **ProjectTask**: Tasks within projects with status tracking
- **ProjectMember**: Manages project team membership
- **TimeLog**: Tracks time spent on tasks
- **TaskComment**: Allows communication on tasks

### Controllers

- **AuthController**: Handles user registration, login, and authentication
- **ProjectController**: Manages project CRUD operations
- **TasksController**: Handles task management
- **UsersController**: Manages user profiles and search
- **TimeLogsController**: Tracks time spent on tasks

## API Endpoints

### AuthController (`/api/auth`)
| Endpoint | Method | Description | Request Body | Response |
|----------|--------|-------------|--------------|----------|
| `/register` | POST | Register new user | `RegisterDto` | `AuthResponseDto` |
| `/login` | POST | User login | `LoginDto` | `AuthResponseDto` |
| `/refresh-token` | POST | Refresh access token | `{ refreshToken }` | `AuthResponseDto` |

### ProjectsController (`/api/projects`)
| Endpoint | Method | Description | Request Body | Response |
|----------|--------|-------------|--------------|----------|
| `` | GET | Get all projects (filter by status) | - | `List<ProjectListDto>` |
| `/{id}` | GET | Get project details by ID | - | `ProjectDetailDto` |
| `` | POST | Create new project | `CreateProjectDto` | `ProjectDetailDto` |
| `/{id}` | PUT | Update project | `UpdateProjectDto` | `ProjectDetailDto` |
| `/{id}/members` | GET | Get all project members | - | `List<ProjectMemberDto>` |
| `/{id}/members` | POST | Add member to project | `AddProjectMemberDto` | `ProjectMemberDto` |
| `/{id}/members/{memberId}` | DELETE | Remove member from project | - | `{ success }` |

### TasksController (`/api/tasks`)
| Endpoint | Method | Description | Request Body | Response |
|----------|--------|-------------|--------------|----------|
| `/project/{projectId}` | GET | Get all tasks in project | - | `List<TaskListDto>` |
| `/{id}` | GET | Get task details | - | `TaskDetailDto` |
| `` | POST | Create new task | `CreateTaskDto` | `TaskDetailDto` |
| `/{id}` | PUT | Update task | `UpdateTaskDto` | `TaskDetailDto` |
| `/{id}/status` | PATCH | Update task status | `{ status }` | `TaskListDto` |
| `/{id}/comments` | GET | Get task comments | - | `List<TaskCommentDto>` |
| `/{id}/comments` | POST | Add comment | `CreateTaskCommentDto` | `TaskCommentDto` |

### UsersController (`/api/users`)
| Endpoint | Method | Description | Request Body | Response |
|----------|--------|-------------|--------------|----------|
| `/me` | GET | Get logged-in user profile | - | `UserProfileDto` |
| `/search` | GET | Search users (for assignment) | `?query=john` | `List<UserSearchResultDto>` |
| `/me/avatar` | POST | Upload avatar | `IFormFile` | `{ avatarUrl }` |

### TimeLogsController (`/api/timelogs`)
| Endpoint | Method | Description | Request Body | Response |
|----------|--------|-------------|--------------|----------|
| `/task/{taskId}` | GET | Get time logs for task | - | `List<TimeLogDto>` |
| `` | POST | Log work time | `CreateTimeLogDto` | `TimeLogDto` |
| `/{id}` | DELETE | Delete time log | - | `{ success }` |

## Web Interface

The application provides a user-friendly web interface with:
- Dashboard overview
- Project listing and details
- Task management views
- User profile management
- Responsive design for mobile and desktop

## Setup Instructions

### Prerequisites
- .NET 8.0
- PostgreSQL database

### Database Setup
1. Update connection string in `appsettings.json`
2. Run migrations using: `./update-database.sh`

### Running the Application
1. Build the project: `dotnet build`
2. Run the application: `dotnet run`
3. Access the application at: `https://localhost:5001`

## Development Scripts
- `create-migration.sh`: Create a new database migration
- `update-database.sh`: Apply pending migrations
- `reset-database.sh`: Reset the database to a clean state