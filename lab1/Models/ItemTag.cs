﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace lab1.Models
{
    public class ItemTag
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        
    }
}
