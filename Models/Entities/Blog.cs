using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogApis.Models.Entities
{

    public class Blog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id  { get; set; }
        [Required]
        [StringLength(100)]
        public string Title { get; set; }  
        [Required]        
        public string Content { get; set; }  
        [Required]
        [StringLength(100)]
        public string  urlslug { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        [DefaultValue(false)]
        public bool IsActive { get; set; } 
        [Required]
        public int TagsId { get; set; }        
        public Tags Tags { get; set; }


    }

}