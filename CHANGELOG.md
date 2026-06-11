# Changelog

## [0.3.0](https://github.com/GabrrelS/Plataforma-de-Marketplace-de-Servi-os/compare/v0.2.0...v0.3.0) (2026-06-11)


### Novas Funcionalidades

* adicionar handlers CQRS, API Gateway com Ocelot, Dockerfiles dos microsserviços e configuração do PostgreSQL ([896d682](https://github.com/GabrrelS/Plataforma-de-Marketplace-de-Servi-os/commit/896d68260b04fc19e2b5c31c22caa9a7282fe1e6))


### Correcoes de Bug

* **ci:** corrige versão do dotnet para 8.0.x e ajusta comando de teste para usar a solution ([7d09b67](https://github.com/GabrrelS/Plataforma-de-Marketplace-de-Servi-os/commit/7d09b6759ebb53c75a1aaf80294f37c3db1ab34d))
* **config:** alinha credenciais do banco entre env.example e appsettings ([2c0defe](https://github.com/GabrrelS/Plataforma-de-Marketplace-de-Servi-os/commit/2c0defe7dce7bbfebcc5bd99f0c28aa0cd0e678c))
* **frontend:** corrige target do proxy para http://localhost:5006 ([c66920a](https://github.com/GabrrelS/Plataforma-de-Marketplace-de-Servi-os/commit/c66920a57db4ec78a4bfcfb0eddd54382a0dd4a4))
* **frontend:** implementa autenticação JWT real na tela de login ([3a054ec](https://github.com/GabrrelS/Plataforma-de-Marketplace-de-Servi-os/commit/3a054ec8a319053d01121929f824727768a2a68d))
* **notification:** implementa broadcast via SignalR hub no NotificacaoService ([71e4a15](https://github.com/GabrrelS/Plataforma-de-Marketplace-de-Servi-os/commit/71e4a159e8816359759adf78cd0cdd4b3a92d820))

## [0.2.0](https://github.com/GabrrelS/Plataforma-de-Marketplace-de-Servi-os/compare/v0.1.0...v0.2.0) (2026-06-04)


### Novas Funcionalidades

* adiciona github actions ([165ecf7](https://github.com/GabrrelS/Plataforma-de-Marketplace-de-Servi-os/commit/165ecf752d170b6301413d93fcd30347e8fc862b))
* **infra:** forcar geracao da primeira release ([#2](https://github.com/GabrrelS/Plataforma-de-Marketplace-de-Servi-os/issues/2)) ([94bf659](https://github.com/GabrrelS/Plataforma-de-Marketplace-de-Servi-os/commit/94bf6599d45035d47bf32c6add1158bf6795f865))
