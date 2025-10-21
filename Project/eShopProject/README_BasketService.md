# 🛒 eShopProject – Basket Microservice (.NET 9)

## Overview
Basket microservice is part of the **eShopProject** solution (UCL Semester Project).  
It is built using **.NET 9**, **PostgreSQL**, **RabbitMQ**, and follows the **Strangler Pattern** for migration from monolith to microservices.

This service handles:
- Shopping basket CRUD operations  
- Checkout events via RabbitMQ  
- Persistent data in PostgreSQL  

---

## 🧩 Project Components
| Project | Description |
|----------|--------------|
| `eShop.Basket.API` | Web API (controllers, Swagger, Serilog) |
| `eShop.Basket.Application` | Business logic, DTOs, service interfaces |
| `eShop.Basket.Domain` | Entities (`ShoppingBasket`, `BasketItem`) |
| `eShop.Basket.Infrastructure` | EF Core + RabbitMQ implementation |
| `docker-compose.yml` | Starts PostgreSQL + RabbitMQ containers |

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
1. Start **PostgreSQL** and **RabbitMQ** using Docker  
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

### 6️⃣ Stop Services
When done, stop with:
```powershell
Ctrl + C
docker compose down
```

> ⚠️ **Do not use `docker compose down -v`**, or you will delete the database volume.

---

## 🧠 Notes for Developers
- Database data is stored in Docker volume `basketdata` and persists between restarts.  
- Logs are stored in `eShop.Basket.API/Logs`.  
- RabbitMQ management UI available at [http://localhost:15672](http://localhost:15672)  
  - Username: `guest`  
  - Password: `guest`

---

## 🧱 Next Step (Trin 13)
We will create a shared **EventBus** library (`eShop.BuildingBlocks.EventBus`) to allow all microservices (Basket, Catalog, Order) to publish and subscribe to integration events.

---

### 💻 Author
**Ihab & Team – UCL Software Development (Top-Up)**  
Semester Project: _Strangler Pattern – Monolith to Microservices_
