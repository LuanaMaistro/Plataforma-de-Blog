# Passo a Passo - Desafio Plataforma de Blog (Arquitetura DDD)

Este documento apresenta um guia detalhado para completar o desafio tecnico de desenvolvimento de uma plataforma de blog utilizando **ASP.NET Core** (backend) e **Next.js** (frontend), aplicando os principios de **Domain-Driven Design (DDD)**.

---

## Visao Geral da Arquitetura DDD

### Principios Fundamentais

- **Domain Layer**: Coracao da aplicacao, contem regras de negocio
- **Application Layer**: Orquestra casos de uso e coordena o dominio
- **Infrastructure Layer**: Implementacoes tecnicas (banco, APIs externas)
- **Presentation Layer**: Interface com o mundo externo (API/UI)

### Beneficios

- Separacao clara de responsabilidades
- Codigo mais testavel e manutenivel
- Regras de negocio isoladas e reutilizaveis
- Facilidade para escalar e evoluir o sistema

---

## Fase 1: Configuracao Inicial do Projeto

### 1.1 Criar Estrutura do Repositorio

- [ ] Criar repositorio publico no GitHub
- [ ] Criar pasta `backend/` na raiz
- [ ] Criar pasta `frontend/` na raiz
- [ ] Criar arquivo `README.md` na raiz com instrucoes de setup

### 1.2 Configurar Backend (ASP.NET Core com DDD)

```bash
cd backend

# Criar Solution
dotnet new sln -n Blog

# Criar projetos por camada
dotnet new classlib -n Blog.Domain
dotnet new classlib -n Blog.Application
dotnet new classlib -n Blog.Infrastructure
dotnet new webapi -n Blog.API

# Adicionar projetos a solution
dotnet sln add Blog.Domain/Blog.Domain.csproj
dotnet sln add Blog.Application/Blog.Application.csproj
dotnet sln add Blog.Infrastructure/Blog.Infrastructure.csproj
dotnet sln add Blog.API/Blog.API.csproj

# Configurar referencias entre projetos
cd Blog.Application
dotnet add reference ../Blog.Domain/Blog.Domain.csproj

cd ../Blog.Infrastructure
dotnet add reference ../Blog.Domain/Blog.Domain.csproj
dotnet add reference ../Blog.Application/Blog.Application.csproj

cd ../Blog.API
dotnet add reference ../Blog.Application/Blog.Application.csproj
dotnet add reference ../Blog.Infrastructure/Blog.Infrastructure.csproj
```

**Pacotes NuGet por projeto:**

```bash
# Blog.Domain (sem dependencias externas - mantem puro)

# Blog.Application
dotnet add package MediatR
dotnet add package FluentValidation
dotnet add package AutoMapper

# Blog.Infrastructure
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Pomelo.EntityFrameworkCore.MySql
dotnet add package BCrypt.Net-Next

# Blog.API
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
dotnet add package Swashbuckle.AspNetCore
dotnet add package MediatR.Extensions.Microsoft.DependencyInjection
```

### 1.3 Configurar Frontend (Next.js com DDD)

```bash
cd frontend
npx create-next-app@latest blog-frontend --typescript --tailwind --eslint
cd blog-frontend
npm install axios zod zustand
```

---

## Fase 2: Arquitetura DDD - Backend

### 2.1 Estrutura de Pastas do Backend

