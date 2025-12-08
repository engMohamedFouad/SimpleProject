using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleProject.Models.Identity;
using SimpleProject.UnitOfWorks;
using SimpleProject.ViewModels.Identity.Users;
using securityClaims = System.Security.Claims;


namespace SimpleProject.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUnitOfWork _unitOfWork;
        public UserController(UserManager<User> userManager,
                              IMapper mapper,
                              RoleManager<IdentityRole> roleManager,
                              IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _mapper = mapper;
            _roleManager = roleManager;
            _unitOfWork = unitOfWork;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            var usersList = _mapper.Map<List<GetUsersViewModel>>(users);
            return View(usersList);
        }
        [HttpGet]
        public async Task<IActionResult> Update(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();
            var response = _mapper.Map<UpdateUserViewModel>(user);
            return View(response);
        }
        [HttpPost]
        public async Task<IActionResult> Update(UpdateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.Id);
                if (user == null) return NotFound();
                var mapper = _mapper.Map(model, user);
                var result = await _userManager.UpdateAsync(mapper);
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
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();
            var response = _mapper.Map<GetUserByIdViewModel>(user);
            return View(response);
        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirm(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();
            var result = await _userManager.DeleteAsync(user);
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
        public async Task<IActionResult> ManageRolesInUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            var roles = await _roleManager.Roles.ToListAsync();
            var userRoles = _mapper.Map<List<ManageRolesInUserViewModel>>(roles);
            foreach (var role in userRoles)
            {
                if (await _userManager.IsInRoleAsync(user, role.RoleName))
                    role.IsSelected = true;
            }
            ViewBag.UserId=userId;
            return View(userRoles);
        }

        [HttpPost]
        public async Task<IActionResult> ManageRolesInUser(List<ManageRolesInUserViewModel> manageRoles, string userId)
        {
            var trans = await _unitOfWork.BeginTransactionAsync();
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null) return NotFound();
                foreach (var role in manageRoles)
                {
                    if (role.IsSelected&&!(await _userManager.IsInRoleAsync(user, role.RoleName)))
                    {
                        await _userManager.AddToRoleAsync(user, role.RoleName);
                    }
                    else if (!role.IsSelected&&await _userManager.IsInRoleAsync(user, role.RoleName))
                    {
                        await _userManager.RemoveFromRoleAsync(user, role.RoleName);
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
                return View(manageRoles);
            }
        }
        [HttpGet]
        public async Task<IActionResult> ManageUserClaims(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();
            var claims = await _unitOfWork.Repository<Claim>().GetListAsync();
            var userClaimsList = claims.Where(x => x.IsUserClaim==true);
            //user Claims
            var userClaims = await _userManager.GetClaimsAsync(user);
            var model = new ManageUserClaimViewModel();
            model.UserId=userId;
            foreach (var claim in userClaimsList)
            {
                var userClaim = new UserClaim();
                userClaim.ClaimType=claim.NameEn;
                if (userClaims.Any(x => x.Type==claim.NameEn))
                    userClaim.IsSelected = true;
                else
                    userClaim.IsSelected = false;
                model.Claims.Add(userClaim);
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ManageUserClaims(ManageUserClaimViewModel model)
        {
            var trans = await _unitOfWork.BeginTransactionAsync();
            try
            {
                var user = await _userManager.FindByIdAsync(model.UserId);
                if (user == null) return NotFound();
                //user Claims
                var userClaims = await _userManager.GetClaimsAsync(user);
                var result = await _userManager.RemoveClaimsAsync(user, userClaims);
                if (!result.Succeeded)
                {
                    ModelState.AddModelError(string.Empty, "Error While Remove Claims");
                    return View(model);
                }
                var selectedUserClaims = model.Claims.Where(x => x.IsSelected).Select(x => new securityClaims.Claim(x.ClaimType, x.IsSelected.ToString())).ToList();
                var addClaimsResult = await _userManager.AddClaimsAsync(user, selectedUserClaims);
                if (!addClaimsResult.Succeeded)
                {
                    ModelState.AddModelError(string.Empty, "Error While Add User Claims");
                    return View(model);
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
