using Data;
using Entities;
using Microsoft.AspNetCore.Mvc;
using SoundofSilence.IServices;
using Microsoft.AspNetCore.Cors;

namespace SoundofSilence.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly ServiceContext _serviceContext;

        public CategoryController(ICategoryService categoryService, ServiceContext serviceContext)
        {
            _categoryService = categoryService;
            _serviceContext = serviceContext;
        }

        [HttpPost(Name = "InsertCategory")]
        public IActionResult PostCategory([FromBody] Category category)
        {
            try
            {
                // Validaciones de categoría aquí (puedes verificar si ya existe, por ejemplo)
                if (category == null)
                {
                    return BadRequest("La categoría proporcionada es nula.");
                }

                _serviceContext.Category.Add(category);
                _serviceContext.SaveChanges();

                // Devolver una respuesta con el objeto recién creado
                return Ok(category); // Devolvemos un código 200 OK junto con la categoría insertada
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al insertar la categoría: {ex.Message}");
            }
        }

        [HttpPut("{id}", Name = "UpdateCategory")]
        public IActionResult PutCategory(int id, [FromBody] Category category)
        {
            try
            {
                var existingCategory = _serviceContext.Category.Find(id);

                if (existingCategory == null)
                {
                    return NotFound($"La categoría con ID {id} no fue encontrada.");
                }

                // Actualizar las propiedades de la categoría existente
                existingCategory.name_category = category.name_category;

                _serviceContext.SaveChanges();

                return Ok(existingCategory); // Devolvemos la categoría actualizada con un código 200 OK
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al actualizar la categoría: {ex.Message}");
            }
        }
    }
}