```
backend/
├── Blog.sln
├── Blog.Domain/                    # Camada de Dominio
│   ├── Entities/
│   │   ├── Usuario.cs
│   │   └── Postagem.cs
│   ├── ValueObjects/
│   │   ├── Email.cs
│   │   └── SenhaHash.cs
│   ├── Interfaces/
│   │   ├── IUsuarioRepository.cs
│   │   └── IPostagemRepository.cs
│   ├── Services/
│   │   └── PostagemDomainService.cs
│   └── Exceptions/
│       ├── DomainException.cs
│       └── BusinessRuleException.cs
│
├── Blog.Application/               # Camada de Aplicacao
│   ├── DTOs/
│   │   ├── Usuario/
│   │   │   ├── UsuarioRegistroDto.cs
│   │   │   ├── UsuarioLoginDto.cs
│   │   │   └── UsuarioResponseDto.cs
│   │   └── Postagem/
│   │       ├── PostagemCreateDto.cs
│   │       ├── PostagemUpdateDto.cs
│   │       └── PostagemResponseDto.cs
│   ├── UseCases/
│   │   ├── Auth/
│   │   │   ├── RegistrarUsuario/
│   │   │   │   ├── RegistrarUsuarioCommand.cs
│   │   │   │   ├── RegistrarUsuarioHandler.cs
│   │   │   │   └── RegistrarUsuarioValidator.cs
│   │   │   └── LoginUsuario/
│   │   │       ├── LoginUsuarioCommand.cs
│   │   │       ├── LoginUsuarioHandler.cs
│   │   │       └── LoginUsuarioValidator.cs
│   │   └── Postagens/
│   │       ├── CriarPostagem/
│   │       ├── ListarPostagens/
│   │       ├── ObterPostagem/
│   │       ├── EditarPostagem/
│   │       └── DeletarPostagem/
│   ├── Interfaces/
│   │   ├── ITokenService.cs
│   │   └── IPasswordHasher.cs
│   └── Mappings/
│       └── AutoMapperProfile.cs
│
├── Blog.Infrastructure/            # Camada de Infraestrutura
│   ├── Data/
│   │   ├── BlogDbContext.cs
│   │   └── Configurations/
│   │       ├── UsuarioConfiguration.cs
│   │       └── PostagemConfiguration.cs
│   ├── Repositories/
│   │   ├── UsuarioRepository.cs
│   │   └── PostagemRepository.cs
│   ├── Services/
│   │   ├── TokenService.cs
│   │   └── PasswordHasher.cs
│   └── DependencyInjection.cs
│
└── Blog.API/                       # Camada de Apresentacao
    ├── Controllers/
    │   ├── AuthController.cs
    │   └── PostsController.cs
    ├── Middlewares/
    │   └── ExceptionMiddleware.cs
    ├── Extensions/
    │   └── ServiceCollectionExtensions.cs
    └── Program.cs
```

### 2.2 Camada de Dominio (Blog.Domain)

#### 2.2.1 Entidades

**Usuario.cs:**
```csharp
public class Usuario
{
    public int Id { get; private set; }
    public string Nome { get; private set; }
    public Email Email { get; private set; }
    public SenhaHash Senha { get; private set; }
    public ICollection<Postagem> Postagens { get; private set; }
    public DateTime DataCriacao { get; private set; }

    private Usuario() { } // EF Core

    public Usuario(string nome, Email email, SenhaHash senha)
    {
        ValidarNome(nome);
        Nome = nome;
        Email = email;
        Senha = senha;
        DataCriacao = DateTime.UtcNow;
        Postagens = new List<Postagem>();
    }

    private void ValidarNome(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new DomainException("Nome e obrigatorio");
        if (nome.Length < 3)
            throw new DomainException("Nome deve ter pelo menos 3 caracteres");
    }
}
```

**Postagem.cs:**
```csharp
public class Postagem
{
    public int Id { get; private set; }
    public string Titulo { get; private set; }
    public string Conteudo { get; private set; }
    public DateTime DataCriacao { get; private set; }
    public DateTime? DataAtualizacao { get; private set; }
    public int UsuarioId { get; private set; }
    public Usuario Usuario { get; private set; }

    private Postagem() { }

    public Postagem(string titulo, string conteudo, int usuarioId)
    {
        Validar(titulo, conteudo);
        Titulo = titulo;
        Conteudo = conteudo;
        UsuarioId = usuarioId;
        DataCriacao = DateTime.UtcNow;
    }

    public void Atualizar(string titulo, string conteudo)
    {
        Validar(titulo, conteudo);
        Titulo = titulo;
        Conteudo = conteudo;
        DataAtualizacao = DateTime.UtcNow;
    }

    public bool PertenceAoUsuario(int usuarioId) => UsuarioId == usuarioId;

    private void Validar(string titulo, string conteudo)
    {
        if (string.IsNullOrWhiteSpace(titulo))
            throw new DomainException("Titulo e obrigatorio");
        if (string.IsNullOrWhiteSpace(conteudo))
            throw new DomainException("Conteudo e obrigatorio");
    }
}
```

