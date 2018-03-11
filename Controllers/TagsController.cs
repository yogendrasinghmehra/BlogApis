using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using BlogApis.Models.Entities;
using Microsoft.Extensions.Configuration;
using BlogApis.Models;
using System.Text;
using System.Data;
namespace BlogApis.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class TagsController:Controller
    {
        private IConfiguration config;   
        private BlogDbContext blogDbContext;

        public TagsController(IConfiguration _config,BlogDbContext _blogDb)
        {
            config=_config;
            blogDbContext=_blogDb;

        }
        [HttpGet]
        public IEnumerable<Tags> GetTags()
        {
            return blogDbContext.Tags.ToList();
        }

       [HttpPost]
       public IActionResult AddTags([FromBody] Tags tags)
       {
           IActionResult responce=Unauthorized();
           if(ModelState.IsValid)
           {
               tags.CreatedDate=DateTime.Now;
               blogDbContext.Tags.Add(tags);
               blogDbContext.SaveChanges();

               responce=Ok();

           }
           return responce;

       }

       [HttpDelete("{id}")]
       public IActionResult Delete(long id)
       {
         IActionResult responce=NotFound();
           if(id>0)
           {
            var tag=blogDbContext.Tags.Where(i=>i.Id==id).SingleOrDefault();
            if(tag!=null)
            {
             blogDbContext.Tags.Remove(tag);
             blogDbContext.SaveChanges();
            responce= Ok();
            }           
           }
           return responce;
       }



        


    }
}