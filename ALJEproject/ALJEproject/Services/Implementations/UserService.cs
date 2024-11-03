using ALJEproject.Models;
using ALJEproject.Services.Interfaces;
using ALJEproject.Data; // Ensure this namespace is correct
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ALJEproject.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly ALJEprojectDbContext _context;

        public UserService(ALJEprojectDbContext context)
        {
            _context = context;
        }

        public IEnumerable<UserRoleView> GetPaginatedUsers(int page, int pageSize)
        {
            return _context.UserRoles
                .OrderBy(u => u.UserId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(u => new UserRoleView
                {
                    UserId = u.UserId,
                    UserName = u.UserName,
                    FullName = u.FullName,
                    RoleName = u.RoleName,
                    CompanyName = u.CompanyName,
                    EmailAddress = u.EmailAddress,
                    Phone = u.Phone,
                    CreatedBy = u.CreatedBy,
                    CreatedDate = u.CreatedDate,
                    UpdatedBy = u.UpdatedBy,
                    UpdatedDate = u.UpdatedDate
                })
                .ToList();
        }

        public List<UserRoleView> SearchUsers(string search, int page, int pageSize)
        {
            // Start building the query
            var query = _context.UserRoles.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var searchLower = search.ToLower(); // Convert the search term to lowercase

                query = query.Where(u =>  u.UserId.ToString().ToLower().Contains(searchLower) ||
                                          u.UserName.ToLower().Contains(searchLower) ||                                          
                                          u.RoleName.ToLower().Contains(searchLower) ||
                                          u.CompanyName.ToLower().Contains(searchLower));
            }

            var totalUsersCount = query.Count();

            // Perform paging and select the desired fields
            var users = query.OrderBy(u => u.UserId)
                             .Skip((page - 1) * pageSize)
                             .Take(pageSize)
                             .Select(u => new UserRoleView
                             {
                                 UserId = u.UserId,
                                 UserName = u.UserName,
                                 FullName = u.FullName,
                                 RoleName = u.RoleName,
                                 CompanyName = u.CompanyName,
                                 EmailAddress = u.EmailAddress,
                                 Phone = u.Phone,
                                 CreatedBy = u.CreatedBy,
                                 CreatedDate = u.CreatedDate,
                                 UpdatedBy = u.UpdatedBy,
                                 UpdatedDate = u.UpdatedDate
                             })
                             .ToList(); // Execute the query

            return users; // Return the list of users
        }

        public int GetTotalUsersCount(string search = null) // New method for total count based on search
        {
            var query = _context.UserRoles.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var searchLower = search.ToLower(); // Convert the search term to lowercase

                query = query.Where(u => u.UserName.ToLower().Contains(searchLower) ||
                                          u.FullName.ToLower().Contains(searchLower) ||
                                          u.EmailAddress.ToLower().Contains(searchLower) ||
                                          u.Phone.ToLower().Contains(searchLower) ||
                                          u.CompanyName.ToLower().Contains(searchLower));
            }

            return query.Count(); // Returns the total count based on the current query
        }

        public int GetTotalUsersCount()
        {
            return _context.Users.Count();
        }

        public void AddUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public void UpdateUser(User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
        }

        public User GetUserById(int id)
        {
            return _context.Users.Find(id);
        }

        public bool DeleteUser(int id)
        {
            var user = _context.Users.Find(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public IEnumerable<Role> GetRoles()
        {
            return _context.Roles.ToList(); // Assuming you have a Roles DbSet
        }

        public async Task<IEnumerable<UserRoleView>> SearchUsersAsync(string search, int page, int pageSize)
        {
            return await _context.UserRoles
                .Where(u => u.UserName.Contains(search) || u.FullName.Contains(search))
                .OrderBy(u => u.UserId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(u => new UserRoleView
                {
                    UserId = u.UserId,
                    UserName = u.UserName,
                    FullName = u.FullName,
                    RoleName = u.RoleName, // Asumsi Anda memiliki relasi antara pengguna dan peran
                CompanyName = u.CompanyName, // Asumsi Anda memiliki relasi antara pengguna dan perusahaan
                EmailAddress = u.EmailAddress,
                    Phone = u.Phone,
                    CreatedBy = u.CreatedBy,
                    CreatedDate = u.CreatedDate,
                    UpdatedBy = u.UpdatedBy,
                    UpdatedDate = u.UpdatedDate
                })
                .ToListAsync();
        }

        public async Task<int> GetTotalUsersCountAsync(string search)
        {
            return await _context.UserRoles
                .Where(u => u.UserName.Contains(search) || u.FullName.Contains(search))
                .CountAsync();
        }

        public async Task<IEnumerable<UserRoleView>> GetPaginatedUsersAsync(int page, int pageSize)
        {
            return await _context.UserRoles
                .OrderBy(u => u.UserId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(u => new UserRoleView
                {
                    UserId = u.UserId,
                    UserName = u.UserName,
                    FullName = u.FullName,
                    RoleName = u.RoleName, // Asumsi Anda memiliki relasi antara pengguna dan peran
                CompanyName = u.CompanyName, // Asumsi Anda memiliki relasi antara pengguna dan perusahaan
                EmailAddress = u.EmailAddress,
                    Phone = u.Phone,
                    CreatedBy = u.CreatedBy,
                    CreatedDate = u.CreatedDate,
                    UpdatedBy = u.UpdatedBy,
                    UpdatedDate = u.UpdatedDate
                })
                .ToListAsync();
        }

        public async Task<int> GetTotalUsersCountAsync()
        {
            return await _context.UserRoles.CountAsync();
        }

        //role
        public List<RoleView> SearchRoles(string search, int page, int pageSize)
        {
            // Start building the query
            var query = _context.Roles.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var searchLower = search.ToLower(); // Convert the search term to lowercase

                query = query.Where(u => u.RoleID.ToString().ToLower().Contains(searchLower) ||
                                          u.RoleName.ToLower().Contains(searchLower) ||
                                          u.CreatedBy.ToLower().Contains(searchLower));
            }

            var totalRolesCount = query.Count();

            // Perform paging and select the desired fields
            var roles = query.OrderBy(u => u.RoleID)
                             .Skip((page - 1) * pageSize)
                             .Take(pageSize)
                             .Select(u => new RoleView
                             {
                                 RoleID = u.RoleID,
                                 RoleName = u.RoleName,                            
                                 CreatedBy = u.CreatedBy,
                                 CreatedDate = u.CreatedDate,
                                 UpdatedBy = u.UpdatedBy,
                                 UpdatedDate = u.UpdatedDate
                             })
                             .ToList(); // Execute the query

            return roles; // Return the list of users
        }

        public int GetTotalRolesCount(string search = null) // New method for total count based on search
        {
            var query = _context.Roles.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var searchLower = search.ToLower(); // Convert the search term to lowercase

                query = query.Where(u => u.RoleID.ToString().ToLower().Contains(searchLower) ||
                             u.RoleName.ToLower().Contains(searchLower) ||
                             u.CreatedBy.ToLower().Contains(searchLower));
            }

            return query.Count(); // Returns the total count based on the current query
        }

        public IEnumerable<RoleView> GetPaginatedRoles(int page, int pageSize)
        {
            return _context.Roles
                .OrderBy(u => u.RoleID)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(u => new RoleView
                {
                    RoleID = u.RoleID,
                    RoleName = u.RoleName,                    
                    CreatedBy = u.CreatedBy,
                    CreatedDate = u.CreatedDate,
                    UpdatedBy = u.UpdatedBy,
                    UpdatedDate = u.UpdatedDate
                })
                .ToList();
        }

        public int GetTotalRolesCount()
        {
            return _context.Roles.Count();
        }


    }
}
