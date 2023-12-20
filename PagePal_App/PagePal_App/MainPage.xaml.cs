using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using PagePal_App;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static PagePal_App.BookTables;

namespace PagePal_App
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        //private Entry searchEntry;

        public MainPage()
        {
            InitializeComponent();
            LoadAuthors();
            LoadGenres();
            //string name = App.UserName;
            LoggedIn.Text = "Welcome to PagePal";
            UserIN.Text = "User: " + App.UserName;
            OnAppearing();
        }

        protected override async void OnAppearing()
        {
            try
            {
                base.OnAppearing();
                await App.Database.GetDistinctAuthorsAsync();
                await App.Database.GetDistinctGenresAsync();
            }
            catch { }
        }

        /*private void LoadAuthors()
        {
            // Retrieve distinct authors from the database
            var authors = App.Database.GetDistinctAuthorsAsync().Result;

            // Check if there are authors in the database
            if (authors.Any())
            {
                // Populate the Author Picker with the retrieved authors
                foreach (var author in authors)
                {
                    authorPicker.Items.Add(string.Format("{0} {1}", author.AuthorFirstName, author.AuthorLastName));
                }
            }
            else
            {
                // If no authors in the database, provide a default placeholder
                authorPicker.Items.Add("No Authors Found");
            }
        }*/

        private async void LoadAuthors()
        {
            // Retrieve distinct authors from the database asynchronously
            var authors = await App.Database.GetDistinctAuthorsAsync();

            // Check if there are authors in the database
            if (authors.Any())
            {
                // Populate the Author Picker with the retrieved authors
                foreach (var author in authors)
                {
                    authorPicker.Items.Add(string.Format("{0} {1}", author.FirstName, author.LastName));
                }
            }
            else
            {
                // If no authors in the database, provide a default placeholder
                authorPicker.Items.Add("No Authors Found");
            }
        }


        private async void LoadGenres()
        {
            try
            {
                var genres = await App.Database.GetDistinctGenresAsync();
                if (genres != null && genres.Any())
                {
                    genrePicker.ItemsSource = genres;
                }
                else
                {
                    genrePicker.Items.Add("No Genres Found");
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                Debug.WriteLine($"Error loading genres: {ex.Message}");
                await DisplayAlert("Error", "An error occurred while loading genres.", "OK");
            }
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                string selectedGenre = genrePicker.SelectedItem as string;
                string selectedAuthor = authorPicker.SelectedItem as string;

                string[] authorNames = selectedAuthor?.Split(' ');
                if (authorNames?.Length == 1)
                {
                    // Extend the array to prevent index out of range issues
                    authorNames = new[] { authorNames[0], "" };
                }

                var filteredBooks = await App.Database.GetBooksBasedOnFiltersAsync(selectedGenre, authorNames);

                if (filteredBooks.Any())
                {
                    Random random = new Random();
                    var randomBook = filteredBooks[random.Next(filteredBooks.Count)];
                    await DisplayAlert("Filtered Book", $"Title: {randomBook.BookTitle}\nAuthor: {randomBook.AuthorLastName} {randomBook.AuthorFirstName}\nGenre: {randomBook.Genre}", "OK");
                }
                else
                {
                    await DisplayAlert("Error", "No books found with the specified filters.", "OK");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in Button_Clicked: {ex.Message}");
                await DisplayAlert("Error", "An error occurred while processing your request.", "OK");
            }
        }

        private void Button_Clicked1(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AddBookPage());
        }

        private void Button_Clicked2(object sender, EventArgs e)
        {
            Navigation.PushAsync(new RandomBook());
        }

        private async void Button_Clicked_AllBooks(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AllBooks());
        }

        private void Button_Clicked_Login(object sender, EventArgs e)
        {
            Navigation.PushAsync(new LoginPage());
        }
        async void Profile_Clicked(Object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ProfilePage());
        }
        async void Signout_Clicked(Object sender, EventArgs e)
        {
            App.IsUserLoggedIn = false;
            Navigation.InsertPageBefore(new LoginPage(), this);
            await Navigation.PopAsync();
        }
    }
}
