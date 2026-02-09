Desafio de desenvolvimento

Soluções que geram valor

(Next.js + ASP.NET)
Proposta: Plataforma de Blog Simples
Você deverá desenvolver uma plataforma de blog simples voltada para pessoas que desejam expressar ideias,
compartilhar aprendizados ou publicar conteúdos autorais. A aplicação permitirá que usuários se registrem, façam login,
e possam criar, visualizar, editar e excluir suas próprias postagens, além de acessar conteúdos públicos de outros
autores.
Essa plataforma pode ser usada como um espaço de publicação pessoal, blog técnico, diário digital ou até mesmo uma
ferramenta de aprendizado colaborativo.
Backend
Desenvolva uma API RESTful utilizando ASP.NET Core.
Tecnologias Obrigatórias:
- Linguagem: C# (.NET 6 ou superior recomendado)
- Framework: ASP.NET Core Web API
- Gerenciador de dependências: NuGet
- Banco de Dados: MySQL

Modelagem de Dados:
- Usuario: Deve conter id, nome, email e senha com hash.
- Postagem: Deve conter id, titulo, conteudo, dataCriacao e um
relacionamento @ManyToOne com Usuario (para referenciar o autor).
Rotas da API:
1. POST /api/auth/register: Cadastra um novo usuário. A senha deve ser armazenada usando um encoder
(ex: BCryptPasswordEncoder).
2. POST /api/auth/login: Autentica um usuário com e-mail e senha. Em caso de sucesso, deve retornar
um token JWT contendo informações do usuário (como ID e e-mail).
3. POST /api/posts: [Requer autenticação] Cadastra uma nova postagem associada ao usuário autenticado.
4. GET /api/posts: [Requer autenticação] Retorna uma lista de todas as postagens, ordenadas da mais recente
para a mais antiga.
- Deve aceitar um parâmetro opcional, ex: ?meus_posts=true, para retornar apenas as postagens do
usuário que fez a requisição.

5. GET /api/posts/{id}: [Requer autenticação] Retorna os detalhes da postagem com o ID especificado.
6. PUT /api/posts/{id}: [Requer autenticação e autorização] Edita uma postagem. A API deve validar se o
usuário autenticado é o autor da postagem antes de permitir a alteração.
7. DELETE /api/posts/{id}: [Requer autenticação e autorização] Deleta uma postagem. A API deve validar se o
usuário autenticado é o autor da postagem.

Desafio de desenvolvimento

Soluções que geram valor

Frontend (Next.js + Tailwind CSS)
Desenvolva uma aplicação web do tipo SPA utilizando Next.js, com renderização baseada em rotas e componentes
reutilizáveis. Use Tailwind CSS para estilização.
Tecnologias Recomendadas:
- Framework: Next.js (com suporte a rotas e páginas dinâmicas)
- HTTP Client: Axios
- Estilização: Tailwind CSS
- Gerenciamento de estado (opcional): Context API, Zustand ou outro.

Histórias de Usuário:
1. Como visitante, eu desejo me cadastrar na plataforma para poder criar postagens.
2. Como visitante, eu desejo realizar login para acessar minha conta.
3. Como usuário autenticado, eu desejo visualizar uma página com todas as postagens de todos os usuários.
4. Como usuário autenticado, eu desejo clicar em uma postagem para ver seus detalhes em uma página
separada.
5. Como usuário autenticado, eu desejo ter uma página ou um filtro para visualizar apenas as minhas postagens.
6. Como usuário autenticado, eu desejo ter um formulário para criar uma nova postagem.
7. Como usuário autenticado, eu desejo editar uma postagem que eu criei.
8. Como usuário autenticado, eu desejo deletar uma postagem que eu criei.

Diferenciais
- Qualidade de Código: Uso de DTOs (Data Transfer Objects) para desacoplar a API das entidades.
- Deploy: Aplicação publicada em um ambiente de nuvem (ex: backend no Heroku/Render e frontend no
Vercel/Netlify).
- UX/UI: Uma interface limpa, responsiva e com feedback visual para o usuário (ex: loaders durante
requisições, mensagens de sucesso/erro).

Instruções Gerais
1. Crie um novo repositório público no GitHub.
2. Estruture o projeto com duas pastas principais: backend e frontend.
3. Crie um arquivo README.md na raiz do projeto com um passo a passo claro de como configurar e rodar a
sua aplicação.
4. Ao finalizar, nos envie o link do repositório.