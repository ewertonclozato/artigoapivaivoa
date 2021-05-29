using GeradorCartaoAPI.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GeradorCartaoAPI.Controllers
{
    [Route("api/admcontroller")]
    [ApiController]
    public class AdmController : ControllerBase
    {
        private readonly AdmDbContext _context;

        public AdmController(AdmDbContext context)
        {
            _context = context;
        }

        /*Criacao de metodo para inserir clientes
         *Metodo que tem como retorno a entidade de cliente e recebe como parametro a mesma entidade*/
        [HttpPost("insert-cliente")]
        public async Task<ActionResult<Cliente>> PostCliente(Cliente cliente)
        {
            _context.Cliente.Add(cliente);
            await _context.SaveChangesAsync();

            return Ok();
        }

        /*Criacao de metodo para inserir os cartoes por cliente
         *Metodo que tem como retorno a entidade de cartoes e recebe como parametro a entidade de cliente*/
        [HttpPost("insert-cartao")]
        public async Task<ActionResult<Cartao>> PostCartao(Cliente clienteRequest)
        {
            //Verificacao no request se nao é nulo
            if (clienteRequest == null)
            {
                return BadRequest();
            }

            //Aqui utilizamos LINQ para obter buscar o cliente no banco com base no e-mail recebido
            var cliente = await (from cli in _context.Cliente
                                 where cli.Email == clienteRequest.Email
                                 select cli).ToListAsync();

            //caso o cliente seja nulo, devolver NotFound
            if (cliente == null)
            {
                return NotFound();
            }

            //Aqui utilizamos a classe Random do System que sera utilizado para gerar numeros aleatorios para compor o numero do nosso carato
            Random random = new Random();
            string codigoCartao = string.Empty;

            //Aqui utilizamos o for para percorrer 16 vezes e gerar um novo numero aleatorio incluindo na variavel criada anteriormente
            for (int i = 0; i < 16; i++)
            {
                var numero = Convert.ToChar(random.Next(9).ToString());
                codigoCartao += numero;
            }

            //Aqui criamos uma instancia da classe cartao para ser executada no banco de dados, utilizando o codigo gerado na propriedade "Numero"
            var cartao = new Cartao
            {
                Bandeira = "Visa",
                Numero = codigoCartao,
                Cliente = cliente.FirstOrDefault()
            };

            //Utilizamos a funcao Add e posteriormente o SaveChangesAsync() do context do entity para salvar na base de dados
            _context.Cartao.Add(cartao);
            await _context.SaveChangesAsync();

            //Aqui retornamos Ok, que representa o codigo 200, com o cartao que geramos
            return Ok(cartao);
        }

        /*Criacao de metodo para retornar todos os cartoes com base no email recebido
        *Metodo que tem como parametro a string do email e retorna a entidade do Cliente com a lista de cartoes do mesmo*/
        [HttpGet("{email}")]
        public async Task<ActionResult<Cliente>> GetCliente(string email)
        {
            //aqui verificamos se o parametro do request nao está vazio nem nulo
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest();
            }

            /*qui utilizamos da funcao o LINQ, assim como no exemplo anterior,
            *porem foi utilizado a funcao Include para fazer o papel do Join com a tabela de Cartao, para retornar os dados do mesmo*/
            var cliente = await (from cli in _context.Cliente.Include(c => c.Cartoes.OrderBy(o => o.DataCriacao))
                                 where cli.Email == email
                                 select cli).ToListAsync();

            //caso nao encontre na base, retornamos NotFound
            if (cliente == null)
            {
                return NotFound();
            }

            //Caso encontre, utilizamos a funcao FirstOrDefault() para retornar o primeiro/unico cliente encontrado.
            var result = cliente.FirstOrDefault();

            //retornamos Ok com o resultado
            return Ok(result);
        }
    }
}
