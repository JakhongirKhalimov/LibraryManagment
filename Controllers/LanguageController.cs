using LibraryManagementSystem.Services;
using LibraryManagementSystem.ViewModels;
using LibraryManagementSystem.ViewModels.LanguageViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class LanguageController : Controller
    {
        #region Fields
        private readonly ILanguageService _languageService;
        #endregion

        #region Constructor
        public LanguageController(ILanguageService languageService)
        {
            _languageService = languageService;
        }
        #endregion

        #region Methods
        [HttpGet]
        public async Task<IActionResult> LanguageList([FromQuery] FilterOptions filterOptions)
        {
            var languageListResponse = await _languageService.LanguageListAsync(filterOptions);

            ViewData["sortBy"] = filterOptions.SortBy;

            ViewData["searchQuery"] = filterOptions.SearchQuery;

            return View(languageListResponse.List);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateLanguageViewModel createLanguageViewModel)
        {
            var response = await _languageService.CreateAsync(createLanguageViewModel);

            if (response != null && response.IsValid)
            {
                return RedirectToAction("LanguageList", "Language");
            }

            if (response != null) ViewData["ValidationMessage"] = response.ValidationMessage;

            return View(createLanguageViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var languageById = await _languageService.LanguageByIdAsync(id);

            if (languageById != null && languageById.IsValid && languageById.Value != null)
            {
                UpdateLanguageViewModel updateLanguageViewModel = new UpdateLanguageViewModel()
                {
                    Id = languageById.Value.Id,
                    Name = languageById.Value.Name,
                    ShortName = languageById.Value.ShortName
                };

                return View(updateLanguageViewModel);
            }

            return RedirectToAction("languageList", "Language");
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateLanguageViewModel updateLanguageViewModel)
        {
            var response = await _languageService.UpdateAsync(updateLanguageViewModel);

            ViewData["ValidationMessage"] = response.ValidationMessage;

            if (response.IsValid)
            {
                return RedirectToAction("languageList", "Language");
            }

            return View(response);
        }
        #endregion
    }
}
