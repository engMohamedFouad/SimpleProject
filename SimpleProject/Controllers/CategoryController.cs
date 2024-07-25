using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SimpleProject.Models;
using SimpleProject.Services.Interfaces;
using SimpleProject.ViewModels.Categories;

namespace SimpleProject.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;
        public CategoryController(ICategoryService categoryService, IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var categories = await _categoryService.GetCategoriesAsync();
            var result = _mapper.Map<List<GetCategoriesListViewModel>>(categories);
            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id==null) return NotFound();
            var category = await _categoryService.GetCategoryByIdAsync((int)id);
            var result = _mapper.Map<GetCategoryByIdViewModel>(category);
            return View(result);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(AddCategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var category = _mapper.Map<Category>(model);
                var result = await _categoryService.AddCategoryAsync(category);
                if (result!="Success")
                {
                    return BadRequest(result);
                }
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {
            if (id==null) return NotFound();
            var category = await _categoryService.GetCategoryByIdAsync((int)id);
            if (category==null) return NotFound();
            var response = _mapper.Map<UpdateCategoryViewModel>(category);
            return View(response);
        }
        [HttpPost]
        public async Task<IActionResult> Update(UpdateCategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var oldCategory = await _categoryService.GetCategoryByIdWithoutIncludeAsync(model.Id);
                if (oldCategory==null) return NotFound();
                var category = _mapper.Map(model, oldCategory);
                var result = await _categoryService.UpdateCategoryAsync(category);
                if (result!="Success")
                {
                    return BadRequest(result);
                }
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id==null) return NotFound();
            var category = await _categoryService.GetCategoryByIdAsync((int)id);
            if (category==null) return NotFound();
            var response = _mapper.Map<GetCategoryByIdViewModel>(category);
            return View(response);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirm(int? id)
        {
            if (id==null) return NotFound();
            var category = await _categoryService.GetCategoryByIdWithoutIncludeAsync((int)id);
            if (category==null) return NotFound();
            var result = await _categoryService.DeleteCategoryAsync(category);
            if (result!="Success")
            {
                return BadRequest(result);
            }
            return RedirectToAction(nameof(Index));
        }

    }
}
