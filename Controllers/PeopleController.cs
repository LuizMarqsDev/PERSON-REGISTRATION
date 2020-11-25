using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RecordWebMVC.Data;
using RecordWebMVC.Models;
using RecordWebMVC.Models.ViewModels;
using RecordWebMVC.Services;
using RecordWebMVC.Services.Exceptions;

namespace RecordWebMVC.Controllers
{
    public class PeopleController : Controller
    {
        private readonly RecordWebMVCContext _context;
        private readonly PeopleService _people;

        public PeopleController(RecordWebMVCContext context, PeopleService people)
        {
            _context = context;
            _people = people;
        }

        public async Task<IActionResult> Index()
        {
            var list = await _people.FindAllAsync();
            return View(list);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Person person, string cpf)
        {

            await _people.InsertAsync(person);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not provided" });
            }

            var obj = await _people.FindByIdAsync(id.Value);
            if(obj == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not found" });
            }

            return View(obj);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _people.RemoveAsync(id);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not provided" });
            }

            var obj = await _people.FindByIdAsync(id.Value);
            if (obj == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not found" });
            }

            return View(obj);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not provided" });
            }

            var obj = await _people.FindByIdAsync(id.Value);
            if (obj == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not found" });
            }
            return View(obj);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Person person)
        {

            if (id != person.Id)
            {
                return RedirectToAction(nameof(Error), new { message = "Id mismatch" });

            }


            try
            {
                await _people.UpdateAsync(person);
                return RedirectToAction(nameof(Index));
            }
            catch (ApplicationException e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }
          
        }

        public IActionResult Error(string message)
        {
            var viewModel = new ErrorViewModel
            {
                Message = message,
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };

            return View(viewModel);
        }

 
    }
}
