﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Npgsql;

namespace milestone1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Business currentCell;
        private Categories currentSelectedCategory;
        public class Business
        {

            //SELECT DISTINCT business_name, address, city, statecode, postalcode, business_stars, review_rating, num_checkins
            public string name { get; set; }
            public string state { get; set; }
            public string city { get; set; }
            public string address { get; set; }
            public string zip { get; set; }
            public double stars { get; set; }
            public double rating { get; set; }
            public int checkins { get; set;}
            //public string business_id { get; set; }

        }

        public class Reviews
        {
            public string text { get; set; }
            public double stars { get; set; }
        }

        public class Categories
        {
            public string category_name { get; set; }
        }
        public MainWindow()
        {
            InitializeComponent();
            addStates();
            //addCities(); //called in the StateList_SelectionChanged function
            cityList.IsEnabled = zipcodeList.IsEnabled = categoriesGrid.IsEnabled = addReviewButton.IsEnabled = categoryAddBtn.IsEnabled =
                categoryRemoveBtn.IsEnabled = chosenCategoryGrid.IsEnabled = searchBusinessBtn.IsEnabled = false; //prevents user misuse

            addColumns2Grid(); //creates name,state,city columns
        }

        public void addStates()
        {
            //stateList.Items.Add("WA");
            //stateList.Items.Add("ID");
            //stateList.Items.Add("CA");

            using (var conn = new NpgsqlConnection(buildConnString()))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT DISTINCT statecode FROM Business ORDER BY statecode;"; //Queries list of all states
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read() )
                        {
                            stateList.Items.Add(reader.GetString(0)); //adds it to the drop down stateList box
                        }
                    }
                }
                conn.Close();
            }
        }

        public void addCities() //queries cities list based on chosen state
        {
            using (var conn = new NpgsqlConnection(buildConnString()))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT DISTINCT city FROM Business WHERE statecode = '" + stateList.SelectedItem.ToString() + "' ORDER BY city;";
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cityList.Items.Add(reader.GetString(0)); //add to cityList drop down box
                        }
                    }
                }
                conn.Close();
            }
        }

        public void addZipcodes() //queries cities list based on chosen city
        {
            using (var conn = new NpgsqlConnection(buildConnString()))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT DISTINCT postalcode FROM Business WHERE city = '" + cityList.SelectedItem.ToString() + "' ORDER BY postalcode;";
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            zipcodeList.Items.Add(reader.GetString(0)); //add to cityList drop down box
                        }
                    }
                }
                conn.Close();
            }
        }

        public void addCategories() //queries cities list based on chosen city
        {
            using (var conn = new NpgsqlConnection(buildConnString()))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT DISTINCT category_name FROM Categories, Business WHERE city = '" + cityList.SelectedItem.ToString() +
                            "' AND statecode = '" + stateList.SelectedItem.ToString() + "' AND postalcode = '" + zipcodeList.SelectedItem.ToString() + 
                            "' AND Business.business_id = Categories.business_id ORDER BY category_name;";
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            categoriesGrid.Items.Add(new Categories() { category_name = reader.GetString(0) }); //add to cityList drop down box
                        }
                    }
                }
                categoryAddBtn.IsEnabled = categoryRemoveBtn.IsEnabled = chosenCategoryGrid.IsEnabled = searchBusinessBtn.IsEnabled = true;
                conn.Close();
            }
        }


        private string buildConnString()
        {   //If using VM: Host=vmhost
            //Port=5432
            return "Host=localhost; Username=postgres; Password=password; Database = milestone2db"; //login info for database
        }

        public void addColumns2Grid() //creating columns
        {
            //Business Grid

            //SELECT DISTINCT business_name, address, city, statecode, postalcode, business_stars, review_rating, num_checkins

            DataGridTextColumn col1 = new DataGridTextColumn();
            col1.Header = "Business Name";
            col1.Binding = new Binding("name");
            col1.Width = 200;
            businessGrid.Columns.Add(col1);

            DataGridTextColumn col2 = new DataGridTextColumn();
            col2.Header = "State";
            col2.Binding = new Binding("state");
            businessGrid.Columns.Add(col2);

            DataGridTextColumn col3 = new DataGridTextColumn();
            col3.Header = "City";
            col3.Binding = new Binding("city");
            businessGrid.Columns.Add(col3);

            DataGridTextColumn col4 = new DataGridTextColumn();
            col4.Header = "Zipcode";
            col4.Binding = new Binding("zip");
            businessGrid.Columns.Add(col4);

            DataGridTextColumn col9 = new DataGridTextColumn(); //out of order
            col9.Header = "Stars";
            col9.Binding = new Binding("stars");
            businessGrid.Columns.Add(col9);

            DataGridTextColumn col10 = new DataGridTextColumn(); //out of order
            col10.Header = "Ratings";
            col10.Binding = new Binding("rating");
            businessGrid.Columns.Add(col10);

            DataGridTextColumn col11 = new DataGridTextColumn(); //out of order
            col11.Header = "# of Checkins";
            col11.Binding = new Binding("checkins");
            businessGrid.Columns.Add(col11);


            //Reviews Grid
            DataGridTextColumn col5 = new DataGridTextColumn();
            col5.Header = "Text";
            col5.Binding = new Binding("text");
            col5.Width = 100;
            reviewsGrid.Columns.Add(col5);

            DataGridTextColumn col6 = new DataGridTextColumn();
            col6.Header = "Stars";
            col6.Binding = new Binding("stars");
            col6.Width = 100;
            reviewsGrid.Columns.Add(col6);

            //categories Grid
            DataGridTextColumn col7 = new DataGridTextColumn();
            col7.Header = "Category_Name";
            col7.Binding = new Binding("category_name");
            col7.Width = 200;
            categoriesGrid.Columns.Add(col7);

            DataGridTextColumn col8 = new DataGridTextColumn();
            col8.Header = "Category_Name";
            col8.Binding = new Binding("category_name");
            col8.Width = 200;
            chosenCategoryGrid.Columns.Add(col8);
            chosenCategoryGrid.HeadersVisibility = DataGridHeadersVisibility.None; //hide headers since grid is too small to need it

        }

        private void StateList_SelectionChanged(object sender, SelectionChangedEventArgs e) //simply chooses a state, we don't want to list the names until a city is chosen
        {
            businessGrid.Items.Clear(); //empty list so that we don't append new list
            cityList.Items.Clear(); // empty city list since changing states will contain different list of cities
            zipcodeList.Items.Clear();
            if (stateList.SelectedIndex > -1)
            {
                using (var conn = new NpgsqlConnection(buildConnString()))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;
                        //cmd.CommandText = "SELECT DISTINCT name,state FROM business WHERE state = '" + stateList.SelectedItem.ToString() + "';";
                        using (var reader = cmd.ExecuteReader())
                        {
                            cityList.IsEnabled = true; //enable city selection button
                            addCities();
                            //while (reader.Read())
                            //{
                            //    businessGrid.Items.Add(new Business() { name = reader.GetString(0), state = reader.GetString(1) });
                            //}
                        }
                    }              
                    conn.Close();
                }
            }
        }

        private void CityList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {           
            //businessGrid.Items.Clear(); //we don't want to append new list to current list
            zipcodeList.Items.Clear();
            if (cityList.SelectedIndex > -1)
            {
                using (var conn = new NpgsqlConnection(buildConnString()))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;
                        //cmd.CommandText = "SELECT DISTINCT business_name,state,city FROM Business WHERE city = '" + cityList.SelectedItem.ToString() + "' AND state = '" + stateList.SelectedItem.ToString() + "';";
                        using (var reader = cmd.ExecuteReader())
                        {
                            zipcodeList.IsEnabled = true;
                            addZipcodes();
                            //while (reader.Read())
                            //{
                            //    businessGrid.Items.Add(new Business() { name = reader.GetString(0), state = reader.GetString(1), city = reader.GetString(2) });
                            //}
                        }
                    }
                    conn.Close();
                }
            }
        }

        private void ZipcodeList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            categoriesGrid.Items.Clear();
            //businessGrid.Items.Clear(); //we don't want to append new list to current list
            if (zipcodeList.SelectedIndex > -1)
            {
                using (var conn = new NpgsqlConnection(buildConnString()))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "SELECT DISTINCT business_name,statecode,city, business_id FROM Business WHERE city = '" + cityList.SelectedItem.ToString() + "' AND statecode = '" + stateList.SelectedItem.ToString() + "' AND postalcode = '" + zipcodeList.SelectedItem.ToString() + "';";
                        using (var reader = cmd.ExecuteReader())
                        {
                            categoriesGrid.IsEnabled = true;
                            addCategories();
                            //while (reader.Read())
                            //{
                            //    businessGrid.Items.Add(new Business() { name = reader.GetString(0), state = reader.GetString(1), city = reader.GetString(2), business_id = reader.GetString(3) });
                            //}
                        }
                    }
                    conn.Close();
                }
            }

        }

        //private void categoriesGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    businessGrid.Items.Clear(); //we don't want to append new list to current list
        //    if (categoriesGrid.SelectedIndex > -1)
        //    {
        //        using (var conn = new NpgsqlConnection(buildConnString()))
        //        {
        //            conn.Open();
        //            using (var cmd = new NpgsqlCommand())
        //            {
        //                cmd.Connection = conn;
        //                cmd.CommandText = "SELECT DISTINCT business_name,statecode,city,Business.business_id FROM Business, Categories WHERE city = '" + cityList.SelectedItem.ToString() + 
        //                    "' AND statecode = '" + stateList.SelectedItem.ToString() + "' AND postalcode = '" + zipcodeList.SelectedItem.ToString() + 
        //                    "' AND category_name ='" + categoriesGrid.SelectedItem.ToString() + "' AND Business.business_id = Categories.business_id;";
        //                using (var reader = cmd.ExecuteReader())
        //                {
        //                    while (reader.Read())
        //                    {
        //                        businessGrid.Items.Add(new Business() { name = reader.GetString(0), state = reader.GetString(1), city = reader.GetString(2), business_id = reader.GetString(3) });
        //                    }
        //                }
        //            }
        //            conn.Close();
        //        }
        //    }
        //}

        private void BusinessGrid_GotFocus(object sender, RoutedEventArgs e) //business selected
        {
            reviewsGrid.Items.Clear(); //prevent appending previous list
            currentCell = (Business)businessGrid.CurrentCell.Item;
            //MessageBox.Show("name = "+ temp.name+" city =  "+ temp.city + " statecode = " + temp.state);
            
            //Populate the list of reviews
            using (var conn = new NpgsqlConnection(buildConnString()))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT text, review_stars FROM Review, Business WHERE Review.business_id = Business.business_id AND Business.business_name = '" + currentCell.name + "';";
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            reviewsGrid.Items.Add(new Reviews() { text = reader.GetString(0), stars = reader.GetDouble(1)});
                        }
                    }
                    addReviewButton.IsEnabled = true;
                }
                conn.Close();
            }



        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void AddReviewButton_GotFocus(object sender, RoutedEventArgs e) //review selected
        {
            //MessageBox.Show("Alert");
            Form1 newReview = new Form1();
            newReview.updateBusinessID(currentCell.business_id);
            newReview.Show();      
        }

        private void ReviewsGrid_GotFocus(object sender, RoutedEventArgs e)
        {
            Reviews temp = (Reviews)reviewsGrid.CurrentCell.Item;
            MessageBox.Show("\t\t\tReview Description: \n\n" + temp.text + "\n\n\t\t\tStars: " + temp.stars.ToString());
        }

        private void CategoriesGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //not in use
        }


        private void CategoryAddBtn_Click(object sender, RoutedEventArgs e)
        {
            if(currentSelectedCategory != null)
            {
                chosenCategoryGrid.Items.Add(currentSelectedCategory);
            }
        }

        private void CategoriesGrid_GotFocus(object sender, RoutedEventArgs e)
        {
            currentSelectedCategory = (Categories)categoriesGrid.CurrentCell.Item;
        }

        private void CategoryRemoveBtn_Click(object sender, RoutedEventArgs e)
        {
            chosenCategoryGrid.Items.Remove(currentSelectedCategory);
        }

        private void ChosenCategoryGrid_GotFocus(object sender, RoutedEventArgs e)
        {
            currentSelectedCategory = (Categories)chosenCategoryGrid.CurrentCell.Item;
        }

        private void SearchBusinessBtn_Click(object sender, RoutedEventArgs e)
        {
            businessGrid.Items.Clear(); //removes appending of items for each search
            string baseCmd = "SELECT DISTINCT business_name,statecode,city,Business.business_id FROM Business, Categories WHERE city = '" + cityList.SelectedItem.ToString() +
                            "' AND statecode = '" + stateList.SelectedItem.ToString() + "' AND postalcode = '" + zipcodeList.SelectedItem.ToString() + "'"; 
            if (chosenCategoryGrid.Items.IsEmpty) //No chosen category
            {
                using (var conn = new NpgsqlConnection(buildConnString()))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = baseCmd; //Search without categories
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                businessGrid.Items.Add(new Business() { name = reader.GetString(0), state = reader.GetString(1), city = reader.GetString(2), business_id = reader.GetString(3) });
                            }
                        }
                    }
                    conn.Close();
                }
            }
            else //chosen categories, loop through entire list
            {

                //SQL Format: 

                //SELECT DISTINCT business_name, address, city, statecode, postalcode, business_stars, review_rating, num_checkins
                // FROM Business, Categories
                // WHERE
                //Business.business_id IN(SELECT business_id

                //    FROM Categories
                //   WHERE category_name = 'Coffee & Tea'
                //   AND Business.business_id
                //   IN (SELECT business_id

                //     FROM Categories
                //     WHERE category_name = 'Breakfast & Brunch'))
                //AND Business.business_id = Categories.business_id;


                string tempCmd = "";
                foreach (Categories item in chosenCategoryGrid.Items) //loop through all items in this grid
                {
                    tempCmd = tempCmd + " AND category_name = '" + item.category_name + "'";
                }
                tempCmd = tempCmd + " AND Business.business_id = Categories.business_id;";
                tempCmd = baseCmd + tempCmd;
                Console.WriteLine(tempCmd); //for debugging

                using (var conn = new NpgsqlConnection(buildConnString()))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = tempCmd; 
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                businessGrid.Items.Add(new Business() { name = reader.GetString(0), state = reader.GetString(1), city = reader.GetString(2), business_id = reader.GetString(3) });
                            }
                        }
                    }
                    conn.Close();
                }
            }
        }
    }
}