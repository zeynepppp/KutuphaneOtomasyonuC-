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
    public partial class Ogr_Guncelle : Form
    {
        public SqlConnection baglanti;
        public int ID = 0;

        public Ogr_Guncelle()
        {
            InitializeComponent();
        }
        
        private void button2_Click(object sender, EventArgs e)
        {
            string txt = textBox1.Text;
            SqlCommand cmd = new SqlCommand("", baglanti); ;
            baglanti = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["baglanti"].ConnectionString);// SQL Server'a bağlanmamız için gerekli ayarları yapıyoruz.
            if (baglanti.State == ConnectionState.Closed)// Bağlantının açık olup olmadığını kontorl ediyoruz
            {
                baglanti.Open();// Kapalıysa açıyoruz
            }

            switch (comboBox1.SelectedItem.ToString())  // Combobox'tan hangi nitelik ile arama yapılacağı seçiliyor. 
            {                                           // Buradan gelen sonuç bir switch case yapısı ile SQL sorgusu hazırlanıyor.
                case "Öğrenci Adı": cmd = new SqlCommand("SELECT * FROM ogrenci WHERE adi LIKE @SEARCH ", baglanti); 
                                    cmd.Parameters.AddWithValue("@SEARCH", "%" + txt + "%"); break;                  

                case "Öğrenci Numarası": cmd = new SqlCommand("SELECT * FROM ogrenci WHERE ogrenci_no LIKE @SEARCH ", baglanti);
                                         cmd.Parameters.AddWithValue("@SEARCH", "%"+ txt + "%"); break;

                case "Cep Telefonu": cmd = new SqlCommand("SELECT * FROM ogrenci WHERE telefon LIKE @SEARCH ", baglanti);
                                     cmd.Parameters.AddWithValue("@SEARCH", "%" + txt + "%"); break; 

                default:MessageBox.Show("Lütfen bir şık seçiniz!");break; // Eğer combobox seçilmedi ise uyarı veriyor.
            }

            using (SqlDataReader reader = cmd.ExecuteReader()) // SQL sorgumuzun sonucunda gelen dataları SQLDataReader objesi aracılığıyla okuyoruz
            {
                if (reader.Read())
                {
                    textBox5.Text = String.Format("{0}", reader["adi"]);// TextBoxlara bu değerleri tek tek atıyoruz. Textboxlara atamamızın sebebi önceki halini görüp üzerinde güncelleme yapılabilmesi
                    textBox2.Text = String.Format("{0}", reader["telefon"]);
                    textBox3.Text = String.Format("{0}", reader["ogrenci_no"]);
                    textBox4.Text = String.Format("{0}", reader["bolumu"]);
                    ID= Int32.Parse(String.Format("{0}", reader["ID"]));
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {// Güncelleme Yapılıyor 
            if (ID != 0) //Eğer kullanıcı daha önce seçilmediyse güncelleme yapılamayacağı için kullanıcıdan seçim yapması isteniliyor.
            {
                baglanti = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["baglanti"].ConnectionString); // SQL bağlantısı kuruluyor
                baglanti.Open();
                string kayit = "UPDATE ogrenci SET adi=@adi, telefon=@tlf,ogrenci_no=@ogr_no, bolumu=@bolumu WHERE ID=@ID";// SQL sorgusu hazırlanıyor
               
                SqlCommand komut = new SqlCommand(kayit, baglanti);
                //Sorgumuzu ve baglantimizi parametre olarak alan bir SqlCommand nesnesi oluşturuyoruz.
                komut.Parameters.AddWithValue("@adi", textBox5.Text); // Parametreler TextBox'tan alınarak SQL sorgusuna ekleniyor
                komut.Parameters.AddWithValue("@tlf", textBox2.Text);
                komut.Parameters.AddWithValue("@ogr_no", textBox3.Text);
                komut.Parameters.AddWithValue("@bolumu", textBox4.Text);
                komut.Parameters.AddWithValue("@ID", ID);
                komut.ExecuteNonQuery();
                MessageBox.Show("Kayıt güncellendi yapıldı");
                //Veritabanında değişiklik yapacak komut işlemi bu satırda gerçekleşiyor.
                baglanti.Close();
            }
            else
            {
                MessageBox.Show("Lütfen önce güncellemek istediğiniz kayıdı seçiniz!");
            }


        }
    }
}
