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
    public partial class Ogr_Rapor : Form
    {
        int index=0;
        int ogr_ID = 0;
        SqlConnection baglanti;
        public Ogr_Rapor()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {

            baglanti = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["baglanti"].ConnectionString); // Bağlantımız oluşturuluyor

            string txt = textBox2.Text; // Aranacak öğrencinin bilgilerini alıyoruz
            SqlCommand cmd = new SqlCommand("", baglanti); ;

            if (baglanti.State == ConnectionState.Closed)
            {
                baglanti.Open(); // Bağlantının açık olup olmadığı kontrol ediliyor. Eğer açık değil ise açılıyor.
            }

            switch (comboBox2.SelectedItem.ToString()) // Seçilen değere göre oluşturulacak SQL sorgusu için Switch-case yapısı kullanılıyor
            {
                case "Öğrenci Adı":
                    cmd = new SqlCommand("SELECT * FROM ogrenci WHERE adi=@SEARCH ", baglanti);
                    cmd.Parameters.AddWithValue("@SEARCH", txt); break;                           // Seçilen niteliğe göre SQL sorgusu oluşturuluyor.

                case "Öğrenci Numarası":
                    cmd = new SqlCommand("SELECT * FROM ogrenci WHERE ogrenci_no=@SEARCH ", baglanti);
                    cmd.Parameters.AddWithValue("@SEARCH", "%" + txt + "%"); break;

                case "Cep Telefonu":
                    cmd = new SqlCommand("SELECT * FROM ogrenci WHERE telefon=@SEARCH ", baglanti);
                    cmd.Parameters.AddWithValue("@SEARCH", "%" + txt + "%"); break;

                default: MessageBox.Show("Lütfen bir şık seçiniz!"); break;
            }

            using (SqlDataReader reader = cmd.ExecuteReader()) // SQLDataReader sorgusu ile değerler veri tabanından çekilmeye başlanıyor
            {
                if (reader.Read())
                {
                    label1.Text = String.Format("{0}", reader["adi"]);
                    label2.Text = String.Format("{0}", reader["telefon"]);
                    label5.Text = String.Format("{0}", reader["ogrenci_no"]); // Çektiğimiz öznitelikleri labellere yazıyoruz
                    label6.Text = String.Format("{0}", reader["bolumu"]);
                    label7.Text = String.Format("{0}", reader["borcu"]) + " TL";
                    ogr_ID = Int32.Parse(String.Format("{0}", reader["ID"]));

                }
            }


            // Bağlantı açıldığında çalışacak sql sorgusu için cmd nesnesi oluşturulur:

            cmd = new SqlCommand("SELECT kitaplar.ID, kitaplar.kitap_adi, kitaplar.yazar, hareketler.alis_tarihi, hareketler.veris_tarihi " +
            "FROM kitaplar " +
            "INNER JOIN hareketler " +
            "ON kitaplar.ID=hareketler.ktp_id WHERE ogr_id= @ogr_ID", baglanti); // Bu sorgunun amacı öğrencinin daha önce okuduğu ve şu anda da okumaya devam ettiği kitapları listelemektir
            cmd.Parameters.AddWithValue("@ogr_ID", ogr_ID);                         // Buna göre öğrenci tablosu ve hareketler tablosu SQL'in INNER JOIN  yöntemi ile tek bir tabloymuş gibi
            cmd.ExecuteNonQuery();                                               // işlem yapılmıştır ve ek sorguya gerek kalmadan tek sorgu ile işlem tamamlanmıştır.
            SqlDataAdapter adp = new SqlDataAdapter(cmd);

            DataTable dtable = new DataTable();                                 // DataGridView oluşturuyorlar

            adp.Fill(dtable);                                       // SQLden çekilen veriler datagridviewe ekleniyor

            dataGridView1.DataSource = dtable;
            foreach (DataGridViewRow row in dataGridView1.Rows)    // DataGridView'deki her bir satır, yani her bir kitap alma işlemi tek tek kontrol ediliyor
                                                                   // Buna göre boyama işlemleri yapılmakta
            {

                String alis = row.Cells[3].Value.ToString();  // Kitabın alındığı tarih
                DateTime dt = Convert.ToDateTime(alis);
                String veris = row.Cells[4].Value.ToString();  // Kitabın teslim edildiği tarih

                if (veris != "")
                {
                    DateTime vt = Convert.ToDateTime(veris);

                    dt = dt.AddDays(15);                           // Alis tarihine 15 gün ekliyoruz. Çünkü teslim tarihi max 15 gün olduğu varsayılıyor
                    if (DateTime.Compare(dt, vt) < 0 )   // Eğer kitap gecikmiş ise kırmızıya boyanıyor
                    {
                        row.DefaultCellStyle.BackColor = Color.Red;
                    }
                    else if (DateTime.Compare(vt, dt) <= 0) // Eğer zamanında getirilmiş ise yeşile boyanıyor
                    {
                        row.DefaultCellStyle.BackColor = Color.Green;
                    }
                }
                else if(DateTime.Compare(dt.AddDays(15), DateTime.Now) < 0)
                {
                    row.DefaultCellStyle.BackColor = Color.Red;
                }
                else if (DateTime.Compare(dt.AddDays(13), DateTime.Now)< 0)
                {
                    row.DefaultCellStyle.BackColor = Color.Yellow;
                }


                baglanti.Close();
            }
        }
        private void Ogr_Rapor_Load(object sender, EventArgs e)
        {

        }
    }
}
