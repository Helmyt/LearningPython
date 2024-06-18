using LearningPython.BLL.Services;
using LearningPython.DAL.Interfaces;
using LearningPython.DAL.Models;
using LearningPython.Web.ViewModels.LessonSectionViewVM;
using Microsoft.AspNetCore.Mvc;


namespace LearningPython.Web.Controllers
{
    public class LessonSectionViewController : Controller
    {
        private readonly IPhotoService _photoService;
        private readonly ILessonRepository _lessonRepository;
        private readonly ISectionRepository _sectionRepository;

        public LessonSectionViewController(ILessonRepository lessonRepository, ISectionRepository sectionRepository, IPhotoService photoService)
        {
            _lessonRepository = lessonRepository;
            _sectionRepository = sectionRepository;
            _photoService = photoService;
        }
        public async Task<IActionResult> Index(int id)
        {
            var sections = await _sectionRepository.GetAllSectionsAsync(id);
            var sortedSections = sections.OrderBy(s => s.Title); // Сортування за назвою
            var lesson = await _lessonRepository.GetByIdAsync(id);
            var sectionViewModel = new IndexViewModel
            {
                LessonId = lesson.Id,
                Title = lesson.Title,
                Sections = sortedSections
            };
            return sections == null ? NotFound() : View(sectionViewModel);
        }
        public IActionResult Create(int id)
        {
            var createSectionViewModel = new CreateViewModel { LessonId = id };
            return View(createSectionViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateViewModel sectionVM)
        {
            if (ModelState.IsValid)
            {
                var section = new Section
                {
                    LessonId = sectionVM.LessonId,
                    Lesson = await _lessonRepository.GetByIdAsync(sectionVM.LessonId)
                };

                if (sectionVM.Title != null)
                {
                    section.Title = sectionVM.Title;

                }
                if (sectionVM.Text != null)
                {
                    section.Text = sectionVM.Text;

                }
                if (sectionVM.Code != null)
                {
                    section.Code = sectionVM.Code;

                }
                if (sectionVM.Image != null)
                {
                    section.Image = (await _photoService.AddRawAsync(sectionVM.Image)).Url.ToString();

                }
                if (sectionVM.JsFile != null)
                {
                    section.JsCanvasWidth = sectionVM.JsWidth;
                    section.JsCanvasHeight = sectionVM.JsHeight;
                    section.JsFileUrl = (await _photoService.AddJsAsync(sectionVM.JsFile)).Url.ToString();
                }

                _sectionRepository.Add(section);
                return RedirectToAction("Index", new { id = sectionVM.LessonId });
            }
            else
            {
                ModelState.AddModelError("", "Photo upload failed");
            }

            return View(sectionVM);
        }
        public async Task<IActionResult> Edit(int id)
        {
            var section = await _sectionRepository.GetByIdAsync(id);
            if (section == null) return View("Error");
            var sectionVM = new EditViewModel
            {
                Title = section.Title,
                Text = section.Text,
                Code = section.Code,
                ImageURL = section.Image,
                JsFileURL = section.JsFileUrl,
                JsWidth = section.JsCanvasWidth,
                JsHeight = section.JsCanvasHeight,
                LessonId = section.LessonId
            };
            return View(sectionVM);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditViewModel sectionVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit club");
                return View("Edit", sectionVM);
            }

            var userSection = await _sectionRepository.GetByIdAsyncNoTracking(id);

            if (userSection != null)
            {
                string photoResult = null;
                string jsFileResult = null;
                if (sectionVM.Image != null)
                {
                    if (userSection.Image != null)
                    {
                        try
                        {
                            await _photoService.DeletePhotoAsync(userSection.Image);
                        }
                        catch (Exception ex)
                        {
                            ModelState.AddModelError("", "Could not delete photo");
                            return View(sectionVM);
                        }
                    }

                    photoResult = (await _photoService.AddRawAsync(sectionVM.Image)).Url.ToString();
                }
                if (sectionVM.JsFile != null)
                {
                    if (userSection.JsFileUrl != null)
                    {
                        try
                        {
                            await _photoService.DeletePhotoAsync(userSection.JsFileUrl);
                        }
                        catch (Exception ex)
                        {
                            ModelState.AddModelError("", "Could not delete JS file");
                            return View(sectionVM);
                        }
                    }

                    jsFileResult = (await _photoService.AddJsAsync(sectionVM.JsFile)).Url.ToString();
                }

                var section = new Section
                {
                    Id = id,
                    Title = sectionVM.Title,
                    Text = sectionVM.Text,
                    Code = sectionVM.Code,
                    LessonId = sectionVM.LessonId,
                    JsCanvasWidth = sectionVM.JsWidth,
                    JsCanvasHeight = sectionVM.JsHeight
                };

                if (photoResult != null)
                {
                    section.Image = photoResult;
                }
                else
                {
                    section.Image = userSection.Image;
                }
                if (jsFileResult != null)
                {
                    section.JsFileUrl = jsFileResult;
                }
                else 
                {
                    section.JsFileUrl = userSection.JsFileUrl;
                }
                _sectionRepository.Update(section);

                return RedirectToAction("Index", new { id = sectionVM.LessonId });
            }
            else
            {
                return View(sectionVM);
            }
        }
        public async Task<IActionResult> Delete(int id)
        {
            var sectionDetails = await _sectionRepository.GetByIdAsync(id);
            if (sectionDetails == null) return View("Error");
            return View(sectionDetails);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sectionDetails = await _sectionRepository.GetByIdAsync(id);
            if (sectionDetails == null) return View("Error");

            _sectionRepository.Delete(sectionDetails);
            return RedirectToAction("Index", new { id = sectionDetails.LessonId });
        }
    }
}
