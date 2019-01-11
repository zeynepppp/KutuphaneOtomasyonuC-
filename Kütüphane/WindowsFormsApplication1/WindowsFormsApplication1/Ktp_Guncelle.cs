using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
namespace WindowsFormsApplication1
{
    public partial class Ktp_Guncelle : Form
    {
        public SqlConnection baglanti;
        public int ID = 0;

        public Ktp_Guncelle()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (ID != 0) // Güncellenecek kayıdın seçili olup olmadığını kontrol ediyoruz
            {
                baglanti = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["baglanti"].ConnectionString); // Bağlantı oluşturuluyor
                baglanti.Open();//Bağlantı açılıyor
                SqlCommand cmd = new SqlCommand("", baglanti); ;
                string kayit = "UPDATE kitaplar SET kitap_adi=@kitap_adi, yazar=@yazar,basim_tarihi=@basim_tarihi, tur=@tur, sayfa=@sayfa WHERE ID=@ID";
                //Güncelleme yapılacak olan kitaplar tablosu için bir sorgu oluşturuluyor.
                SqlCommand komut = new SqlCommand(kayit, baglanti);
                //Sorgumuzu ve baglantimizi parametre olarak alan bir SqlCommand nesnesi oluşturuyoruz.
                komut.Parameters.AddWithValue("@kitap_adi", textBox5.Text); // Güncellenecen ögeler teker teker veritabanına ekleniyor
                komut.Parameters.AddWithValue("@yazar", textBox2.Text);
                komut.Parameters.AddWithValue("@basim_tarihi", textBox6.Text);
                komut.Parameters.AddWithValue("@tur", textBox3.Text);
                komut.Parameters.AddWithValue("@sayfa", textBox4.Text);
                komut.Parameters.AddWithValue("@ID", ID);
                //Parametrelerimize Form üzerinde ki kontrollerden girilen verileri aktarıyoruz.
                komut.ExecuteNonQuery();
                //Veritabanında değişiklik yapacak komut işlemi bu satırda gerçekleşiyor.
                baglanti.Close();
            }
            else
            {
                MessageBox.Show("Lütfen önce güncellemek istediğiniz kayıdı seçiniz!");
            }
            
        }

        private void button2_Click(object sender, EventArgs e)//Güncelleme yapılan fonksiyon
        {
            string txt = textBox1.Text; // Sorgu yapılacak stringi alıyoruz
            SqlCommand cmd = new SqlCommand("", baglanti); ;
            baglanti = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["baglanti"].ConnectionString); // Bağlantı oluşturuluyor
            if (baglanti.State == ConnectionState.Closed)
            {
                baglanti.Open();// Bağlantı açık mı diye kontrol ediliyor. Kapalı ise açılıyor.
            }

            cmd = new SqlCommand("SELECT * FROM kitaplar WHERE kitap_adi LIKE @SEARCH ", baglanti);//Arama sorgusu hazırlanıyor
            cmd.Parameters.AddWithValue("@SEARCH", txt); 

            using (SqlDataReader reader = cmd.ExecuteReader())// Veritabanından değerler okunuyor
            {
                if (reader.Read())
                {
                    textBox5.Text = String.Format("{0}", reader["kitap_adi"]);// Okunan değerler textboxlara ekleniyor.
                    textBox2.Text = String.Format("{0}", reader["yazar"]);   // böylece kullanıcı üzerinde değişiklik yapabiliyor.
                    textBox3.Text = String.Format("{0}", reader["tur"]);
                    textBox4.Text = String.Format("{0}", reader["basim_tarihi"]);
                    textBox6.Text = String.Format("{0}", reader["sayfa"]);
                    ID = Int32.Parse(String.Format("{0}", reader["ID"]));
                }
            }
        }
    }
}
