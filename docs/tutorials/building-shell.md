Construindo o E5R.Tools.Bit.Shell
=================================

Nesse rapidíssimo tutorial vamos mostrar como construir o __E5R.Tools.Bit.Shell__ após
adquirir o código fonte de algum canal de distribuição.

O __E5R.Tools.Bit.Shell__ é a camada de _interface_ com o usuário final. Enquanto __E5R.Tools.Bit__ é a ferramenta que executa toda a lógica e é escrita em .NET Core, assim, requer de toda a infraestrutura .NET antes de ser executada pela primeira vez. Em contraste, temos o componente __E5R.Tools.Bit.Shell__, esse é escrito em C++ e tem um tamanho bem menor visto que não requer a estrutura .NET para se executado. Por isso os passos de consrução são ligeiramente diferentes.

Esse componente é o __"bit"__ que o usuário chama na linha de comando.

Saiba mais sobre a arquitetura da ferramenta em https://github.com/e5r/bit/blob/master/IDEA.md.

> OBS: Estamos trabalhando para unificar o processo de build, mas por enquanto você deverá construir cada um de forma distinta.

## Instruções para Windows

### Pré-requisitos

* [Visual C++ Build Tools 2015 (ou posterior)][vcpp-buildtools]
* [CMake 2.8 (ou posterior)][cmake]

### Passos

## Instruções para Linux e macOS

TODO: ...

<!-- Links -->
[vcpp-buildtools]: http://landinghub.visualstudio.com/visual-cpp-build-tools
[cmake]: https://cmake.org/
