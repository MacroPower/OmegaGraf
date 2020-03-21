# Prereqs
sudo apt update
sudo apt install -y apt-transport-https ca-certificates curl gnupg2 software-properties-common

# Add key
RELEASE_ID=$(lsb_release -is | tr '[:upper:]' '[:lower:]')
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