using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BulkyBook.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string ISBN { get; set; }
        [Required]
        public string Author { get; set; }

        [Required]
        [Range(1, 10000)]
        public double ListPrice { get; set; }

        [Required]
        [Range(1, 10000)]
        public double Price { get; set; }

        [Required]
        [Range(1, 10000)]
        public double Price50 { get; set; }

        [Required]
        [Range(1, 10000)]
        public double Price100 { get; set; }



        public string ImageUrl { get; set; }

        public int CategoryId { get; set; }

        [ForeignKey(nameof(CategoryId))] //not required FK Category
        public Category Category { get; set; }

        public int CoverTypeId { get; set; }     //Automatic FK CoverType
        public CoverType CoverType { get; set; }    //Automatic FK CoverType

    }
}
