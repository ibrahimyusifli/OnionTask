﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProniaOnion202.Domain.Entities
{
    internal class Category:BaseNameableEntity
    {
        //Relational Properties
        public ICollection<Product>? Products { get; set; }
    }
}
