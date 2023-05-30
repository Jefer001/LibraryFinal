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

        public async Task<IEnumerable<SelectListItem>> GetDDLCataloguesAsync(IEnumerable<Catalogue> filterCatatalogues)
        {
            List<Catalogue> catalogues = await _context.Catalogues.ToListAsync();
            List<Catalogue> catalogueFiltered = new();

            foreach (Catalogue catalogue in catalogues)
                if (!filterCatatalogues.Any(c => c.Id.Equals(catalogue.Id)))
                    catalogueFiltered.Add(catalogue);

            List<SelectListItem> ListCatalogues = catalogueFiltered
                .Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                })
                .OrderBy(c => c.Text)
                .ToList();

            ListCatalogues.Insert(0, new SelectListItem
            {
                Text = "Seleccione un catálogo...",
                Value = Guid.Empty.ToString(),
                Selected = true
            });

            return ListCatalogues;
        }

        public async Task<IEnumerable<SelectListItem>> GetDDLUniversitiesAsync()
        {
            List<SelectListItem> listUniversities = await _context.Universities
                .Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString(),
                })
                .OrderBy(c => c.Text)
                .ToListAsync();

            listUniversities.Insert(0, new SelectListItem
            {
                Text = "Seleccione una universidad...",
                Value = Guid.Empty.ToString(),
                Selected = true
            });

            return listUniversities;
        }
        #endregion
    }
}
