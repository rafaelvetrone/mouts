# 🛠️ Mouts Sales API

API for Sales and Items

## ⚙️ Prerequisites

Before you start, ensure you have installed:
- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download)
- [Docker Desktop](https://www.docker.com/products/docker-desktop)
- [Git](https://git-scm.com/)

## Getting Started

### 1️⃣ Clone the repository
```bash
git clone https://github.com/rafaelvetrone/mouts.git
cd mouts

### 🗄️ Database Migrations

To apply migrations:

```bash
dotnet ef database update -p src/Project.Infrastructure -s src/Project.Api

### 🧪 Testing
To run tests

```bash
dotnet test

### 🚀 Run with Docker Compose

```bash
docker-compose up --build