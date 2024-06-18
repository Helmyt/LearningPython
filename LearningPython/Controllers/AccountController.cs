using LearningPython.DAL.Models;
using LearningPython.BLL.Data.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using LearningPython.DAL.Context;
using LearningPython.Web.ViewModels.AccountVM;
using LearningPython.BLL.Services;
using LearningPython.DAL.Interfaces;
using LearningPython.DAL.Repository;
using System.Security.Claims;

namespace LearningPython.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly AppDbContext _context;
        private readonly IPhotoService _photoService;
        private readonly IUserRepository _userRepository;
        private readonly ITestUserAnswerRepository _testUserAnswerRepository;
        private readonly ILessonRepository _lessonRepository;
        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, ILessonRepository lessonRepository, IUserRepository userRepository, ITestUserAnswerRepository testUserAnswerRepository, AppDbContext context, IPhotoService photoService)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
            _photoService = photoService;
            _testUserAnswerRepository = testUserAnswerRepository;
            _userRepository = userRepository;
            _lessonRepository = lessonRepository;
        }

        public IActionResult Login()
        {
            var response = new LoginViewModel();
            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid) return View(loginViewModel);

            var user = await _userManager.FindByEmailAsync(loginViewModel.EmailAddress);

            if (user != null)
            {
                //User is found, check password
                var passwordCheck = await _userManager.CheckPasswordAsync(user, loginViewModel.Password);
                if (passwordCheck)
                {
                    //Password correct, sign in
                    var result = await _signInManager.PasswordSignInAsync(user, loginViewModel.Password, false, false);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                //Password is incorrect
                TempData["Error"] = "Wrong credentials. Please try again";
                return View(loginViewModel);
            }
            //User not found
            TempData["Error"] = "Wrong credentials. Please try again";
            return View(loginViewModel);
        }
        public IActionResult Register()
        {
            var response = new RegisterViewModel();
            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(registerViewModel);
            }

            var user = await _userManager.FindByEmailAsync(registerViewModel.EmailAddress);
            if (user != null)
            {
                TempData["Error"] = "This email address is already in use";
                return View(registerViewModel);
            }

            var newUser = new User()
            {
                Email = registerViewModel.EmailAddress,
                UserName = registerViewModel.EmailAddress
            };
            var newUserResponse = await _userManager.CreateAsync(newUser, registerViewModel.Password);

            if (newUserResponse.Succeeded)
            {
                await _userManager.AddToRoleAsync(newUser, UserRoles.User);
            }
            else
            {
                TempData["Error"] = "Error! Invalid form of password (password should have minimum 8 chracters, upper and lower case symbol, number, special character)";
                return View(registerViewModel);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> Profile()
        {
            var user = await _userRepository.GetUserById(User.GetUserId());
            if (user == null)
            {
                return NotFound();
            }

            var testUserAnswers = await _testUserAnswerRepository.GetTestUserAnswers(user.Id);

            var testHistorySummaries = testUserAnswers
                .GroupBy(a => a.TestQuestion.LessonId)
                .Select(g => new TestHistorySummaryViewModel
                {
                    LessonId = g.Key,
                    TotalQuestions = g.Count(),
                    CorrectAnswers = g.Count(a => a.IfAnswerWasRight.HasValue && a.IfAnswerWasRight.Value),
                    Percentage = (double)g.Count(a => a.IfAnswerWasRight.HasValue && a.IfAnswerWasRight.Value) / g.Count() * 100
                }).ToList();

            // Fetch lesson titles
            foreach (var summary in testHistorySummaries)
            {
                var lesson = await _lessonRepository.GetByIdAsync(summary.LessonId);
                summary.LessonTitle = lesson?.Title ?? "Unknown";
            }

            var model = new ProfileViewModel
            {
                UserName = user.UserName,
                EmailAddress = user.Email,
                Image = user.Image,
                TestHistorySummaries = testHistorySummaries
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Profile(ProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            user.Email = model.EmailAddress;
            user.UserName = model.UserName;

            if (model.ImageFile != null)
            {
                if (!string.IsNullOrEmpty(user.Image))
                {
                    await _photoService.DeletePhotoAsync(user.Image);
                }

                var photoResult = await _photoService.AddPhotoAsync(model.ImageFile);
                user.Image = photoResult.Url.ToString();
            }

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Failed to update profile");
                return View(model);
            }

            // Re-fetch the test history summaries
            var testUserAnswers = await _testUserAnswerRepository.GetTestUserAnswers(user.Id);
            var testHistorySummaries = testUserAnswers
                .GroupBy(a => a.TestQuestion.LessonId)
                .Select(g => new TestHistorySummaryViewModel
                {
                    LessonId = g.Key,
                    TotalQuestions = g.Count(),
                    CorrectAnswers = g.Count(a => a.IfAnswerWasRight.HasValue && a.IfAnswerWasRight.Value),
                    Percentage = (double)g.Count(a => a.IfAnswerWasRight.HasValue && a.IfAnswerWasRight.Value) / g.Count() * 100
                }).ToList();

            // Fetch lesson titles
            foreach (var summary in testHistorySummaries)
            {
                var lesson = await _lessonRepository.GetByIdAsync(summary.LessonId);
                summary.LessonTitle = lesson?.Title ?? "Unknown";
            }

            model.TestHistorySummaries = testHistorySummaries;

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteTestHistory(int lessonId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var testUserAnswers = await _testUserAnswerRepository.GetTestUserAnswers(userId);
            var answersToDelete = testUserAnswers.Where(a => a.TestQuestion.LessonId == lessonId).ToList();

            if (answersToDelete.Any())
            {
                foreach (var answer in answersToDelete)
                {
                    _testUserAnswerRepository.Delete(answer);
                }
            }

            return RedirectToAction("Profile");
        }
    }
}
