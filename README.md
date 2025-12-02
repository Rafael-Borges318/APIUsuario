# API de Gerenciamento de Usuários

## Descrição

A API de Gerenciamento de Usuários foi desenvolvida com o objetivo de fornecer um sistema simples, organizado e escalável para cadastro, atualização, listagem e exclusão de usuários. Seguindo boas práticas de arquitetura, o projeto separa responsabilidades entre camadas, garantindo maior manutenção, clareza e extensibilidade do código.

A aplicação utiliza Entity Framework Core com SQLite para persistência de dados, além de integrar validações robustas usando FluentValidation. A estrutura combina padrões de projeto amplamente usados no mercado, permitindo que o sistema seja facilmente evoluído ou integrado a outros serviços.

Esta API também conta com documentação via Swagger, tornando a exploração dos endpoints e testes de requisições muito mais prática e intuitiva.

---

## Tecnologias Utilizadas

- .NET 8.0  
- Entity Framework Core  
- SQLite  
- FluentValidation  
- Swagger / Swashbuckle  
- Dependency Injection (DI)  
- Outras ferramentas auxiliares…

---

## Padrões de Projeto Implementados

- **Repository Pattern** – Responsável pelo acesso a dados, deixando a lógica de persistência isolada.
- **Service Pattern** – Centraliza as regras de negócio e tratamento dos fluxos entre API e repositórios.
- **DTO Pattern** – Padroniza os objetos de entrada e saída, evitando expor diretamente entidades do domínio.
- **Dependency Injection** – Reduz acoplamento e favorece testabilidade.

---

## Como Executar o Projeto

### Pré-requisitos

- .NET SDK **8.0** ou superior

---
Autor: Rafael Borges de Souza Lima
RA: 2025001110
Curso: ADS