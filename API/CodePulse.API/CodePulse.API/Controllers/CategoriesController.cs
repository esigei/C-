﻿using CodePulse.API.Data;
using CodePulse.API.Models.Domain;
using CodePulse.API.Models.DTO;
using CodePulse.API.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CodePulse.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository categoryRepository;

        public CategoriesController(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }
        [HttpPost]
        public async Task<IActionResult>CreateCategory(CreateCategoryRequestDto request)
        {
            //Map DTO to Domain Model
            var category = new Category
            {
                Name = request.Name,
                UrlHandle = request.UrlHandle,
            };

            await categoryRepository.CreateAsync(category);
            //Map Domain Model to DTO
            var response = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                UrlHandle = category.UrlHandle,
            };

            return Ok(response);

        }
        // GET:https://localhost:7194/api/Categories
        [HttpGet]
       public async Task<IActionResult> GetAllCategories()
        {
            var categories = await categoryRepository.GetAllAsync();
            //Map Domain Model to DOTO
            var response=new List<CategoryDto>();
            foreach (var category in categories)
            {
                response.Add(new CategoryDto 
                { 
                    Id = category.Id, 
                    Name = category.Name,
                    UrlHandle=category.UrlHandle
                });   
            }
            return Ok(response);
        }
        //get categories by id
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetCategoryById([FromRoute]Guid id)
        {
            var existingCategory = await categoryRepository.GetById(id);
            if(existingCategory is null)
            {
                return NotFound();
            }
            var response=new CategoryDto 
            { 
                Id = existingCategory.Id, 
                Name = existingCategory.Name,
                UrlHandle=existingCategory.UrlHandle
            };
            return Ok(response);
        }
        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> EditCategory([FromRoute] Guid id,UpdateCategoryRequestDto request)
        {
            //convert DTO to Domain Model
            var category = new Category
            {
                Id = id,
                Name = request.Name,
                UrlHandle = request.UrlHandle
            };
           category= await categoryRepository.UpdateAsync(category);
            if(category == null)
            {
                return NotFound();
            }
            //convert Domain Model to DTO
            var response = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                UrlHandle = category.UrlHandle
            };
            return Ok(response);

        }
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteCategory([FromRoute] Guid id)
        {
            var category= await categoryRepository.DeleteAsync(id);
            if(category == null)
            {
                return NotFound();
            }
            //convert Domain Model to DTO
            var response = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                UrlHandle = category.UrlHandle
            };
            return Ok(response);
        }


    }
}