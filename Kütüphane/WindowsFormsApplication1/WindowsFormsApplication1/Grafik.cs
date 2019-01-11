using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZedGraph;
namespace WindowsFormsApplication1
{
    public partial class Grafik : Form
    {
        
        public SqlConnection baglanti;
        public int ID = 0;
        int count, count2;
        public Grafik()
        {
            InitializeComponent();


        }

        private void button1_Click(object sender, EventArgs e)
        {

            GraphPane myPane = zedGraphControl1.GraphPane;
            myPane.Title.Text = textBox1.Text;
            zedGraphControl1.Refresh();
            zedGraphControl1.GraphPane.GraphObjList.Clear();
        }

        private void Grafik_Load(object sender, EventArgs e)
        {
            SqlCommand cmd;
            baglanti = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["baglanti"].ConnectionString);
            // Bağlantı durumu kontrol edilip kapalı ise açılır:
            if (baglanti.State == ConnectionState.Closed)
            {
                baglanti.Open();
            }

            cmd = new SqlCommand("SELECT COUNT(*) FROM kitaplar", baglanti);
            count = (int)cmd.ExecuteScalar();

            cmd = new SqlCommand("SELECT COUNT(*) FROM kitaplar WHERE kullanan=@kllanan ", baglanti);
            cmd.Parameters.AddWithValue("@kllanan", 0);
            count2 = (int)cmd.ExecuteScalar();

            baglanti.Close();
            grafik_ciz(textBox1.Text, count, count2, count - count2);

        }

        public void grafik_ciz(String title, int x1, int x2, int x3)
        {

            GraphPane myPane = zedGraphControl1.GraphPane;
            string[] labels = { "Toplam Kitap Sayısı", "Kullanılan Kitaplar", "Kütüphanede Bulunan Kitaplar" };
            double[] y = { x1, x2, x3 };
            BarItem myBar = myPane.AddBar("Kitap Sayısı", null, y,
                                                        Color.Red);
            myBar.Bar.Fill = new Fill(Color.Red, Color.White,
                                                        Color.Red);
            myPane.XAxis.MajorTic.IsBetweenLabels = true;

            myPane.XAxis.Scale.TextLabels = labels;
            myPane.XAxis.Type = AxisType.Text;

            myPane.Chart.Fill = new Fill(Color.White,
                  Color.FromArgb(255, 255, 166), 90F);
            myPane.Title.IsVisible = true;
            myPane.Title.Text = "Grafik";
            myPane.Fill = new Fill(Color.FromArgb(250, 250, 255));
            zedGraphControl1.AxisChange();

        }

    }
}
