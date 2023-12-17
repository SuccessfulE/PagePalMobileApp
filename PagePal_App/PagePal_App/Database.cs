using MongoDB.Bson.Serialization;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PagePal_App.BookTables;
namespace PagePal_App
{
    public class Database
    {
        private readonly SQLiteAsyncConnection _database;
        public Database(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<BookTables.Books>();
            _database.CreateTableAsync<BookTables.Users>();
        }
        //Get books
        public Task<List<BookTables.Books>> GetBooksAsync()
        {
            return _database.Table<BookTables.Books>().ToListAsync();
        }
        //Get Authors
        public Task<List<BookTables.Books>> GetDistinctAuthorsAsync()
        {
            return _database.Table<BookTables.Books>().ToListAsync();
        }
        //Get Genres
        public async Task<List<string>> GetDistinctGenresAsync()
        {
            // Fetch all book details
            var bookDetailsList = await _database.Table<Books>().ToListAsync();

            // Extract and return distinct genres
            var distinctGenres = bookDetailsList.Select(bookDetail => bookDetail.Genre)
                                                .Distinct()
                                                .ToList();
            return distinctGenres;
        }
        //Get Users
        public Task<List<BookTables.Users>> GetUsers()
        {
            return _database.Table<BookTables.Users>().ToListAsync();
        }

        //Get users by username for login purposes. 
        public Task<BookTables.Users> GetUserByUsernameAsync(string username)
        {
            return _database.Table<BookTables.Users>().FirstOrDefaultAsync(u => u.UUsername == username);
        }


        //Get books based on filters
        /*public async Task<List<BookTables.Books>> GetBooksBasedOnFiltersAsync(string genre, string[] authorNames)
        {
            // Create a base query to retrieve books
            var query = _database.Table<BookTables.Books>();
            // Filter by genre if specified
            if (!string.IsNullOrEmpty(genre))
            {
                query = query.Where(b => b.Genre == genre);
            }
            // Filter by author if specified
            if (authorNames != null && authorNames.Length > 0)
            {
                // Assuming authorNames[0] is the first name and authorNames[1] is the last name
                query = query.Where(b => b.AuthorFirstName == authorNames[0] && b.AuthorLastName == authorNames[1]);
            }
            // Execute the query and return the results
            return await query.ToListAsync();
        }*/
        public async Task<List<BookTables.Books>> GetBooksBasedOnFiltersAsync(string genre, string[] authorNames)
        {
            var query = _database.Table<BookTables.Books>();

            if (!string.IsNullOrEmpty(genre))
            {
                query = query.Where(b => b.Genre == genre);
            }

            if (authorNames != null)
            {
                string firstName = authorNames.Length > 0 ? authorNames[0] : string.Empty;
                string lastName = authorNames.Length > 1 ? authorNames[1] : string.Empty;

                if (!string.IsNullOrEmpty(firstName) || !string.IsNullOrEmpty(lastName))
                {
                    query = query.Where(b => (firstName == string.Empty || b.AuthorFirstName == firstName) &&
                                             (lastName == string.Empty || b.AuthorLastName == lastName));
                }
            }

            return await query.ToListAsync();
        }

        //SaveBook
        public Task<int> SaveBookAsync(BookTables.Books book)
        {
            return _database.InsertAsync(book);
        }
        //DeleteEntireDatabase
        public Task<int> DeleteAllItems<T>()
        {
            return _database.DeleteAllAsync<T>();
        }
        //UpdateBook
        public Task<int> UpdateBook(BookTables.Books book)
        {
            return _database.UpdateAsync(book);
        }
        //Delete Book
        public Task<int> DeleteBook(BookTables.Books book)
        {
            return _database.DeleteAsync(book);
        }
        //Check if User exists
        public Task<BookTables.Users> GetUserByEmailAsync(string email)
        {
            return _database.Table<BookTables.Users>().FirstOrDefaultAsync(u => u.email == email);
        }
        //Save User
        public Task<int> SaveUserAsync(BookTables.Users users)
        {
            return _database.InsertAsync(users);
        }
        //Update User
        public Task<int> UpdateUser(BookTables.Users users)
        {
            return _database.UpdateAsync(users);
        }
        //Delete User
        public Task<int> DeleteUser(BookTables.Users users)
        {
            return _database.DeleteAsync(users);
        }
        public Task<List<BookTables.Users>> GetLoggedIn()
        {
            return _database.Table<BookTables.Users>().ToListAsync();
        }

        /* //Test user
         public Task<int> InsertTestUserAsync()
         {
             var testUser = new BookTables.Users
             {
                 UUsername = "TestUser",
                 email = "test@example.com",
                 UFirstName = "Test",
                 ULastName = "User",
                 UPassword = "password123"
             };
             return SaveUserAsync(testUser);
         }*/

    }
}