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
    public partial class Ktp_Al : Form
    {
        public int index1 = 0;
        public int index2 = 0;
        public SqlConnection baglanti;
        public int ogr_ID = 0;
        public int ktp_ID = 0;

        public Ktp_Al()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string txt = textBox1.Text;
            SqlCommand cmd = new SqlCommand("", baglanti);            
            baglanti = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["baglanti"].ConnectionString); // Bağlantılarımız oluşturuluyor
            if (baglanti.State == ConnectionState.Closed)
            {
                baglanti.Open(); // Bağlantının açık olup olmadığı kontrol ediliyor. Kapalı ise bağlantı açılıyor.
            }
            
            cmd = new SqlCommand("SELECT * FROM kitaplar WHERE kitap_adi LIKE @SEARCH ", baglanti);//Veritabanı sorgusu oluşturuluyor. Öğrenci 
            cmd.Parameters.AddWithValue("@SEARCH", "%" + txt + "%");  // Gerekli parametreler sorguya ekleniyor
            
            SqlDataAdapter adp = new SqlDataAdapter(cmd); // DataGridView dolduruluyor. Buna göre girilen sorguya göre çıkan kitaplar listeleniyor.
            DataTable dtable = new DataTable();             // Amaç kullanıcıya daha rahat sorgu yapma şansı vermek. 
                                                          // Yani Sefiller kitabını aramak isteyen kullanıcı sadece "efi", "sefil" gibi sonuçlar aratarak benzer sonuçları listeletebilir.
            adp.Fill(dtable); 
            baglanti.Close();                               // Datagridview dolduruluyor.
            dataGridView1.DataSource = dtable;
            baglanti.Close();

        }

        private void button2_Click(object sender, EventArgs e)  // Yukarıda kitaplar tablosunu doldurmuştuk burada ise öğrencilerin tablosunu aynı mantık ile dolduruyoruz
        {
            string txt = textBox1.Text; // Arama metnini giriyoruz
            SqlCommand cmd = new SqlCommand("", baglanti); ;
            baglanti = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["baglanti"].ConnectionString);// Bağlantılar yapılandırılıyor
            if (baglanti.State == ConnectionState.Closed)
            {
                baglanti.Open(); // Bağlantının açık oluğ olmadığı kontrol ediliyor eğer kapalı ise açılıyor
            }

            switch (comboBox2.SelectedItem.ToString())  // Girilen değere göre SQL Sorgusu oluşturuluyor.
            {
                case "Öğrenci Adı":
                    cmd = new SqlCommand("SELECT * FROM ogrenci WHERE adi LIKE @SEARCH ", baglanti);
                    cmd.Parameters.AddWithValue("@SEARCH", "%" + txt + "%"); break;

                case "Öğrenci Numarası":
                    cmd = new SqlCommand("SELECT * FROM ogrenci WHERE ogrenci_no LIKE @SEARCH ", baglanti);
                    cmd.Parameters.AddWithValue("@SEARCH", "%" + txt + "%"); break;

                case "Cep Telefonu":
                    cmd = new SqlCommand("SELECT * FROM ogrenci WHERE telefon LIKE @SEARCH ", baglanti);
                    cmd.Parameters.AddWithValue("@SEARCH", "%" + txt + "%"); break;

                default: MessageBox.Show("Lütfen bir şık seçiniz!"); break;
            }

            SqlDataAdapter adp = new SqlDataAdapter(cmd);
            DataTable dtable = new DataTable();  // İkinci Datagridview oluşturuluyor. Bunada arama sonucunda ortaya çıkan öğrenciler yazılıyor
            adp.Fill(dtable);                   //Yine yukarıdaki gibi eksik arama terimi girilse bile doğru bir sonuca ulaşılması sağlanıyor.
            baglanti.Close();
            dataGridView2.DataSource = dtable;
            baglanti.Close();


        }

        private void button1_Click(object sender, EventArgs e) // Bu fonksiyonda veritabanı ve transaction işlemleri gerçekleştirilmekte
        {
            DateTime dateTimeVariable = DateTime.Today; // Mevcut tarihi alıyoruz Kitabın hangi tarihte alındığını eklemek için
            SqlCommand cmd;
            DataGridViewRow selectedRow;
            // get the Row Index
            selectedRow = dataGridView1.Rows[index1]; //Seçili kitabın ID'sini alıyoruz. Çünkü veritabanındaki hareketler tablosunda bütün işlemler ID üzerinden yapılmakta
            ktp_ID=Int32.Parse(selectedRow.Cells[0].Value.ToString());//ID integera dönüştürülüyor.
            if (Int32.Parse(selectedRow.Cells[6].Value.ToString()) == 0) // 
            {
                selectedRow = dataGridView2.Rows[index2];  // Öğrencinin ID'sini alıyoruz
                ogr_ID = Int32.Parse(selectedRow.Cells[0].Value.ToString());

                baglanti = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["baglanti"].ConnectionString); // bağlantılar oluşturuluyor
                cmd = new SqlCommand("INSERT INTO hareketler(ogr_id,ktp_id,alis_tarihi) VALUES (@ogr,@ktp,@alis)", baglanti);
                // Hareketler tablosuna kitabı alan öğrenciyi, aldığı kitabı ve aldığı tarihi ekliyoruz
                cmd.Parameters.AddWithValue("@ogr", ogr_ID);
                cmd.Parameters.AddWithValue("@ktp", ktp_ID);
                cmd.Parameters.Add("@alis", SqlDbType.Date).Value = DateTime.Now.ToString("yyyy-MM-dd");
                if (baglanti.State == ConnectionState.Closed)
                {
                    baglanti.Open();
                }

                cmd.ExecuteNonQuery(); // Bağlantılar oluşturuluyor ve SQL sorgusu çalıştırılıyor


                
                string ktp = "UPDATE kitaplar SET kullanan=@ogr WHERE ID=@ID"; // Bu sorguda ise kitaplar listesinde kitabın kimde olduğu güncelleniyor böylece bir kitabı aynı anda birden fazla öğrenci alamıyor
                cmd = new SqlCommand(ktp, baglanti);
                cmd.Parameters.AddWithValue("@ID", ktp_ID);
                cmd.Parameters.AddWithValue("@ogr", ogr_ID);
                cmd.ExecuteNonQuery();

                baglanti.Close();

                MessageBox.Show("Eklendi.");
            }
            else
            {
                MessageBox.Show("Bu kitap zaten başka bir kullanıcıda!");
            }
        }
        
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            index1 = e.RowIndex;// Bu fonksiyonda kitaplar listesinde seçilen sıranın ID'sini alıp index1 global değişkenine atıyoruz ki heryerde kullanabilelim
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            index2 = e.RowIndex;// Bu fonksiyonda ogrenci listesinde seçilen sıranın ID'sini alıp index2 global değişkenine atıyoruz ki heryerde kullanabilelim
        }
    }
}
