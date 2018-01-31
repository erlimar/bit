Construindo o E5R.Tools.Bit
===========================

Nesse rapidíssimo tutorial vamos mostrar como construir o __E5R.Tools.Bit__ após
adquirir o código fonte de algum canal de distribuição.

## Instruções para Windows

Para exibir mais informações sobre o script de build:

```powershell
# Exibe a ajuda do comando
$ .\build.ps1 --help

# Exibe os alvos de construção disponíveis
$ .\build.ps1 --showdescription
```

Para construir a ferramenta:

```powershell
# Em modo Debug
$ .\build.ps1 -target=Build

# Em modo Release
$ .\build.ps1 -target=Build -configuration=Release
```

Você pode executar a ferramenta assim:

```powershell
# Substitua {config} por "Debug" ou "Release"
$ dotnet ".\src\E5R.Tools\bit\bin\{config}\netcoreapp2.0\bit.dll" [args]
```

Para construir e executar os testes automatizados:

```powershell
# Em modo Debug
$ .\build.ps1 -target=Test

# Em modo Release
$ .\build.ps1 -target=Test -configuration=Release
```

Para empacotar a ferramenta:

```powershell
# Em modo Debug
$ .\build.ps1 -target=Pack

# Em modo Release
$ .\build.ps1 -target=Pack -configuration=Release
```

Isso irá compilar a ferramenta e gerar as distribuições para todas as plataformas suportadas
no subdiretório `dist`. Lá você encontra um arquivo `.zip` para cada plataforma suportada,
além de outro subdiretório `app` com uma pasta contendo os binários de cada plataforma também.
O conteúdo dentro de cada subpasta `dist/app/{platform}` corresponde ao conteúdo compactado
em `dist/{platform}.zip`.

Você poderá executar a ferramenta diretamente sem a necessidade de usar o utilitário `dotnet`
assim:

```powershell
$ ".\dist\app\win10-x64\bit" [args]
```
Aqui assumimos a plataforma `Windos 10 64bits`, mas você poderia executar da mesma forma em
todas as outras plataformas suportadas, bastando para isso indicar o subdiretório correspondente
no local adequado.

Vale ressaltar que você precisará garantir os pré-requisitos do próprio .NET Core em cada
plataforma pretendida antes de executar a ferramenta. Veja abaixo quais são:

* Pré-requisitos do Windows: https://docs.microsoft.com/pt-br/dotnet/core/windows-prerequisites?tabs=netcore2x
* Pré-requisitos do macOS: https://docs.microsoft.com/pt-br/dotnet/core/macos-prerequisites?tabs=netcore2x
* Pré-requisitos do Linux: https://docs.microsoft.com/pt-br/dotnet/core/linux-prerequisites?tabs=netcore2x

> Estamos trabalhando para entregar os pacotes binários da distribuição da ferramenta livre de
> pré-requisitos. Quando conseguirmos isso, bastará instalar (copiar e colar) os arquivos finais
> e executá-los para que funcionem. Mas por enquanto ainda são necessários os pré-requisitos.

## Instruções para Linux e macOS

Como usamos [Cake][cake] como sistema de construção, as instruções para `Windows` também se
aplicam de igual forma para `Linux` e `macOS`, bastando apenas substituir o prefixo de comando
`.\build.ps1` por `./build.sh`. O resto continua a mesma coisa!

Exemplo:

```sh
# Exibe a ajuda do comando
$ ./build.sh --help

# Exibe os alvos de construção disponíveis
$ ./build.sh --showdescription
``` 

<!-- Links -->

[cake]: https://cakebuild.net