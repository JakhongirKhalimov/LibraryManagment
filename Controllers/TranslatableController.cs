using LibraryManagementSystem.Services;
using LibraryManagementSystem.ViewModels;
using LibraryManagementSystem.ViewModels.TranslatableViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class TranslatableController : Controller
    {
        #region Fields
        private readonly ITranslatableService _translatableService;
        #endregion

        #region Constructor
        public TranslatableController(ITranslatableService translatableService)
        {
            _translatableService = translatableService;
        }
        #endregion

        #region Methods
        [HttpGet]
        public async Task<IActionResult> TranslatableList([FromQuery] FilterOptions filterOptions)
        {
            var translatableListResponse = await _translatableService.GetTranslatableListAsync(filterOptions);

            ViewData["sortBy"] = filterOptions.SortBy;

            ViewData["searchQuery"] = filterOptions.SearchQuery;

            return View(translatableListResponse.List);
        }

        [HttpGet]
        public IActionResult Create()
        {
            CreateTranslatableViewModel createTranslatableViewModel = new CreateTranslatableViewModel()
            {
                Entities = _translatableService.GetEntities(),
                Attributes = new List<string>()
            };

            return View(createTranslatableViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTranslatableViewModel createTranslatableViewModel)
        {
            if(createTranslatableViewModel.Entity != null && createTranslatableViewModel.Attribute != null)
            {
                var response = await _translatableService.CreateAsync(createTranslatableViewModel);

                if (response != null && response.IsValid)
                {
                    return RedirectToAction("TranslatableList", "Translatable");
                }

                if (response != null) ViewData["ValidationMessage"] = response.ValidationMessage;
            }

            if (createTranslatableViewModel == null) createTranslatableViewModel = new CreateTranslatableViewModel();

            if (createTranslatableViewModel.Entities == null) createTranslatableViewModel.Entities = _translatableService.GetEntities();

            if(createTranslatableViewModel.Entity != null)
            {
                createTranslatableViewModel.Attributes = _translatableService.GetAttributes(createTranslatableViewModel.Entity);
            }
            else
            {
                createTranslatableViewModel.Attributes = new List<string>();
            }

            return View(createTranslatableViewModel);
        }

        [HttpPost]
        public JsonResult GetAttributes(string Entity)
        {
            var attributes = _translatableService.GetAttributes(Entity);

            return Json(attributes);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var response = await _translatableService.DeleteAsync(id);

            ViewData["ValidationMessage"] = response.ValidationMessage;

            return RedirectToAction("TranslatableList", "Translatable");
        }
        #endregion
    }
}
