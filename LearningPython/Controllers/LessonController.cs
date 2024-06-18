using LearningPython.DAL.Context;
using LearningPython.Web.ViewModels.LessonVM;
using LearningPython.DAL.Models;
using LearningPython.BLL.Data.Enums;
using LearningPython.DAL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using LearningPython.BLL.Services;
using System.Net;

namespace LearningPython.Web.Controllers
{
    public class LessonController : Controller
    {
        private readonly IPhotoService _photoService;
        private readonly ILessonRepository _lessonRepository;

        public LessonController(ILessonRepository lessonRepository, IPhotoService photoService)
        {
            _lessonRepository = lessonRepository;
            _photoService = photoService;
        }

        public async Task<IActionResult> Index(int tag = -1, int page = 1, int pageSize = 6, string searchTerm = null)
        {
            if (page < 1 || pageSize < 1)
            {
                return NotFound();
            }

            IEnumerable<Lesson> lessons;
            int count;

            if (!string.IsNullOrEmpty(searchTerm))
            {
                lessons = await _lessonRepository.SearchLessonsAsync(searchTerm, (page - 1) * pageSize, pageSize);
                count = await _lessonRepository.GetCountBySearchAsync(searchTerm);
            }
            else
            {
                lessons = tag == -1 ?
                    await _lessonRepository.GetSliceAsync((page - 1) * pageSize, pageSize) :
                    await _lessonRepository.GetLessonsByTagAndSliceAsync((LessonTag)tag, (page - 1) * pageSize, pageSize);

                count = tag == -1 ?
                    await _lessonRepository.GetCountAsync() :
                    await _lessonRepository.GetCountByTagAsync((LessonTag)tag);
            }

            var lessonViewModel = new IndexViewModel
            {
                Lessons = lessons,
                Page = page,
                PageSize = pageSize,
                TotalLessons = count,
                TotalPages = (int)Math.Ceiling(count / (double)pageSize),
                Tag = tag,
                SearchTerm = searchTerm
            };

            return View(lessonViewModel);
        }

        public IActionResult Create()
        {
            var curUserId = HttpContext.User.GetUserId();
            var createClubViewModel = new CreateViewModel { UserId = curUserId };
            return View(createClubViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateViewModel lessonVM)
        {
            if (ModelState.IsValid)
            {
                var result = await _photoService.AddPhotoAsync(lessonVM.LessonImage);

                var lesson = new Lesson
                {
                    Title = lessonVM.Title,
                    Description = lessonVM.Description,
                    LessonTag = lessonVM.LessonTag,
                    Image = result.Url.ToString(),
                    UserId = lessonVM.UserId,
                };
                _lessonRepository.Add(lesson);
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Photo upload failed");
            }

            return View(lessonVM);
        }
        public async Task<IActionResult> Edit(int id)
        {
            var lesson = await _lessonRepository.GetByIdAsync(id);
            if (lesson == null) return View("Error");
            var clubVM = new EditViewModel
            {
                Title = lesson.Title,
                Description = lesson.Description,
                URL = lesson.Image,
                LessonTag = lesson.LessonTag
            };
            return View(clubVM);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditViewModel lessonVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit club");
                return View("Edit", lessonVM);
            }

            var userLesson = await _lessonRepository.GetByIdAsyncNoTracking(id);

            if (userLesson != null)
            {
                string photoResult = null;

                if (lessonVM.LessonImage != null)
                {
                    if (userLesson.Image != null)
                    {
                        try
                        {
                            await _photoService.DeletePhotoAsync(userLesson.Image);
                        }
                        catch (Exception ex)
                        {
                            ModelState.AddModelError("", "Could not delete photo");
                            return View(lessonVM);
                        }
                    }

                    photoResult = (await _photoService.AddPhotoAsync(lessonVM.LessonImage)).Url.ToString();
                }
                
                var lesson = new Lesson
                {
                    Id = id,
                    Title = lessonVM.Title,
                    Description = lessonVM.Description,
                    LessonTag = lessonVM.LessonTag
                };

                if (photoResult != null)
                {
                    lesson.Image = photoResult;
                }
                else 
                {
                    lesson.Image = userLesson.Image;
                }

                _lessonRepository.Update(lesson);

                return RedirectToAction("Index");
            }
            else
            {
                return View(lessonVM);
            }
        }
        public async Task<IActionResult> Delete(int id)
        {
            var lessonDetails = await _lessonRepository.GetByIdAsync(id);
            if (lessonDetails == null) return View("Error");
            return View(lessonDetails);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var lessonDetails = await _lessonRepository.GetByIdAsync(id);
            if (lessonDetails == null) return View("Error");

            _lessonRepository.Delete(lessonDetails);
            return RedirectToAction("Index");
        }
    }
}
