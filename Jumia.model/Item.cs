﻿using Jumia.model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jumia.Model
{
    public class Item : BaseEntity
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public string Color { get; set; }
        public int ProductID { get; set; }
        public Product Product { get; set; }
    }
}
