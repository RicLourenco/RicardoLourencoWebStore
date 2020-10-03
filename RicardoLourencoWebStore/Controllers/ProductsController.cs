using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
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
        public IActionResult Index()
        {
            var list = _productRepository.GetAllWithCategories();

            List<ProductViewModel> modelList = new List<ProductViewModel>();

            foreach(var product in list)
            {
                modelList.Add(_converterHelper.ToProductViewModel(product));
            }

            return View(modelList);
        }

        //public IActionResult UrlDataSource([FromBody] DataManagerRequest dataManager)
        //{
        //    IEnumerable dataSource = _productRepository.GetAll();

        //    DataOperations operation = new DataOperations();

        //    int count = dataSource.Cast<Product>().Count();

        //    if (dataManager.Skip != 0)
        //    {
        //        dataSource = operation.PerformSkip(dataSource, dataManager.Skip);   //Paging
        //    }

        //    if (dataManager.Take != 0)
        //    {
        //        dataSource = operation.PerformTake(dataSource, dataManager.Take);
        //    }

        //    return dataManager.RequiresCounts ? Json(new { result = dataSource, count = count }) : Json(dataSource);
        //}

        //public async Task<ActionResult> Insert([FromBody]CRUDModel<Product> model)
        //{
        //    await _productRepository.CreateAsync(model.Value);
        //    return Json(model.Value);
        //}

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

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            var categories = _categoryRepository.GetComboCategories();

            var model = new ProductViewModel
            {
                Categories = categories
            };

            return View(model);
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
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

        // GET: Products/Edit/5
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
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price,LastPurchase,LastSale,IsAvailable,Stock,ImageUrl")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
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
            return View(product);
        }

        // GET: Products/Delete/5
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

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
