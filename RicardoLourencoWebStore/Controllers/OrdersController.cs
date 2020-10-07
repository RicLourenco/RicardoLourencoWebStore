using System;
using System.Collections.Generic;
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

namespace RicardoLourencoWebStore.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        readonly IOrderRepository _orderRepository;
        readonly IProductRepository _productRepository;
        readonly IMailHelper _mailHelper;
        readonly IPdfHelper _pdfHelper;
        readonly IUserHelper _userHelper;
        readonly DataContext _context;

        public OrdersController(
            IOrderRepository orderRepository,
            IProductRepository productRepository,
            IMailHelper mailHelper,
            IPdfHelper pdfHelper,
            IUserHelper userHelper,
            DataContext context)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _mailHelper = mailHelper;
            _pdfHelper = pdfHelper;
            _userHelper = userHelper;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var model = await _orderRepository.GetOrdersAsync(User.Identity.Name);
            return View(model);
        }

        public async Task<IActionResult> Create()
        {
            var model = await _orderRepository.GetDetailTempAsync(User.Identity.Name);
            return View(model);
        }

        public IActionResult AddProduct()
        {
            var model = new AddItemViewModel
            {
                Quantity = 1,
                Products = _productRepository.GetComboProducts()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(AddItemViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _orderRepository.AddItemToOrderAsync(model, User.Identity.Name, User.IsInRole("ReSeller"));

                return RedirectToAction("Create");
            }

            return View(model);
        }

        public async Task<IActionResult> DeleteItem(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            await _orderRepository.DeleteDetailTempAsync(id.Value);

            return RedirectToAction("Create");
        }

        public async Task<IActionResult> Increase(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            await _orderRepository.ModifyOrderDetailTempQuantityAsync(id.Value, 1);

            return RedirectToAction("Create");
        }

        public async Task<IActionResult> Decrease(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            await _orderRepository.ModifyOrderDetailTempQuantityAsync(id.Value, -1);

            return RedirectToAction("Create");
        }

        public async Task<IActionResult> ConfirmOrder()
        {
            var response = await _orderRepository.ConfirmOrderAsync(User.Identity.Name);

            if (response)
            {
                return RedirectToAction("Index");
            }

            return RedirectToAction("Create");
        }

        public async Task<IActionResult> Deliver(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _orderRepository.GetOrderAsync(id.Value);

            if (order == null)
            {
                return NotFound();
            }

            var model = new DeliveryViewModel
            {
                Id = order.Id,
                DeliveryDate = DateTime.Today
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Deliver(DeliveryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
                if(user.Address == null || user.FullName == string.Empty)
                {
                    ModelState.AddModelError(string.Empty, "User must have a full name and an address to deliver");
                    return View(model);
                }

                await _orderRepository.DeliverOrderAsync(model);

                var order = await _orderRepository.GetByIdAsync(model.Id);

                order.User = user;

                IEnumerable<OrderDetail> items = _context.OrderDetails.Where(i =>i.OrderId == order.Id);

                order.Items = items;

                _mailHelper.SendInvoiceMail(User.Identity.Name, model, await _pdfHelper.GenerateBillPDFAsync(order));

                return RedirectToAction("Index");
            }

            return View();
        }
    }
}
