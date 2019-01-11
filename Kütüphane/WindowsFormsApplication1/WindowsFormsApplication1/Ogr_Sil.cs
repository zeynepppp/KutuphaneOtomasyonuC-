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
    public partial class Ogr_Sil : Form
    {
        public SqlConnection baglanti;
        public int ID = 0;
        public Ogr_Sil()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e) // Bu fonksiyonun amacı Silinecek kullanıcıyı arayarak bulmak
        {
            string txt = textBox1.Text;
            SqlCommand cmd = new SqlCommand("", baglanti); ;
            baglanti = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["baglanti"].ConnectionString); // SQL bağlantısı yapılıyor
            if (baglanti.State == ConnectionState.Closed) // SQL bağlantısının açık olup olmadığına bakılıyor
            {
                baglanti.Open();                            // Eğer bağlantı kapalıysa açılıyor
            }

            switch (comboBox1.SelectedItem.ToString())// Combobox'tan yapılan seçim alınıyor
            {
                case "Öğrenci Adı":
                    cmd = new SqlCommand("SELECT * FROM ogrenci WHERE adi LIKE @SEARCH  ", baglanti); // SQL sorguları hazırlanıyor
                    cmd.Parameters.AddWithValue("@SEARCH", "%" + txt + "%"); break;

                case "Öğrenci Numarası":
                    cmd = new SqlCommand("SELECT * FROM ogrenci WHERE ogrenci_no LIKE @SEARCH  ", baglanti);
                    cmd.Parameters.AddWithValue("@SEARCH", "%" + txt + "%"); break;

                case "Cep Telefonu":
                    cmd = new SqlCommand("SELECT * FROM ogrenci WHERE telefon LIKE @SEARCH  ", baglanti);
                    cmd.Parameters.AddWithValue("@SEARCH", "%" + txt + "%"); break;

                 default: MessageBox.Show("Lütfen bir şık seçiniz!"); break;  // Eğer Seçim yapılmadıysa çekilmesini istiyor
            }

            using (SqlDataReader reader = cmd.ExecuteReader()) //SQLDataReader objesini kullanarak Silinecek öğrencinin niteliklerini çekiyoruz.
            {
                if (reader.Read())
                {
                    label3.Text = String.Format("{0}", reader["adi"]); // Çektiğimiz değerleri labellere yazdırıyoruz
                    label5.Text = String.Format("{0}", reader["telefon"]);
                    label6.Text = String.Format("{0}", reader["ogrenci_no"]);
                    label4.Text = String.Format("{0}", reader["bolumu"]);
                    ID = Int32.Parse(String.Format("{0}", reader["ID"]));

                }
            }

        }

        private void button1_Click(object sender, EventArgs e) // Sil butonuna tıklanınca bu fonksiyon devreye giriyor.
        {
            if (ID != 0)//Öğrencinin seçilip seçilmediği kontrol ediliyor.
            {
                baglanti = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["baglanti"].ConnectionString); // Bağlantılar oluşturuluyor
                baglanti.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM ogrenci WHERE ID = @id", baglanti); ; // SQL sorgusunda DELETE komutu ile siliyoruz.
                cmd.Parameters.AddWithValue("@id", ID);
                //Parametrelerimize Form üzerinde ki kontrollerden girilen verileri aktarıyoruz.
                cmd.ExecuteNonQuery();
                //Veritabanında değişiklik yapacak komut işlemi bu satırda gerçekleşiyor.
                baglanti.Close();
                MessageBox.Show("Başarıyla Sildiniz!");
                label3.Text = "";
                label5.Text = "";
                label6.Text = "";// Olası bir yeni işlem için Labelleri temizliyoruz
                label4.Text = "";
            }
            else
            {
                MessageBox.Show("Lütfen önce güncellemek istediğiniz kayıdı seçiniz!");
            }

        }
    }
}
