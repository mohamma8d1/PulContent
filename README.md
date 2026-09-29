# PulContent API

A .NET-based content management API with AI capabilities powered by OpenAI, Redis caching, and asynchronous messaging with RabbitMQ.

## 📋 Table of Contents

- [Overview](#-overview)
- [Features](#-features)
- [Tech Stack](#-tech-stack)
- [Prerequisites](#-prerequisites)
- [Getting Started](#-getting-started)
  - [Installation](#installation)
  - [Configuration](#configuration)
  - [Running the Application](#running-the-application)
- [API Documentation](#api-documentation)
- [Project Structure](#project-structure)
- [Testing](#testing)
- [Contributing](#contributing)
- [License](#license)

---

## 📖 Overview

**PulContent API** is a RESTful web service that provides content management functionality with integrated AI features. It uses JWT-based authentication, Redis for caching, RabbitMQ for asynchronous event messaging, and OpenAI for AI-powered content operations.

---

## ✨ Features

- 🔐 **JWT-based authentication & authorization**
- 🤖 **OpenAI integration** for AI-powered features
- ⚡ **Redis caching** for improved performance
- 🐰 **RabbitMQ integration** for asynchronous event messaging and background processing
- 🔑 **Admin API key protection**
- 📝 **RESTful API endpoints**
- 🗄️ **Entity Framework Core** with SQL database
- 📊 **Structured logging**

---

## 🛠 Tech Stack

- **Framework:** ASP.NET Core
- **Database:** SQL Server / SQLite
- **Cache:** Redis
- **Message Broker:** RabbitMQ
- **AI:** OpenAI API
- **Auth:** JWT Bearer Tokens
- **Logging:** Built-in ASP.NET Core Logging

---

## 📦 Prerequisites

Before running this project, make sure you have:

- .NET SDK (version 8.0 or later)
- Redis server running locally (or a remote instance)
- RabbitMQ broker running locally (or a remote instance)
- SQL Server or SQLite
- An OpenAI API key

---

## 🚀 Getting Started

### Installation

1. **Clone the repository:**
   ```bash
   git clone https://github.com/your-username/PulContent.git
   cd PulContent
   ```

2. **Restore dependencies:**
   ```bash
   dotnet restore
   ```

---

### ⚙️ Configuration

The application requires an `appsettings.json` file to run. Create or update `appsettings.json` in the API project root with the following structure:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=PulContent;Trusted_Connection=True;TrustServerCertificate=True;"
  },
  "OpenAi": {
    "ApiKey": "your-openai-api-key"
  },
  "Jwt": {
    "Key": "YourSuperSecretKeyAtLeast32CharactersLong!",
    "Issuer": "PulContent.Api",
    "Audience": "PulContent.Client"
  },
  "Redis": {
    "Connection": "localhost:6379"
  },
  "RabbitMQ": {
    "HostName": "localhost",
    "Port": 5672,
    "UserName": "guest",
    "Password": "guest",
    "VirtualHost": "/"
  },
  "Admin": {
    "ApiKey": "pc-adm-xK9v2Q7tR4wL8mZ3nB6yH0jF5sD1gE"
  }
}
```

#### 🔧 Configuration Reference

| Key | Description | Example |
| :--- | :--- | :--- |
| `ConnectionStrings:DefaultConnection` | Database connection string | `Server=localhost;Database=PulContent;Trusted_Connection=True;` |
| `OpenAi:ApiKey` | Your OpenAI API key | `sk-proj-...` |
| `Jwt:Key` | Secret key for signing JWT tokens (min 32 chars) | `YourSuperSecretKeyAtLeast32CharactersLong!` |
| `Jwt:Issuer` | JWT issuer identifier | `PulContent.Api` |
| `Jwt:Audience` | JWT audience identifier | `PulContent.Client` |
| `Redis:Connection` | Redis server connection string | `localhost:6379` |
| `RabbitMQ:HostName` | RabbitMQ broker host | `localhost` |
| `RabbitMQ:Port` | RabbitMQ AMQP port | `5672` |
| `RabbitMQ:UserName` | RabbitMQ access username | `guest` |
| `RabbitMQ:Password` | RabbitMQ access password | `guest` |
| `RabbitMQ:VirtualHost` | RabbitMQ virtual host path | `/` |
| `Admin:ApiKey` | API key for admin-protected endpoints | `pc-adm-xK9v2Q7tR4wL8mZ3nB6yH0jF5sD1gE` |

> ⚠️ **Important:** Never commit your `appsettings.json` with real secrets to source control. Use User Secrets or environment variables in production.

#### Using User Secrets (Recommended for Development)

```bash
cd PulContent.Api
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "your-connection-string"
dotnet user-secrets set "OpenAi:ApiKey" "your-openai-key"
dotnet user-secrets set "Jwt:Key" "your-jwt-secret-key-min-32-chars"
dotnet user-secrets set "RabbitMQ:HostName" "localhost"
dotnet user-secrets set "RabbitMQ:UserName" "guest"
dotnet user-secrets set "RabbitMQ:Password" "guest"
```

#### Using Environment Variables (Production)

```bash
export ConnectionStrings__DefaultConnection="your-connection-string"
export OpenAi__ApiKey="your-openai-key"
export Jwt__Key="your-jwt-secret-key"
export Redis__Connection="localhost:6379"
export RabbitMQ__HostName="localhost"
export RabbitMQ__Port="5672"
export RabbitMQ__UserName="guest"
export RabbitMQ__Password="guest"
```

---

### ▶️ Running the Application

1. **Make sure Redis and RabbitMQ are running:**
   ```bash
   redis-server
   ```
   *(Ensure your RabbitMQ service is active or run via Docker: `docker run -d --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3-management`)*

2. **Apply database migrations:**
   ```bash
   dotnet ef database update
   ```

3. **Run the API:**
   ```bash
   dotnet run --project PulContent.Api
   ```

4. **Access the API:**
   - **HTTP:** `http://localhost:5000`
   - **HTTPS:** `https://localhost:5001`
   - **Swagger UI:** `https://localhost:5001/swagger`

---

## 📚 API Documentation

Once running, navigate to `/swagger` to explore the interactive API documentation.

### Authentication

Most endpoints require a JWT Bearer token. Include it in the request header:

```text
Authorization: Bearer <your-jwt-token>
```

Admin endpoints require the admin API key:

```text
X-Admin-ApiKey: pc-adm-xK9v2Q7tR4wL8mZ3nB6yH0jF5sD1gE
```

---

## 📁 Project Structure

```text
PulContent/
├── PulContent.Api/           # Main API project
│   ├── Controllers/          # API controllers
│   ├── Models/               # Data models
│   ├── Services/             # Business logic & RabbitMQ producers/consumers
│   ├── Data/                 # DbContext and migrations
│   ├── appsettings.json      # Configuration
│   └── Program.cs            # Entry point
├── PulContent.Tests/         # Unit and integration tests
└── README.md
```

---

## 🧪 Testing

Run the test suite:

```bash
dotnet test
```

---

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

---

## 📄 License

This project is licensed under the MIT License - see the `LICENSE` file for details.

---

## 🔒 Security Notes

- Change the default Admin API key before deploying to production
- Use strong JWT keys (at least 32 characters, randomly generated)
- Change default RabbitMQ credentials (`guest/guest`) in production environments
- Enable HTTPS in production
- Store secrets in a secure vault (Azure Key Vault, AWS Secrets Manager, HashiCorp Vault, etc.)
- Rotate API keys regularly
