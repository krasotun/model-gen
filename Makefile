.PHONY: install build test format test-watch format-check check run

install:
	dotnet restore
	dotnet tool restore

build:
	dotnet build

test:
	dotnet test

test-watch:
	dotnet watch test --project tests/ModelGen.Tests

format:
	dotnet csharpier format .

format-check:
	dotnet csharpier check .

check: format-check build test

run:
	dotnet run --project src/ModelGen


