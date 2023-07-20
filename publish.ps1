Remove-Item -Path ./publish/* -Recurse -Force


# Publish gui application
dotnet publish -c Release -f net7.0 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -p:PublishTrimmed=true -p:TrimMode=copyused --runtime win-x86 -o "./publish/Ava.SocketTool (Win X86)" ./Ava.SocketTool
dotnet publish -c Release -f net7.0 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -p:PublishTrimmed=true -p:TrimMode=copyused --runtime win-x64 -o "./publish/Ava.SocketTool (Win X64)" ./Ava.SocketTool
dotnet publish -c Release -f net7.0 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -p:PublishTrimmed=true -p:TrimMode=copyused --runtime linux-x64 -o "./publish/Ava.SocketTool (Linux X64)" ./Ava.SocketTool
dotnet publish -c Release -f net7.0 --self-contained true -p:PublishSingleFile=true -p:PublishTrimmed=true -p:TrimMode=copyused --runtime osx-x64 -o "./publish/Ava.SocketTool (macOS 10.12 X64)" ./Ava.SocketTool

# Remove pdb files
Copy-Item "./publish/Ava.SocketTool (Win X86)/Ava.SocketTool.exe" -Destination "./publish/Ava.SocketTool-Win-X86.exe"
Copy-Item "./publish/Ava.SocketTool (Win X64)/Ava.SocketTool.exe" -Destination "./publish/Ava.SocketTool-Win-X64.exe"
Copy-Item "./publish/Ava.SocketTool (Linux X64)/Ava.SocketTool" -Destination "./publish/Ava.SocketTool-Linux-X64"
Copy-Item "./publish/Ava.SocketTool (macOS 10.12 X64)/Ava.SocketTool" -Destination "./publish/Ava.SocketTool-macOS-X64"

# Compress each build for lower disk usage
$compress = @{
  Path = "./publish/Ava.SocketTool-Win-X86.exe"
  CompressionLevel = "Optimal"
  DestinationPath = "./publish/Ava.SocketTool-Win-X86.zip"
}
Compress-Archive @compress

$compress = @{
  Path = "./publish/Ava.SocketTool-Win-X64.exe"
  CompressionLevel = "Optimal"
  DestinationPath = "./publish/Ava.SocketTool-Win-X64.zip"
}
Compress-Archive @compress

$compress = @{
  Path = "./publish/Ava.SocketTool-Linux-X64"
  CompressionLevel = "Optimal"
  DestinationPath = "./publish/Ava.SocketTool-Linux-X64.zip"
}
Compress-Archive @compress

$compress = @{
  Path = "./publish/Ava.SocketTool-macOS-X64", "./publish/Ava.SocketTool (macOS 10.12 X64)/*.dylib"
  CompressionLevel = "Optimal"
  DestinationPath = "./publish/Ava.SocketTool-macOS-X64.zip"
}
Compress-Archive @compress

tar -C ./publish -cvzf ./publish/Ava.SocketTool-Linux-X64.tar.gz Ava.SocketTool-Linux-X64