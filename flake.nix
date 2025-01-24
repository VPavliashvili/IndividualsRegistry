{
  description = "A .NET development environment";

  inputs = {
    nixpkgs.url = "github:NixOS/nixpkgs/nixos-24.11";
    flake-utils.url = "github:numtide/flake-utils";
  };

  outputs = { self, nixpkgs, flake-utils }:
    flake-utils.lib.eachDefaultSystem (system:
      let
        pkgs = import nixpkgs {
          inherit system;
          config.allowUnfree = true;
        };
      in
      {
        devShells.default = pkgs.mkShell {
          buildInputs = with pkgs; [
            dotnet-sdk_8
            dotnet-runtime_8
            dotnet-aspnetcore_8
          ];

          shellHook = ''
            # Set DOTNET_ROOT to the correct location
            export DOTNET_ROOT="${pkgs.dotnet-sdk_8}"

            # Set the ASPNETCORE environment to Development
            export DOTNET_ENVIRONMENT=Development
            
            # Disable telemetry
            export DOTNET_CLI_TELEMETRY_OPTOUT=1
            
            # Set up temp directory for .NET
            export DOTNET_CLI_HOME="/tmp/dotnet-cli-home"
            mkdir -p $DOTNET_CLI_HOME
            chmod 755 $DOTNET_CLI_HOME
            
            # Add dotnet tools to PATH
            export PATH=$PATH:$DOTNET_CLI_HOME/.dotnet/tools 

            # install ef core if not present
            if ! dotnet tool list -g | grep "dotnet-ef"; then
              dotnet tool install --global dotnet-ef
            fi

            # specify certs
            export SSL_CERT_FILE="${pkgs.cacert}/etc/ssl/certs/ca-bundle.crt"
            
            # Welcome message
            echo "Welcome to .NET 8 development environment!"
            echo "dotnet SDK version: $(dotnet --version)"
          '';
        };
      });
}
