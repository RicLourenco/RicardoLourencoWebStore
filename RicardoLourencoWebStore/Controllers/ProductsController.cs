using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RicardoLourencoWebStore.Data;
using RicardoLourencoWebStore.Data.Entities;
using RicardoLourencoWebStore.Data.Repositories.Interfaces;
using RicardoLourencoWebStore.Helpers.Interfaces;
using RicardoLourencoWebStore.Models;
using Syncfusion.EJ2;
using Syncfusion.EJ2.Base;

namespace RicardoLourencoWebStore.Controllers
{
    public class ProductsController : Controller
    {
        readonly DataContext _context;
        readonly IConverterHelper _converterHelper;
        readonly IProductRepository _productRepository;
        readonly ICategoryRepository _categoryRepository;
        readonly IImageHelper _imageHelper;

        public ProductsController(
            DataContext context,
            IConverterHelper converterHelper,
            IProductRepository productRepository,
            ICategoryRepository categoryRepository,
            IImageHelper imageHelper)
        {
            _context = context;
            _converterHelper = converterHelper;
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _imageHelper = imageHelper;
        }

        // GET: Products
        public IActionResult Index(int? id)
        {
            IEnumerable<Product> list;

            if (id != null)
            {
                list = _productRepository.GetAllWithCategories().Where(p => p.CategoryId == id);
            }
            else
            {
                list = _productRepository.GetAllWithCategories();
            }

            List<ProductViewModel> modelList = new List<ProductViewModel>();

            foreach(var product in list)
            {
                if (User.IsInRole("ReSeller"))
                {
                    product.Price *= 0.8f;
                }

                modelList.Add(_converterHelper.ToProductViewModel(product));
            }

            return View(modelList);
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            if (User.IsInRole("ReSeller"))
            {
                product.Price = product.Price * 0.8f;
            }

            return View(product);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            var categories = _categoryRepository.GetComboCategories();

            var model = new ProductViewModel
            {
                Categories = categories
            };

            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                var path = string.Empty;

                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    path = await _imageHelper.UploadImageAsync(model.ImageFile, "Products");
                }

                var product = _converterHelper.ToProduct(model, path, true);

                await _productRepository.CreateAsync(product);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            var view = _converterHelper.ToProductViewModel(product);

            return View(view);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var path = model.ImageUrl;

                    if (model.ImageFile != null && model.ImageFile.Length > 0)
                    {
                        path = await _imageHelper.UploadImageAsync(model.ImageFile, "Products");
                    }

                    var product = _converterHelper.ToProduct(model, path, false);

                    await _productRepository.UpdateAsync(product);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _productRepository.ExistsAsync(model.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var product = await _context.Products.FindAsync(id);
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View("DeleteError");
            }
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
