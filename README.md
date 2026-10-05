# 🛒 Shopping Platform

> **A Full-Stack E-Commerce Platform built with .NET, Angular, and Microservices Architecture.**

🚧 **This project is currently under development.**

Shopping Platform is a Full-Stack E-Commerce platform designed to demonstrate modern software engineering and distributed systems concepts through a practical application.

The business scope is intentionally focused, while the technical architecture covers microservices, reliable service communication, messaging, resilience, caching, observability, and infrastructure.

---

## 🏛️ System Architecture

The platform follows a **Microservices Architecture**, where each service is independently responsible for its own business domain and data.

### Backend Services

* **User & Authentication Service** — User management, authentication, authorization, and access control
* **Product Service** — Product catalog and inventory management
* **Order Service** — Cart, orders, checkout, and order workflow
* **Payment Service** — Payment processing and payment status management

The services follow **Clean Architecture**, with clear separation between API, Application, Domain, and Infrastructure layers.

### Service Communication

* **YARP API Gateway** — Entry point for client requests
* **HTTP** — Synchronous service-to-service communication
* **Apache Kafka** — Asynchronous event-driven communication
* **Saga Orchestration** — Coordinates distributed checkout operations

### Data & Infrastructure

* **SQL Server** — Persistent data storage
* **Redis** — Distributed caching
* **Docker** — Containerization
* **.NET Aspire** — Local distributed application orchestration
* **OpenTelemetry** — Observability and distributed tracing

---

## 🛠️ Technology Stack

| Layer              | Technologies                                                 |
| ------------------ | ------------------------------------------------------------ |
| **Backend**        | C#, ASP.NET Core Web API, EF Core, MediatR, FluentValidation |
| **Architecture**   | Clean Architecture, DDD, CQRS                                |
| **Messaging**      | Apache Kafka, Outbox Pattern, Saga Pattern                   |
| **Resilience**     | Retry, Timeout, Circuit Breaker                              |
| **Caching**        | Redis, Cache-Aside, Cache Invalidation                       |
| **Gateway**        | YARP                                                         |
| **Database**       | SQL Server                                                   |
| **Infrastructure** | Docker, .NET Aspire                                          |
| **Observability**  | Health Checks, Structured Logging, OpenTelemetry             |
| **Frontend**       | Angular, TypeScript, Feature-Based Architecture              |

---

## 🔧 Key Features

* Product Management
* Inventory & Stock Reservation
* Cart Management
* Checkout Workflow
* Payment Integration
* Saga-based Checkout Coordination
* Kafka Event Communication
* Outbox Pattern
* Idempotent Consumers
* Redis Caching & Cache Invalidation
* Retry / Timeout / Circuit Breaker
* Health Checks
* Structured Logging
* Docker
* .NET Aspire

---

## 🔄 Checkout Flow

The checkout process coordinates multiple services while handling failures through compensation:

**Create Order → Reserve Stock → Initiate Payment → Payment Result**

* If stock reservation fails → checkout is rejected.
* If payment succeeds → the order is completed and the cart is cleared.
* If payment fails → a compensation event releases the reserved stock.
* If payment remains pending beyond the defined period → the order can be cancelled and stock released.

---

## 🧠 Key Design Decisions

### Database per Service

Each microservice owns its data and database, preventing direct database access between services and maintaining service boundaries.

### HTTP + Kafka

HTTP is used when an immediate response is required between services, while Kafka is used for asynchronous events where services should remain loosely coupled.

### Saga Orchestration

The checkout workflow uses an **Orchestrator** to coordinate the distributed steps and trigger compensation when a later step fails.

### Outbox Pattern

Events are stored in the service's database before being published to Kafka, reducing the risk of losing an event when a database operation succeeds but message publishing fails.

### Idempotent Consumers

Consumers track processed event IDs to prevent the same event from being applied more than once.

---

## 📁 Project Structure

```text
ShoppingPlatform/
├── Back/
│   ├── ProductService/
│   ├── OrderService/
│   ├── PaymentService/
│   ├── UserService/
│   ├── BuildingBlocks/
│   ├── ShoppingPlatform.AppHost/
│   └── ShoppingPlatform.sln
│
├── FrontEnd/
│   └── shopping-platform/
│
├── .github/
├── .gitignore
└── README.md
```

---

## 🚀 How to Run

### Prerequisites

* .NET SDK 8+
* Docker Desktop
* SQL Server
* Node.js & Angular CLI

### Backend

Clone the repository and run the Aspire AppHost:

```bash
git clone <repository-url>
cd ShoppingPlatform/Back/ShoppingPlatform.AppHost
dotnet run
```

.NET Aspire will start the distributed application and its required infrastructure services.

The **Aspire Dashboard** provides access to running services, logs, resources, and telemetry.

### Frontend

```bash
cd ShoppingPlatform/FrontEnd/shopping-platform
npm install
ng serve
```

> **Note:** Exact service URLs and Swagger endpoints may change during development.

---


