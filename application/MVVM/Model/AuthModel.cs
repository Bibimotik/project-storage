using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CSharpFunctionalExtensions;

namespace application.MVVM.Model
{
    class AuthModel
    {
        public static string? Email { get; set; }
        public static string? Password { get; set; }

        public AuthModel() {}
        //public AuthModel(string email, string password)
        //{
        //    Email = email;
        //    Password = password;
        //}

        //public static Result Login(string email, string password)
        //{

        //}
	}
}
