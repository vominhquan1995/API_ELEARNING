using Api_ELearning.ActionModels;
using Api_ELearning.Services;
using Api_ELearning.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api_ELearning.Data
{
    public static class InitDb
    {
        public static void Init(ELearningDbContext _context)
        {
            _context.Database.EnsureCreated();
            if (!_context.Roles.Any())
            {
                _context.Roles.Add(new Role
                {
                    RoleName = "User"
                });
                _context.Roles.Add(new Role
                {
                    RoleName = "Administrator"
                });
                _context.Roles.Add(new Role
                {
                    RoleName = "Teacher"
                });
                _context.SaveChanges();
            }                                      
        }
    }
}
