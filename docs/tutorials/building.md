Construindo o E5R.Tools.Bit
===========================

Ness rapidíssimo tutorial vamos mostrar como você pode construir o __E5R.Tools.Bit__ após adquirir o código fonte de algum canal de distribuição.

## Instruções para Windows

Para exibir mais informações sobre o script de build:

```powershell
# Show help information
$ .\build.ps1 --help

# Show build options
$ .\build.ps1 --showdescription
```

Para construir a ferramenta:

```powershell
# Em modo Debug
$ .\build.ps1 -Target=Build

# Em modo Release
$ .\build.ps1 -Target=Build -Configuration=Release
```

Você pode executar a ferramenta assim:

```powershell
# Substitua {Configuration} por "Debug" ou "Release"
$ dotnet ".\src\E5R.Tools\bit\bini\{Configuration}\netcoreapp2.0\bit.dll" [args]
```

Para construir e executar os testes automatizados:

```powershell
# Em modo Debug
$ .\build.ps1 -Target=Test

# Em modo Release
$ .\build.ps1 -Target=Test -Configuration=Release
```

Para empacotar a ferramente:

```powershell
# Em modo Debug
$ .\build.ps1 -Target=Pack

# Em modo Release
$ .\build.ps1 -Target=Pack -Configuration=Release
```

Isso irá compilar a ferramenta e gerar as distribuições para todas as plataformas suportadas
no subdiretório `dist`. Lá você encontra um arquivo `.zip` para cada plataforma suportada,
além de outro subdiretório `app` com uma pasta contendo os binários de cada plataforma também.
O conteúdo dentro de cada subpasta `dist/app/{palatform}` corresponde ao conteúdo compactado
em `dist/{platform}.zip`.

Você poderá executar a ferramenta diretamente sem a necessidade de usar o utilitário `dotnet`
assim:

```powershell
$ ".\dist\app\win10-x64\bit" [args]
```
Aqui assumimos a plataforma `win10-x64`, mas você pode copiar os arquivos da distribuição que
necessitar e instalar (copiar e colar) na máquina destino, e executar da mesma forma.

Você só precisará garantir os pré-requisitos do próprio .NET Core em cada plataforma.
Veja quais são os pré-requisitos abaixo:

* Pré-requisitos do Windows: https://docs.microsoft.com/pt-br/dotnet/core/windows-prerequisites?tabs=netcore2x
* Pré-requisitos do macOS: https://docs.microsoft.com/pt-br/dotnet/core/macos-prerequisites?tabs=netcore2x
* Pré-requisitos do Linux: https://docs.microsoft.com/pt-br/dotnet/core/linux-prerequisites?tabs=netcore2x

## Instruções para Linux e macOS

Como usamos [Cake][cake] como sistema de construção, e o mesmo é também multi-plataforma, as instruções
para `Windows` também se aplicam de igual forma para `Linux` e `macOS`, bastando apenas substituir o
prefixo de comando `.\build.ps1` por `./build.sh`. O resto é o a mesma coisa!

Exemplo:

```sh
# Show help information
$ ./build.sh --help

# Show build options
$ ./build.sh --showdescription
``` 