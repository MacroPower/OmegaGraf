# install

Scripts to assist with installing OmegaGraf.

## Usage

```
wget <installer.sh>
sudo chmod +x <installer.sh>
./<installer.sh>
```

## SSL

```
wget -q https://packages.microsoft.com/config/ubuntu/16.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb
sudo apt-get update
export DOTNET_CLI_TELEMETRY_OP=1
sudo apt-get install -y dotnet-sdk-3.1
dotnet dev-certs https
```
