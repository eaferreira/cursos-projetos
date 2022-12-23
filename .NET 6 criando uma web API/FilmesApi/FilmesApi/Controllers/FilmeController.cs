using AutoMapper;
using FilmesApi.Data;
using FilmesApi.Data.Dtos;
using FilmesApi.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace FilmesApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FilmeController : ControllerBase
    {
        private FilmeContextDb _context;
        private IMapper _mapper;

        public FilmeController(FilmeContextDb context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        /// <summary>
        /// Adiciona um filme ao banco de dados
        /// </summary>
        /// <param name="Adicionar filme">Objeto com os campos necessários para criação de um filme</param>
        /// <returns>IActionResult</returns>
        /// <response code="201">Caso inserção seja feita com sucesso</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Add([FromBody]CreateFilmeDto filmeDto)
        {
            FilmeModel filme = _mapper.Map<FilmeModel>(filmeDto);

            _context.Filmes.Add(filme);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = filme.Id }, filme);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var filme = _context.Filmes.FirstOrDefault(x => x.Id == id);

            if(filme.IsNullOrEmpty())
                return NotFound();

            var filmeDto = _mapper.Map<ReadFilmeDto>(filme);

            return Ok(filmeDto);
        }

        [HttpGet]
        public IEnumerable<ReadFilmeDto> GetAll([FromQuery]int skip = 0, [FromQuery]int take = 50)
        {
            return _mapper.Map<List<ReadFilmeDto>>(_context.Filmes.Skip(skip).Take(take));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody]UpdateFilmeDto filmeDto)
        {
            var filme = _context.Filmes.FirstOrDefault(x=> x.Id == id);

            if (filme.IsNullOrEmpty())
                return NotFound();

            _mapper.Map(filmeDto, filme);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateParcial(int id, [FromBody] JsonPatchDocument<UpdateFilmeDto> path)
        {
            var filme = _context.Filmes.FirstOrDefault(x => x.Id == id);

            if (filme.IsNullOrEmpty())
                return NotFound();

            //mapeia o filme do banco para update filme
            var filmeUpdate = _mapper.Map<UpdateFilmeDto>(filme);

            //tenta aplicar/valida filmeUpdate com path passado na requisicao
            path.ApplyTo(filmeUpdate, ModelState);

            if (!TryValidateModel(filmeUpdate))
            {
                return ValidationProblem(ModelState);
            }
            //mapeia novamente filme para update e salva no banco de dados
            _mapper.Map<UpdateFilmeDto>(filme);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var filme = _context.Filmes.FirstOrDefault(x => x.Id == id);

            if (filme.IsNullOrEmpty())
                return NotFound();

            _context.Remove(filme);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
