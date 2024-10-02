using System;
using Microsoft.AspNetCore.Mvc;
using SecureWebApp.Data;
using SecureWebApp.Models;


namespace SecureWebApp.Controllers;

public class StudentController : Controller
{

    private readonly Istudents _studentData;

    public StudentController(Istudents studentData)
    {
        _studentData = studentData;
    }
    public IActionResult Index()
    {
        var students = _studentData.GetStudents();
        return View(students);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(Student student)
    {
        try
        {
            _studentData.AddStudent(student);
            return RedirectToAction("Index");
        }
        catch (System.Exception ex)
        {
            ViewBag.Error = ex.Message;
        }

        return View(student);

    }

}
