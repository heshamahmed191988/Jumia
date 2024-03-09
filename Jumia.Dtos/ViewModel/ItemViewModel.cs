﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jumia.Dtos.ViewModel
{
    public class ItemViewModel
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int Quantity { get; set; }
        public string Color { get; set; }
        public string ProductName { get; set; }
        public int ProductId { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
