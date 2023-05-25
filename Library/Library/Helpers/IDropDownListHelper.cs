using Library.DAL.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Library.Helpers
{
    public interface IDropDownListHelper
    {
        Task<IEnumerable<SelectListItem>> GetDDLCataloguesAsync();

        Task<IEnumerable<SelectListItem>> GetDDLCataloguesAsync(IEnumerable<Catalogue> filterCatatalogues);
    }
}