#### 2.2.2 Value Objects

- [ ] Criar `Email.cs` - Valida formato de email
- [ ] Criar `SenhaHash.cs` - Encapsula senha hasheada

```csharp
public class Email
{
    public string Valor { get; }

    public Email(string email)
    {
        if (!IsValid(email))
            throw new DomainException("Email invalido");
        Valor = email.ToLowerInvariant();
    }

    private bool IsValid(string email)
    {
        return !string.IsNullOrWhiteSpace(email) &&
               email.Contains("@") &&
               email.Contains(".");
    }
}
```

#### 2.2.3 Interfaces de Repositorio

- [ ] `IUsuarioRepository` - Contrato para persistencia de usuarios
- [ ] `IPostagemRepository` - Contrato para persistencia de postagens

```csharp
public interface IUsuarioRepository
{
    Task<Usuario?> ObterPorIdAsync(int id);
    Task<Usuario?> ObterPorEmailAsync(string email);
    Task<bool> ExisteEmailAsync(string email);
    Task AdicionarAsync(Usuario usuario);
    Task SalvarAlteracoesAsync();
}

public interface IPostagemRepository
{
    Task<Postagem?> ObterPorIdAsync(int id);
    Task<IEnumerable<Postagem>> ListarTodasAsync();
    Task<IEnumerable<Postagem>> ListarPorUsuarioAsync(int usuarioId);
    Task AdicionarAsync(Postagem postagem);
    Task RemoverAsync(Postagem postagem);
    Task SalvarAlteracoesAsync();
}
```

### 2.3 Camada de Aplicacao (Blog.Application)

#### 2.3.1 DTOs

- [ ] Criar DTOs de entrada (Commands)
- [ ] Criar DTOs de saida (Responses)
- [ ] Manter DTOs simples e focados

#### 2.3.2 Use Cases com MediatR

**Exemplo - CriarPostagemCommand:**
```csharp
public record CriarPostagemCommand(string Titulo, string Conteudo, int UsuarioId)
    : IRequest<PostagemResponseDto>;

public class CriarPostagemHandler : IRequestHandler<CriarPostagemCommand, PostagemResponseDto>
{
    private readonly IPostagemRepository _repository;
    private readonly IMapper _mapper;

    public CriarPostagemHandler(IPostagemRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<PostagemResponseDto> Handle(CriarPostagemCommand request, CancellationToken ct)
    {
        var postagem = new Postagem(request.Titulo, request.Conteudo, request.UsuarioId);

        await _repository.AdicionarAsync(postagem);
        await _repository.SalvarAlteracoesAsync();

        return _mapper.Map<PostagemResponseDto>(postagem);
    }
}
```

#### 2.3.3 Validators com FluentValidation

```csharp
public class CriarPostagemValidator : AbstractValidator<CriarPostagemCommand>
{
    public CriarPostagemValidator()
    {
        RuleFor(x => x.Titulo)
            .NotEmpty().WithMessage("Titulo e obrigatorio")
            .MaximumLength(200).WithMessage("Titulo deve ter no maximo 200 caracteres");

        RuleFor(x => x.Conteudo)
            .NotEmpty().WithMessage("Conteudo e obrigatorio");
    }
}
```

### 2.4 Camada de Infraestrutura (Blog.Infrastructure)

#### 2.4.1 DbContext e Configuracoes

