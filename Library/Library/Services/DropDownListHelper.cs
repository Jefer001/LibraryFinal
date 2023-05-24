using Library.DAL;
using Library.DAL.Entities;
using Library.Helpers;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Library.Services
{
    public class DropDownListHelper : IDropDownListHelper
    {
        #region Constants
        private readonly DataBaseContext _context;
        #endregion

        #region Builder
        public DropDownListHelper(DataBaseContext context)
        {
            _context = context;
        }
        #endregion

        #region Public methods
        public async Task<IEnumerable<SelectListItem>> GetDDLCataloguesAsync()
        {
            List<SelectListItem> listCatalogues = await _context.Catalogues
                .Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString(),
                })
                .OrderBy(c => c.Text)
                .ToListAsync();

            listCatalogues.Insert(0, new SelectListItem
            {
                Text = "Seleccione un catálogo...",
                Value = Guid.Empty.ToString(),
                Selected = true
            });

            return listCatalogues;
        }

        //public async Task<IEnumerable<SelectListItem>> GetDDLCataloguesAsync(IEnumerable<Catalogue> filterCatatalogues)
        //{
        //    List<Catalogue> catalogue = await _context.Catalogues.ToListAsync();
        //    List<Catalogue> filterCatatalogues = new();

        //    foreach (Catalogue catalogues in catalogue)
        //        if (!filterCatatalogues.Any(c => c.Id.Equals(catalogue.Id)))
        //            catataloguesFiltered.Add(catalogue);

        //    List<SelectListItem> ListCatalogues = catataloguesFiltered
        //        .Select(c => new SelectListItem
        //        {
        //            Text = c.Name,
        //            Value = c.Id.ToString()
        //        })
        //        .OrderBy(c => c.Text)
        //        .ToList();

        //    ListCatalogues.Insert(0, new SelectListItem
        //    {
        //        Text = "Seleccione un catálogo...",
        //        Value = Guid.Empty.ToString(),
        //        Selected = true
        //    });

        //    return ListCatalogues;
        //}
        #endregion
    }
}
