# Install

Scripts to assist with installing OmegaGraf.

## Linux Installation

### Linux Prerequisites

You will need to install `docker-ce`.
Click for instructions:
[Debian](https://docs.docker.com/engine/install/debian/),
[Ubuntu](https://docs.docker.com/engine/install/ubuntu/),
[CentOS](https://docs.docker.com/engine/install/centos/), or
[Fedora](https://docs.docker.com/engine/install/fedora/).

If you want to enable SSL, you also need to install `dotnet-sdk-3.1`.
Click for instructions:
[Debian](https://docs.microsoft.com/en-us/dotnet/core/install/linux-debian),
[Ubuntu](https://docs.microsoft.com/en-us/dotnet/core/install/linux-ubuntu),
[CentOS](https://docs.microsoft.com/en-us/dotnet/core/install/linux-centos), or
[Fedora](https://docs.microsoft.com/en-us/dotnet/core/install/linux-fedora).

### Running the Linux Installer

The `install.sh` script should work on all major distributions.

You must run as a user with sudo _privileges_ for package installs. _Do not run with sudo or as root._

Run the script:

```shell
bash <(curl -s https://raw.githubusercontent.com/MacroPower/OmegaGraf/master/install/install.sh)
```

Alternatively:

- Download the script directly, e.g. <br>
  `curl https://raw.githubusercontent.com/MacroPower/OmegaGraf/master/install/install.sh --output install.sh`
- `chmod +x install.sh`
- `./install.sh`

Don't want to use the installer? You can find manual instructions under [Getting Started](getting-started.md).

## Windows Installation

### Windows Prerequisites

You will need to install [Docker Desktop](https://hub.docker.com/editions/community/docker-ce-desktop-windows).

Your OmegaGraf data directory must be shared under `Resources -> File Sharing`.

### Running the Windows Installer

Expect an `install.ps1` script in the near future. Until then, you can find manual instructions under [Getting Started](getting-started.md).
