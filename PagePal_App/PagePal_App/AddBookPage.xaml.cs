﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PagePal_App
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddBookPage : ContentPage
    {
        public AddBookPage()
        {
            InitializeComponent();
        }

        BookTables.Books _bewks;
        public AddBookPage(BookTables.Books emp)
        {
            InitializeComponent();
            Title = "Edit Information";
            _bewks = emp;
            BookTitle.Text = emp.BookTitle;
            AuthorLastName.Text = emp.AuthorLastName;
            AuthorFirstName.Text = emp.AuthorFirstName;
            genreEntry.Text = emp.Genre;
            BookTitle.Focus();
        }

        private async void SaveButton_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(BookTitle.Text) ||
                string.IsNullOrEmpty(AuthorLastName.Text) ||
                string.IsNullOrEmpty(AuthorFirstName.Text) ||
                string.IsNullOrEmpty(genreEntry.Text))
            {
                await DisplayAlert("Error", "Please fill in all required fields.", "OK");
            }
            else if (_bewks != null)
            {
                UpdateBook();
            }
            else
            {
                var newBook = new BookTables.Books
                {
                    BookTitle = BookTitle.Text,
                    AuthorLastName = AuthorLastName.Text,
                    AuthorFirstName = AuthorFirstName.Text,
                    Genre = genreEntry.Text,
                };
                // Save the new book to the database using the SaveBookAsync method
                await App.Database.SaveBookAsync(newBook);

                // Display a success message
                await DisplayAlert("Success", "Book saved successfully!", "OK");

                // Clear all fields after successful save
                ClearFields();

                await Navigation.PopAsync();
            }
        }

        private void ClearFields()
        {
            BookTitle.Text = string.Empty;
            AuthorLastName.Text = string.Empty;
            AuthorFirstName.Text = string.Empty;
            genreEntry.Text = string.Empty;
        }

        async void UpdateBook()
        {
            _bewks.BookTitle = BookTitle.Text;
            _bewks.AuthorLastName = AuthorLastName.Text;
            _bewks.AuthorFirstName = AuthorFirstName.Text;
            _bewks.Genre = genreEntry.Text;
            await App.Database.UpdateBook(_bewks);
            await DisplayAlert("Success", "Book updated successfully!", "OK");
            await Navigation.PopAsync();
        }

    }
}