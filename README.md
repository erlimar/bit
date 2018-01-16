# E5R.Tools.Bit

[![Build status](https://ci.appveyor.com/api/projects/status/toqbgycdwfua2ide?svg=true)](https://ci.appveyor.com/project/erlimar/bit)
[![Build Status](https://travis-ci.org/erlimar/bit.svg?branch=master)](https://travis-ci.org/erlimar/bit)

Uma ferramenta mínima, extensível, independente e auto-suficiente para o desenvolvimento de software multiplataforma.

Build on Windows:
-----------------

```powershell
# Show help information
$ .\build-coreclr.ps1 --help

# Show build options
$ .\build-coreclr.ps1 --showdescription
```

Build on Unix:
--------------
```sh
# Show help information
$ ./build-coreclr.sh --help

# Show build options
$ ./build-coreclr.sh --showdescription
```

Premissas:
----------

* Ser o mínimo necessário para possibilitar portabilidade a vários sistemas operacionais, e leve o suficiente para um bootstrap rápido;

* Ser extensível para que se possa adicionar funcionalidades o quanto for necessário, sustentando inclusive uma comunidade e ecossistema;

* Ser independente de pré-requisitos de software, a não ser os essenciais: Shell e Internet;

* Ser auto-suficiente para que resolva suas dependências e extensões na execução de um único script shell sem argumentos;

> [Mais detalhes no arquivo IDEA.md.](IDEA.md) e [DRAFT.md](DRAFT.md)
