# .NET Framework 4.6 Web API pipeline example

This repository is a small ASP.NET Web API 2 sample targeting **.NET Framework 4.6**. It demonstrates this flow:

```text
Client API call
   -> Web API controller listens on POST /api/orders
   -> SQL Server database copy/save
   -> map the saved row into a new external message layout
   -> publish the formatted message to Kafka
   -> call an external HTTP API with the same formatted message
```

## Project layout

| Path | Purpose |
| --- | --- |
| `Controllers/OrdersController.cs` | Listens for incoming API calls and orchestrates the whole workflow. |
| `Models/IncomingOrderRequest.cs` | Original request layout accepted by your API. |
| `Models/OrderRecord.cs` | Database row layout after saving the request. |
| `Models/ExternalMessage.cs` | New formatted layout sent to Kafka and the external API. |
| `Services/OrderRepository.cs` | Copies the incoming request into SQL Server through a stored procedure. |
| `Services/OrderMessageFormatter.cs` | Converts the saved database row to the new outbound message format. |
| `Services/KafkaMessageProducer.cs` | Publishes the new formatted message to Kafka. |
| `Services/ExternalApiClient.cs` | Sends the formatted message to another REST API. |
| `Sql/schema.sql` | Example SQL table and stored procedure. |
| `Web.config` | Database connection string, Kafka topic/server, and external API URL. |

## Request example

Send a request to the API after hosting the project in IIS Express or IIS:

```http
POST /api/orders HTTP/1.1
Content-Type: application/json

{
  "customerId": "CUST-1001",
  "orderNumber": "ORD-90001",
  "amount": 125.50,
  "currency": "usd",
  "message": "Original message received from upstream system",
  "requestedAtUtc": "2026-05-12T10:00:00Z"
}
```

## Response example

```json
{
  "databaseId": 1,
  "kafkaTopic": "formatted-orders",
  "externalApiStatus": "OK",
  "messageId": "ORD-1-71bfba16fe874240ad11dd0167f8d96f"
}
```

## How the code follows the architecture

1. **Listen to the API call**: `OrdersController.Post` accepts `POST /api/orders` and validates that the request body matches `IncomingOrderRequest`.
2. **Copy the data to database**: `OrderRepository.SaveAsync` inserts the incoming request into SQL Server using `dbo.SaveIncomingOrder`, then returns an `OrderRecord` containing the generated database ID.
3. **Format the data to a different layout**: `OrderMessageFormatter.Format` transforms `OrderRecord` into `ExternalMessage`. This is where field names and values change for downstream systems.
4. **Send the formatted Kafka message**: `KafkaMessageProducer.PublishAsync` serializes `ExternalMessage` to JSON and publishes it to the `formatted-orders` topic.
5. **Call the external API**: `ExternalApiClient.SendAsync` posts the same formatted JSON payload to the configured partner endpoint.

## Configuration

Update these values in `Web.config` before running in your environment:

```xml
<connectionStrings>
  <add name="OrdersDb" connectionString="Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=OrderMessages;Integrated Security=True" providerName="System.Data.SqlClient" />
</connectionStrings>
<appSettings>
  <add key="KafkaBootstrapServers" value="localhost:9092" />
  <add key="KafkaTopic" value="formatted-orders" />
  <add key="ExternalApiUrl" value="https://partner.example.com/api/messages" />
</appSettings>
```

## Database setup

Run `Sql/schema.sql` in SQL Server to create:

- `dbo.IncomingOrders`
- `dbo.SaveIncomingOrder`

## Notes for production

This sample keeps the code direct so the data flow is easy to understand. For production, consider adding:

- Dependency injection container registration instead of the controller's default constructor wiring.
- Retry and dead-letter behavior for Kafka and external API failures.
- Transaction/outbox pattern if Kafka publishing must be guaranteed after the database save.
- Authentication and authorization on `POST /api/orders`.
- Structured logging and correlation IDs.