```csharp
public class BlogDbContext : DbContext
{
    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Postagem> Postagens => Set<Postagem>();

    public BlogDbContext(DbContextOptions<BlogDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BlogDbContext).Assembly);
    }
}
```

#### 2.4.2 Implementacao dos Repositorios

```csharp
public class PostagemRepository : IPostagemRepository
{
    private readonly BlogDbContext _context;

    public PostagemRepository(BlogDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Postagem>> ListarTodasAsync()
    {
        return await _context.Postagens
            .Include(p => p.Usuario)
            .OrderByDescending(p => p.DataCriacao)
            .ToListAsync();
    }

    // ... demais metodos
}
```

#### 2.4.3 Services (Token, Password)

- [ ] `TokenService` - Implementa geracao de JWT
- [ ] `PasswordHasher` - Implementa BCrypt

### 2.5 Camada de API (Blog.API)

#### 2.5.1 Controllers usando MediatR

```csharp
[ApiController]
[Route("api/posts")]
[Authorize]
public class PostsController : ControllerBase
{
    private readonly IMediator _mediator;

    public PostsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] PostagemCreateDto dto)
    {
        var usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var command = new CriarPostagemCommand(dto.Titulo, dto.Conteudo, usuarioId);
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(ObterPorId), new { id = result.Id }, result);
    }

    [HttpGet]
    public async Task<IActionResult> Listar([FromQuery] bool meus_posts = false)
    {
        var usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var query = new ListarPostagensQuery(meus_posts ? usuarioId : null);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    // ... demais endpoints
}
```

---

## Fase 3: Arquitetura DDD - Frontend

### 3.1 Estrutura de Pastas do Frontend

```
frontend/blog-frontend/
├── src/
│   ├── domain/                     # Camada de Dominio
│   │   ├── entities/
│   │   │   ├── Usuario.ts
│   │   │   └── Postagem.ts
│   │   ├── value-objects/
│   │   │   └── Email.ts
│   │   └── interfaces/
│   │       ├── IAuthRepository.ts
│   │       └── IPostRepository.ts
│   │
│   ├── application/                # Camada de Aplicacao
│   │   ├── use-cases/
│   │   │   ├── auth/
│   │   │   │   ├── loginUseCase.ts
│   │   │   │   └── registerUseCase.ts
│   │   │   └── posts/
│   │   │       ├── createPostUseCase.ts
│   │   │       ├── listPostsUseCase.ts
│   │   │       ├── getPostUseCase.ts
│   │   │       ├── updatePostUseCase.ts
│   │   │       └── deletePostUseCase.ts
│   │   ├── dtos/
│   │   │   ├── LoginDto.ts
│   │   │   ├── RegisterDto.ts
│   │   │   └── PostDto.ts
│   │   └── validators/
│   │       ├── loginValidator.ts
│   │       └── postValidator.ts
│   │
│   ├── infrastructure/             # Camada de Infraestrutura
│   │   ├── http/
│   │   │   └── axiosClient.ts
│   │   ├── repositories/
│   │   │   ├── AuthRepository.ts
│   │   │   └── PostRepository.ts
│   │   └── storage/
│   │       └── tokenStorage.ts
│   │
│   ├── presentation/               # Camada de Apresentacao
│   │   ├── components/
│   │   │   ├── ui/
│   │   │   │   ├── Button.tsx
│   │   │   │   ├── Input.tsx
│   │   │   │   ├── Card.tsx
│   │   │   │   └── Loading.tsx
│   │   │   ├── forms/
│   │   │   │   ├── LoginForm.tsx
│   │   │   │   ├── RegisterForm.tsx
│   │   │   │   └── PostForm.tsx
│   │   │   └── layout/
│   │   │       ├── Header.tsx
│   │   │       └── Footer.tsx
│   │   ├── hooks/
│   │   │   ├── useAuth.ts
│   │   │   └── usePosts.ts
│   │   └── stores/
│   │       ├── authStore.ts
│   │       └── postStore.ts
│   │
│   └── app/                        # Rotas Next.js (App Router)
│       ├── page.tsx
│       ├── login/page.tsx
│       ├── register/page.tsx
│       ├── posts/
│       │   ├── page.tsx
│       │   ├── [id]/page.tsx
│       │   ├── new/page.tsx
│       │   └── edit/[id]/page.tsx
│       └── my-posts/page.tsx
```

