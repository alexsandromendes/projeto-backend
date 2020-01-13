using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectBackendTest.Models.Request;
using ProjectBackendTest.Services.Contracts;

namespace ProjectBackendTest.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioService _service;
   

        public UsuariosController(IUsuarioService service)
        {
            _service = service;
        }

        // GET: api/Usuarios
        [HttpGet]
        public  List<UsuarioRequest> GetUsuario()
        {
            var reponse = _service.GetAll();
            return reponse;
        }

        //GET: api/Usuarios/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUsuarios([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = _service.GetPorId(id);
            
            if(response == null)
            {
                return NotFound();
            }
            return Ok(response);
        }

        // PUT: api/Usuarios/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuario([FromRoute] int id, [FromBody] UsuarioRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != request.Id.Value)
            {
                return BadRequest("Id para atualizar diferente id da Usuario");
            }

            try
            {
                var response = _service.Atualizar(id, request);
                if(!response)
                {
                    return NotFound("Usuario não cadastro.");
                }
                return Ok(response);
            }
            catch (Exception e)
            {
                return BadRequest(e);

            }

            return NoContent();
        }

        // POST: api/Usuarios
        [HttpPost]
        public IActionResult PostUsuario([FromBody] UsuarioRequest request)
         {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _service.Salvar(request);

                return CreatedAtAction("GetUsuario", new { id = request.Nome }, request);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
                return BadRequest(ModelState);
            }

        }

        // DELETE: api/Usuarios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = _service.Remover(id);

            if (!result)
            {
                return NoContent();
            }

            return Ok();
        }
    }
}