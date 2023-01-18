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

        //метод возвращения информации по конкретной цене товара
        [HttpGet("price/{price}")]
        public async Task<ActionResult<IEnumerable<Product>>> Get(int price)
        {
            List<Product> products = await db.Products.Where(x => x.Price == price).ToListAsync();
            if (products.Count == 0) return NotFound();
            return products;
        }

        //метод возвращения количества товаров с определённым именем
        [HttpGet("namecount/{name}")]
        public async Task<ActionResult<int>> GetNameCount(string name)
        {
            List<Product> products = await db.Products.Where(x => x.Name == name).ToListAsync();
            if (products.Count == 0) return NotFound();

            return products.Count;
        }

        //метод получения информации о средней цене по товарам
        [HttpGet("priceaverage")]
        public async Task<ActionResult<double>> GetPriceAverage()
        {
            int totalprice = 0;
            double priceavg = 0;
            List<Product> products = await db.Products.ToListAsync();
            foreach (var p in products)
            {
                totalprice += p.Price;
            }
            priceavg = totalprice / products.Count;
            return priceavg;
        }

        //метод скидки на все товары
                                                                                 //Invoke-RestMethod http://localhost:5000/api/product/discountall/50 -Method PUT
        [HttpPut("discountall/{discount}")]
        public async Task<ActionResult<Product>> PutDiscountAll(int discount)
        {
            List<Product> products = await db.Products.ToListAsync();

            foreach (var p in products)
            {
                p.Price = p.Price/100*discount;
                db.Update(p);
            }
     
            await db.SaveChangesAsync();
            return Ok(products);
        }

        //метод скидки на товар с определённым именем        
                                                                                //Invoke-RestMethod http://localhost:5000/api/product/discountname/cheese/50 -Method PUT
        [HttpPut("discountname/{name}/{discount}")]
        public async Task<ActionResult<Product>> PutDiscountName(string name, int discount)
        {
            List<Product> products = await db.Products.ToListAsync();

            foreach (var p in products)
            {
                if (p.Name == name) p.Price = p.Price / 100 * discount;
                db.Update(p);
            }

            await db.SaveChangesAsync();
            return Ok(products);
        }





    }
}
