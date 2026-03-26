## 📁 Estrutura do Projeto





```text
plataforma-servicos/
│
├── services/
│   ├── PrestadoresService/
│   ├── ClientesService/
│   ├── PropostasService/
│   │   ├── Controllers/
│   │   ├── Models/
│   │   ├── Data/
│   │   ├── Services/
│   │   ├── Events/
│   │   │   └── PropostaAceitaEvent.cs
│   │   ├── Program.cs
│   │   ├── appsettings.json
│   │   └── PropostasService.csproj
│   │
│   ├── ContratosService/
│   ├── AvaliacoesService/
│   └── RankingsService/
│       ├── Queries/
│       ├── Models/
│       ├── Services/
│       ├── Program.cs
│       ├── appsettings.json
│       └── RankingsService.csproj
│
├── shared/
│   ├── Events/
│   └── DTOs/
│
├── gateway/
│   └── ApiGateway/
│
├── realtime/
│   └── RealtimeService/
│
└── docker-compose.yml
