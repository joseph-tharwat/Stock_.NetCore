using Microsoft.AspNetCore.Mvc;

namespace StockProj.ViewComponents.Category
{
    public class AllCategoryViewComponent: ViewComponent
    {
        private readonly CategoryServices _categoryServ;
        public AllCategoryViewComponent(CategoryServices CategoryServ)
        {
            _categoryServ = CategoryServ;
        }

        public IViewComponentResult Invoke()
        {
            return View(_categoryServ.GetList());
        }
    }
}
