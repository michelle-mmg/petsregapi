using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PetsRegApi.Models
{
    public class  PetInfo : GeneralInfo
    {
        [Key]
        public int petId { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string petName { get; set; }

        [Column(TypeName = "nvarchar(10)")]
        public string sex { get; set; }

        public int age { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string species { get; set; }

        
    }

    public class GeneralInfo
    {
        public DateTime CreateDateTime { get; set; }
        public string CreatedBy { get; set; }
    }

    
}
