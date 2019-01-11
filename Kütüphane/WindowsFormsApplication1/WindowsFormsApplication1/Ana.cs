using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Ana : Form
    {
        public Ana()
        {
            InitializeComponent(); // Ana ekranımız. Burada butonlara basıldığı anda ilgili butona bağlı olan formlar açılacaktır.
        }

        private void Ana_Load(object sender, EventArgs e)
        {
                                    
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Ogr_Ekle frm = new Ogr_Ekle();
            frm.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Ogr_Guncelle frm = new Ogr_Guncelle();
            frm.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Ogr_Sil frm = new Ogr_Sil();
            frm.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Ogr_Rapor frm = new Ogr_Rapor();
            frm.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Ktp_Ekle frm = new Ktp_Ekle();
            frm.Show();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Ktp_Guncelle frm = new Ktp_Guncelle();
            frm.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Ktp_Sil frm = new Ktp_Sil();
            frm.Show();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            Ktp_Rapor frm = new Ktp_Rapor();
            frm.Show();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Ktp_Al frm = new Ktp_Al();
            frm.Show();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            Ktp_Iade frm = new Ktp_Iade();
            frm.Show();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Grafik frm = new Grafik();
            frm.Show();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
