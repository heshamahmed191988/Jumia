﻿using AutoMapper;
using Jumia.Application.Contract;
using Jumia.Dtos;
using Jumia.Dtos.ResultView;
using Jumia.Application.Services;
using Jumia.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jumia.Dtos.ViewModel;

namespace Jumia.Application.Services
{
    public class ProductService: IProductService
    {
        private readonly IProductReposatory productReposatory;
        private readonly IMapper _mapper;
        private readonly ICategoryReposatory categoryRepository;

        public ProductService(IProductReposatory productReposatory, IMapper mapper, ICategoryReposatory categoryRepository)
        {
            this.productReposatory = productReposatory;
            _mapper = mapper;
            this.categoryRepository = categoryRepository;
        }

        public async Task<ResultView<ProuductViewModel>> Create(ProuductViewModel product)
        {
            var Query = (await productReposatory.GetAllAsync());
            var OldProduct = Query.Where(p => p.Id == product.Id).FirstOrDefault();
            if (OldProduct != null)
            {
                return new ResultView<ProuductViewModel> { Entity = null, IsSuccess = false, Message = "Already Exist" };
            }
            else
            {
                var Prd = _mapper.Map<Product>(product);
                var NewPrd = await productReposatory.CreateAsync(Prd);
                await productReposatory.SaveChangesAsync();
                var PrdDto = _mapper.Map<ProuductViewModel>(NewPrd);
                return new ResultView<ProuductViewModel> { Entity = PrdDto, IsSuccess = true, Message = "Created Successfully" };
            }

        }
        public async Task<ResultView<ProuductViewModel>> SoftDelete(ProuductViewModel product)
        {
            try
            {
                var PRd = _mapper.Map<Product>(product);
                var Oldprd = (await productReposatory.GetAllAsync()).FirstOrDefault(p => p.Id == product.Id);
                Oldprd.IsDeleted = true;
                await productReposatory.SaveChangesAsync();
                var PrdDto = _mapper.Map<ProuductViewModel>(Oldprd);
                return new ResultView<ProuductViewModel> { Entity = PrdDto, IsSuccess = true, Message = "Deleted Successfully" };
            }
            catch (Exception ex)
            {
                return new ResultView<ProuductViewModel> { Entity = null, IsSuccess = false, Message = ex.Message };

            }
        }
        public async Task<ResultView<ProuductViewModel>> Update(ProuductViewModel proudect)
        {
            try
            {
                var uproudect = await productReposatory.GetByIdAsync(proudect.Id);
                uproudect.Name = proudect.Name;
                uproudect.Price = proudect.Price;
                uproudect.StockQuantity = proudect.StockQuantity;
                uproudect.Description = proudect.Description;
                uproudect.DateListed = proudect.DateListed;
                uproudect.CategoryID = proudect.CategoryId;
                uproudect.IsDeleted = proudect.IsDeleted;


                await productReposatory.SaveChangesAsync();


                var updateProudectDto = _mapper.Map<ProuductViewModel>(uproudect);
                return new ResultView<ProuductViewModel> { Entity = updateProudectDto, IsSuccess = true, Message = "Book updated successfully." };
            }
            catch (Exception ex)
            {
                return new ResultView<ProuductViewModel> { Entity = null, IsSuccess = false, Message = ex.Message };
            }
        }

        public async Task<ProuductViewModel> GetOne(int ID)
        {
            var product = await productReposatory.GetByIdAsync(ID);
            var REturnproudect = _mapper.Map<ProuductViewModel>(product);
            return REturnproudect;
        }

        public async Task<ResultDataList<ProuductViewModel>> GetAllPagination(int items, int pagenumber)
        {
            var AlldAta = (await productReposatory.GetAllAsync());
            var activeProducts = AlldAta.Where(p => p.IsDeleted == false);
            var proudects = activeProducts.Skip(items * (pagenumber - 1)).Take(items).Select(p => new ProuductViewModel()
            {
                Id = p.Id,
                Name = p.Name,
                CategoryName = p.Category.Name,
                StockQuantity = p.StockQuantity,
                Price = p.Price,
                DateListed = p.DateListed,
                Description = p.Description,
                SellerID = p.SellerID,
                IsDeleted = p.IsDeleted,
            }).ToList();
            ResultDataList<ProuductViewModel> resultDataList = new ResultDataList<ProuductViewModel>();
            resultDataList.Entities = proudects;
            resultDataList.Count = AlldAta.Count();
            return resultDataList;
        }

        public async Task<List<CateogaryViewModel>> GetAllCategories()
        {
            try
            {
                var categories = await categoryRepository.GetAllAsync();
                var categoryViewModels = _mapper.Map<List<CateogaryViewModel>>(categories);
                return categoryViewModels;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

    }
    //public async Task<List<CateogaryViewModel>> GetAllCategories()
    //{
    //    try
    //    {
    //        var categories = await categoryRepository.GetAllAsync();
    //        var categoryViewModels = _mapper.Map<List<CateogaryViewModel>>(categories);
    //        return categoryViewModels;
    //    }
    //    catch (Exception ex)
    //    {
    //        // Handle exceptions
    //        throw ex;
    //    }
    //}
}
