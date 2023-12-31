﻿using System.ComponentModel.DataAnnotations;

namespace ProiectAE2.Models
{
    public class LoginVM
    {
        public LoginVM()
        {
            UserName = string.Empty;
            Password = string.Empty;
        }

        [Required(AllowEmptyStrings = false)]
        public string UserName { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string Password { get; set; }
    }
}
