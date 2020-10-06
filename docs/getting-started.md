# Getting Started

## Compatibility

**OmegaGraf is supported on all major Linux distributions, as well as Windows.** Our testing is done using Ubuntu, so using that will probably result in the best / most bug-free experience.

\* _We do not currently support MacOS, but if you're up for working with the C# Docker package on MacOS, feel free to give it a try!_

## Installation

OmegaGraf has a few dependencies that must be installed prior to use.

OmegaGraf has interactive [installers](https://github.com/MacroPower/OmegaGraf/tree/master/install) that can assist with installation. The installers page details prerequisites, and how to run the script.

Alternatively, you can do the following:

- Install `apt-transport-https ca-certificates gnupg2 software-properties-common`
- Install `docker-ce`. Click for instructions:
  [Debian](https://docs.docker.com/engine/install/debian/),
  [Ubuntu](https://docs.docker.com/engine/install/ubuntu/),
  [CentOS](https://docs.docker.com/engine/install/centos/), or
  [Fedora](https://docs.docker.com/engine/install/fedora/).
- Add the user that will be running OmegaGraf, e.g. `$USER`, to the docker group.
- (Optional) Generate an SSL cert
  - Install `dotnet-sdk-3.1`. Click for instructions:
    [Debian](https://docs.microsoft.com/en-us/dotnet/core/install/linux-debian),
    [Ubuntu](https://docs.microsoft.com/en-us/dotnet/core/install/linux-ubuntu),
    [CentOS](https://docs.microsoft.com/en-us/dotnet/core/install/linux-centos), or
    [Fedora](https://docs.microsoft.com/en-us/dotnet/core/install/linux-fedora).
  - Run `dotnet dev-certs https`.
  - When you run OmegaGraf, use an `https` listening address.
- Download an OmegaGraf binary from [releases](https://github.com/MacroPower/OmegaGraf/releases).
- Extract the binary, e.g. `tar -xzf OmegaGraf.tar.gz`.
- Ensure you have execute permissions on the binary.
- See [Using OmegaGraf](#using-omegagraf) for next steps.

To run OmegaGraf on Windows:

- Install [Docker Desktop](https://hub.docker.com/editions/community/docker-ce-desktop-windows).
- Share the directory you will be using for OmegaGraf data under `Resources -> File Sharing`.
- Download an OmegaGraf binary from [releases](https://github.com/MacroPower/OmegaGraf/releases).
- Unzip the OmegaGraf binary.
- Open a shell and target the directory containing the binary.
- If you are using WSL rather than a native shell:
  - Use WSL 2 or expose docker over tcp.
  - Use the `--socket` parameter as needed.
  - Be aware that you may run into issues, especially with WSL 2.
- See [Using OmegaGraf](#using-omegagraf) for next steps.

## Using OmegaGraf

The installer will print an example start command, e.g. `./OmegaGraf --host http://0.0.0.0:5000`, which you can use to launch OmegaGraf. For more help on usage, see the [walkthrough](walkthrough.md).

Note: **OmegaGraf currently uses the current _working directory_ to store container data.** Please keep this in mind. If you want to change this behavior, you can supply the `--path` argument along with an absolute or relative path to store data.

For more documentation on parameters, see [parameters](parameters.md).
