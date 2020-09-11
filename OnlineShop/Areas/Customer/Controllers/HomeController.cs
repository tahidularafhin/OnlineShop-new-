﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Data;
using OnlineShop.Models;
using OnlineShop.Utility;
using X.PagedList;

namespace OnlineShop.Controllers
{
  [Area("Customer")]
  public class HomeController : Controller
  {
    private ApplicationDbContext _db;

    public HomeController(ApplicationDbContext db)
    {
      _db = db;
    }

    public IActionResult Index(int? page)
    {
      return View(_db.Products.Include(navigationPropertyPath:c=>c.ProductTypes).Include(navigationPropertyPath: c=>c.SpecialTag).ToList().ToPagedList(pageNumber:page??1, pageSize:9));
    }


    //GET Product Detail Action

     public ActionResult Detail(int? id)
    {
      if (id==null)
      {
        return NotFound();
      }
      var product = _db.Products.Include(c => c.ProductTypes).FirstOrDefault(c => c.Id == id);
      if (product==null)
      {
        return NotFound();
      }

      return View(product);
    }


    //public IActionResult Privacy()
    //{
    //  return View();
    //}

    //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    //public IActionResult Error()
    //{
    //  return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    //}




    //POST Product Detail Action

    [HttpPost]
    [ActionName("Detail")]

    public ActionResult ProductDetail(int? id)
    {
      List<Products> products = new List<Products>();
      if (id == null)
      {
        return NotFound();
      }
      var product = _db.Products.Include(c => c.ProductTypes).FirstOrDefault(c => c.Id == id);
      if (product == null)
      {
        return NotFound();
      }
      products = HttpContext.Session.Get<List<Products>>("products");
      if (products == null)
      {
        products = new List<Products>();
      }
      products.Add(product);
      HttpContext.Session.Set("products", products);
      return RedirectToAction(nameof(Index));

    }


    //GET Remove Action  Method
    [ActionName("Remove")]

    public IActionResult RemoveToCart(int? id)
    {
      List<Products> products = HttpContext.Session.Get<List<Products>>("products");
      if (products != null)
      {
        var product = products.FirstOrDefault(c => c.Id == id);
        if (product != null)
        {
          products.Remove(product);
          HttpContext.Session.Set("products", products);
        }
      }
      return RedirectToAction(nameof(Index));
    }


    [HttpPost]
    public IActionResult Remove(int? id)
    {
      List<Products> products = HttpContext.Session.Get<List<Products>>("products");
      if (products !=null)
      {
        var product = products.FirstOrDefault(c => c.Id == id);
        if (product != null)
        {
          products.Remove(product);
          HttpContext.Session.Set("products", products);
        }
      }
      return RedirectToAction(nameof(Index));
    }

    //Get Product Cart Action Method

    public IActionResult Cart()
    {
      List<Products> products = HttpContext.Session.Get<List<Products>>("products");

      if (products==null)
      {
        products = new List<Products>();
      }
      return View(products);
    }
  }
}
