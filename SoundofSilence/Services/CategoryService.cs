using Data;
using Entities;
using SoundofSilence.IServices;

namespace SoundofSilence.Services
{
    public class CategoryService: BaseContextService, ICategoryService
    {

            public CategoryService(ServiceContext serviceContext) : base(serviceContext)
            {
            }

            public int InsertCategory(Category category)
            {
                _serviceContext.Category.Add(category);
                _serviceContext.SaveChanges();
                return category.Id_category;
            }



        }
}
