using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;

namespace milestone1
{
    public partial class Form1 : Form
    {
        private string business_id;
        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private string buildConnString()
        {   //If using VM: Host=vmhost
            //Port=5432
            return "Host=localhost; Username=postgres; Password=password; Database = milestone2db"; //login info for database
        }

        public void updateBusinessID(string bid)
        {
            business_id = bid;
            //MessageBox.Show(business_id);
        }

        private void button1_Click(object sender, EventArgs e) //submit button
        {
            Guid randomID = Guid.NewGuid(); //generate random ID
            string reviewID = randomID.ToString();
            reviewID = reviewID.Substring(0, 22); //reviewId has a length of 22

            //randomID = Guid.NewGuid();
            //string userID = randomID.ToString();
            //userID = userID.Substring(0, 22); //temporarily using this random userID for inserting a new review. It may change in the future when we choose a specific user
            string userID = "3L7nHnaeSHSdZXzV_vdvAQ";

            string today = DateTime.UtcNow.Date.ToString("yyyy/MM/dd"); // FORMAT: year/month/day
            today = today.Replace('/', '-');
            //MessageBox.Show(today);

            //MessageBox.Show(strID);
            //MessageBox.Show(DateTime.UtcNow.Date.ToString("yyyy/MM/dd")); // FORMAT: year/month/day

            string text = descriptionBox.Text;
            double stars = 0;
            if (double.TryParse(starsBox.Text, out stars))
            {
                using (var conn = new NpgsqlConnection(buildConnString()))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "INSERT INTO Review(review_id,user_id,business_id,review_stars,dates,text) VALUES" +
                            "('" + reviewID + "','" + userID + "','" + business_id + "','" + stars + "','" + today + "','" + text + "');";
                        using (var reader = cmd.ExecuteReader())
                        {
                        }
                    }
                    conn.Close();
                }
            }
            else
            {
                MessageBox.Show("You need to enter a number from 1 to 5 for stars!");
            }
        }
    }
}
