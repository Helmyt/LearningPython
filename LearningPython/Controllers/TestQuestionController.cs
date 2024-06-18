using LearningPython.BLL.Data.Enums;
using LearningPython.BLL.Services;
using LearningPython.DAL.Interfaces;
using LearningPython.DAL.Models;
using LearningPython.DAL.Repository;
using LearningPython.Web.ViewModels.TestQuestionVM;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static System.Collections.Specialized.BitVector32;

namespace LearningPython.Web.Controllers
{
    public class TestQuestionController : Controller
    {
        private readonly IPhotoService _photoService;
        private readonly ILessonRepository _lessonRepository;
        private readonly ITestQuestionRepository _testQuestionRepository;
        private readonly ITestUserAnswerRepository _testUserAnswerRepository;
        private readonly IUserRepository _userRepository;

        public TestQuestionController(ILessonRepository lessonRepository, ITestQuestionRepository testQuestionRepository, ITestUserAnswerRepository testUserAnswerRepository, IUserRepository userRepository, IPhotoService photoService)
        {
            _lessonRepository = lessonRepository;
            _testQuestionRepository = testQuestionRepository;
            _testUserAnswerRepository = testUserAnswerRepository;
            _photoService = photoService;
            _userRepository = userRepository;
        }
        public async Task<IActionResult> Index(int id)
        {
            var testQuestion = await _testQuestionRepository.GetAllTestQuestionsAsync(id);
            var lesson = await _lessonRepository.GetByIdAsync(id);
            var sectionViewModel = new IndexViewModel
            {
                LessonId = lesson.Id,
                Title = lesson.Title,
                TestQuestions = testQuestion
            };
            return testQuestion == null ? NotFound() : View(sectionViewModel);
        }
        public IActionResult Create(int id)
        {
            var createSectionViewModel = new CreateViewModel { LessonId = id };
            return View(createSectionViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateViewModel testQuestionVM)
        {
            if (!ModelState.IsValid)
            {
                return View(testQuestionVM);
            }

            var question = new TestQuestion
            {
                LessonId = testQuestionVM.LessonId,
                Text = testQuestionVM.Text,
                Answer = testQuestionVM.CorrectAnswer,
                Options = new List<string>(testQuestionVM.Options), // Copy options
                Category = testQuestionVM.Category,
            };

            if (testQuestionVM.Image != null)
            {
                string photoResult = (await _photoService.AddRawAsync(testQuestionVM.Image)).Url.ToString();
                question.Image = photoResult;
            }

            // Ensure the correct answer is part of the options
            if (testQuestionVM.Category == TestCategory.Quiz && !question.Options.Contains(testQuestionVM.CorrectAnswer))
            {
                var rng = new Random();
                question.Options.Insert(rng.Next(question.Options.Count + 1), testQuestionVM.CorrectAnswer);
            }

            _testQuestionRepository.Add(question);

            return RedirectToAction("Index", new { id = testQuestionVM.LessonId });
        }

        [HttpPost]
        public async Task<IActionResult> SubmitTest(IndexViewModel model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var testQuestions = await _testQuestionRepository.GetAllTestQuestionsAsync(model.LessonId);

            if (userId != null)
            {
                foreach (var question in testQuestions)
                {
                    // Find existing answers
                    var existingAnswers = await _testUserAnswerRepository.GetTestUserAnswers(userId);

                    // Delete existing answers for the same question
                    foreach (var existingAnswer in existingAnswers.Where(ea => ea.TestQuestionId == question.Id))
                    {
                        _testUserAnswerRepository.Delete(existingAnswer);
                    }
                }

                // Save changes after deletion
                await _testUserAnswerRepository.SaveChangesAsync();
            }
            var lessonId = model.LessonId;
            var questions = await _testQuestionRepository.GetAllTestQuestionsAsync(lessonId);
            var userAnswers = model.Answers;

            // Debugging logs
            Console.WriteLine($"Lesson ID: {lessonId}");
            Console.WriteLine($"Total Questions: {questions.Count}");
            Console.WriteLine($"User Answers Count: {userAnswers.Count}");

            var result = new SubmitAnswerVM
            {
                TotalQuestions = questions.Count,
                CorrectAnswers = 0,
                Answers = new List<AnswerResultVM>()
            };

            foreach (var question in questions)
            {
                var userAnswer = userAnswers.FirstOrDefault(a => a.TestQuestionId == question.Id);
                if (userAnswer != null)
                {
                    var isCorrect = question.Category == TestCategory.Quiz
                        ? question.Options.Contains(userAnswer.UserAnswer) && userAnswer.UserAnswer == question.Answer
                        : userAnswer.UserAnswer == question.Answer;

                    result.Answers.Add(new AnswerResultVM
                    {
                        QuestionText = question.Text,
                        UserAnswer = userAnswer.UserAnswer,
                        IsCorrect = isCorrect
                    });

                    if (isCorrect)
                    {
                        result.CorrectAnswers++;
                    }

                    // Debugging logs
                    Console.WriteLine($"Question: {question.Text}");
                    Console.WriteLine($"User Answer: {userAnswer.UserAnswer}");
                    Console.WriteLine($"Is Correct: {isCorrect}");
                }
            }

            result.SuccessPercentage = (double)result.CorrectAnswers / result.TotalQuestions * 100;

            // Debugging logs
            Console.WriteLine($"Correct Answers: {result.CorrectAnswers}");
            Console.WriteLine($"Success Percentage: {result.SuccessPercentage}");

            if (User.Identity.IsAuthenticated)
            {
                foreach (var userAnswer in userAnswers)
                {
                    var answerResult = result.Answers.FirstOrDefault(a => a.QuestionText == questions.FirstOrDefault(q => q.Id == userAnswer.TestQuestionId)?.Text);

                    if (answerResult != null)
                    {
                        var testUserAnswer = new TestUserAnswer
                        {
                            UserId = userId,
                            TestQuestionId = userAnswer.TestQuestionId,
                            UserAnswer = userAnswer.UserAnswer,
                            IfAnswerWasRight = answerResult.IsCorrect,
                            LessonId = lessonId
                        };

                        await _testUserAnswerRepository.AddAsync(testUserAnswer);
                    }
                }

                await _testUserAnswerRepository.SaveChangesAsync();
            }

            return View("SubmitAnswer", result);
        }
        public async Task<IActionResult> Delete(int id)
        {
            var testQuestion = await _testQuestionRepository.GetByIdAsync(id);
            if (testQuestion == null)
            {
                return NotFound();
            }
            return View(testQuestion);
        }


        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var testQuestionDetails = await _testQuestionRepository.GetByIdAsync(id);
            if (testQuestionDetails == null) return View("Error");

            _testQuestionRepository.Delete(testQuestionDetails);
            return RedirectToAction("Index", new { id = testQuestionDetails.LessonId });
        }
    }
}
