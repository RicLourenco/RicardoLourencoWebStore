using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RicardoLourencoWebStore.Data.Entities
{
    public class OrderDetail : IEntity
    {
        public int Id { get; set; }


        [Required]
        public Product Product { get; set; }


        [DisplayFormat(DataFormatString = "{0:C2}")]
        public float Price { get; set; }


        [DisplayFormat(DataFormatString = "{0:N2}")]
        public int Quantity { get; set; }


        [DisplayFormat(DataFormatString = "{0:C2}")]
        public float Value { get => Price * Quantity; }
    }
}
