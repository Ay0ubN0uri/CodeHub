
namespace CodeHub.Models.ViewModels
{
    public class DashboardViewModel
    {
        public int ProductCount { get; set; }
        public int CategoryCount { get; set; }
        public int UserCount { get; set; }
        public List<CategoryProductCountViewModel> CategoryProductCount { get; set; }
    }
}
