# Configurando o Scalar
Utilizaremos o Scalar para testes e documentação da API.
```bash
if (app.Environment.IsDevelopment()) {
	app.MapOpenApi();   
	app.MapScalarApiReference(options => {  
	    List<ScalarServer> servers = [];   
	    string? httpsPort = Environment
		    .GetEnvironmentVariable("ASPNETCORE_HTTPS_PORT");   
	    if (httpsPort is not null)   
	    {   
		    servers.Add(new ScalarServer($"https://localhost:{httpsPort}"));   
	    }  
    
	    string? httpPort = Environment
		    .GetEnvironmentVariable("ASPNETCORE_HTTP_PORT");   
	    if (httpPort is not null) {   
		    servers.Add(new ScalarServer($"http://localhost:{httpPort}"));   
	    }   
  
	    options.Servers = servers;   
	    options.Title = "MinhaPrimeiraAPI";   
	    options.ShowSidebar = true;  
    });
}
```
# Aplicando Migrations Diretamente no Container da API
## Os serviços
Para aplicar as migrations diretamente no container da API foi necessário criar dois serviços:
- **migrate-add**: Gerando as migrations no container da API.
- **migrate-update**: Atualizando o banco de dados com as migrations.
## Porque eu criei serviços?
Havia 3 formas para aplicar as migrations:
1. Aplicá-las **localmente**.
2. Aplicá-las **diretamente no Dockerfile**.
3. Aplicá-las separadamente em um **serviço**.
Vamos dar um overview dos motivos pelos quais eu decidi utilizar a terceira opção.

| Critério                                 | Local                                    | Dockerfile                                                | Serviço                                                  |
| ---------------------------------------- | ---------------------------------------- | --------------------------------------------------------- | -------------------------------------------------------- |
| **Reprodutibilidade**                    | Depende da máquina de cada desenvolvedor | Fixa o estado do DB no momento da build, não no deploy    | Mesmo comando roda em qualquer host/CI                   |
| **Princípio de Imutabilidade** da imagem | Imagem não mexe no DB                    | Imagem _mutável_ (migra e salva dados ao construir)       | Imagem continua imutável; migração fora da build         |
| **Deploy contínuo**                      | Manual, propenso a esquecer              | DB fica desatualizado se você rebuilda sem recriar volume | Automatizado; CI roda `docker compose run migrator`      |
| **Segurança** (exposição de credenciais) | Senha no host / user-secrets             | Senha vai para Dockerfile (camada de build)               | Senha via `.env`/secrets no Compose (não vaza em imagem) |
| **Rollback / controle de versões**       | Difícil coordenar                        | Precisa rebuildar imagem antiga                           | Facilita: cada commit + migration versionada             |
| **Tamanho da imagem**                    | Pequena (runtime)                        | Aumenta muito (SDK + tool)                                | Pequena (runtime) - migrator usa imagem SDK descartável  |
| **Prod e Dev iguais**                    | Risco de drift                           | Imagem de produção faz build                              | Pipeline único para dev, staging, prod                   |
## Como funciona a geração e aplicação das migrations
### Shell Script
Primeiramente vamos falar do **shell script `.sh`**:
```bash
#!/usr/bin/env sh
# docker/migrate.sh add <name> 	# gera migrations
# docker/migrate.sh update 	# aplica migrations

set -e

export PATH="$PATH:/root/.dotnet/tools"

# instala dotnet-ef se ainda não tiver
if ! dotnet tool list -g | grep -q dotnet-ef; then
	dotnet tool install -g dotnet-ef
fi

case "$1" in
	add)
		shift
		dotnet ef migrations add "$@" \
			--project BarCodeAPI.csproj \
			--startup-project .
		;;
	update)
		dotnet ef database update \
			--project BarCodeAPI.csproj \
			--startup-project .
		;;
	*)
	 echo "Usage: migrate.sh {add <Name>|update}"
	 exit 1
esac
```
Aqui está ambos os comandos, de **geração** e de **aplicação** das migrations. Em suma, o shell script poderá ser executado de duas formas:
- **sh migrate.sh add**: Gera uma migração diretamente no diretório do projeto da API.
- **sh migrate.sh update**: Aplica uma migração ao banco de dados.
### Docker Compose
Agora os serviços do **docker-compose**:
```yaml
migrate-add:
    image: mcr.microsoft.com/dotnet/sdk:9.0
    env_file:
      - .env
    volumes:
      - ../backend/BarCodeAPI/BarCodeAPI:/src
      - ./migrate.sh:/src/migrate.sh:ro
    working_dir: /src
    entrypoint: ["sh", "migrate.sh", "add"]
    depends_on:
      - database
    networks:
      - barcode_net

  migrate-update:
    image: mcr.microsoft.com/dotnet/sdk:9.0
    env_file:
      - .env
    volumes:
      - ../backend/BarCodeAPI/BarCodeAPI:/src
      - ./migrate.sh:/src/migrate.sh:ro
    working_dir: /src
    entrypoint: ["sh", "migrate.sh", "update"]
    depends_on:
      - database
    networks:
      - barcode_net
```
Em ambas os serviços é gerado uma **imagem descartável** do sdk para a execução da geração e da aplicação da migration. No entrypoint é executado o shell script de acordo com o seu devido propósito.
### Makefile
E por fim o arquivo **Makefile**:
```bash
db-add:
	docker compose run --rm migrate-add $(NAME)
db-update:
	docker compose run --rm migrate-update
```
Usando o makefile é possível gerenciar melhor a geração e aplicação das migrations.
- **`make db-add NAME=nome-da-migration`**: Comando para execução do serviço `migrate-add`, gerando as migrations. Assim que o comando é executado o container é excluido.
- **`make db-update`**: Comando para execução do serviço `migrate-update`, aplicando as migrations. Assim que o comando é executado o container é excluido.
