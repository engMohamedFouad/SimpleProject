using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SimpleProject.Helpers;
using SimpleProject.Models.Identity;
using SimpleProject.Services.Interfaces;
using SimpleProject.ViewModels.Identity.Claims;

namespace SimpleProject.Controllers
{
    public class ClaimController : Controller
    {
        private readonly IClaimService _claimService;
        private readonly IMapper _mapper;
        public ClaimController(IClaimService claimService, IMapper mapper)
        {
            _claimService = claimService;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            var result = await _claimService.GetClaimsAsync();
            var claims = _mapper.Map<List<GetClaimsViewModel>>(result);
            return View(claims);
        }
        [HttpGet]
        public IActionResult Create()
        {
            ViewData["chooses"] = new SelectList(Store.Chooses, "KeyValue", CultureHelper.IsArabic() ? "NameAr" : "NameEn");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(AddClaimViewModel model)
        {
            if (ModelState.IsValid)
            {
                var mapper = _mapper.Map<Claim>(model);
                var result = await _claimService.AddClaimAsync(mapper);
                if (result=="Success") return RedirectToAction(nameof(Index));
                ModelState.AddModelError(string.Empty, result);
                ViewData["chooses"] = new SelectList(Store.Chooses, "Id", CultureHelper.IsArabic() ? "NameAr" : "NameEn");
                return View(result);
            }
            ViewData["chooses"] = new SelectList(Store.Chooses, "Id", CultureHelper.IsArabic() ? "NameAr" : "NameEn");
            return View(model);
        }

    }
}
