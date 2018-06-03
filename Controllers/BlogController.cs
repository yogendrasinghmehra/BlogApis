using System;
using BlogApis.Models;
using BlogApis.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace BlogApis.Controllers
{
    [Route("api/[Controller]")]

    public class BlogController : ControllerBase
    {
        private IConfiguration config;
        private BlogDbContext blogDbContext;

        public BlogController(IConfiguration _config, BlogDbContext _dbContext)
        {
            config = _config;
            blogDbContext = _dbContext;
        }
        [HttpGet]
        public IActionResult GetBlogs(parameters parameters)
        {
            int? prePage = null, nextPage = null;

            //getting total counts       
            var total = blogDbContext.Blog.Count();

            //setting previous and next page index for pagination...
            if (total > parameters.pageSize * parameters.pageNo)
            {
                nextPage = parameters.pageNo + 1;
                prePage = parameters.pageNo - 1;
            }
            else if (parameters.pageNo > 1)
            {
                nextPage = null;
                prePage = parameters.pageNo - 1;
            }
            //getting how many records to skip
            var skip = parameters.pageSize * (parameters.pageNo - 1);

            //getting tags if tag is not null..
            int? tagId=null;
            if (!string.IsNullOrEmpty(parameters.tag))
            {
                var tags = blogDbContext.Tags.Where(t => t.Name == parameters.tag).SingleOrDefault();
                tagId = tags.Id;

            };

            //finally getting records.
           
                return Ok(new
                {
                    list = GetBlogListByTag(skip, parameters.pageSize, tagId),
                    prePage = prePage,
                    nextPage = nextPage
                });
            
           
        }

        [HttpGet("{Id:int}")]
        public IActionResult GetBlogById(int id)
        {
            //return blogDbContext.blog.            
            return Ok(new { BlogDetails = blogDbContext.Blog.Where(i => i.Id == id).SingleOrDefault(), Tags = blogDbContext.Tags.ToList() });
        }


        [HttpGet]
        [Route("{urlSlug}")]
        public IActionResult GetBlogBySlug(string urlSlug)
        {
            //return blogDbContext.blog.            
            return Ok(new { BlogDetails = blogDbContext.Blog.Where(i => i.urlslug == urlSlug).SingleOrDefault() });
        }


        [HttpPost]
        [Authorize]
        public IActionResult AddBlog([FromBody] Blog blog)
        {
            if (ModelState.IsValid)
            {
                blog.CreatedDate = DateTime.Now;
                blog.ModifiedDate = DateTime.Now;
                blogDbContext.Blog.Add(blog);
                blogDbContext.SaveChanges();

                return Ok();
            }
            else
            {
                return NotFound();
            }

        }

        [HttpPut]
        [Authorize]
        public IActionResult EditBlog([FromBody] Blog blog)
        {

            if (ModelState.IsValid)
            {
                blog.ModifiedDate = DateTime.Now;
                //blogDbContext.Blog.Update(blog);
                blogDbContext.Entry(blog).State = EntityState.Modified;
                blogDbContext.SaveChanges();

                return Ok();
            }
            else
            {
                return NotFound();
            }

        }

        private List<Blog> GetBlogList(int skip, int pageSize)
        {
            return blogDbContext.Blog.OrderByDescending(i => i.ModifiedDate)
           .Skip(skip)
           .Take(pageSize)
           .ToList();
        }

        private List<Blog> GetBlogListByTag(int skip, int pageSize, int? tagId)
        {
            return blogDbContext.Blog.OrderByDescending(i => i.ModifiedDate)
            .Where(i => i.TagsId == tagId|| tagId==null)
           .Skip(skip)
           .Take(pageSize)
           .ToList();
        }



    }
    public class parameters
    {
        public int pageNo { get; set; } = 1;
        public int pageSize { get; set; } = 10;

        public string tag { get; set; }
    }
}