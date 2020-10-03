using Microsoft.AspNetCore.Mvc.Rendering;
using RicardoLourencoWebStore.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RicardoLourencoWebStore.Data.Repositories.Interfaces
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        IEnumerable<SelectListItem> GetComboCategories();
    }
}
