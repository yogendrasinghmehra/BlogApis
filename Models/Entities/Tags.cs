using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogApis.Models.Entities
{
    public class Tags
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [StringLength(20)]
        [Required]
        public string Name { get; set; }
        public bool? IsActive { get; set; }   
        public DateTime? CreatedDate { get; set; }
                       
    }
}