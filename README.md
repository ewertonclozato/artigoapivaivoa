# Artigo: API com Entity Framework Core em C# (Projeto VaiVoa)

<p>
<p>

## Neste artigo será utilizado o Visual Studio, que pode ser baixado em: https://visualstudio.microsoft.com/pt-br/downloads/

<br>
<br>
<br>

## 1. Iniciando o Visual Studio e criando um novo projeto ASP.NET Core Web Application, para criação da API:
<br>

* 1.1 - Primeiramente damos início ao projeto clicando em: New, Project... Conforme a imagem;
<br>
<br>
![](https://github.com/ewertonclozato/artigoapivaivoa/blob/master/imagens/novoprojeto.png?raw=true)

* 1.2 - Busque pelo Template: ASP.NET Core Web Application;
<br>
<br>

![](https://github.com/ewertonclozato/artigoapivaivoa/blob/master/imagens/1.2.png?raw=true)

* 1.3 - Configure seu novo projeto, escolhendo seu nome e pasta onde o mesmo será guardado junto com sua solução;
<br>
<br>

![](https://github.com/ewertonclozato/artigoapivaivoa/blob/master/imagens/1.3.png?raw=true)

* 1.4 - Por fim, escolha o Template da sua aplicação, e clique em concluir:
<br>
<br>

![](https://github.com/ewertonclozato/artigoapivaivoa/blob/master/imagens/1.4.png?raw=true)

<br>
<br>

## 2. Configurando o projeto para que possamos da início a nossa aplicação (instalando referências do Entity Framework Core no nosso projeto):

<br>

* 2.1 - Antes de iniciarmos a codar o nosso projeto, é necessário a inclusão de algumas referências do Entity Framework Core, que podemos obter de duas maneiras:
  * 2.1.1 - Incluindo manualmente os pacotes de referências do Entity Framework abrindo o arquivo principal do seu projeto;
  <br>
  ![](https://github.com/ewertonclozato/artigoapivaivoa/blob/master/imagens/2.1.png?raw=true)
<br>
  * 2.1.2 - Ou podemos ir em: "**<em>Tools, NuGet Package Manager, Manage Nuget Packages for Solution</em>**";
  <br>

  ![](https://github.com/ewertonclozato/artigoapivaivoa/blob/master/imagens/2.1.2.png?raw=true)
  <br>

  E buscamos(em Browse) os pacotes de referências do Entity Framework necessários para nossa aplicação no momento. **Lembrando sempre de selecionar a última versão ESTÁVEL dos pacotes necessários:**
  <br>

  ![](https://github.com/ewertonclozato/artigoapivaivoa/blob/master/imagens/2.1.22.png?raw=true)
<br>
<br>

## 3. Criando a classe "AdmDbContext", que representa o contexto do seu banco de dados:
<br>

* 3.1 - Clicamos com o botão direito no nosso projeto e vamos em "**<em>Add, New Folder</em>**", para criarmos uma nova pasta chamada "**Model**";
  <br>

  ![](https://github.com/ewertonclozato/artigoapivaivoa/blob/master/imagens/3.1.png?raw=true)
  <br>

  * 3.1.1 - Clicando com o botão direito na pasta Model vamos em "**<em>Add, Class</em>**", para adicionar a classe "**AdmDbContext**" em nosso pojeto.
<br>
<br>
* 3.2 - Criamos a classe AdmDbContexte com a seguinte linha de código, esdando "**DbContext**" de ""**Microsoft.EntityFrameworkCore**:

~~~C#
public class AdmDbContext : DbContext
    {
        public AdmDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Cliente> Cliente { get; set; }
        public DbSet<Cartao> Cartao { get; set; }
    }
~~~
  * 3.3 - Provávelmente iremos receber uma mensagem de erro, e para consertar isso precisamos usar a referência do Entity Framework na nossa classe, clicando com o botão direito em cima de algum erro e em seguida clicando em "**<em>using Microsoft.EntityFrameworkCore;</em>**" (ou Ctrl+.):
  <br>

  ![](https://github.com/ewertonclozato/artigoapivaivoa/blob/master/imagens/3.3.png?raw=true)
<br>
<br>

## 4. Criando as classes "Cliente" e "Cartão", que será respónsavel pelos dados de cada objeto:
<br>

* 4.1 - Ainda na pasta Model, criamos a classe "**Cliente**" e nela colocamos o seguinte código:

~~~C#
public class Cliente
    {
        //Indica que será chave primária no banco de dados.
        [Key] 
        //Gerando números aleatórios para o ID
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Cpf { get; set; }
        public List<Cartao> Cartoes { get; set; }
    }
~~~
<br>

![](https://github.com/ewertonclozato/artigoapivaivoa/blob/master/imagens/4.1.png?raw=true)

<br>

* 4.2 - De novo na pasta Model, criamos a classe "**Cartao**" e nela colocamos o seguinte código:
~~~C#
public class Cartao
    {
        //Indica que será chave primária no banco de dados.
        [Key]
        //Gerando números aleatórios para o ID
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Numero { get; set; }
        public string Bandeira { get; set; }
        public DateTime? DataCriacao { get; set; }
        [JsonIgnore]

        //Indica chave secundário para o banco de dados
        public Cliente Cliente { get; set; }
    }
~~~
<br>

![](https://github.com/ewertonclozato/artigoapivaivoa/blob/master/imagens/4.2.png?raw=true)

<br>
<br>

## 5. Configurando o "**DbContext**" e apontando para a nossa string de conexão com o banco de dados:
<br>

* 5.1 - Na classe Startup alteramos o método "**ConfigureServices**" para inserir a o trecho de código que inicializa o DBontext com a nossastring de conexão com o banco de dados conforme o código abaixo;
~~~C#
public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AdmDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });
            services.AddControllers();
        }
~~~
<br>

![](https://github.com/ewertonclozato/artigoapivaivoa/blob/master/imagens/5.1.png?raw=true)
![](https://github.com/ewertonclozato/artigoapivaivoa/blob/master/imagens/5.11.png?raw=true)

<br>

* 5.2 - Definimos nossa string de conexão no arquivo de configuraçao que se encontra em "**appsettings.json**":
~~~json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=Administradora;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
~~~
<br>

![](https://github.com/ewertonclozato/artigoapivaivoa/blob/master/imagens/5.2.png?raw=true)

<br>

## 6. Criando a base de dados e tabelas usando o recurso do Entity Framework chamado "**Add-Migration**":
<br>

* 6.1 - Abrimos o Package Maneger Console para usar a ferramenta "**Migration**" do Entity Framework:
<br>

![](https://github.com/ewertonclozato/artigoapivaivoa/blob/master/imagens/6.1.png?raw=true)

<br>

* 6.2 Usamos o comando "**<em>Add-migration initial</em>**" para criar o script de criação(ou atualização) do banco de dados:
<br>

![](https://github.com/ewertonclozato/artigoapivaivoa/blob/master/imagens/6.2.png?raw=true)

<br>

Podemos observar que automaticamente é criado a pasta "**Migraton**", com os dados do script de atualização do banco de dados:
<br>

![](https://github.com/ewertonclozato/artigoapivaivoa/blob/master/imagens/6.22.png?raw=true)

<br>

* 6.3 - Usamos o comando "**<em>Update-Database</em>**", para criar ou atualizar nosso banco de dados e suas respectivas tabelas (Cliente e Cartão):
<br>

![](https://github.com/ewertonclozato/artigoapivaivoa/blob/master/imagens/6.3.png?raw=true)

<br>

## 7. Criando a classe "**AdmController**" em controller, onde armazenamos as API's(Métodos) que serão expostas para serem requisitadas e criando as API's:

<br>

* 7.1 - Criamos a classe "**AdmController**";
<br>

![](https://github.com/ewertonclozato/artigoapivaivoa/blob/master/imagens/7.1.png?raw=true)

<br>

* 7.2 - Na classe controller, precisamos herdar da classe ControllerBase.
Precisamos também definir a tag de [Route("api/admcontroller")] e [ApiController].
Route para especificar um pedaço da rota da nossas APIS e ApiController para definir que a classe se trata de uma API;

<br>

![](https://github.com/ewertonclozato/artigoapivaivoa/blob/master/imagens/7.2.png?raw=true)

<br>

* 7.3 - Criamos um método para inserir clientes que serão utilizados nas outras APIs posteriormente. Mas, caso seja de sua preferência, pode inserir direto no banco de dados;
(colar imagem da API de cliente)

<br>

![](https://github.com/ewertonclozato/artigoapivaivoa/blob/master/imagens/7.3.png?raw=true)

<br>

* 7.4 - Criamos um método que será a nossa API que gera os respectivos números dos cartões por cliente, que recebemos o e-mail do mesmo com o parâmetro do request.
A explicação de cada trecho de código está em forma de comentário abaixo:

<br>

![](https://github.com/ewertonclozato/artigoapivaivoa/blob/master/imagens/7.4.png?raw=true)

<br>

* 7.5 - Criamos um método que retorna os clientes e seus respectivos cartões por ordem de criação. 
A explicação de cada trecho de código está em forma de comentário abaixo:

<br>

![](https://github.com/ewertonclozato/artigoapivaivoa/blob/master/imagens/7.5.png?raw=true)

<br>

* 7.6 - Finalizando esse processo da criação da controller, temos então nossas APIs criadas.
Para testarmos, vamos utilizar o Postman para fazer a requisição das APIs.
O postman pode ser baixado pelo site: https://www.postman.com/downloads/

<br>

![](https://github.com/ewertonclozato/artigoapivaivoa/blob/master/imagens/7.6.png?raw=true)

<br>

* 7.7 - Antes de abrir o postman, execute o projeto para que copie de forma facil a url local que utilizaremos no postman:
print do browser:

<br>

![](https://github.com/ewertonclozato/artigoapivaivoa/blob/master/imagens/7.7.png?raw=true)

<br>

* 7.8 - Copie a url do browser, abra o postman e coloque a url e selecione o verbo POST da chamada.
Para completar a url com o restante das informações, insira dessa forma:
api/admcontroller/insert-cliente
essa url consiste no padrão que definimos de exibição, com o nome da controller no meio (insert-cliente) e com o nome do nosso método no final (insert-cliente):

<br>

![](https://github.com/ewertonclozato/artigoapivaivoa/blob/master/imagens/7.8.png?raw=true)

<br>

* 7.9 - Definimos que o request da nossa API é a classe de Cliente. 
Para definirmos no postman que vamos utilizar json como tipo de parametro, selecione a aba Body, a opção "raw"e depois a opção ""JSON"
Então, depois disso, devemos montar nosso json de request da seguinte maneira:
{
    "nome": "Ewerton",
    "email": "ewerton@hotmail.com",
    "cpf": "88746576325"
}:

<br>

![](https://github.com/ewertonclozato/artigoapivaivoa/blob/master/imagens/7.9.png?raw=true)

<br>

* 7.10 - Depois de definirmos as configurações anteriores, abra uma nova aba e refaça as configurações alterando apenas o JSON, que passaremos o email que definimos de request e a URL chamada:

<br>

![](https://github.com/ewertonclozato/artigoapivaivoa/blob/master/imagens/7.10.png?raw=true)
(Faça esse processo algumas vezes para ter mais registros na base)

<br>

* 7.11 - Agora vamos testar o retorno dos clientes e seus respectivos cartões.
Como definimos que o e-mail seria apenas o parametro de entrada e não recebemos a data de criação.
Fizemos um ajuste simples no banco de dados colocando datas fictícias para efeito de teste somente. 
Segue script:

<br>

![](https://github.com/ewertonclozato/artigoapivaivoa/blob/master/imagens/7.11.png?raw=true)
Atribua os IDs corretos com base no select da tabela dos seus registros criados.

  UPDATE Cartao SET DataCriacao = '2019-10-10'
  where Id = 9

  UPDATE Cartao SET DataCriacao = '2020-08-22'
  where Id = 10

  UPDATE Cartao SET DataCriacao = '2021-05-01'
  where Id = 11

  <br>

* 7.12 - Feito isso, podemos testar finalmente o nosso GET.
Abra novamente o Postman , faça o mesmo processo que foi explicado anteriromente mas agora alterando a url desta forma: https://localhost:44392/api/admcontroller/:email. Onde aqui definimos que usaremos o parametro de e-mail no request.

Abaixo, coloque o e-mail que criamos anteriormente que será usado de parâmetro, conforme imagem abaixo:

<br>

![](https://github.com/ewertonclozato/artigoapivaivoa/blob/master/imagens/7.12.png?raw=true)
(Faça esse processo algumas vezes para ter mais registros na base)

<br>

* 7.13 - Depois de clicar em SEND, podemos ver que temos o retorno do cliente que pedimos e seus respectivos cartões, ordenados por ordem de criação:

<br>

![](https://github.com/ewertonclozato/artigoapivaivoa/blob/master/imagens/7.13.png?raw=true)
(Faça esse processo algumas vezes para ter mais registros na base)

<br>