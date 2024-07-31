# Sistema de Blog com Autenticação

Um sistema de blog onde usuários podem criar, editar e comentar em posts, com autenticação integrada.

## Funcionalidades
- Criação e edição de posts.
- Comentários em posts.
- Sistema de autenticação e autorização.

## Tecnologias
- ASP.NET Core
- jwt Bearer 
- SqlServer
- Entity Framework 

## Como Rodar
1. Clone o repositório.
2. Configure o banco de dados PostgreSQL.
3. Atualize a string de conexão no arquivo `appsettings.json`.
4. Execute o projeto com `dotnet run`.

## Endpoints
- `GET /api/posts`
- `POST /api/posts`
- `PUT /api/posts/{id}`
- `DELETE /api/posts/{id}`
- `POST /api/comments`
