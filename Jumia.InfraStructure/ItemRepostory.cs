﻿using Jumia.Application.Contract;
using Jumia.Context;
using Jumia.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jumia.InfraStructure
{
    public class ItemRepostory : Repository<Item, int> , IItemReposatory
    {
        private readonly JumiaContext _context;

        public ItemRepostory(JumiaContext context) : base(context)
        {
            _context = context;
        }

        public Task<int> GetProductID(string ProductName)
        {
            return Task.FromResult(_context.products.Where(p => p.Name == ProductName).Select(p => p.Id).FirstOrDefault());
        }
        public Task<string> GetProductName(int ID)
        {
            return Task.FromResult(_context.products.Where(p => p.Id == ID).Select(p => p.Name).FirstOrDefault());
        }
    }
}