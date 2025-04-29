Berikut daftar lengkap endpoint untuk semua controller dalam aplikasi Project Management (**MVP Focus**):

### **1. AuthController.cs** (`/api/auth`)
| Endpoint | Method | Deskripsi | Request Body | Response |
|----------|--------|-----------|--------------|----------|
| `/register` | POST | Registrasi user baru | `RegisterDto` | `AuthResponseDto` |
| `/login` | POST | Login user | `LoginDto` | `AuthResponseDto` |
| `/refresh-token` | POST | Refresh access token | `{ refreshToken }` | `AuthResponseDto` |

### **2. ProjectsController.cs** (`/api/projects`)
| Endpoint | Method | Deskripsi | Request Body | Response |
|----------|--------|-----------|--------------|----------|
| `` | GET | Get semua proyek (filter by status) | - | `List<ProjectListDto>` |
| `/{id}` | GET | Get detail proyek by ID | - | `ProjectDetailDto` |
| `` | POST | Buat proyek baru | `CreateProjectDto` | `ProjectDetailDto` |
| `/{id}` | PUT | Update proyek | `UpdateProjectDto` | `ProjectDetailDto` |
| `/{id}/members` | GET | Get semua anggota proyek | - | `List<ProjectMemberDto>` |
| `/{id}/members` | POST | Tambah anggota ke proyek | `AddProjectMemberDto` | `ProjectMemberDto` |
| `/{id}/members/{memberId}` | DELETE | Hapus anggota dari proyek | - | `{ success }` |

### **3. TasksController.cs** (`/api/tasks`)
| Endpoint | Method | Deskripsi | Request Body | Response |
|----------|--------|-----------|--------------|----------|
| `/project/{projectId}` | GET | Get semua task dalam proyek | - | `List<TaskListDto>` |
| `/{id}` | GET | Get detail task | - | `TaskDetailDto` |
| `` | POST | Buat task baru | `CreateTaskDto` | `TaskDetailDto` |
| `/{id}` | PUT | Update task | `UpdateTaskDto` | `TaskDetailDto` |
| `/{id}/status` | PATCH | Update status task | `{ status }` | `TaskListDto` |
| `/{id}/comments` | GET | Get komentar task | - | `List<TaskCommentDto>` |
| `/{id}/comments` | POST | Tambah komentar | `CreateTaskCommentDto` | `TaskCommentDto` |

### **4. UsersController.cs** (`/api/users`)
| Endpoint | Method | Deskripsi | Request Body | Response |
|----------|--------|-----------|--------------|----------|
| `/me` | GET | Get profil user yang login | - | `UserProfileDto` |
| `/search` | GET | Cari user (untuk assign task/proyek) | `?query=john` | `List<UserSearchResultDto>` |
| `/me/avatar` | POST | Upload avatar | `IFormFile` | `{ avatarUrl }` |

### **5. TimeLogsController.cs** (`/api/timelogs`)
| Endpoint | Method | Deskripsi | Request Body | Response |
|----------|--------|-----------|--------------|----------|
| `/task/{taskId}` | GET | Get time log untuk task | - | `List<TimeLogDto>` |
| `` | POST | Log waktu kerja | `CreateTimeLogDto` | `TimeLogDto` |
| `/{id}` | DELETE | Hapus time log | - | `{ success }` |

---