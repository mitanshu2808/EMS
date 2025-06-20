﻿using System.ComponentModel.DataAnnotations;

namespace Data.FormModels
{
    public class RegisterViewModel
    {
        public string? Id { get; set; }

        [Required]
        public string Firstname { get; set; }

        [Required]
        public string Lastname { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public DateTime DateofJoin { get; set; }

        [Required]
        public DateTime DateofBirth { get; set; }

        [Required]
        public string AadharNumber { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Password and confirmation password not match.")]
        public string ConfirmPassword { get; set; }
    }
}
