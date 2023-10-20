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
                
                if (category == null)
                {
                    return BadRequest("The category provided is null.");
                }

                _serviceContext.Category.Add(category);
                _serviceContext.SaveChanges();

                
                return Ok(category); 
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error inserting category: {ex.Message}");
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
                    return NotFound($"The category with ID {id} not found.");
                }

               
                existingCategory.name_category = category.name_category;

                _serviceContext.SaveChanges();

                return Ok(existingCategory); 
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error updating category: {ex.Message}");
            }
        }
    }
}
