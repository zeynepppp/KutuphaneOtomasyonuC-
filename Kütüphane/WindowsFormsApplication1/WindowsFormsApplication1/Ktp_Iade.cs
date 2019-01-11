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
    public partial class Ktp_Iade : Form
    {
        int index = 0;
        int ktp_ID = 0;
        SqlConnection baglanti;


        public Ktp_Iade()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            baglanti = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["baglanti"].ConnectionString);// Bağlantılar oluşturuluyor

            string txt = textBox2.Text;
            SqlCommand cmd = new SqlCommand("", baglanti); ;

            if (baglanti.State == ConnectionState.Closed)// Eğer bağlantı kapalı ise açılıyor
            {
                baglanti.Open();
            }
            
            cmd = new SqlCommand("SELECT * FROM kitaplar WHERE kitap_adi LIKE @SEARCH ", baglanti); // Aranan kitabın bilgileri çekiliyor
            cmd.Parameters.AddWithValue("@SEARCH", txt );

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    ktp_ID = Int32.Parse(String.Format("{0}", reader["ID"]));//Girilen kitap adına göre kitap ID'si çekiliyor. Böylece hareketler tablosunda işlem yapılabilsin.
                    label1.Text += String.Format("{0}", reader["kitap_adi"]); //Okunan değerler tek tek okunarak labellara yazılıyor. Aranan değerin doğruluğu sağlanıyor
                    label2.Text += String.Format("{0}", reader["yazar"]);
                    label5.Text += String.Format("{0}", reader["tur"]);
                    label6.Text += String.Format("{0}", reader["basim_tarihi"]);
                    label7.Text += String.Format("{0}", reader["sayfa"]);
                }
            }
            
        }
        
        private void button1_Click(object sender, EventArgs e)
        { // Veritabanı işlemleri bu aşamada yapılıyor. 
            int borcu=0;
            int ogr_id = 0;
            string kayit;
            SqlCommand komut;
            baglanti = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["baglanti"].ConnectionString);// Bağlantı oluşturuluyor
            baglanti.Open();// Bağlantı açılıyor
            
            kayit = "UPDATE kitaplar SET kullanan=@kitap_adi WHERE ID=@ktp_ID";//Kitaplar tablosunda kitabı kullanan kişi hücresi güncelleniyor
            komut = new SqlCommand(kayit, baglanti);// Böylece yeni kişiler tarafından alınabilsin.
            komut.Parameters.AddWithValue("@kitap_adi", 0);
            komut.Parameters.AddWithValue("@ktp_ID", ktp_ID);
            komut.ExecuteNonQuery();


            kayit = "SELECT alis_tarihi, ogr_id FROM hareketler WHERE ktp_id=@ktp_ID AND veris_tarihi IS NULL";
            komut = new SqlCommand(kayit, baglanti);// Hareketler tablosundan kitabı kullanan öğrenci tespit ediliyor.
            komut.Parameters.AddWithValue("@ktp_ID", ktp_ID);
            komut.ExecuteNonQuery();
            using (SqlDataReader reader = komut.ExecuteReader())
            {
                if (reader.Read())
                {
                    ogr_id = Convert.ToInt32(reader["ogr_id"]);
                    TimeSpan ts = DateTime.Now - Convert.ToDateTime(reader["alis_tarihi"]); // Kitabın teslim tarihine göre borç hesaplanıyor         
                    borcu = ts.Days;
                    if (borcu > 15)// Eğer 15 günden fazla sürdüyse gecikme başına 1 tl borç ekleniyor
                    {
                        borcu = borcu - 15;
                    }
                    else
                    {
                        borcu = 0;
                    }
                }
            
            }


            kayit = "UPDATE hareketler SET veris_tarihi=@tarih  WHERE ktp_id=@ktp_ID AND veris_tarihi IS NULL";
            komut = new SqlCommand(kayit, baglanti);
            komut.Parameters.Add("@tarih", SqlDbType.Date).Value = DateTime.Now.ToString("yyyy-MM-dd");//hareketler tablosuna veris tarihi ekleniyor
            komut.Parameters.AddWithValue("@ktp_ID", ktp_ID);
            komut.ExecuteNonQuery();

            kayit = "UPDATE ogrenci SET borcu=@borcu WHERE ID=@ogr_ID";
            komut = new SqlCommand(kayit, baglanti);
            komut.Parameters.AddWithValue("@kitap_adi", 0);// Öğrencinin borcu tabloya işleniyor
            komut.Parameters.AddWithValue("@borcu", borcu);
            komut.Parameters.AddWithValue("@ogr_ID", ogr_id);
            komut.ExecuteNonQuery();


            baglanti.Close();
        }
    }
}