### 3.2 Camada de Dominio (Frontend)

#### 3.2.1 Entidades

**entities/Usuario.ts:**
```typescript
export interface Usuario {
  id: number;
  nome: string;
  email: string;
}

export interface UsuarioAutenticado extends Usuario {
  token: string;
}
```

**entities/Postagem.ts:**
```typescript
export interface Postagem {
  id: number;
  titulo: string;
  conteudo: string;
  dataCriacao: Date;
  autor: {
    id: number;
    nome: string;
  };
}

export class PostagemEntity {
  constructor(
    public readonly id: number,
    public readonly titulo: string,
    public readonly conteudo: string,
    public readonly dataCriacao: Date,
    public readonly autorId: number,
    public readonly autorNome: string
  ) {}

  pertenceAoUsuario(usuarioId: number): boolean {
    return this.autorId === usuarioId;
  }

  get resumo(): string {
    return this.conteudo.length > 150
      ? this.conteudo.substring(0, 150) + '...'
      : this.conteudo;
  }
}
```

#### 3.2.2 Interfaces de Repositorio

```typescript
// interfaces/IAuthRepository.ts
export interface IAuthRepository {
  login(email: string, senha: string): Promise<UsuarioAutenticado>;
  register(nome: string, email: string, senha: string): Promise<void>;
}

// interfaces/IPostRepository.ts
export interface IPostRepository {
  listar(apenasMinhas?: boolean): Promise<Postagem[]>;
  obterPorId(id: number): Promise<Postagem>;
  criar(titulo: string, conteudo: string): Promise<Postagem>;
  atualizar(id: number, titulo: string, conteudo: string): Promise<Postagem>;
  deletar(id: number): Promise<void>;
}
```

### 3.3 Camada de Aplicacao (Frontend)

#### 3.3.1 Use Cases

**use-cases/posts/createPostUseCase.ts:**
```typescript
import { IPostRepository } from '@/domain/interfaces/IPostRepository';
import { CreatePostDto } from '@/application/dtos/PostDto';
import { postValidator } from '@/application/validators/postValidator';

export class CreatePostUseCase {
  constructor(private postRepository: IPostRepository) {}

  async execute(dto: CreatePostDto) {
    // Validacao
    const validation = postValidator.safeParse(dto);
    if (!validation.success) {
      throw new Error(validation.error.errors[0].message);
    }

    // Execucao
    return await this.postRepository.criar(dto.titulo, dto.conteudo);
  }
}
```

#### 3.3.2 Validators com Zod

**validators/postValidator.ts:**
```typescript
import { z } from 'zod';

export const postValidator = z.object({
  titulo: z.string()
    .min(1, 'Titulo e obrigatorio')
    .max(200, 'Titulo deve ter no maximo 200 caracteres'),
  conteudo: z.string()
    .min(1, 'Conteudo e obrigatorio')
});

export type CreatePostDto = z.infer<typeof postValidator>;
```

### 3.4 Camada de Infraestrutura (Frontend)

#### 3.4.1 HTTP Client

**http/axiosClient.ts:**
```typescript
import axios from 'axios';
import { tokenStorage } from '../storage/tokenStorage';

export const api = axios.create({
  baseURL: process.env.NEXT_PUBLIC_API_URL || 'http://localhost:5000/api'
});

api.interceptors.request.use((config) => {
  const token = tokenStorage.get();
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

api.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response?.status === 401) {
      tokenStorage.remove();
      window.location.href = '/login';
    }
    return Promise.reject(error);
  }
);
```

