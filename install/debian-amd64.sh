#!/bin/bash

RELEASE_ID=$(lsb_release -is | tr '[:upper:]' '[:lower:]')
RELEASE_CODE=$(lsb_release -cs)
RELEASE_VERSION=$(lsb_release -rs)

echo ">> installing OmegaGraf for $RELEASE_ID/$RELEASE_CODE $RELEASE_VERSION"

# Prereqs
sudo apt update
sudo apt install -y apt-transport-https ca-certificates curl gnupg2 software-properties-common acl

read -p "Would you like to enable SSL (BETA)? (y/n)" answer
case ${answer:0:1} in
	y|Y )
		echo "OK! I will be installing certs and running on HTTPS port 5001."
		echo ""
		echo ">> installing microsoft tooling for developer certs"
		wget -q "https://packages.microsoft.com/config/$RELEASE_ID/$RELEASE_VERSION/packages-microsoft-prod.deb"
		sudo dpkg -i packages-microsoft-prod.deb
		sudo apt update
		rm packages-microsoft-prod.deb
		sudo apt install -y dotnet-sdk-3.1
		dotnet dev-certs https
		export OMEGAGRAF_HOST="https://0.0.0.0:5001"
	;;
	* )
		echo "OK! I will be running on HTTP port 5000 by default."
		export OMEGAGRAF_HOST="http://0.0.0.0:5000"
	;;
esac

echo ">> installing docker"

# Add key
curl -fsSL "https://download.docker.com/linux/$RELEASE_ID/gpg" | sudo apt-key add -

# Add repo
RELEASE_CODE=$(lsb_release -cs)
sudo add-apt-repository "deb [arch=amd64] https://download.docker.com/linux/$RELEASE_ID $RELEASE_CODE stable"

# Install docker
sudo apt update
apt-cache policy docker-ce
sudo apt install -y docker-ce

# Check install
sudo systemctl status docker
docker -v

# Allow current user
sudo usermod -aG docker $USER

echo ">> downloading OmegaGraf"

# Remove old binaries
rm OmegaGraf*.tar.gz
rm OmegaGraf

# Download the latest OmegaGraf release
curl -s "https://api.github.com/repos/OmegaGraf/OmegaGraf/releases/latest" \
	| grep "OmegaGraf.*tar.gz" \
	| cut -d : -f 2,3 \
	| tr -d \" \
	| wget -qi -

# Extract the OmegaGraf binary
tar -xzf OmegaGraf*.tar.gz

# Create the default OmegaGraf data directory
mkdir data
chmod -R 777 data

# Allow execute on binary
chmod +x OmegaGraf

echo "Thanks for using OmegaGraf! Printing --help now."

# Print help
./OmegaGraf --help

echo "To run OmegaGraf, you can use:"
echo "./OmegaGraf --host $OMEGAGRAF_HOST"
