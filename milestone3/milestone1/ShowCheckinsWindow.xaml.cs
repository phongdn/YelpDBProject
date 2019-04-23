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
using System.Windows.Shapes;
using Npgsql;

namespace milestone1
{
    /// <summary>
    /// Interaction logic for ShowCheckinsWindow.xaml
    /// </summary>
    public partial class ShowCheckinsWindow : Window
    {
        public ShowCheckinsWindow()
        {
            InitializeComponent();
            ColumChart();
        }

        private void ColumChart()
        {
            string business_id_value = "dwQEZBFen2GdihLLfWeexA";
            // Need to get current business ID
            List<KeyValuePair<string, int>> myChartData = new List<KeyValuePair<string, int>>();
            using (var conn = new NpgsqlConnection("Host=localhost; Username=postgres; Password=password; Database = milestone2db"))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    // Retrieve all rows
                    cmd.CommandText = "select checkin_day, counts from Checkins where business_id='" + business_id_value + "' order by checkin_day";
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            myChartData.Add(new KeyValuePair<string, int>(reader.GetString(0), reader.GetInt32(1)));
                        }
                    }
                }
            }
            
            //MyChart.DataContext = myChartData;
        }
    }
}
