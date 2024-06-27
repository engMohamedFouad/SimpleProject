using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SimpleProject.Models;
using SimpleProject.Services.Interfaces;
using SimpleProject.ViewModels;

namespace SimpleProject.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly IFileService _fileService;
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;
        public ProductController(IProductService productService,
                                 IFileService fileService,
                                 ICategoryService categoryService,
                                 IMapper mapper)
        {
            _productService = productService;
            _fileService = fileService;
            _categoryService = categoryService;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var products = await _productService.GetProducts();
            var result = _mapper.Map<List<GetProductListViewModel>>(products);
            ViewBag.count = result.Count();
            return View(result);
        }
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            return View(product);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewData["categories"] = new SelectList(await _categoryService.GetCategoriesAsync(), "Id", "NameAr");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(AddProductViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    var product = _mapper.Map<Product>(model);
                    var result = await _productService.AddProduct(product, model.Files);
                    if (result != "Success")
                    {
                        ModelState.AddModelError(string.Empty, result);
                        TempData["Failed"] = result;
                        return View(model);
                    }
                    TempData["Success"] = "Create Successfully";
                    return RedirectToAction(nameof(Index));
                }
                ViewData["categories"] = new SelectList(await _categoryService.GetCategoriesAsync(), "Id", "Name");
                return View(model);
            }
            catch (Exception ex)
            {
                ViewData["categories"] = new SelectList(await _categoryService.GetCategoriesAsync(), "Id", "Name");
                TempData["Failed"] = ex.Message + "--" + ex.InnerException;
                return View(model);
            }

        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null) return NotFound();
            var response = _mapper.Map<UpdateProductViewModel>(product);
            ViewData["categories"] = new SelectList(await _categoryService.GetCategoriesAsync(), "Id", "NameAr");
            return View(response);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id, UpdateProductViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (id != model.Id) return NotFound();
                    var product = await _productService.GetProductByIdWithoutIncludeAsync(id);
                    if (product == null) return NotFound();

                    var newProduct = _mapper.Map(model, product);
                    var result = await _productService.UpdateProduct(newProduct, model.Files);
                    if (result != "Success")
                    {
                        ModelState.AddModelError(string.Empty, result);
                        ViewData["categories"] = new SelectList(await _categoryService.GetCategoriesAsync(), "Id", "NameAr");
                        return View(model);
                    }
                    return RedirectToAction(nameof(Index));
                }
                return View(model);
            }
            catch (Exception)
            {
                ViewData["categories"] = new SelectList(await _categoryService.GetCategoriesAsync(), "Id", "NameAr");
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productService.GetProductByIdWithoutIncludeAsync(id);
            if (product == null) return NotFound();
            return View(product);
        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            try
            {
                var product = await _productService.GetProductByIdWithoutIncludeAsync(id);
                if (product == null) return NotFound();
                await _productService.DeleteProduct(product);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                return View();
            }
        }
        [HttpPost]
        public async Task<IActionResult> IsProductNameArExist(string nameAr)
        {
            var result = await _productService.IsProductNameArExistAsync(nameAr);
            if (result)
                return Json(false);
            return Json(true);
        }
        [HttpPost]
        public async Task<IActionResult> IsProductNameEnExist(string nameEn)
        {
            var result = await _productService.IsProductNameEnExistAsync(nameEn);
            if (result)
                return Json(false);
            return Json(true);
        }
    }
}
