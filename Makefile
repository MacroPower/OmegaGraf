PREFIX            ?= $(shell pwd)
GIT               ?= $(shell which git)
RELEASE_ID        ?= $(shell lsb_release -is | tr '[:upper:]' '[:lower:]')
RELEASE_CODE      ?= $(shell lsb_release -cs)
RELEASE_VERSION   ?= $(shell lsb_release -rs)

help: ## Displays help.
	@awk 'BEGIN {FS = ":.*##"; printf "\nUsage:\n  make \033[36m<target>\033[0m\n\nTargets:\n"} /^[a-zA-Z_-]+:.*?##/ { printf "  \033[36m%-10s\033[0m %s\n", $$1, $$2 }' $(MAKEFILE_LIST)

pre-build:
	@export DOTNET_CLI_TELEMETRY_OPTOUT=1

.PHONY: all
all: format build

.PHONY: deps
deps: ## Installs developer dependencies.
deps: pre-build
	@echo ">> installing deps for $(RELEASE_ID)/$(RELEASE_CODE) $(RELEASE_VERSION)"
	@wget -q "https://packages.microsoft.com/config/$(RELEASE_ID)/$(RELEASE_VERSION)/packages-microsoft-prod.deb" -O packages-microsoft-prod.deb
	@sudo dpkg -i packages-microsoft-prod.deb
	@sudo apt-get update
	@sudo apt-get install -y npm powershell dotnet-sdk-3.1

.PHONY: build
build: ## Builds OmegaGraf binaries.
build: pre-build
	@echo ">> building binaries"
	@sudo pwsh ./scripts/build.ps1
