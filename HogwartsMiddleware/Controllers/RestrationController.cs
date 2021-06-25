using Hogwarts.Middleware.Dtos;
using Hogwarts.Middleware.Interfaces.Service;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Hogwarts.Middleware.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CharacterController : ControllerBase
    {
        public ICharacterService _service { get; set; }
        public CharacterController(ICharacterService service)
        {
            _service = service;
        }
        [HttpGet("GetAllCharacterHouse/house={house}")]
        public async Task<ActionResult> GetAllCharacterHouse(string house)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                return Ok(await _service.GetAllCharacterHouse(house));
            }
            catch (ArgumentException e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }
        [HttpGet]
        [Route("{id}", Name = "GetWithId")]
        public async Task<ActionResult> Get(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                return Ok(await _service.Get(id));
            }
            catch (ArgumentException e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CharacterInsertDto character)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var id = Guid.Parse(character.house);
                var HousesExists = await _service.GetHousesApiPotter(id);
                if (!HousesExists)
                {
                    return StatusCode((int)HttpStatusCode.Unauthorized, "Casa não encontrada em potterApi!");
                }
                var result = await _service.Post(character);
                if (result != null)
                {
                    return Created(new Uri(Url.Link("GetWithId", new { id = result.id })), result);
                }
                else
                {                    
                    return BadRequest();
                }
            }
            catch (ArgumentException e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult> Put([FromBody] CharacterUpdateDto character)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var id = Guid.Parse(character.house);
                var HousesExists = await _service.GetHousesApiPotter(id);
                if (!HousesExists)
                {
                    return StatusCode((int)HttpStatusCode.Unauthorized, "Casa não encontrada em potterApi!");
                }
                var result = await _service.Put(character);
                if (result != null)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (ArgumentException e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                return Ok(await _service.Delete(id));
            }
            catch (ArgumentException e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }
    }
}