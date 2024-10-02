using System;
using SecureWebApp.Models;

namespace SecureWebApp.Data;

public interface Istudents
{
    IEnumerable<Student> GetStudents();
    Student GetStudent(string nim);
    Student AddStudent(Student student);
    Student UpdateStudent(Student student);
    void DeleteStudent(String nim);
}