#### 3.4.2 Implementacao dos Repositorios

**repositories/PostRepository.ts:**
```typescript
import { IPostRepository } from '@/domain/interfaces/IPostRepository';
import { Postagem } from '@/domain/entities/Postagem';
import { api } from '../http/axiosClient';

export class PostRepository implements IPostRepository {
  async listar(apenasMinhas = false): Promise<Postagem[]> {
    const params = apenasMinhas ? { meus_posts: true } : {};
    const response = await api.get('/posts', { params });
    return response.data;
  }

  async obterPorId(id: number): Promise<Postagem> {
    const response = await api.get(`/posts/${id}`);
    return response.data;
  }

  async criar(titulo: string, conteudo: string): Promise<Postagem> {
    const response = await api.post('/posts', { titulo, conteudo });
    return response.data;
  }

  async atualizar(id: number, titulo: string, conteudo: string): Promise<Postagem> {
    const response = await api.put(`/posts/${id}`, { titulo, conteudo });
    return response.data;
  }

  async deletar(id: number): Promise<void> {
    await api.delete(`/posts/${id}`);
  }
}
```

### 3.5 Camada de Apresentacao (Frontend)

#### 3.5.1 Stores com Zustand

**stores/authStore.ts:**
```typescript
import { create } from 'zustand';
import { Usuario } from '@/domain/entities/Usuario';
import { tokenStorage } from '@/infrastructure/storage/tokenStorage';

interface AuthState {
  user: Usuario | null;
  isAuthenticated: boolean;
  setUser: (user: Usuario, token: string) => void;
  logout: () => void;
}

export const useAuthStore = create<AuthState>((set) => ({
  user: null,
  isAuthenticated: false,
  setUser: (user, token) => {
    tokenStorage.set(token);
    set({ user, isAuthenticated: true });
  },
  logout: () => {
    tokenStorage.remove();
    set({ user: null, isAuthenticated: false });
  }
}));
```

#### 3.5.2 Custom Hooks

**hooks/usePosts.ts:**
```typescript
import { useState, useEffect } from 'react';
import { Postagem } from '@/domain/entities/Postagem';
import { PostRepository } from '@/infrastructure/repositories/PostRepository';
import { ListPostsUseCase } from '@/application/use-cases/posts/listPostsUseCase';

export function usePosts(apenasMinhas = false) {
  const [posts, setPosts] = useState<Postagem[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const repository = new PostRepository();
    const useCase = new ListPostsUseCase(repository);

    useCase.execute(apenasMinhas)
      .then(setPosts)
      .catch((err) => setError(err.message))
      .finally(() => setLoading(false));
  }, [apenasMinhas]);

  return { posts, loading, error };
}
```

#### 3.5.3 Componentes UI

- [ ] `Button` - Variantes: primary, secondary, danger
- [ ] `Input` - Com label, erro, estados
- [ ] `Card` - Container estilizado
- [ ] `Loading` - Spinner animado
- [ ] `ErrorMessage` - Alerta de erro
- [ ] `PostCard` - Card de postagem
- [ ] `PostForm` - Formulario reutilizavel

#### 3.5.4 Paginas

Cada pagina deve:
- [ ] Usar hooks para carregar dados
- [ ] Chamar use cases para acoes
- [ ] Exibir estados de loading/erro
- [ ] Redirecionar quando necessario

---

## Fase 4: Configuracao do Banco de Dados

### 4.1 MySQL Setup

- [ ] Instalar MySQL ou usar Docker

```bash
docker run --name blog-mysql -e MYSQL_ROOT_PASSWORD=root -e MYSQL_DATABASE=blog_db -p 3306:3306 -d mysql:8
```

