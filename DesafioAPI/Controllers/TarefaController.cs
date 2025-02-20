using Microsoft.AspNetCore.Mvc;
using TrilhaApiDesafio.Context;
using TrilhaApiDesafio.Models;

namespace TrilhaApiDesafio.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TarefaController : ControllerBase
    {
        private readonly OrganizadorContext _context;

        public TarefaController(OrganizadorContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public IActionResult ObterPorId(int id)
        {
           
            var tarefa = _context.Tarefas.Find(id);

             if (tarefa == null)
            {
                return NotFound("Tarefa não encontrada.");
            }
    
            return Ok(tarefa);
        }

        [HttpGet("ObterTodos")]
        public IActionResult ObterTodos()
        {
           
            var tarefa = _context.Tarefas.ToList();

            if (tarefa.Count == 0)
            {
                return NotFound("Nenhum tarefa encontrada");
            }

            return Ok(tarefa);
        }

        [HttpGet("ObterPorTitulo")]
        public IActionResult ObterPorTitulo(string titulo)
        {
           
            var tarefa = _context.Tarefas.Where(x => x.Titulo.Contains(titulo)).ToList();
            
            
            return Ok(tarefa);
        }

        [HttpGet("ObterPorData")]
        public IActionResult ObterPorData(DateTime data)
        {
            var tarefa = _context.Tarefas.Where(x => x.Data.Date == data.Date);
            return Ok(tarefa);
        }

        [HttpGet("ObterPorStatus")]
        public IActionResult ObterPorStatus(EnumStatusTarefa status)
        {
           
            var tarefa = _context.Tarefas.Where(x => x.Status == status).ToList();
            if (tarefa == null || tarefa.Count == 0)
            {
                return NotFound("Nenhuma tarefa encontrada com o status informado");
            }
            return Ok(tarefa);
        }

        [HttpPost]
        public IActionResult Criar(Tarefa tarefa)
        {
            if (tarefa.Data == DateTime.MinValue)
                return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });
                
                _context.Add(tarefa);
                _context.SaveChanges();

            return CreatedAtAction(nameof(ObterPorId), new { id = tarefa.Id }, tarefa);
        }

        [HttpPut("{id}")]
        public IActionResult Atualizar(int id, Tarefa tarefa)
        {
            var tarefaBanco = _context.Tarefas.Find(id);

            if (tarefaBanco == null)
                return NotFound();

            if (tarefa.Data == DateTime.MinValue)
                return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });

            // TODO: Atualizar as informações da variável tarefaBanco com a tarefa recebida via parâmetro
            tarefaBanco.Titulo = tarefa.Titulo; // Exemplo de campo a ser atualizado
            tarefaBanco.Status = tarefa.Status;   // Exemplo de campo a ser atualizado
            tarefaBanco.Data = tarefa.Data;  
            // TODO: Atualizar a variável tarefaBanco no EF e salvar as mudanças (save changes)
            _context.SaveChanges();                // Salva as alterações no banco de dados

            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Deletar(int id)
        {
            var tarefaBanco = _context.Tarefas.Find(id);

            if (tarefaBanco == null)
                return NotFound();

            // TODO: Remover a tarefa encontrada através do EF e salvar as mudanças (save changes)
            _context.Tarefas.Remove(tarefaBanco);
            _context.SaveChanges(); // Salva as mudanças no banco de dados

            return NoContent();
        }
    }
}
