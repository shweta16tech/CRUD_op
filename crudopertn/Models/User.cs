using Microsoft.Extensions.Primitives;
using System;
using System.ComponentModel.DataAnnotations;


namespace crudopertn.Models
{
    public class User
    {
        public int ID { get; set; }
        
        [Required]
        public string Username { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }


    }
}