### 4.2 Connection String

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=blog_db;User=root;Password=root;"
  }
}
```

### 4.3 Migrations

```bash
cd Blog.Infrastructure
dotnet ef migrations add InitialCreate --startup-project ../Blog.API
dotnet ef database update --startup-project ../Blog.API
```

---

## Fase 5: Testes

### 5.1 Testes Unitarios - Backend

- [ ] Testes para entidades de dominio
- [ ] Testes para value objects
- [ ] Testes para handlers/use cases
- [ ] Usar mocks para repositorios

### 5.2 Testes Unitarios - Frontend

- [ ] Testes para use cases
- [ ] Testes para validators
- [ ] Testes para hooks
- [ ] Testes para componentes

### 5.3 Testes de Integracao

- [ ] Testar fluxos completos
- [ ] Testar API endpoints
- [ ] Testar autenticacao

---

## Fase 6: Documentacao

### 6.1 README.md

```markdown
# Plataforma de Blog - DDD

## Arquitetura
Projeto desenvolvido seguindo Domain-Driven Design (DDD):
- **Domain Layer**: Regras de negocio
- **Application Layer**: Casos de uso
- **Infrastructure Layer**: Implementacoes tecnicas
- **Presentation Layer**: API/UI

## Tecnologias
### Backend
- ASP.NET Core 8
- Entity Framework Core
- MySQL
- MediatR (CQRS)
- FluentValidation
- JWT Authentication

### Frontend
- Next.js 14
- TypeScript
- Tailwind CSS
- Zustand (State Management)
- Zod (Validation)
- Axios

## Como Executar

### Pre-requisitos
- .NET 8 SDK
- Node.js 18+
- MySQL 8+ ou Docker

### Backend
```bash
cd backend
dotnet restore
dotnet ef database update --project Blog.Infrastructure --startup-project Blog.API
dotnet run --project Blog.API
```

### Frontend
```bash
cd frontend/blog-frontend
npm install
npm run dev
```

## Estrutura do Projeto
[Documentar estrutura DDD]
```

---

## Fase 7: Deploy (Diferencial)

### 7.1 Backend
- [ ] Configurar variaveis de ambiente
- [ ] Deploy em Render/Railway/Azure
- [ ] MySQL em nuvem (PlanetScale)

### 7.2 Frontend
- [ ] Configurar NEXT_PUBLIC_API_URL
- [ ] Deploy em Vercel

---

## Checklist Final

### Arquitetura DDD
- [ ] Camadas bem separadas e independentes
- [ ] Domain sem dependencias externas
- [ ] Use Cases implementados
- [ ] Repositorios com interfaces no dominio
- [ ] Injecao de dependencia configurada

### Backend
- [ ] Entidades com regras de negocio
- [ ] Value Objects validados
- [ ] DTOs para entrada/saida
- [ ] Autenticacao JWT
- [ ] Validacoes com FluentValidation
- [ ] CORS configurado

### Frontend
- [ ] Estrutura DDD aplicada
- [ ] Use Cases isolados
- [ ] State management com Zustand
- [ ] Validacoes com Zod
- [ ] Componentes reutilizaveis
- [ ] Interface responsiva

### Geral
- [ ] README completo
- [ ] Codigo limpo e organizado
- [ ] Repositorio publico no GitHub
- [ ] (Diferencial) Deploy em producao
- [ ] (Diferencial) Testes unitarios

---

## Recursos Uteis

### DDD
- [Domain-Driven Design Reference](https://www.domainlanguage.com/ddd/reference/)
- [Clean Architecture with ASP.NET Core](https://docs.microsoft.com/en-us/dotnet/architecture/modern-web-apps-azure/)

### Backend
- [ASP.NET Core Documentation](https://docs.microsoft.com/aspnet/core)
- [MediatR Documentation](https://github.com/jbogard/MediatR)
- [FluentValidation](https://docs.fluentvalidation.net/)

### Frontend
- [Next.js Documentation](https://nextjs.org/docs)
- [Zustand](https://github.com/pmndrs/zustand)
- [Zod](https://zod.dev/)
- [Tailwind CSS](https://tailwindcss.com/docs)
