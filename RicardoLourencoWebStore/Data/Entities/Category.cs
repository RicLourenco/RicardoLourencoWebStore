﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RicardoLourencoWebStore.Data.Entities
{
    public class Category : IEntity
    {
        public int Id { get; set; }


        [Display(Name = "Category")]
        public string Name { get; set; }
    }
}
