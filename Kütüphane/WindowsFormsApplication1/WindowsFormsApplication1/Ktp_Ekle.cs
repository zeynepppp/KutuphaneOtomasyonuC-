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
    public partial class Ktp_Ekle : Form
    {
        SqlConnection baglanti;
        SqlCommand komut;
        SqlDataAdapter da;
        public Ktp_Ekle()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                baglanti = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["baglanti"].ConnectionString);// Bağlantı oluşturuluyor

                // Bağlantı açıldığında çalışacak sql sorgusu için cmd nesnesi oluşturulur:                  
                SqlCommand cmd = new SqlCommand("INSERT INTO kitaplar (kitap_adi, yazar, basim_tarihi, tur, sayfa,kullanan) VALUES (@kitap_adi,@yazar,@basim_tarihi, @tur, @sayfa,@kullanan)", baglanti);


                // TextBox'lardan alınan bilgiler etiketlere, oradan da sorguya gönderilir:
                cmd.Parameters.AddWithValue("@kitap_adi", textBox1.Text);
                cmd.Parameters.AddWithValue("@yazar", textBox2.Text);
                cmd.Parameters.AddWithValue("@tur", textBox3.Text);
                cmd.Parameters.AddWithValue("@basim_tarihi", textBox4.Text);
                cmd.Parameters.AddWithValue("@sayfa",textBox5.Text);
                cmd.Parameters.AddWithValue("@kullanan", 0);
            //Bağlantı kapalı ise açılır:
            if (baglanti.State == ConnectionState.Closed)
                {
                    baglanti.Open();
                }


                // Sorgu çalıştırılır:
                cmd.ExecuteNonQuery();


                // Bağlantı kapatılır:
                baglanti.Close();


                // Eklendi mesajı gösterilir:
                MessageBox.Show("Eklendi.");
            }


           //  Bir yerde hata varsa catch ile yakalanır ve mesaj verilir:
            catch (SqlException)
            {
                MessageBox.Show("Hata Olustu!");
            }
        }
    }
}
