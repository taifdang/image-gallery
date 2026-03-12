# Image Gallery
Practical implementation of image post-processing with cloud services integration.

# Architecture

```mermaid
flowchart LR
  X[Browser] --> A[Frontend]
  A --> B[C# API]
  B --> C[Storage]
  B --> D[Messaging]
  B --> E[Database]
  D --> |Event Trigger| F[Background Worker<br/>or Serverless]
  F --> C
  F --> E
```
## Storage Providers

| Storage         | Status         | Path |
|------------------|:------:|------|
| Azure Blob       | <center>✅</center>     | [/Storage/Azure](src/Backend/Infrastructure/Storage/Azure)|
| Fake             | <center>✅</center>     | [/Storage/Fake](src/Backend/Infrastructure/Storage/Fake) |

## Messaging Providers

| Messaging            | Status         | Path           |
|---------------------|:------:|----------------|
| Azure Queue Storage | <center>✅</center>     | [/Messaging/AzureQueueStorage](src/Backend/Infrastructure/Messaging/AzureQueueStorage) |
| Fake                | <center>✅</center>     | [/Messaging/Fake](src/Backend/Infrastructure/Messaging/Fake) |

# How to Build and Run Single Page Applications:

React:

- Navigate to folder: [/UI/reactjs/](src/UI/reactjs)

```
npm install
npm run dev
```
- Go to http://localhost:5173/
- 
<img width="1499" height="935" alt="image" src="https://github.com/user-attachments/assets/8d8753c2-3142-45c9-b61b-391d69718363" />


