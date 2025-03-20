using FS0924_S18_L4.Data;
using FS0924_S18_L4.Models;
using FS0924_S18_L4.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace FS0924_S18_L4.Services
{
    public class SchoolClassService
    {
        private readonly ApplicationDbContext _context;

        public SchoolClassService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ClassesListViewModel> GetAllSchoolClasses()
        {
            try
            {
                var classes = new ClassesListViewModel();
                classes.Classes = await _context.SchoolClasses.ToListAsync();

                return classes;
            }
            catch (Exception ex)
            {
                return new ClassesListViewModel() { Classes = new List<SchoolClass>() };
            }
        }
    }
}
