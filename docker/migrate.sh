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
