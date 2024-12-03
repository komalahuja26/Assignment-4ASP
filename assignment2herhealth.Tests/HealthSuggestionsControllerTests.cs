using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using assignment2herhealth.Controllers;
using assignment2herhealth.Data;
using assignment2herhealth.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace assignment2herhealth.Tests
{
    [TestClass]
    public class HealthSuggestionsControllerTests
    {
        private ApplicationDbContext _context;
        private HealthSuggestionsController _controller;

        [TestInitialize]
        public void TestInitialize()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _context = new ApplicationDbContext(options);

            var periodEntry = new PeriodEntry { PeriodEntryId = 1, Name = "User1", Age = 25, PeriodStartDate = DateTime.Now };
            _context.PeriodEntry.Add(periodEntry);

            _context.HealthSuggestion.Add(new HealthSuggestion
            {
                HealthSuggestionId = 1,
                PeriodEntryId = 1,
                WaterIntake = 8,
                HealthyFoods = "Fruits, Vegetables"
            });

            _context.HealthSuggestion.Add(new HealthSuggestion
            {
                HealthSuggestionId = 2,
                PeriodEntryId = 1,
                WaterIntake = 6,
                HealthyFoods = "Nuts, Seeds"
            });

            _context.HealthSuggestion.Add(new HealthSuggestion
            {
                HealthSuggestionId = 3,
                PeriodEntryId = 1,
                WaterIntake = 10,
                HealthyFoods = "Lean Protein"
            });

            _context.SaveChanges();
            _controller = new HealthSuggestionsController(_context);
        }

        [TestMethod]
        public async Task Details_ValidId_ReturnsCorrectHealthSuggestion()
        {
            var result = await _controller.Details(1) as ViewResult;
            var model = result?.Model as HealthSuggestion;

            Assert.IsNotNull(result);
            Assert.IsNotNull(model);
            Assert.AreEqual(1, model.HealthSuggestionId);
        }

        [TestMethod]
        public async Task Details_InvalidId_ReturnsNotFound()
        {
            var result = await _controller.Details(999) as NotFoundResult;

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task Details_NullId_ReturnsNotFound()
        {
            var result = await _controller.Details(null) as NotFoundResult;

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task Create_ValidHealthSuggestion_RedirectsToIndex()
        {
            var newHealthSuggestion = new HealthSuggestion
            {
                HealthSuggestionId = 4,
                PeriodEntryId = 1,
                WaterIntake = 12,
                HealthyFoods = "Whole Grains"
            };

            var result = await _controller.Create(newHealthSuggestion) as RedirectToActionResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
        }

        [TestMethod]
        public async Task Create_InvalidHealthSuggestion_ReturnsView()
        {
            _controller.ModelState.AddModelError("HealthyFoods", "Required");

            var newHealthSuggestion = new HealthSuggestion
            {
                HealthSuggestionId = 5,
                PeriodEntryId = 1,
                WaterIntake = 0 
            };

            var result = await _controller.Create(newHealthSuggestion) as ViewResult;

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }
    }
}
