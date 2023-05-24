using Library.DAL.Entities;
using Library.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Library.DAL
{
    public class SeederDB
    {
        #region Constants
        private readonly DataBaseContext _context;
        //private readonly IUserHelpers _userHelpers;
        private readonly IAzureBlobHelper _azureBlobHelper;
        #endregion

        #region Builder
        public SeederDB(DataBaseContext context, IAzureBlobHelper azureBlobHelper)
        {
            _context = context;
            //_userHelpers = userHelpers;
            ////_azureBlobHelper = azureBlobHelper;
        }
        #endregion

        #region Public methods
        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();

            await PopulateliteraryGendersAsync();
            //await PopulateBookAsync();
            //await PopulateRolesAsync();
            //await PopulateUserAsync("Steve", "Jobs", "steve_jobs_admin@yopmail.com", "3002323232", "Street Apple", "102030", "admin_use.png", UserType.Admin);
            //await PopulateUserAsync("Bill", "Gates", "bill_gates_user@yopmail.com", "4005656656", "Street Microsoft", "405060", "user.png", UserType.User);

            await _context.SaveChangesAsync();
        }
        #endregion

        #region Private methods
        private async Task PopulateliteraryGendersAsync()
        {
            if (!_context.LiteraryGenders.Any())
            {
                _context.LiteraryGenders.Add(new LiteraryGender { Name = "Épico", Description = "En el género épico, el diálogo y la descripción forman parte de la narrativa, de eventos reales o ficticios.", CreatedDate = DateTime.Now });
                _context.LiteraryGenders.Add(new LiteraryGender { Name = "Lírico", Description = "En el género épico, el diálogo y la descripción forman parte de la narrativa, de eventos reales o ficticios.", CreatedDate = DateTime.Now });
                _context.LiteraryGenders.Add(new LiteraryGender { Name = "Dramático", Description = "Es un tipo de texto donde el autor expresa sus sentimientos. Se llama así, porque en la Antigua Grecia los cantaban acompañándose de un instrumento llamado Lira.", CreatedDate = DateTime.Now });
                _context.LiteraryGenders.Add(new LiteraryGender { Name = "Didáctico", Description = "El género literario dramático, se desarrolla en episodios, con subtramas, donde el diálogo es fundamental.", CreatedDate = DateTime.Now });
            }
        }

        //private async Task PopulateBookAsync()
        //{
        //    if (!_context.Books.Any())
        //    {
        //        await AddBooktAsync("Un pájaro de aire", "María Emilia López", 27, new List<string>() { "Didáctico" }, new List<string>() { "pajaro.png" });
        //        await AddBooktAsync("Cien años de soledad", "Gabriel García Márquez", 300, new List<string>() { "Narración" }, new List<string>() { "cien.png", "garcia.png" });
        //    }
        //}

        //private async Task AddBooktAsync(string name, string author, int stock, List<string> literary, List<string> images)
        //{
        //    Book book = new()
        //    {
        //        Name = name,
        //        Author = author,
        //        Stock = stock,
        //        BookGenders = new List<BookGender>(),
        //        BookImages = new List<BookImage>()
        //    };

        //    foreach (string? literaryGender in literary)
        //    {
        //        book.BookGenders.Add(new BookGender { Literary = await _context.LiteraryGenders.FirstOrDefaultAsync(lg => lg.Name.Equals(literaryGender)) });
        //    }

        //    foreach (string? image in images)
        //    {
        //        Guid imageId = await _azureBlobHelper.UploadAzureBlobAsync($"{Environment.CurrentDirectory}\\wwwroot\\images\\books\\{image}", "products");
        //        book.BookImages.Add(new BookImage { ImageId = imageId });
        //    }

        //    _context.Books.Add(book);
        //}

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
