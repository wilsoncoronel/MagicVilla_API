using AutoMapper;
using MagicVilla_API.Datos;
using MagicVilla_API.Modelos;
using MagicVilla_API.Modelos.Dto;
using MagicVilla_API.Repositorios.IRepositorio;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace MagicVilla_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaController : ControllerBase
    {
        private readonly ILogger<VillaController> _logger;
        private readonly IVillaRepositorio _villaRepo;
        private readonly IMapper _mapper;
        protected APIResponse _response;

        public VillaController(ILogger<VillaController> logger, IVillaRepositorio villaRepo, IMapper mapper)
        {
            _logger = logger;
            _villaRepo = villaRepo;
            _mapper = mapper;
            _response = new();
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<APIResponse>> GetVillas()
        {
            try
            {
                _logger.LogInformation("Obtener las villas");
                IEnumerable<Villa> villaList = await _villaRepo.ObtenerTodos();
                _response.Resultado = _mapper.Map<IEnumerable<VillaDto>>(villaList);
                _response.statusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex) {
                _response.IsExitoso = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpGet("id:int", Name ="GetVilla")]//se envia el id para buscar la villa
        //Codigos de estado de respuesta
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<APIResponse>> GetVilla(int id)
        {
            try
            {
                if (id == 0)
                {
                    _logger.LogError("Error al traer la villa con id: " + id);
                    _response.statusCode = HttpStatusCode.BadRequest;
                    _response.IsExitoso=false;
                    return BadRequest(_response);
                }

                //var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);
                var villa = await _villaRepo.Obtener(v => v.Id == id);
                if (villa == null)
                {
                    _response.statusCode= HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                _response.Resultado = _mapper.Map<VillaDto>(villa);
                _response.statusCode = HttpStatusCode.OK;
                
                return Ok(_response);
            }
            catch (Exception ex) {
                _response.IsExitoso=false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }  

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(405)]
        public async Task<ActionResult<APIResponse>> CrearVilla([FromBody] VillaCreateDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (await _villaRepo.Obtener(v => v.Nombre.ToLower() == createDto.Nombre.ToLower()) != null)
                {
                    ModelState.AddModelError("NombreExiste", "La Villa con ese Nombre ya existe!");
                    return BadRequest(ModelState);
                }

                if (createDto == null)
                {
                    return BadRequest(createDto);
                }

                //villaDto.Id = VillaStore.villaList.OrderByDescending(v=>v.Id).FirstOrDefault().Id+1;
                //VillaStore.villaList.Add(villaDto);
                Villa modelo = _mapper.Map<Villa>(createDto);
                await _villaRepo.Crear(modelo);
                _response.Resultado = modelo;
                _response.statusCode = HttpStatusCode.Created;
                return CreatedAtRoute("GetVilla", new { id = modelo.Id }, modelo);
            }
            catch (Exception ex) { 
                _response.IsExitoso = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpDelete("id:int")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(405)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteVilla(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.IsExitoso =false;
                    _response.statusCode=HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var villa = await _villaRepo.Obtener(v => v.Id == id);
                if (villa == null)
                {
                    _response.IsExitoso = false;
                    _response.statusCode=HttpStatusCode.NotFound;
                    return NotFound();
                }
                await _villaRepo.Remover(villa);
                _response.statusCode = HttpStatusCode.NoContent;
                return Ok(_response);
            }
            catch (Exception ex) { 
                _response.IsExitoso=false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return BadRequest(_response);
        }

        [HttpPut("id: int")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(405)]
        public async Task<IActionResult> UpdateVilla(int id, [FromBody] VillaUpdateDto updateDto)
        {
            if (updateDto == null || id != updateDto.Id)
            {
                _response.IsExitoso = false;
                _response.statusCode=HttpStatusCode.BadRequest;
                return BadRequest(_response);    
            }
            Villa modelo = _mapper.Map<Villa>(updateDto);

            await _villaRepo.Actualizar(modelo);
            _response.statusCode = HttpStatusCode.NoContent;
            return Ok(_response);

        }

        [HttpPatch("id: int")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(405)]
        public async Task<IActionResult> UpdatePartialVilla(int id, JsonPatchDocument<VillaUpdateDto> patchDto)
        {
            if (patchDto == null || id == 0) 
            {
                return BadRequest();
            }
            //var villa = VillaStore.villaList.FirstOrDefault(v=> v.Id == id);

            var villa = await _villaRepo.Obtener(v => v.Id == id, tracked: false);
            VillaUpdateDto villaDto = _mapper.Map<VillaUpdateDto>(villa);
            if(villa == null) { return BadRequest(); }
            patchDto.ApplyTo(villaDto, ModelState);

            if (!ModelState.IsValid) 
            {
                return BadRequest(ModelState);
            }

            Villa modelo = _mapper.Map<Villa>(villaDto);

            await _villaRepo.Actualizar(modelo);
            _response.statusCode = HttpStatusCode.NoContent;

            return Ok(_response);
        }
    }
}