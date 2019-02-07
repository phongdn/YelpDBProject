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
        public class Business
        {
            public string name { get; set; }
            public string state { get; set; }
            public string city { get; set; }

        }
        public MainWindow()
        {
            InitializeComponent();
            addStates();
            //addCities();
            cityList.IsEnabled = false; //enable once a state has been chosen
            addColumns2Grid();
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
                    cmd.CommandText = "SELECT DISTINCT state FROM business ORDER BY state;";
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read() )
                        {
                            stateList.Items.Add(reader.GetString(0));
                        }
                    }
                }
                conn.Close();
            }
        }

        public void addCities()
        {
            using (var conn = new NpgsqlConnection(buildConnString()))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT DISTINCT city FROM business WHERE state = '" + stateList.SelectedItem.ToString() + "' ORDER BY city;";
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cityList.Items.Add(reader.GetString(0));
                        }
                    }
                }
                conn.Close();
            }
        }

        private string buildConnString()
        {   //If using VM: Host=vmhost
            //Port=5432
            return "Host=localhost; Username=postgres; Password=password; Database = milestone1db";
        }

        public void addColumns2Grid()
        {
            DataGridTextColumn col1 = new DataGridTextColumn();
            col1.Header = "Business Name";
            col1.Binding = new Binding("name");
            col1.Width = 255;
            businessGrid.Columns.Add(col1);

            DataGridTextColumn col2 = new DataGridTextColumn();
            col2.Header = "State";
            col2.Binding = new Binding("state");
            businessGrid.Columns.Add(col2);

            DataGridTextColumn col3 = new DataGridTextColumn();
            col3.Header = "City";
            col3.Binding = new Binding("city");
            businessGrid.Columns.Add(col3);

            //businessGrid.Items.Add(new Business() { name = "business1", state = "WA" });
            //businessGrid.Items.Add(new Business() { name = "business2", state = "CA" });
            //businessGrid.Items.Add(new Business() { name = "business3", state = "ID" });


        }

        private void StateList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            businessGrid.Items.Clear();
            cityList.Items.Clear(); // empty city list since changing states will contain different list of cities
            if (stateList.SelectedIndex > -1)
            {
               // if (cityList.SelectedIndex > -1)
               // {
                   // organize();
                    //return;
               // }
                using (var conn = new NpgsqlConnection(buildConnString()))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;
                        //cmd.CommandText = "SELECT DISTINCT name,state FROM business WHERE state = '" + stateList.SelectedItem.ToString() + "';";
                        using (var reader = cmd.ExecuteReader())
                        {
                            cityList.IsEnabled = true;
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
            businessGrid.Items.Clear();
            if (cityList.SelectedIndex > -1)
            {
                //if (stateList.SelectedIndex > -1)
                //{
                //   // organize();
                //    //return;
                //}
                using (var conn = new NpgsqlConnection(buildConnString()))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "SELECT DISTINCT name,state,city FROM business WHERE city = '" + cityList.SelectedItem.ToString() + "' AND state = '" + stateList.SelectedItem.ToString() + "';";
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                businessGrid.Items.Add(new Business() { name = reader.GetString(0), state = reader.GetString(1), city = reader.GetString(2) });
                            }
                        }
                    }
                    conn.Close();
                }
            }
        }
    }
}
