# Links:

## Roslyn:
* http://www.tugberkugurlu.com/archive/compiling-c-sharp-code-into-memory-and-executing-it-with-roslyn
* https://msdn.microsoft.com/pt-br/magazine/mt808499.aspx

## Deployment:
* https://github.com/dotnet/core/blob/master/Documentation/self-contained-linux-apps.md

## Command line:
* https://gist.github.com/iamarcel/9bdc3f40d95c13f80d259b7eb2bbcabb
* https://gist.github.com/iamarcel/8047384bfbe9941e52817cf14a79dc34
* https://www.areilly.com/2017/04/21/command-line-argument-parsing-in-net-core-with-microsoft-extensions-commandlineutils/
* https://msdn.microsoft.com/en-us/magazine/mt763239.aspx
* http://fclp.github.io/fluent-command-line-parser/

## SEMVER:
* https://github.com/adamreeve/semver.net

# Notas sobre repositórios:

* Um repositório é um diretório de conteúdo estruturado que pode estar
  tanto em um diretório local, (na rede) ou em um servidor HTTP/1.1 padrão.

* A estrutura do diretório é a seguinte:
 - Considerando a raiz de um diretório como "/", temos:

```
  /hash.txt
  /index.json
  /history.json
```

/hash.txt
---------
   Este arquivo tem uma linha com o hash para cada arquivo do repositório.
   No padrão SHA256 Ex:
   
   ```txt
   7182207cea19516277f71456324bf13dfac31b7913dcae5405f290e3db88178b index.json
   166011c6eec829afdbc5e483b487167bb7fc050f9d26bac03c59432c3b44f215 history.json
   ```

   Esses hashs são computados localmente quando os arquivos index.json e history.json
   são baixados para validação, e ficam armazenados. Sempre que uma solicitação
   para atualizar o repositório for feita, primeiramente esse arquivo (hash.txt) é
   baixado para verificar se algo mudou no repositório, se nada mudou, o repositório
   está atualizado.

/index.json
-----------
   Este arquivo contém as informações do repositório:
   Ex:
   ```json
   {
       "repoid": "unique-name",
       "description": "Repository description",
       "license": {
           "type": "MIT",
           "file:" "LICENSE.txt"
       },
       "url": "",
       "owner": {
           "name": "User Name",
           "email": "email@site.com",
           "twitter": "@user"
       },
       "authors": [
           {
               "name": "User Name",
               "email": "email@site.com",
               "twitter": "@user"
           }
       ],
       "patterns": {
           "lib": "files/@{version}/libs/@{file}",
           "doc": "files/@{version}/docs/@{file}",
           "cmd": "files/@{version}/commands/@{file}"
       }
   }
   ```

/history.json
-------------
  Este arquivo contém o histórico de versões com seus respectivos arquivos disponíveis
  em cada uma das versões.
  "O número da versão deve obedecer o padrão SEMVER.
  Ex:
  ```json
  {
      "0.1.1": {
          "lib": [
              "utils",
              "crypto"
          ],
          "doc": [
              "my-other-command"
          ],
          "cmd": [
              "my-other-command"
              "my-command"
          ]
      },
      "0.1.1-dev": {
          "lib": [
              "utils"
          ],
          "doc": [
              "my-command"
          ],
          "cmd": [
              "my-command"
          ]
      }
  }
  ```

## Como os arquivos são identificados?

Considerando os arquivos `index.json` e `history.json` exemplificados acima. Podemos deduzir o seguinte:

Quando o comando abaixo for chamado:

```sh
$ bit my-command
```

Será identificada a última versão `0.1.1` contendo a entrada de comando `my-command` na lista `cmd`. Que será traduzida para o arquivo `MyCommand.cs` no path `~/files/0.1.1/commands/MyCommand.cs`

Quando o comando de documentação for chamado:

```sh
$bit doc my-other-command
```

Será identificada a última versão `0.1.1` contendo a entrada de comando `my-other-command` na lista `doc`. Que será traduzida para o arquivo `MyOtherCommand.txt` no path `~/files/0.1.1/docs/MyOtherCommand.txt`

Um comando de versão específico também pode ser acionado:

```sh
$ bit my-other-command@0.1.1-dev 
```

Esse terá a versão `0.1.1-dev` NÃO contendo a entrada de comando `my-other-command` na lista `cmd`. O quee seria traduzido para o arquivo `MyOtherCommand.cs` no path `~/files/0.1.1-dev/commands/MyOtherCommand.cs`. Mas como a entrada não existe. O comando não existe e não será executado.


Sobre as libs
-------------
  O propósito das libs é permitir a colaboração entre equipes em um ecossistema. Uma vez que podemos desenvolver somente libs que podem ser utilizadas por diversos comandos.

  Aqui nós, além de informarmos as versões dos arquivos, também precisaremos informar o nome dos repositórios (arquivo `index.json`).

  Exemplificando:

  No exemplo acima nós temos na versão `0.1.1-dev` uma lib chamada `utils`, e na versão `0.1.1` duas libs chamadas `utils` e `crypto`. No final são duas libs, `utils` que tem duas versões (`0.1.1-dev` e `0.1.1`), e `crypto` com uma única vesão `0.1.1`. O objetivo das libs é fornecer serviços que podem ser usados por outros comandos. Então veja o comando abaixo:

  ```csharp

  using E5R.Sdk.Services;

  [AssemblyNeutral] interface IStringUtils {}
  [AssemblyNeutral] interface IByteUtils {}
  [AssemblyNeutral] interface ICrypto
  {
      string Encrypt(string original);
  }

  [Require("lib://crypto@unique-name/~1.2.1", nameof(ICrypto))]
  [Require("lib://utils", "IStringUtils", "IByteUtils")]
  public class MyThirdPartyCommand
  {
      private readonly IBitOutput _output;
      private readonly ICrypto _crypto;

      public MyThirdPartyCommand(IBitOutput output, ICypto crypto, ...)
      {
          _output = output ?? throw new ArgumentNullException(nameof(output));
          _crypto = crypto ?? throw new ArgumentNullException(nameof(crypto));
      }

      public async Task<BitResult> Main(BitArguments args)
      {
          return Task.Run(() => {
              string originalMessage = "My message";
              string cryptoMessage = _crypto.Encrypt(originalMessage);

              _output.WriteLine($"Original message: {originalMessage}");
              _output.WriteLine($"Crypto message: {cryptoMessage}");

              return BitResult.Ok();
          });
      }
  }
  ```

  Nesse exemplo nós temos um comando `MyThirdPartyCommand` que se utiliza de serviços que estão
  em bibliotecas de terceiros (no caso dele mesmo, mas poderia ser de qualquer outro repositório).

  A assinatura `[Require("lib://crypto@unique-name/~1.2.1", nameof(ICrypto))]` diz para o engine que os
  tipos que implementam o serviço `ICrypto` na biblioteca `crypto`, versão `>=1.2.1 < 1.3.0` e que está no repositório `unique-name` deve ser registrado e injetado nesse comando. Veja o mesmo sendo injetado no construtor, junto com outros serviços (`IBitoutput`) nativos do próprio Sdk.

  Ainda temos a assinatura `[Require("lib://utils", "IStringUtils", "IByteUtils")]` que não especifica nem um nome de repositório, nem uma versão. Isso indica que os serviços `IStringUtils` e `IByteUtils` que estão sendo solicitados estão no mesmo repositório do comando que está em execução, e também na mesma versão.
