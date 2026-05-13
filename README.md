# TierLab Backend

API backend do projeto TierLab, construído com **.NET 9** seguindo **Clean Architecture**.

## 📁 Estrutura do Projeto

```
TierLab.sln
└── src/
    ├── TierLab.Domain/           # Entidades, Value Objects, Interfaces de domínio
    │   ├── Common/               # Entity, ValueObject, ISoftDeletable
    │   └── Interfaces/           # IRepository<T>, IUnitOfWork
    │
    ├── TierLab.Application/      # Casos de uso, DTOs, Validadores
    │   ├── Common/               # Result, PagedResult, Exceptions
    │   └── Interfaces/           # Contratos de serviços de aplicação
    │
    ├── TierLab.Infrastructure/   # Implementações concretas (EF Core, Repositórios)
    │   └── Persistence/          # DbContext, UnitOfWork, RepositoryBase
    │
    └── TierLab.Api/              # Controllers, Middleware, Configuração
        ├── Controllers/          # BaseController, HealthController
        └── Middleware/           # GlobalExceptionMiddleware
```

## 🏗️ Arquitetura

Segue os princípios da **Clean Architecture**:

- **Domain** → Núcleo puro, sem dependências externas
- **Application** → Orquestra casos de uso, define contratos
- **Infrastructure** → Implementa persistência e serviços externos
- **Api** → Ponto de entrada HTTP, middleware, configuração

## 🚀 Como Rodar

```bash
# Restaurar dependências
dotnet restore

# Executar em modo desenvolvimento
dotnet run --project src/TierLab.Api

# Build de produção
dotnet build -c Release
```

## 🔧 Configuração

- **Banco de dados**: PostgreSQL (configurável em `appsettings.json`)
- **Logging**: Serilog (console + arquivo)
- **Documentação**: Swagger disponível em `/swagger` (apenas em Development)

## 📦 Tecnologias

| Camada         | Tecnologia                          |
|----------------|-------------------------------------|
| Runtime        | .NET 9                              |
| ORM            | Entity Framework Core 9             |
| Banco de Dados | PostgreSQL (Npgsql)                 |
| Validação      | FluentValidation                    |
| Logging        | Serilog                             |
| Docs           | Swagger / OpenAPI                   |
