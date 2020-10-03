using Microsoft.EntityFrameworkCore;
using RicardoLourencoWebStore.Data.Entities;
using RicardoLourencoWebStore.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RicardoLourencoWebStore.Data.Repositories.Classes
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        DataContext _context;

        public ProductRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public IEnumerable<Product> GetAllWithCategories()
        {
            return GetAll().Include(p => p.Category);
        }
    }
}
