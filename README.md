# Studio Beauty - Sistema de Agendamento

Sistema completo de agendamento para salão de sobrancelhas, desenvolvido em ASP.NET Core Razor Pages com SQL Server.

---

## 🚀 Como Executar o Projeto

### Pré-requisitos
- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [SQL Server 2022](https://www.microsoft.com/pt-br/sql-server/sql-server-downloads) ou SQL Server Express
- [SQL Server Management Studio (SSMS) 22](https://learn.microsoft.com/pt-br/sql/ssms/download-sql-server-management-studio-ssms)

### Passo 1: Configurar o Banco de Dados

1. Abra o **SSMS 22**
2. Conecte-se ao seu servidor SQL (localhost\SQLEXPRESS ou localhost)
3. Abra o arquivo `script_banco.sql` deste projeto
4. Execute o script (F5 ou botão "Executar")
5. Verifique se o banco `StudioBeautyPIM` foi criado

### Passo 2: Configurar a Connection String

Verifique o arquivo `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=StudioBeautyPIM;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

Ajuste o `Server` conforme sua instalação do SQL Server.

### Passo 3: Instalar Pacotes e Rodar

No terminal (na pasta do projeto):

```bash
# Restaurar pacotes
dotnet restore

# Rodar o projeto
dotnet run
```

Ou abra no **Visual Studio** e pressione **F5**.

---

## 🔐 Acesso ao Sistema

### Perfil Profissional (Admin)
- **Email:** admin@studiobeauty.com
- **Senha:** Admin@123

### Perfil Cliente
- Cadastre-se na página inicial ou use o login de teste

---

## 📁 Estrutura do Projeto

```
PIM2026/
├── Data/
│   ├── AppDbContext.cs      # Contexto do Entity Framework
│   └── DbInitializer.cs     # Seed de dados iniciais
├── Models/
│   ├── Usuario.cs           # Entidade Cliente/Profissional
│   ├── Servico.cs           # Entidade Serviço
│   └── Agendamento.cs       # Entidade Agendamento
├── Pages/
│   ├── Cliente/             # Área do Cliente
│   │   ├── Agendar.cshtml   # Agendar novo horário
│   │   ├── Sucesso.cshtml   # Confirmação de agendamento
│   │   └── Historico.cshtml # Histórico de agendamentos
│   ├── Profissional/        # Área da Profissional
│   │   ├── Dashboard.cshtml # Painel principal
│   │   ├── Servicos.cshtml  # Gerenciar serviços
│   │   └── Relatorios.cshtml# Relatórios financeiros
│   ├── Shared/
│   │   └── _Layout.cshtml   # Layout principal
│   ├── Index.cshtml         # Login/Cadastro
│   └── Logout.cshtml        # Logout
├── Services/
│   ├── AuthHelper.cs        # Helper de autenticação
│   ├── PasswordHasher.cs    # Hash de senhas (BCrypt)
│   └── AgendamentoService.cs# Regras de negócio
├── wwwroot/
│   └── css/site.css         # Estilos personalizados
├── Program.cs               # Configuração da aplicação
└── script_banco.sql         # Script SQL do banco
```

---

## ✨ Funcionalidades

### Cliente
- ✅ Cadastro e login
- ✅ Agendamento com seleção de serviço, data e horário
- ✅ Visualização de horários disponíveis (em tempo real)
- ✅ Histórico de agendamentos
- ✅ Cancelamento (com regra de 24h)
- ✅ Código de confirmação único

### Profissional
- ✅ Dashboard com resumo diário
- ✅ Visualização de todos os agendamentos
- ✅ Confirmação e cancelamento de agendamentos
- ✅ Cadastro e edição de serviços
- ✅ Relatórios por período (receita, atendimentos)

### Segurança
- ✅ Autenticação por Session/Cookie
- ✅ Senhas hasheadas com BCrypt
- ✅ Redirecionamento automático por perfil
- ✅ Validação de acesso em todas as páginas

---

## 🛠️ Tecnologias

- **Backend:** ASP.NET Core 10 (Razor Pages)
- **Banco de Dados:** SQL Server 2022
- **ORM:** Entity Framework Core 10
- **Segurança:** BCrypt para senhas, Session Authentication
- **Frontend:** Bootstrap 5, CSS personalizado
- **Fontes:** Playfair Display, DM Sans

---

## 📝 Comandos Úteis

```bash
# Restaurar pacotes NuGet
dotnet restore

# Compilar o projeto
dotnet build

# Rodar o projeto
dotnet run

# Criar migration (opcional, já temos script SQL)
dotnet ef migrations add NomeDaMigration

# Atualizar banco via EF (opcional)
dotnet ef database update

# Limpar build
dotnet clean
```

---

## ⚠️ Troubleshooting

### Erro de conexão com o banco
- Verifique se o SQL Server está rodando
- Confirme o nome da instância (SQLEXPRESS vs MSSQLSERVER)
- Teste a connection string no SSMS

### Erro de pacotes
```bash
dotnet restore --force
dotnet clean
dotnet build
```

### Porta em uso
O projeto usa a porta padrão. Para mudar, adicione em `Properties/launchSettings.json`:
```json
"applicationUrl": "https://localhost:5001;http://localhost:5000"
```

---

Desenvolvido com 💖 para Studio Beauty
