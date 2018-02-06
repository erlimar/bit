Construindo o E5R.Tools.Bit.Shell
=================================

Nesse rapidíssimo tutorial vamos mostrar como construir o __E5R.Tools.Bit.Shell__ após
adquirir o código fonte de algum canal de distribuição.

O __E5R.Tools.Bit.Shell__ é a camada de _interface_ com o usuário final. Enquanto __E5R.Tools.Bit__ é a ferramenta que executa toda a lógica e é escrita em .NET Core, assim, requer de toda a infraestrutura .NET antes de ser executada pela primeira vez. Em contraste, temos o componente __E5R.Tools.Bit.Shell__, esse é escrito em C++ e tem um tamanho bem menor visto que não requer a estrutura .NET para ser executado. Por isso os passos de construção são ligeiramente diferentes.

Esse componente é o __"bit"__ que o usuário chama na linha de comando.

Saiba mais sobre a arquitetura da ferramenta em https://github.com/e5r/bit/blob/master/IDEA.md.

> OBS: Estamos trabalhando para unificar o processo de build, mas por enquanto você deverá construir cada um de forma distinta.

## Instruções para Windows

> __NOTA__: Nos exemplos de execução de comando abaixo, estamos usando a notação do [PowerShell][powershell], mas você pode usar a linha de comando que preferir.

### Pré-requisitos

* [Visual C++ Build Tools 2015 (ou posterior)][vcpp-buildtools]
* [CMake 3.6 (ou posterior)][cmake]
* [Biblioteca cURL][building-shell-requirements]

Antes de executar os passos à seguir, tenha certeza que os comandos abaixo estão
disponíveis para execução (estejam no `%PATH%` do sistema).

```powershell
$ cl /?           # Compilador C/C++
$ link /?         # Linker C/C++
$ nmake           # Makefile
$ cmake --version # CMake (3.6.3 ou posterior)
```

### Passos

1. Crie o diretório para os artefatos da construção e vá para lá.
   Nós sugerimos um local, mas você tem a liberdade de escolher o que preferir.

```powershell
$ mkdir ".\build\shell"
$ cd ".\build\shell"
```

2. Gere os artefatos com o _CMake_. Recomendamos aqui o gerador _"NMake Makefiles"_, mas você pode usar o que melhor lhe atender.

```powershell
# Para modo Release
$ cmake -G "NMake Makefiles" "..\.."

# ou para modo Debug
$ cmake -DCMAKE_BUILD_TYPE=Debug -G "NMake Makefiles" "..\.."
```

3. Agora é só construir.

```powershell
$ cmake --build .
```

Se tudo correu como esperado, seu executável __"bit"__ está disponível em  `".\build\shell\src\E5R.Tools.Bit.Shell\bit.exe"`, veja:

```powershell
# Lembrando que você já está no diretório ".\build\shell"
$ ".\src\E5R.Tools.Bit.Shell\bit.exe"
```

4. E para gerar o pacote de distribuição no formato _Zip_:

```powershell
$ cpack -G ZIP
```

E mais uma vez, se tudo correu como esperado, seu pacote zip está disponível em  `".\build\shell\bit-{version}-{arch}.zip"`.

Aqui, `{version}` é a versão da ferramenta, ex: `1.0.0`. E `{arch}` é a arquitetura, ex: `win64`. Logo, você teria um arquivo `".\build\shell\bit-1.0.0-win64.zip"`.

## Instruções para Linux e macOS

TODO: ...

<!-- Links -->
[vcpp-buildtools]: http://landinghub.visualstudio.com/visual-cpp-build-tools
[cmake]: https://cmake.org/
[powershell]: https://github.com/PowerShell/PowerShell
[building-shell-requirements]: building-shell-requirements.md
