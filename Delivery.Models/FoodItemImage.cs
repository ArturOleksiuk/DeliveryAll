using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryAll.Models
{
    public class FoodItemImage
    {
        public int Id { get; set; }
        [Required]
        public string ImageUrl { get; set; }
        public int FoodItemId { get; set; }
        [ForeignKey("FoodItemId")]
        public FoodItem FoodItem { get; set; }
    }
}
