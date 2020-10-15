using CinelAirMiles.Web.Backoffice.Data.Entities;
using CinelAirMiles.Web.Backoffice.Helpers.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinelAirMiles.Web.Backoffice.Helpers.Classes
{
    public class UserHelper : IUserHelper
    {
        readonly UserManager<User> _userManager;

        readonly RoleManager<IdentityRole> _roleManager;

        readonly SignInManager<User> _signInManager;


        public UserHelper(
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }


    }
}
