﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace lab1.Models
{
    [Table("Products")]
    public class ItemProducts
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public double Discount { get; set; }
        public double Price { get; set; }
        public int Hot { get; set; }
        public string Photo { get; set; }

    }
}
