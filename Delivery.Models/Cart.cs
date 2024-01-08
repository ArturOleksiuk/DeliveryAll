using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryAll.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public int FoodItemId { get; set; }
        [ValidateNever]
        public FoodItem FoodItem { get; set; }
        public string ApplicationUserId { get; set; }
        [ForeignKey("AplicationUserId")]
        [ValidateNever]
        public ApplicationUser ApplicationUser { get; set; }
        [Range(1,100,ErrorMessage = "Pleace enter a value between 1 and 100")]
        public int Count { get; set; }
        [NotMapped]
        public double Price {  get; set; }
    }
}
