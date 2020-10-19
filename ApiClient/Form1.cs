using Newtonsoft.Json;
using System;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net.Http;
using System.Windows.Forms;

namespace ApiClient
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            DataSet dataSet;
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri("http://localhost:8083/");
                var response = await client.GetAsync("api/emp/1");
                var result = await response.Content.ReadAsStringAsync();
                dataSet = JsonConvert.DeserializeObject<DataSet>(result);
                pb.Image = GetImage(dataSet.Tables[0].Rows[0]["Photo"].ToString());
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private Image GetImage(string imageStr)
        {
            byte[] array = Convert.FromBase64String(imageStr);
            MemoryStream ms = new MemoryStream(array);
            return Image.FromStream(ms);//Exception occurs here
        }

        private byte[] ImageToByteArray(Image image)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, ImageFormat.Jpeg);
                byte[] imageBytes = ms.ToArray();
                return imageBytes;
            }
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            try
            {
                var client = new HttpClient();
                var content = JsonConvert.SerializeObject(new
                {
                    name = tbName.Text,
                    age = tbAge.Text,
                    address = tbAddress.Text,
                    phoneNo = tbPhoneNo.Text,
                    image = ImageToByteArray(pb.Image)
                });
                client.BaseAddress = new Uri("http://localhost:8083/");
                var response = await client.PostAsync("api/emp/", new StringContent(content, System.Text.Encoding.UTF8, "application/json"));
                var result = await response.Content.ReadAsStringAsync();
                MessageBox.Show(result);
                NullControls();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            lblimg.Visible = false;
            ofd.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp)|*.jpg; *.jpeg; *.gif; *.bmp";
            if (ofd.ShowDialog() == DialogResult.OK)  
                pb.Image = new Bitmap(ofd.FileName);
        }

        private void NullControls()
        {
            pb.Image = null;
            tbName.Text = "";
            tbAge.Text = "";
            tbAddress.Text = "";
            tbPhoneNo.Text = "";
        }


    }
}
