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
    public partial class Ogr_Ekle : Form
    {

        SqlConnection baglanti;
        SqlCommand komut;
        SqlDataAdapter da;

        public Ogr_Ekle()
        {
            InitializeComponent();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                baglanti = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["baglanti"].ConnectionString);
               
                // Bağlantı açıldığında çalışacak sql sorgusu için cmd nesnesi oluşturulur:                  
                SqlCommand cmd = new SqlCommand("INSERT INTO ogrenci (adi, telefon, ogrenci_no, bolumu) VALUES (@adi,@telefon,@ogrenci_no,@bolumu)", baglanti);


                // TextBox'lardan alınan bilgiler etiketlere, oradan da sorguya gönderilir:
                cmd.Parameters.AddWithValue("@adi", textBox1.Text);
                cmd.Parameters.AddWithValue("@telefon", textBox2.Text);
                cmd.Parameters.AddWithValue("@ogrenci_no", textBox3.Text);
                cmd.Parameters.AddWithValue("@bolumu", textBox4.Text);


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


            //Bir yerde hata varsa catch ile yakalanır ve mesaj verilir:
           catch (SqlException)
            {
                MessageBox.Show("Hata Olustu!");
            }
        }

    }
}

