#!/usr/bin/env sh
# docker/migrate.sh add <name> 	# gera migrations
# docker/migrate.sh update 	# aplica migrations

set -e

export PATH="$PATH:/root/.dotnet/tools"

PROJECT="backend/Shared/BarCode.Infrastructure/BarCode.Infrastructure/BarCode.Infrastructure.csproj"
STARTUP="backend/Shared/BarCode.Infrastructure/BarCode.Infrastructure/BarCode.Infrastructure.csproj"

# instala dotnet-ef se ainda não tiver
if ! dotnet tool list -g | grep -q dotnet-ef; then
	dotnet tool install -g dotnet-ef
fi

case "$1" in
	add)
		shift
		dotnet ef migrations add "$@" \
			--project "$PROJECT" \
			--startup-project "$STARTUP" \
			--output-dir Migrations \
			--context AppDbContext
		;;
	update)
		dotnet ef database update \
			--project "$PROJECT" \
			--startup-project "$STARTUP" \
			--context AppDbContext
		;;
	*)
	 echo "Usage: migrate.sh {add <Name>|update}"
	 exit 1
esac
