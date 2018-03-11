using System;
using BlogApis.Models;
using BlogApis.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace BlogApis.Controllers
{
    [Route("api/[Controller]")]
    [Authorize]
    public class BlogController:Controller
    {
        private IConfiguration config;
        private BlogDbContext blogDbContext;

        public BlogController(IConfiguration _config,BlogDbContext _dbContext)
        {
            config=_config;
            blogDbContext=_dbContext;
        }
        [HttpGet]
        public IEnumerable<Blog> GetBlogs()
        {
            return blogDbContext.Blog.ToList();

        }

        [HttpGet("{Id}")]
        public Blog GetBlog(int id)
        {
            //return blogDbContext.blog.            
            return blogDbContext.Blog.Where(i=>i.Id==id).SingleOrDefault();                       
        }

        [HttpPost]
        public IActionResult AddBlog([FromBody] Blog blog)
        {
            if(ModelState.IsValid)
            {
                    blog.CreatedDate=DateTime.Now;
                    blog.ModifiedDate=DateTime.Now;
                    blogDbContext.Blog.Add(blog);
                    blogDbContext.SaveChanges();

                return Ok();
            }
            else
            {
                return NotFound();
            }
          
        }

    }
}