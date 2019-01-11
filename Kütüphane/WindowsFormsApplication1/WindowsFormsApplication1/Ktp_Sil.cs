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
    public partial class Ktp_Sil : Form
    {
        public SqlConnection baglanti;
        public int ID = 0;

        public Ktp_Sil()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string txt = textBox1.Text;
            SqlCommand cmd = new SqlCommand("", baglanti); ;
            baglanti = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["baglanti"].ConnectionString);//Bağlantılar oluştruuluyor
            if (baglanti.State == ConnectionState.Closed)
            {
                baglanti.Open();//Bağlantı açılıyor
            }
            
                    cmd = new SqlCommand("SELECT * FROM kitaplar WHERE kitap_adi LIKE @SEARCH ", baglanti); // SQL Sorgusu hazırlanıyor. Silincek kitabın nitelikleri doğrulama için kullanıcıya yansıtılıyor
                    cmd.Parameters.AddWithValue("@SEARCH", "%" + txt + "%"); 
                           
            using (SqlDataReader reader = cmd.ExecuteReader())//Veriler okunuyor
            {
                if (reader.Read())
                {
                    label3.Text = String.Format("{0}", reader["kitap_adi"]);
                    label6.Text = String.Format("{0}", reader["yazar"]);
                    label5.Text = String.Format("{0}", reader["tur"]);
                    label4.Text = String.Format("{0}", reader["basim_tarihi"]);
                    label7.Text = String.Format("{0}", reader["sayfa"]);
                    ID = Int32.Parse(String.Format("{0}", reader["ID"]));
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {// Silme işlemi başlatılıyor
            if (ID != 0)
            {
                baglanti = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["baglanti"].ConnectionString);
                baglanti.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM kitaplar WHERE ID = @id", baglanti); ;// SQL sorgusu oluşturuluyor buna göre kitabın ID'sine göre veritabanından siliniyor
                cmd.Parameters.AddWithValue("@id", ID);
                //Parametrelerimize Form üzerinde ki kontrollerden girilen verileri aktarıyoruz.
                cmd.ExecuteNonQuery();
                //Veritabanında değişiklik yapacak komut işlemi bu satırda gerçekleşiyor.
                baglanti.Close();
                MessageBox.Show("Başarıyla Sildiniz!");
                label3.Text = "";
                label5.Text = "";//Labellar olası yeni bir silme işlemi için temizleniyor.
                label6.Text = "";
                label4.Text = "";
                label7.Text = "";
            }
            else
            {
                MessageBox.Show("Lütfen önce güncellemek istediğiniz kayıdı seçiniz!");
            }
        }
    }
}
    
