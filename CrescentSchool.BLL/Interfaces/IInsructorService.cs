using CrescentSchool.BLL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrescentSchool.BLL.Interfaces;

public interface IInsructorService
{
    Task<List<StudentDto>> GetInstructorStudents(Guid insrtructorId);
    Task<InstructorDto> GetInstructorByIdAsync(Guid instructorId);
}
