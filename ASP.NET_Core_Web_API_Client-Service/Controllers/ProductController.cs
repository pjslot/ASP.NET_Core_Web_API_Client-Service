using ASP.NET_Core_Web_API_Client_Service.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_Core_Web_API_Client_Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        ProductContext db;
        //конструктор контроллера
        public ProductController(ProductContext context)
        {
            db = context;
            if (!db.Products.Any())
            {
                db.Products.Add(new Product { Name = "Tomato", Price = 12 });
                db.Products.Add(new Product { Name = "Potato", Price = 17 });
                db.Products.Add(new Product { Name = "Lemon", Price = 28 });
                db.SaveChanges();
            }
        }

        //метод возвращения всей информции о БД
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> Get()
        {
            return await db.Products.ToListAsync();
        }

        //метод возвращения информации по конкетному имени товара
        [HttpGet("{name}")]
        public async Task<ActionResult<IEnumerable<Product>>> Get(string name)
        {
            List<Product> products = await db.Products.Where(x => x.Name == name).ToListAsync();
            if (products.Count == 0) return NotFound();
            return products;
        }

        //метод добавление товара с помощью POST-запроса с параметрами
        [HttpPost]
        public async Task<ActionResult<Product>> Post(Product product)
        {
            if (product == null)
            {
                return BadRequest();
            }
            db.Products.Add(product);
            await db.SaveChangesAsync();
            return Ok(product);
        }

        //метод обновления товара с помощью PUT-запроса с параметрами
        [HttpPut]
        public async Task<ActionResult<Product>> Put(Product product)
        {
            if (product == null)
            {
                return BadRequest();
            }
            if (!db.Products.Any(x => x.ProductID == product.ProductID))
            {
                return NotFound();
            }

            db.Update(product);
            await db.SaveChangesAsync();
            return Ok(product);
        }

        //метод удаления товара с помощью DELETE-запроса с параметрами
        [HttpDelete("{product_id}")]
        public async Task<ActionResult<Product>> Delete(string product_id)
        {
            Product user = db.Products.FirstOrDefault(x => x.ProductID == product_id);
            if (user == null)
            {
                return NotFound();
            }
            db.Products.Remove(user);
            await db.SaveChangesAsync();
            return Ok(user);
        }

    }
}
