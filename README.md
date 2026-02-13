# Plataforma de Blog - DDD

Plataforma de blog completa desenvolvida aplicando os principios de **Domain-Driven Design (DDD)** tanto no backend quanto no frontend.

---

## Arquitetura

O projeto segue uma arquitetura em camadas baseada em DDD, com separação clara de responsabilidades:

```
┌────────────────────────────────────────────────────┐
│  Presentation    │  API (Controllers) / UI    │
├────────────────────────────────────────────────────┤
│  Application     │  Use Cases, DTOs, Validators│
├────────────────────────────────────────────────────┤
│  Domain          │  Entidades, Value Objects   │
├────────────────────────────────────────────────────┤
│  Infrastructure  │  Banco de Dados, HTTP, JWT  │
└────────────────────────────────────────────────────┘
```

- **Domain Layer**: Coração da aplicacao — entidades, value objects, interfaces de repositorio e excecoes de dominio. Sem dependencias externas.
- **Application Layer**: Orquestra os casos de uso via MediatR (CQRS), validacoes com FluentValidation/Zod e mapeamentos com AutoMapper.
- **Infrastructure Layer**: Implementações concretas — Entity Framework Core, repositorios, servicos de token JWT e hashing de senha.
- **Presentation Layer**: Controllers REST (backend) e componentes React (frontend).

---

## Tecnologias

### Backend

| Tecnologia | Finalidade |
|---|---|
| ASP.NET Core 9 | Framework web |
| Entity Framework Core 9 | ORM |
| MySQL 8 (Pomelo) | Banco de dados |
| MediatR | CQRS / Mediator pattern |
| FluentValidation | Validacao de commands/queries |
| AutoMapper | Mapeamento entidade → DTO |
| JWT Bearer | Autenticacao |
| BCrypt.Net | Hashing de senhas |
| Swagger/OpenAPI | Documentacao da API |
| xUnit | Testes unitarios |

### Frontend

| Tecnologia | Finalidade |
|---|---|
| Next.js 16 (App Router) | Framework React |
| React 19 | Biblioteca UI |
| TypeScript | Tipagem estatica |
| Tailwind CSS 4 | Estilizacao |
| Zustand | Gerenciamento de estado |
| Zod | Validacao de schemas |
| Axios | Cliente HTTP |
| Lucide React | Icones |

---

## Como Executar

### Pre-requisitos

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Node.js 18+](https://nodejs.org/)
- [MySQL 8+](https://dev.mysql.com/downloads/) 

### Banco de Dados

**MySQL local:**

Crie o banco `blog_db` com usuario `root` e senha `root`.

### Backend

```bash
cd backend

# Restaurar dependencias
dotnet restore

# Aplicar migrations
dotnet ef database update --project Blog.Infrastructure --startup-project Blog.API

# Executar a API (porta 5243)
dotnet run --project Blog.API
```

A API estara disponivel em `http://localhost:5243` com Swagger em `http://localhost:5243/swagger`.

### Frontend

```bash
cd frontend/blog-frontend

# Instalar dependencias
npm install

# Executar em desenvolvimento (porta 3000)
npm run dev
```

A aplicacao estara disponivel em `http://localhost:3000`.

---

## Estrutura do Projeto

```
├── backend/
│   ├── Blog.Domain/                 # Entidades, Value Objects, Interfaces
│   ├── Blog.Application/           # Use Cases (MediatR), DTOs, Validators
│   ├── Blog.Infrastructure/        # EF Core, Repositorios, Services
│   ├── Blog.API/                   # Controllers, Middlewares, Program.cs
│   ├── Blog.Domain.Tests/          # Testes unitarios do dominio
│   └── Blog.Application.Tests/     # Testes unitarios da aplicacao
│
└── frontend/blog-frontend/
    └── src/
        ├── domain/                  # Entidades, Interfaces de repositorio
        ├── application/             # Use Cases, Validators (Zod), DTOs
        ├── infrastructure/          # Axios client, Repositorios, Storage
        ├── presentation/            # Componentes, Hooks, Stores (Zustand)
        └── app/                     # Rotas Next.js (App Router)
```

---

## Endpoints da API

### Autenticacao

| Metodo | Rota | Descricao | Auth |
|---|---|---|---|
| POST | `/api/auth/register` | Registrar usuario | Nao |
| POST | `/api/auth/login` | Login (retorna JWT) | Nao |
| PUT | `/api/auth/profile` | Atualizar perfil | Sim |

### Postagens

| Metodo | Rota | Descricao | Auth |
|---|---|---|---|
| GET | `/api/posts` | Listar todas as postagens | Nao |
| GET | `/api/posts?meus_posts=true` | Listar postagens do usuario | Sim |
| GET | `/api/posts/{id}` | Obter postagem por ID | Nao |
| POST | `/api/posts` | Criar postagem | Sim |
| PUT | `/api/posts/{id}` | Editar postagem (dono) | Sim |
| DELETE | `/api/posts/{id}` | Deletar postagem (dono) | Sim |

---

## Testes

### Backend

```bash
cd backend
dotnet test
```

Cobertura de testes:

- **Domain**: Entidades (Usuario, Postagem) e Value Objects (Email, SenhaHash)
- **Application**: Handlers de todos os use cases e validators (FluentValidation)

---

## Funcionalidades

- Registro e login de usuarios com JWT
- CRUD completo de postagens
- Visualizacao de todas as postagens ou apenas as proprias
- Edicao e exclusao restrita ao autor da postagem
- Atualizacao de perfil (nome e senha)
- Validacao client-side (Zod) e server-side (FluentValidation)
- Tratamento global de erros (ExceptionMiddleware)
- Interface responsiva com Tailwind CSS

## Video


https://github.com/user-attachments/assets/7c6e5a76-da55-40e0-85e5-2a1bfc077b4f





