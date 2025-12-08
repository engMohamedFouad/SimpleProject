using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleProject.Models.Identity;
using SimpleProject.UnitOfWorks;
using SimpleProject.ViewModels.Identity.Roles;
using securityClaims = System.Security.Claims;

namespace SimpleProject.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public RoleController(RoleManager<IdentityRole> roleManager, IMapper mapper, UserManager<User> userManager, IUnitOfWork unitOfWork)
        {
            _roleManager = roleManager;
            _mapper = mapper;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var roles = _roleManager.Roles.ToList();
            var result = _mapper.Map<List<GetRolesViewModel>>(roles);
            return View(result);
        }
        [Authorize(Policy = "CreateRole")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [Authorize(Policy = "CreateRole")]
        [HttpPost]
        public async Task<IActionResult> Create(AddRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var role = new IdentityRole() { Name=model.Name };
                var result = await _roleManager.CreateAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(model);
            }
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Update(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null) return NotFound();
            var result = _mapper.Map<UpdateRoleViewModel>(role);
            return View(result);
        }
        [HttpPost]
        public async Task<IActionResult> Update(UpdateRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var role = await _roleManager.FindByIdAsync(model.Id);
                if (role == null) return NotFound();
                var newRole = _mapper.Map(model, role);
                var result = await _roleManager.UpdateAsync(newRole);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(model);
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null) return NotFound();
            var result = _mapper.Map<GetRoleByIdViewModel>(role);
            return View(result);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null) return NotFound();
            var result = _mapper.Map<GetRoleByIdViewModel>(role);
            return View(result);
        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirm(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null) return NotFound();
            var result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Index));
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ManageUsersInRole(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null) return NotFound();
            var users = await _userManager.Users.ToListAsync();

            var manageUsersList = new List<ManageUsersInRoleViewModel>();
            foreach (var user in users)
            {
                var managedUser = _mapper.Map<ManageUsersInRoleViewModel>(user);
                var isUserInRole = await _userManager.IsInRoleAsync(user, role.Name);
                managedUser.IsSelected=isUserInRole;
                manageUsersList.Add(managedUser);
            }
            ViewBag.roleId=roleId;
            return View(manageUsersList);
        }
        [HttpPost]
        public async Task<IActionResult> ManageUsersInRole(List<ManageUsersInRoleViewModel> manageUsersInRoleModels, string roleId)
        {
            var trans = await _unitOfWork.BeginTransactionAsync();
            try
            {
                var role = await _roleManager.FindByIdAsync(roleId);
                if (role == null) return NotFound();
                foreach (var model in manageUsersInRoleModels)
                {
                    var user = await _userManager.FindByIdAsync(model.UserId);
                    if (user==null) return NotFound();
                    if (model.IsSelected&&!(await _userManager.IsInRoleAsync(user, role.Name)))
                    {
                        await _userManager.AddToRoleAsync(user, role.Name);
                    }
                    else if (!model.IsSelected&&(await _userManager.IsInRoleAsync(user, role.Name)))
                    {
                        await _userManager.RemoveFromRoleAsync(user, role.Name);
                    }
                    else
                    {
                        continue;
                    }
                }
                await trans.CommitAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                await trans.RollbackAsync();
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(manageUsersInRoleModels);
            }

        }
        [HttpGet]
        public async Task<IActionResult> ManageRoleClaims(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null) return NotFound();
            var claims = await _unitOfWork.Repository<Claim>().GetListAsync();
            var roleClaimsList = claims.Where(x => x.IsRoleClaim==true);
            //role Claims
            var roleClaims = await _roleManager.GetClaimsAsync(role);
            var model = new ManageRoleClaimsViewModel();
            model.RoleId=roleId;
            foreach (var claim in roleClaimsList)
            {
                var roleClaim = new RoleClaim();
                roleClaim.ClaimType=claim.NameEn;
                if (roleClaims.Any(x => x.Type==claim.NameEn))
                    roleClaim.IsSelected = true;
                else
                    roleClaim.IsSelected = false;
                model.RoleClaims.Add(roleClaim);
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ManageRoleClaims(ManageRoleClaimsViewModel model)
        {
            var trans = await _unitOfWork.BeginTransactionAsync();
            try
            {
                var role = await _roleManager.FindByIdAsync(model.RoleId);
                if (role == null) return NotFound();
                var claims = await _roleManager.GetClaimsAsync(role);
                var result = new IdentityResult();
                //remove all role claims
                foreach (var claim in claims)
                {
                    result= await _roleManager.RemoveClaimAsync(role, claim);
                    if (!result.Succeeded)
                    {
                        ModelState.AddModelError(string.Empty, string.Join(" - ", result.Errors.ToList()));
                        return View(model);
                    }
                }
                var selectedUserClaims = model.RoleClaims.Where(x => x.IsSelected).Select(x => new securityClaims.Claim(x.ClaimType, x.IsSelected.ToString())).ToList();
                foreach (var claim in selectedUserClaims)
                {
                    result=await _roleManager.AddClaimAsync(role, claim);
                    if (!result.Succeeded)
                    {
                        ModelState.AddModelError(string.Empty, string.Join(" - ", result.Errors.ToList()));
                        return View(model);
                    }
                }
                await trans.CommitAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                await trans.RollbackAsync();
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }
        }
    }
}
