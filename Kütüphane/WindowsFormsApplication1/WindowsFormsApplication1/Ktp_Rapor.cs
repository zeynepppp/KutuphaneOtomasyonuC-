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

namespace WindowsFormsApplication1
{
    public partial class Ktp_Rapor : Form
    {
        int index = 0;
        int ktp_ID = 0;
        SqlConnection baglanti;

        public Ktp_Rapor()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            baglanti = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["baglanti"].ConnectionString);// Bağlantı oluşturuluyor

            string txt = textBox2.Text;
            SqlCommand cmd = new SqlCommand("", baglanti); 

            if (baglanti.State == ConnectionState.Closed)
            {
                baglanti.Open();// Bağlantı açılıyor
            }

            
            cmd = new SqlCommand("SELECT * FROM kitaplar WHERE kitap_adi LIKE @SEARCH ", baglanti); // Kitabın adına göre kitabın özellikleri veritabanından çekiliyor
            cmd.Parameters.AddWithValue("@SEARCH", txt ); 

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    ktp_ID = Int32.Parse(String.Format("{0}", reader["ID"]));// 
                    label1.Text = String.Format("{0}", reader["kitap_adi"]);
                    label2.Text = String.Format("{0}", reader["yazar"]);
                    label5.Text = String.Format("{0}", reader["tur"]);
                    label6.Text = String.Format("{0}", reader["basim_tarihi"]);
                    label7.Text = String.Format("{0}", reader["sayfa"]);
                }
            }


            cmd = new SqlCommand("SELECT ogrenci.adi, ogrenci.ogrenci_no, hareketler.alis_tarihi, hareketler.veris_tarihi " +
            "FROM ogrenci " +   //// Bu sorgunun amacı kitabı daha önce okuyan öğrencileri listelemektir. 
            "INNER JOIN hareketler " +// SQL'in INNERJOIN özelliği kullanılarak iki tablo tek bir tablo imiş gibi kullanılarak veri istenen veriler çekilmiştir
            "ON ogrenci.ID=hareketler.ogr_id WHERE ktp_id= @ktp_ID", baglanti);
            cmd.Parameters.AddWithValue("@ktp_ID", ktp_ID);  
            cmd.ExecuteNonQuery();
            SqlDataAdapter adp = new SqlDataAdapter(cmd);

            DataTable dtable = new DataTable(); // DataGridView güncelleniyor.

            adp.Fill(dtable);

            dataGridView1.DataSource = dtable;


        }

        private void Ktp_Rapor_Load(object sender, EventArgs e)
        {

        }
    }
    }

