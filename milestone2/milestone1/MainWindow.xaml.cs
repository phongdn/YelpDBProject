using System;
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
        public class Business
        {
            public string name { get; set; }
            public string state { get; set; }
            public string city { get; set; }
            public string business_id { get; set; }

        }

        public class Reviews
        {
            public string text { get; set; }
            public double stars { get; set; }
        }
        public MainWindow()
        {
            InitializeComponent();
            addStates();
            //addCities(); //called in the StateList_SelectionChanged function
            cityList.IsEnabled = zipcodeList.IsEnabled = categoriesList.IsEnabled = addReviewButton.IsEnabled = false; //enable once a state has been chosen // Forces user to choose the previous option first

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
                            categoriesList.Items.Add(reader.GetString(0)); //add to cityList drop down box
                        }
                    }
                }
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
            col4.Header = "Business ID";
            col4.Binding = new Binding("business_id");
            businessGrid.Columns.Add(col4);

            //businessGrid.Items.Add(new Business() { name = "business1", state = "WA" });
            //businessGrid.Items.Add(new Business() { name = "business2", state = "CA" });
            //businessGrid.Items.Add(new Business() { name = "business3", state = "ID" });

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
            categoriesList.Items.Clear();
            businessGrid.Items.Clear(); //we don't want to append new list to current list
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
                            categoriesList.IsEnabled = true;
                            addCategories();
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

        private void CategoriesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            businessGrid.Items.Clear(); //we don't want to append new list to current list
            if (categoriesList.SelectedIndex > -1)
            {
                using (var conn = new NpgsqlConnection(buildConnString()))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "SELECT DISTINCT business_name,statecode,city,Business.business_id FROM Business, Categories WHERE city = '" + cityList.SelectedItem.ToString() + 
                            "' AND statecode = '" + stateList.SelectedItem.ToString() + "' AND postalcode = '" + zipcodeList.SelectedItem.ToString() + 
                            "' AND category_name ='" + categoriesList.SelectedItem.ToString() + "' AND Business.business_id = Categories.business_id;";
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
    }
}
