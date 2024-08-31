using System.Diagnostics;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.AspNetCore.Authorization;
using StockTrading.Models;


namespace StockTrading.Controllers;


public class AdminController : Controller
{
    private  CategoryServices CategoryServ;
    private readonly ItemsServices ItemServ;
    private readonly ILogger<AdminController> _logger;
    public AdminController(CategoryServices catServ, ItemsServices itemServ, ILogger<AdminController> logger)
    {
        CategoryServ =  catServ;
        ItemServ = itemServ;
        _logger = logger;
    }

    //login page
    //[Authorize(Roles = "Admin")]
    //public IActionResult Index()
    //{
    //    AdminLogin temp = new AdminLogin();
    //    return View(temp);
    //}

    //[HttpPost]
    //[Authorize(Roles = "Admin")]
    //public ActionResult Login(AdminLogin adminInfo)
    //{
    //    adminServices adminServ = new adminServices();
    //    bool status = adminServ.Login(adminInfo.UserName, adminInfo.Password);
    //    if (status == true)
    //    {
    //        return View("Dashboard");
    //    }
    //    ViewBag.Msg = "error in username or password";
    //    return View("Index", adminInfo);
    //}

    [Route("GetAllItems")]
    //[ServiceFilter(typeof(executionTimeFilter))]
    public ActionResult AllItemsDashboard(int CatId = -1)
    {
        _logger.LogInformation("Start get all items...");

        ItemFilterView item = new ItemFilterView();
        ItemModel model = new ItemModel();
        model.CategoriesId = CreateDropDownList();
        item.ItemModelFilter = model;
        item.AllItems = ItemServ.GetList(CatId);

        _logger.LogInformation("End get all items...");

        return View("ItemsDashboard", item);
    }

    [ResponseCache(Duration = 10)]
    [HttpGet]
    public ActionResult ItemsDashboard(ItemFilterView item)
    {
        // get all items
        if (item.ItemModelFilter == null)
        {
            item.ItemModelFilter = new ItemModel();
            item.AllItems = ItemServ.GetListByNameAndCategoryId("", -1);
        }
        else
        {
            item.AllItems = ItemServ.GetListByNameAndCategoryId(item.ItemModelFilter.Name, item.ItemModelFilter.CategoryId);
        }
        item.ItemModelFilter.CategoriesId = CreateDropDownList();
        return View("ItemsDashboard", item);
    }


    private List<SelectListItem> CreateDropDownList()
    {
        var categoriesId = new List<SelectListItem>();

        categoriesId.Add(new SelectListItem { Value = $"{-1}", Text = "All" });
        foreach (var cat in CategoryServ.GetList())
        {
            categoriesId.Add(new SelectListItem { Value = $"{cat.CategoryId}", Text = cat.CategoryName });
        }

        return categoriesId;
    }

    //[Authorize(Roles = "Admin")]
    public ActionResult CreateItem()
    {
        ItemModel model = new ItemModel();
        model.CategoriesId = CreateDropDownList();
        return View(model);
    }

    [HttpPost]
    //[Authorize(Roles = "Admin")]
    public ActionResult CreateItem(ItemModel ItemInfo)
    {
        bool status = ItemServ.Create(ItemInfo.Name, ItemInfo.Description, ItemInfo.PhotoFile, ItemInfo.Price, ItemInfo.CategoryId);
        if (status == true)
        {
            //Go to the Dashboard page
            return RedirectToAction("ItemsDashboard");
        }
        ViewBag.Msg = "Duplicate Category";
        //Go to Create page
        return View("CreateItem", ItemInfo);
    }


    public ActionResult CategoryDashboard()
    {
        _logger.LogInformation("Get all Categories...");
        //return ViewComponent("AllCategory");
        return View("CategoryDashboard", CategoryServ.GetList());
        return View("CategoryDashboard");
    }

    //[Authorize(Roles = "Admin")]
    public ActionResult CreateCategory()
    {
        return View(new CategoryModel());
    }

    [HttpPost]
    //[Authorize(Roles = "Admin")]
    public ActionResult CreateCategory(CategoryModel CategoryInfo)
    {
        bool status = CategoryServ.Create(CategoryInfo.Name, CategoryInfo.Description, CategoryInfo.PhotoFile);
        if (status == true)
        {
            //Go to the Dashboard page
            return RedirectToAction("CategoryDashboard");
        }
        ViewBag.Msg = "Duplicate Category";
        //Go to Create page
        return View("CreateCategory", CategoryInfo);
    }

    //[Authorize(Roles = "Admin")]
    public ActionResult EditCategory(string catName)
    {
        Category CatToEdit = CategoryServ.FindCategory(catName);

        CategoryModel Cat = new CategoryModel();
        Cat.Name = CatToEdit.CategoryName;
        Cat.Description = CatToEdit.CategoryDescription;
        Cat.Photo = CatToEdit.CategoryPhoto;

        return View("EditCategory", Cat);
    }

    [HttpPost]
    //[Authorize(Roles = "Admin")]
    public ActionResult EditCategory(CategoryModel CategoryInfo)
    {
        bool status = CategoryServ.Edit(CategoryInfo);
        if (status == false)
        {
            ViewBag.Msg = "Category not Found";
            return View("EditCategory", CategoryInfo);
        }
        return RedirectToAction("CategoryDashboard", "Admin");
    }

    //This can be deleteConfirmed and the next one to be HttpPost to delete
    // public ActionResult Delete()
    // {
    //     //Go to the Dashboard page
    //     return View("DeleteConformation", CatName);
    // }

    //[Authorize(Roles = "Admin")]
    public ActionResult DeleteCategory(string CatName)
    {
        CategoryModel CategoryInfo = new CategoryModel();
        CategoryInfo.Name = CatName;
        bool status = CategoryServ.Delete(CategoryInfo);
        if (status == true)
        {
            ViewBag.Msg = "Successful Deleting.";
        }
        else
        {
            ViewBag.Msg = "Can not delete.";
        }
        return RedirectToAction("CategoryDashboard");
    }

   
}
