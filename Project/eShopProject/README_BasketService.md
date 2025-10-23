# 🛒 eShopProject – Basket Microservice (.NET 9)

## Overview
Basket microservice is part of the **eShopProject** solution (UCL Semester Project).  
It is built using **.NET 9**, **PostgreSQL**, **RabbitMQ**, and **Redis** — following the **Strangler Pattern** for migration from monolith to microservices.

This service handles:
- Shopping basket CRUD operations  
- Checkout events via RabbitMQ  
- Persistent data in PostgreSQL  
- Fast caching via Redis for optimized performance  

---

## 🧩 Project Components
| Project | Description |
|----------|--------------|
| `eShop.Basket.API` | Web API (controllers, Swagger, Serilog) |
| `eShop.Basket.Application` | Business logic, DTOs, service interfaces |
| `eShop.Basket.Domain` | Entities (`ShoppingBasket`, `BasketItem`) |
| `eShop.Basket.Infrastructure` | EF Core + RabbitMQ + Redis integration |
| `eShop.BuildingBlocks.EventBus` | Shared EventBus for all microservices |
| `docker-compose.yml` | Starts PostgreSQL, RabbitMQ, and Redis containers |

---

## 🚀 Quick Start (For All Team Members)

### 1️⃣ Requirements
Before running, ensure you have:
- **Docker Desktop** running  
- **.NET 9 SDK** installed  
- **PowerShell** enabled  

---

### 2️⃣ Clone Repository
```powershell
git clone https://github.com/<your-org>/eShopProject.git
cd eShopProject
```

---

### 3️⃣ Enable Script Execution (first time only)
```powershell
Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope CurrentUser
```

---

### 4️⃣ Start Basket Microservice
From the project root (`eShopProject` folder):

```powershell
.\start-basket.ps1
```

The script will automatically:
1. Start **PostgreSQL**, **RabbitMQ**, and **Redis** using Docker  
2. Wait 25 seconds for initialization  
3. Apply **EF Core migrations** (if needed)  
4. Launch **Basket.API**

✅ When you see  
```
Application started. Press Ctrl+C to shut down.
```
the service is ready.

---

### 5️⃣ Open Swagger
Go to:
```
http://localhost:5283/swagger
```

Available endpoints:
- `GET /api/Basket/{customerId}`  
- `POST /api/Basket`  
- `DELETE /api/Basket/{customerId}`  
- `POST /api/Basket/checkout`

---

### 6️⃣ View Redis Cache
Redis automatically stores each basket as a **hash** object with keys like:  
```
basket:cust-1001
basket:cust-1002
```

You can inspect data using:

#### 🔹 Option 1 – Command line
```powershell
docker exec -it basket.redis redis-cli
KEYS *
HGETALL basket:cust-1001
```

#### 🔹 Option 2 – RedisInsight GUI
Download and open **RedisInsight**  
Connect to:
```
Host: localhost
Port: 6379
Database: 0
```
You will see cached baskets under keys `basket:cust-*`.

---

### 7️⃣ Stop Services
When done, stop with:
```powershell
Ctrl + C
docker compose down
```

> ⚠️ **Do not use `docker compose down -v`**, or you will delete the database and Redis data volumes.

---

## 🧠 Notes for Developers
- Database data is stored in Docker volume `basketdata` and persists between restarts.  
- Redis cache improves read performance and reduces PostgreSQL load.  
- RabbitMQ management UI available at [http://localhost:15672](http://localhost:15672)  
  - Username: `guest`  
  - Password: `guest`  


---

### 💻 Author
**Ihab & Team – UCL Software Development (Top-Up)**  
Semester Project: _Strangler Pattern – Monolith to Microservices_
