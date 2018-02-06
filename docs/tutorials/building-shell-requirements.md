Pré-requisitos paa construir E5R.Tools.Bit.Shell
================================================

Nesse rapidíssimo tutorial vamos mostrar como construir/instalar os pré-requisitos para construção do __E5R.Tools.Bit.Shell__ após
adquirir o código fonte de algum canal de distribuição.

Este tutorial é complemento do tutorial 

Para uma construção bem sucedida de __E5R.Tools.Bit.Shell__, além dos pré-requisitos já mencionados em ["Construindo o E5R.Tools.Bit.Shell"][building-shell],
você deve também preparar seu ambiente de projeto com os seguintes componentes:

* [][]

## Instruções para Windows

> __NOTA__: Nos exemplos de execução de comando abaixo, estamos usando a notação do [PowerShell][powershell], mas você pode usar a linha de comando que preferir.


### cURL

1. Crie o diretório base para `cURL` em `".\build\.curl"` com os subdiretórios
   `source`, `build` e `install` para guardar o código fonte, artefatos de construção
   e arquivos de instalação respectivamente:

```powershell
$ mkdir '.\build\.curl\source'
$ mkdir '.\build\.curl\build'
$ mkdir '.\build\.curl\install'
```

2. Baixe o código fonte do GitHub em https://github.com/curl/curl/archive/curl-7_58_0.zip

3. Descompacte em `".\build\.curl\source"`

4. Gere os artefatos com o _CMake_ em `.\build\.curl\build`

```powershell
$ cd ".\build\.curl\build"
$ cmake -DCMAKE_USE_WINSSL=ON -DCMAKE_BUILD_TYPE=Release -DBUILD_CURL_EXE=OFF -DCURL_STATICLIB=ON -DHTTP_ONLY=ON -DBUILD_TESTING=OFF -DCMAKE_INSTALL_PREFIX=..\install -G "NMake Makefiles" ..\source
```

5. Construa `"cURL"`

```powershell
# cd ".\build\.curl\build"
$ cmake --build .
```

6. Instale `"cURL"` com __NMAKE__ no diretório que preparamos

```powershell
# cd ".\build\.curl\build"
$ nmake install
```

Agora você já tem o componente __cURL__ configurado em `".\build\.curl\install"`.

## Instruções para Linux e macOS

TODO: ...

<!-- Links -->
[building-shell]: building-shell.md
[powershell]: https://github.com/PowerShell/PowerShell

