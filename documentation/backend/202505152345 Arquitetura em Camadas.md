# Mudando a Arquitetura do Backend
Foi necessário as mudanças na arquitetura do backend pelo fato de, **organização e manutenção**. Então, foi decidido a mudança e aplicação da **arquitetura em camadas**.
# Como Funciona?
A arquitetura em camadas abordada no backend do projeto dependem de duas camadas somente: **Domínio** e **Infraestrutura**.
## Domínio
A camada de domínio é o **coração do sistema**, pois nessa camada é onde se encontram as **regras de negócios puras**, independentes de banco, web, bibliotecas.
### O que contém no Domínio?
- Entidades (`Product`, `Company`)
- Value Objects
- Interfaces de repositório
- Serviços de domínio
## Infraestrutura
A camada de infraestrutura depende da camada de domínio. Ela resolve detalhes (I/O). Pode mudar tecnologia sem quebrar o domínio. Permite testes de integração isolados.
### O que contém na Infraestrutura?
- AppDbContext
- Mapeamentos EF Core
- Repositórios concretos
- APIs externas
