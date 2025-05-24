# 🧪 Ambev Developer Evaluation - Backend

Projeto backend desenvolvido como parte do desafio técnico da Ambev. Esta aplicação é baseada em ASP.NET Core com Entity Framework Core, seguindo boas práticas de arquitetura limpa (DDD, separação por camadas, mapeamentos, etc), e utiliza Docker para facilitar a configuração do ambiente.

---

## 📦 Tecnologias

- [.NET 8](https://dotnet.microsoft.com/en-us/)
- [Entity Framework Core](https://learn.microsoft.com/ef/core)
- [PostgreSQL](https://www.postgresql.org/)
- [Docker & Docker Compose](https://docs.docker.com/)
- [AutoMapper](https://automapper.org/)
- [FluentValidation](https://docs.fluentvalidation.net/)
- [Dapper (opcional para otimizações futuras)](https://dapper-tutorial.net/)
- Clean Architecture / DDD

---

## 🛠️ Como rodar o projeto localmente

### 🔁 Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download)
- [Docker](https://www.docker.com/products/docker-desktop/)
- [DBeaver (opcional, para visualização do banco)](https://dbeaver.io/)

---

### 🚀 Passo a passo

1. **Clone o repositório**

```bash
git clone https://github.com/VanMachado/AmbevDeveloperEvaluation.git
cd seu-repo
```

2. **Suba o banco de dados com Docker Compose**

```bash
docker-compose --project-name ambev-developer-evaluation up -d
```



3. **Garanta que a porta padrão 5432 esteja disponível. Caso precise alterar, edite o `docker-compose.yml` para fixar a porta:**

```bash
ports: 
    - "5432:5432"
```

4. **Configure o `appsettings.Development.json` com a string de conexão**

```bash
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=ambev_eval;Username=postgres;Password=postgres"
}
```

5. **Execute as migrations**
   Mudar para o projeto de ORM para que o EF identifique as migrations, casso a tag --project nao funcione, e importante executá-la antes de testar a API!

```bash
dotnet ef database update
```

📁 Estrutura do Projeto

```bash
src/
├── Ambev.DeveloperEvaluation.WebApi         # Camada de API (Controllers, Endpoints)
├── Ambev.DeveloperEvaluation.Application    # Casos de uso (Handlers, Services)
├── Ambev.DeveloperEvaluation.Domain         # Entidades, Enums, Interfaces
├── Ambev.DeveloperEvaluation.ORM            # DbContext, Migrations, Repositórios EF
tests/
├── Ambev.DeveloperEvaluation.Tests          # Testes automatizados (xUnit)
docker-compose.yml                           # Banco de dados PostgreSQL

```

🧪 Endpoints de Exemplo
-----------------------

* `POST /api/users`

* `GET /api/users/{id}`

* `POST /api/sales`

* `GET /api/sales/{id}`

✅ Boas práticas utilizadas
--------------------------

* Separação de camadas (Domain, Application, Infrastructure, WebApi)

* Mapeamento com AutoMapper

* Padrões Repository e Unit of Work

* FluentValidation para validação de entrada

* Migrations controladas com EF Core

* Configuração via Docker

* `.gitignore` adaptado para Docker e .NET
