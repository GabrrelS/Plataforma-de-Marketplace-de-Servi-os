Projeto de Tópicos Avançados em Computação


Plataforma de Marketplace de Serviços

src/
│
├── Marketplace.API/
│   ├── Controllers/
│   │   ├── UsuarioController.cs
│   │   ├── ServicoController.cs
│   │   ├── PedidoController.cs
│   │   └── AvaliacaoController.cs
│   │
│   ├── Modelos/
│   │   ├── Usuario.cs
│   │   ├── Servico.cs
│   │   ├── Pedido.cs
│   │   └── Avaliacao.cs
│   │
│   ├── Servicos/
│   │   ├── UsuarioServico.cs
│   │   ├── ServicoServico.cs
│   │   └── PedidoServico.cs
│   │
│   ├── Dados/
│   │   └── AppDbContext.cs
│   │
│   ├── Program.cs
│   └── appsettings.json
│
└── Marketplace.Testes/
    └── ApiTestes.cs