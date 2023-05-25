using Library.DAL.Entities;
using Library.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Library.DAL
{
    public class SeederDB
    {
        #region Constants
        private readonly DataBaseContext _context;
        private readonly IAzureBlobHelper _azureBlobHelper;
        //private readonly IUserHelpers _userHelpers;
        #endregion

        #region Builder
        public SeederDB(DataBaseContext context, IAzureBlobHelper azureBlobHelper)
        {
            _context = context;
            _azureBlobHelper = azureBlobHelper;
            //_userHelpers = userHelpers;
        }
        #endregion

        #region Public methods
        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();

            await PopulateCataloguessAsync();
            await PopulateBookAsync();
            //await PopulateRolesAsync();
            //await PopulateUserAsync("Steve", "Jobs", "steve_jobs_admin@yopmail.com", "3002323232", "Street Apple", "102030", "admin_use.png", UserType.Admin);
            //await PopulateUserAsync("Bill", "Gates", "bill_gates_user@yopmail.com", "4005656656", "Street Microsoft", "405060", "user.png", UserType.User);

            await _context.SaveChangesAsync();
        }
        #endregion

        #region Private methods
        private async Task PopulateCataloguessAsync()
        {
            if (!_context.Catalogues.Any())
            {
                _context.Catalogues.Add(new Catalogue { Name = "Narrativo", Description = "Cuento, epopeya, novela, poema épico, cantares de gesta, fábula, leyendas, romances líricos, epístola.", CreatedDate = DateTime.Now });
                _context.Catalogues.Add(new Catalogue { Name = "Lírico", Description = "Poema, oda, sonetos, elegía, égloga sátira, himnos o canciones.", CreatedDate = DateTime.Now });
                _context.Catalogues.Add(new Catalogue { Name = "Dramático", Description = "Teatro, drama, comedia, amor, ópera, melodrama, farsa, tragedia.", CreatedDate = DateTime.Now });
                _context.Catalogues.Add(new Catalogue { Name = "Didáctico", Description = "Su principal intención es enseñar, centrándose en lo que puede dejar lo narrado.", CreatedDate = DateTime.Now });
            }
        }

        private async Task PopulateBookAsync()
        {
            if (!_context.Books.Any())
            {
                await AddBooktAsync("Un pájaro de aire", "María Emilia López", 27, new List<string>() { "Didáctico" }, new List<string>() { "pajaro.png" });
                await AddBooktAsync("Cien años de soledad", "Gabriel García Márquez", 300, new List<string>() { "Narrativo" }, new List<string>() { "cien.png", "garcia.png" });
            }
        }

        private async Task AddBooktAsync(string name, string author, int stock, List<string> catalogues, List<string> images)
        {
            Book book = new()
            {
                Name = name,
                Author = author,
                Stock = stock,
                BookCatalogues = new List<BookCatalogue>(),
                BookImages = new List<BookImage>()
            };

            foreach (string? catalogue in catalogues)
            {
                book.BookCatalogues.Add(new BookCatalogue { Catalogue = await _context.Catalogues.FirstOrDefaultAsync(c => c.Name.Equals(catalogues)) });
            }

            foreach (string? image in images)
            {
                Guid imageId = await _azureBlobHelper.UploadAzureBlobAsync($"{Environment.CurrentDirectory}\\wwwroot\\images\\books\\{image}", "products");
                book.BookImages.Add(new BookImage { ImageId = imageId });
            }

            _context.Books.Add(book);
        }

        //private async Task PopulateRolesAsync()
        //{
        //    await _userHelpers.AddRoleAsync(UserType.Admin.ToString());
        //    await _userHelpers.AddRoleAsync(UserType.User.ToString());
        //}

        //private async Task PopulateUserAsync(string firstName, string lastName, string email, string phone, string address, string document, string image, UserType userType)
        //{
        //    User user = await _userHelpers.GetUserAsync(email);

        //    if (user == null)
        //    {
        //        Guid imageId = await _azureBlobHelper.UploadAzureBlobAsync($"{Environment.CurrentDirectory}\\wwwroot\\images\\users\\{image}", "users");
        //        user = new User
        //        {
        //            CreatedDate = DateTime.Now,
        //            FirstName = firstName,
        //            LastName = lastName,
        //            Email = email,
        //            UserName = email,
        //            PhoneNumber = phone,
        //            Address = address,
        //            Document = document,
        //            City = _context.Cities.FirstOrDefault(),
        //            ImageId = imageId,
        //            UserType = userType
        //        };
        //        await _userHelpers.AddUserAsync(user, "123456");
        //        await _userHelpers.AddUserToRoleAsync(user, userType.ToString());
        //    }
        //}


        #endregion
    }
}
