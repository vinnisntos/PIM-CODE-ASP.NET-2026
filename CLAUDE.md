# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

Studio Beauty - Sistema de agendamento para salão de sobrancelhas.
- **Stack:** ASP.NET Core 10 Razor Pages + **Supabase (PostgreSQL)**
- **ORM:** Entity Framework Core 9 with Npgsql
- **Arquitetura:** MVC com Razor Pages
- **Autenticação:** Session/Cookie-based com BCrypt
- **Perfis:** Cliente (agenda) e Profissional (gerencia)

## Commands

```bash
# Restore packages
dotnet restore

# Build
dotnet build

# Run
dotnet run

# Clean
dotnet clean

# EF Migrations (optional, app auto-migrates on startup)
dotnet ef migrations add MigrationName
dotnet ef database update
```

## Database: Supabase (PostgreSQL)

### Connection String Format
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=your-project.supabase.co;Port=5432;Database=postgres;Username=postgres;Password=your-password;SSL Mode=Require;Trust Server Certificate=true"
  }
}
```

### Setup Instructions
1. Crie um projeto no [Supabase](https://supabase.com)
2. No SQL Editor, execute o arquivo `script_supabase.sql`
3. Copie a Connection Pooling URL (transaction mode) de Settings > Database
4. Cole no `appsettings.json`

### Tables (PostgreSQL)
- **usuarios** - Clientes e profissionais (IdentityByDefault)
- **servicos** - Serviços oferecidos
- **agendamentos** - Agendamentos com FKs e cascata

### Key PostgreSQL Features Used
- `timestamp with time zone` para datas
- `SERIAL` / `IdentityByDefault` para PKs
- `NOW() AT TIME ZONE 'UTC'` para timestamps
- Índices otimizados para queries frequentes
- Collation pt_BR.utf8

## Project Structure

- **Pages/** - Razor Pages organizadas por área:
  - `Cliente/` - Agendar, Sucesso, Historico
  - `Profissional/` - Dashboard, Servicos, Relatorios
  - `Shared/_Layout.cshtml` - Layout com menus por perfil
  
- **Models/** - Entidades:
  - Usuario, Servico, Agendamento
  - Timestamps em UTC
  
- **Data/** - EF Core DbContext + DbInitializer
  - `AppDbContext.cs` - Configurado para PostgreSQL
  - Auto-migration no startup

- **Services/** - AuthHelper, PasswordHasher (BCrypt), AgendamentoService

- **Migrations/** - Migrations do EF Core para PostgreSQL

## Security

- Passwords hashed with BCrypt (work factor 12)
- Session-based auth com 2h timeout
- Profile-based access control
- HTTPS enforced

## Admin Credentials

- Email: admin@studiobeauty.com
- Password: Admin@123
- Perfil: Profissional

## Common Tasks

- **Connection issues:** Verifique SSL Mode=Require e Trust Server Certificate
- **Migration errors:** Delete Migrations folder e recrie com `dotnet ef migrations add Initial`
- **Timezone:** Todas as datas são UTC no banco, convertidas localmente na aplicação
- **Add field:** Update model + `dotnet ef migrations add FieldName`

## Files for Setup

1. `script_supabase.sql` - Execute no Supabase SQL Editor
2. `appsettings.json` - Configure connection string
3. `Migrations/` - Já contém migration inicial (opcional)
