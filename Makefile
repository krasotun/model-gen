.PHONY: install build test format format-check check run

install:
	dotnet restore
	dotnet tool restore

build:
	dotnet build

test:
	dotnet test

format:
	dotnet csharpier format .

format-check:
	dotnet csharpier check .

check: format-check build test

run:
	dotnet run --project src/ModelGen


