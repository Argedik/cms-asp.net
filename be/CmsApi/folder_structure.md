# Klasörleme Mimarisi

be/CmsApi/
├── Controllers/          # API endpoints (SRP)
├── Services/            # Business logic (SRP)
│   └── Interfaces/      # Service contracts (DIP)
├── Repositories/        # Data access (SRP)
│   └── Interfaces/      # Repository contracts (DIP)
├── DTOs/               # Data transfer objects
│   ├── Requests/       # Input models
│   └── Responses/      # Output models
├── Models/             # Domain entities
├── Data/               # EF Context
├── Validators/         # Input validation (SRP)
├── Middleware/         # Cross-cutting concerns
├── Extensions/         # DI container extensions
├── Common/            # Shared utilities
└── Migrations/        # EF migrations
