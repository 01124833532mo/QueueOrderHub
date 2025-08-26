# 📦 QueueOrderHub - Distributed Order Processing System (Task For Fixed Soloution Company)

The **QueueOrderHub** is a distributed system built with **.NET 8** that demonstrates modern order processing using **background Service** and **messaging queues***.  
It allows clients to submit orders via API, enqueue them for background processing, and query their status asynchronously.  
The system is designed with **Onion Architecture**, ensuring scalability, modularity, and separation of concerns.

---

## 🧩 Features

### Phase 01: Foundations
- Implemented **RESTful API** principles with ASP.NET Core.
- Structured using **Onion Architecture** to separate concerns.
- Configured:
  - **Redis Databse** and **RabbidMQ** .
- BackGround Service Such as **HangFire** .
- Implement **Manual Mapping** for DTO ↔ Entity mapping.

### Phase 02: Core Functionalities
- Developed `Orders` module with:
  - Create new orders.
  - Retrieve status of existing orders.
- Implemented **background job processing** using **Hangfire**.
- Introduced caching mechanism with **Redis**.
- Integrated **RabbitMQ** for publishing and consuming order messages.
- Error handling middleware and logging.
- Configurable retry mechanism for failed jobs.

### Phase 03: Worker Service
- Dedicated **HangFire Service** for processing queued orders.
- Subscribes to RabbitMQ order queue.
- Processes orders asynchronously and updates their status in Redis/DB.
- Fault-tolerance with retry/backoff strategies.
- Can be scaled horizontally for higher throughput.

### Phase 04: Order Tracking
- Order statuses supported:
  - `Pending`
  - `Processing`
  - `Completed`
  - `Failed`
- API endpoints to check current state.
- Data persisted in database and cached in Redis for fast lookup.

### Phase 05: Unit Testing
- Unit tests implemented with **xUnit**.
- Used **FakeItEasy** for mocking services and repositories.
- Applied **FluentAssertions** for more readable test cases.
- Key modules tested:
  - `OrderService` (business logic validation).
  - `OrderController` (API endpoint behavior).
  - `WorkerService` (background processing reliability).
- Ensures code quality, correctness, and regression prevention.

---

## 🛠️ Technologies Used
- **Framework:** ASP.NET Core 8
- **Database:** SQL Server + EF Core
- **Message Queue:** RabbitMQ
- **Background Jobs:** Hangfire
- **Caching:** Redis
- **Documentation:** Swagger, Postman
- **Testing:** xUnit, FakeItEasy, FluentAssertions

---

## 📖 Key Learnings
- Real-world implementation of queue-based order processing.
- Use of Hangfire and RabbitMQ together for robust background processing.
- Onion Architecture with clear separation of Application, Domain, and Infrastructure.
- Redis for fast caching and job result retrieval.
- Scalable design for horizontal worker scaling.

---

## 📂 Project Structure 
```plaintext
QueueOrderHub
├── QueueOrderHub.Api                      # Presentation Layer (Web API)
│   ├── Controllers                        # OrdersController, AuthController
│   ├── Middleware                         # Error handling, Logging
│   ├── Program.cs                         # Entry point
│
├── QueueOrderHub.Worker                   # Worker Service
│   ├── Consumers                          # RabbitMQ message consumers
│   ├── Services                           # OrderProcessorService
│   └── Program.cs                         # Entry point
│
├── QueueOrderHub.Domain                   # Domain Layer
│   ├── Entities                           # Order, User
│   ├── Interfaces                         # IRepository, IUnitOfWork
│   ├── Enums                              # OrderStatus
│
├── QueueOrderHub.Application              # Application Layer
│   ├── Services                           # OrderService, AuthService
│   ├── DTOs                               # OrderDto, OrderStatusDto
│   ├── Mapping                            # AutoMapper Profiles
│   └── Validators                         # OrderValidator, LoginValidator
│
├── QueueOrderHub.Infrastructure           # Infrastructure Layer
│   ├── Persistence                        # EF Core DbContext, Configurations, Migrations
│   ├── Messaging                          # RabbitMQ producer/consumer
│   ├── Caching                            # Redis service
│   └── Repositories                       # GenericRepository, UnitOfWork
│
├── QueueOrderHub.Shared                   # Shared Utilities
│   ├── Settings                           # AppSettings, Connection strings
│   └── Responses                          # ApiResponse, ErrorResponse
│
└── QueueOrderHub.Tests                    # Testing Layer
    ├── UnitTests                          # Service tests, Controller tests
    ├── IntegrationTests                   # API + Worker end-to-end tests
    └── Mocks                              # Fake data & mock dependencies
