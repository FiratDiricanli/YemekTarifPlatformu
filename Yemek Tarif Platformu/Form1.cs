using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Yemek_Tarif_Platformu
{
    public class Malzeme
    {
        [DisplayName("Malzeme Adı")]
        public string malzeme_adi { get; set; }

        [DisplayName("Miktar / Ölçü")]
        public string miktar { get; set; }

        public override string ToString() => $"{miktar} {malzeme_adi}";
    }

    public class Tarif
    {
        [DisplayName("Tarif ID")]
        public int tarif_id { get; set; }

        [DisplayName("Tarif Adı")]
        public string tarif_adi { get; set; }

        [DisplayName("Kategori")]
        public string kategori { get; set; }

        [DisplayName("Hazırlama Süresi (Dk)")]
        public int hazirlama_suresi { get; set; }

        [DisplayName("Ortalama Puan")]
        public double ortalama_puan { get; set; } = 0;

        [Browsable(false)]
        public List<Malzeme> malzemeler { get; set; } = new List<Malzeme>();

        [Browsable(false)]
        public List<int> alinan_puanlar { get; set; } = new List<int>();

        public void tarif_ekle(List<Tarif> veritabani)
        {
            veritabani.Add(this);
        }

        public void tarif_guncelle(string yeniAd, string yeniKategori, int yeniSure)
        {
            this.tarif_adi = yeniAd;
            this.kategori = yeniKategori;
            this.hazirlama_suresi = yeniSure;
        }

        public override string ToString() => tarif_adi;
    }

    public class Kullanici
    {
        [DisplayName("Kullanıcı ID")]
        public int kullanici_id { get; set; }

        [DisplayName("Ad Soyad")]
        public string ad { get; set; }

        public Degerlendirme tarif_degerlendir(Tarif tarif, int puan)
        {
            tarif.alinan_puanlar.Add(puan);
            tarif.ortalama_puan = Math.Round(tarif.alinan_puanlar.Average(), 1);

            return new Degerlendirme
            {
                kullanici_adi = this.ad,
                tarif_adi = tarif.tarif_adi,
                verilen_puan = puan,
                tarih = DateTime.Now.ToShortDateString()
            };
        }

        public override string ToString() => ad;
    }

    public class Degerlendirme
    {
        [DisplayName("Kullanıcı")]
        public string kullanici_adi { get; set; }

        [DisplayName("Değerlendirilen Tarif")]
        public string tarif_adi { get; set; }

        [DisplayName("Puan (1-5)")]
        public int verilen_puan { get; set; }

        [DisplayName("Tarih")]
        public string tarih { get; set; }
    }

    public partial class Form1 : Form
    {
        List<Tarif> tarifler = new List<Tarif>();
        List<Kullanici> kullanicilar = new List<Kullanici>();
        List<Degerlendirme> degerlendirmeler = new List<Degerlendirme>();

        int tarifSayac = 111;

        TabControl sekmeler;
        TabPage sekmeTarif, sekmeMalzeme, sekmeDegerlendirme;
        DataGridView dgvTarifler, dgvMalzemeler, dgvDegerlendirmeler;
        ComboBox cmbTarifSec, cmbKullaniciSec, cmbPuan;
        TextBox txtTarifAd, txtKategori, txtSure, txtMalzemeAd, txtMiktar;

        public Form1()
        {
            this.Text = "Yemek Tarif Platformu - 2300005412 Fırat Diricanlı";
            this.Size = new Size(1150, 750);
            this.StartPosition = FormStartPosition.CenterScreen;

            SistemVerileriniHazirla();
            ArayuzuInsaEt();
        }

        private void SistemVerileriniHazirla()
        {
            kullanicilar.Add(new Kullanici { kullanici_id = 1, ad = "Fırat Diricanlı" });

            Tarif t1 = new Tarif { tarif_id = 101, tarif_adi = "Karnıyarık", kategori = "Ana Yemek", hazirlama_suresi = 45 };
            t1.malzemeler.Add(new Malzeme { malzeme_adi = "Patlıcan", miktar = "4 Adet" });
            t1.malzemeler.Add(new Malzeme { malzeme_adi = "Kıyma", miktar = "250 Gram" });
            t1.tarif_ekle(tarifler);

            Tarif t2 = new Tarif { tarif_id = 102, tarif_adi = "Mercimek Çorbası", kategori = "Çorba", hazirlama_suresi = 30 };
            t2.malzemeler.Add(new Malzeme { malzeme_adi = "Kırmızı Mercimek", miktar = "1 Su Bardağı" });
            t2.malzemeler.Add(new Malzeme { malzeme_adi = "Kuru Soğan", miktar = "1 Adet" });
            t2.tarif_ekle(tarifler);

            Tarif t3 = new Tarif { tarif_id = 103, tarif_adi = "İskender Kebap", kategori = "Ana Yemek", hazirlama_suresi = 40 };
            t3.malzemeler.Add(new Malzeme { malzeme_adi = "Döner Et", miktar = "300 Gram" });
            t3.malzemeler.Add(new Malzeme { malzeme_adi = "Tırnak Pide", miktar = "1 Adet" });
            t3.tarif_ekle(tarifler);

            Tarif t4 = new Tarif { tarif_id = 104, tarif_adi = "Kayseri Mantısı", kategori = "Ana Yemek", hazirlama_suresi = 60 };
            t4.malzemeler.Add(new Malzeme { malzeme_adi = "Mantı Hamuru", miktar = "500 Gram" });
            t4.malzemeler.Add(new Malzeme { malzeme_adi = "Sarımsaklı Yoğurt", miktar = "2 Su Bardağı" });
            t4.tarif_ekle(tarifler);

            Tarif t5 = new Tarif { tarif_id = 105, tarif_adi = "Fırın Sütlaç", kategori = "Tatlı", hazirlama_suresi = 40 };
            t5.malzemeler.Add(new Malzeme { malzeme_adi = "Süt", miktar = "1 Litre" });
            t5.malzemeler.Add(new Malzeme { malzeme_adi = "Pirinç", miktar = "1 Çay Bardağı" });
            t5.tarif_ekle(tarifler);

            Tarif t6 = new Tarif { tarif_id = 106, tarif_adi = "Zeytinyağlı Yaprak Sarma", kategori = "Zeytinyağlı", hazirlama_suresi = 90 };
            t6.malzemeler.Add(new Malzeme { malzeme_adi = "Asma Yaprağı", miktar = "300 Gram" });
            t6.malzemeler.Add(new Malzeme { malzeme_adi = "Pirinç", miktar = "1.5 Su Bardağı" });
            t6.tarif_ekle(tarifler);

            Tarif t7 = new Tarif { tarif_id = 107, tarif_adi = "Lahmacun", kategori = "Ana Yemek", hazirlama_suresi = 35 };
            t7.malzemeler.Add(new Malzeme { malzeme_adi = "Lahmacun İçi", miktar = "200 Gram" });
            t7.malzemeler.Add(new Malzeme { malzeme_adi = "Lahmacun Hamuru", miktar = "4 Adet Beze" });
            t7.tarif_ekle(tarifler);

            Tarif t8 = new Tarif { tarif_id = 108, tarif_adi = "Adana Kebap", kategori = "Ana Yemek", hazirlama_suresi = 30 };
            t8.malzemeler.Add(new Malzeme { malzeme_adi = "Kuzu Kıyma", miktar = "400 Gram" });
            t8.malzemeler.Add(new Malzeme { malzeme_adi = "Kuyruk Yağı", miktar = "50 Gram" });
            t8.tarif_ekle(tarifler);

            Tarif t9 = new Tarif { tarif_id = 109, tarif_adi = "Ev Baklavası", kategori = "Tatlı", hazirlama_suresi = 120 };
            t9.malzemeler.Add(new Malzeme { malzeme_adi = "Baklavalık Yufka", miktar = "1 Paket" });
            t9.malzemeler.Add(new Malzeme { malzeme_adi = "Ceviz İçi", miktar = "250 Gram" });
            t9.tarif_ekle(tarifler);

            Tarif t10 = new Tarif { tarif_id = 110, tarif_adi = "Şehriyeli Pirinç Pilavı", kategori = "Yan Yemek", hazirlama_suresi = 25 };
            t10.malzemeler.Add(new Malzeme { malzeme_adi = "Baldo Pirinç", miktar = "2 Su Bardağı" });
            t10.malzemeler.Add(new Malzeme { malzeme_adi = "Arpa Şehriye", miktar = "2 Yemek Kaşığı" });
            t10.tarif_ekle(tarifler);
        }

        private void ArayuzuInsaEt()
        {
            sekmeler = new TabControl { Dock = DockStyle.Fill, Font = new Font("Segoe UI", 10, FontStyle.Bold) };

            sekmeTarif = new TabPage("Tarif Yönetimi");
            sekmeMalzeme = new TabPage("Malzeme Ekleme");
            sekmeDegerlendirme = new TabPage("Değerlendirme ve Puanlama");

            Panel pnlTarif = new Panel { Dock = DockStyle.Top, Height = 90, BackColor = Color.WhiteSmoke };
            txtTarifAd = new TextBox { Location = new Point(20, 30), Width = 180, PlaceholderText = "Tarif Adı" };
            txtKategori = new TextBox { Location = new Point(210, 30), Width = 150, PlaceholderText = "Kategori (Tatlı, Çorba vb.)" };
            txtSure = new TextBox { Location = new Point(370, 30), Width = 120, PlaceholderText = "Süre (Dakika)" };

            Button btnTarifEkle = new Button { Text = "YENİ TARİF EKLE", Location = new Point(500, 28), Size = new Size(150, 32), BackColor = Color.SteelBlue, ForeColor = Color.White };
            btnTarifEkle.Click += (s, e) => TarifKaydet();

            Button btnTarifGuncelle = new Button { Text = "SEÇİLİ TARİFİ GÜNCELLE", Location = new Point(660, 28), Size = new Size(200, 32), BackColor = Color.DarkOrange, ForeColor = Color.White };
            btnTarifGuncelle.Click += (s, e) => TarifGuncellemeIslemi();

            dgvTarifler = TabloOlustur();
            pnlTarif.Controls.AddRange(new Control[] { txtTarifAd, txtKategori, txtSure, btnTarifEkle, btnTarifGuncelle });
            sekmeTarif.Controls.Add(dgvTarifler);
            sekmeTarif.Controls.Add(pnlTarif);

            Panel pnlMalzeme = new Panel { Dock = DockStyle.Top, Height = 90, BackColor = Color.WhiteSmoke };
            cmbTarifSec = new ComboBox { Location = new Point(20, 30), Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };
            cmbTarifSec.SelectedIndexChanged += SeciliTarifinMalzemeleriniGetir;

            txtMalzemeAd = new TextBox { Location = new Point(240, 30), Width = 180, PlaceholderText = "Malzeme Adı (Örn: Süt)" };
            txtMiktar = new TextBox { Location = new Point(430, 30), Width = 150, PlaceholderText = "Miktar (Örn: 2 Su Bardağı)" };

            Button btnMalzemeEkle = new Button { Text = "MALZEMEYİ TARİFE EKLE", Location = new Point(590, 28), Size = new Size(200, 32), BackColor = Color.SeaGreen, ForeColor = Color.White };
            btnMalzemeEkle.Click += (s, e) => MalzemeKaydet();

            dgvMalzemeler = TabloOlustur();
            pnlMalzeme.Controls.AddRange(new Control[] { cmbTarifSec, txtMalzemeAd, txtMiktar, btnMalzemeEkle });
            sekmeMalzeme.Controls.Add(dgvMalzemeler);
            sekmeMalzeme.Controls.Add(pnlMalzeme);

            Panel pnlDegerlendirme = new Panel { Dock = DockStyle.Top, Height = 100, BackColor = Color.WhiteSmoke };

            Label l1 = new Label { Text = "Kullanıcı:", Location = new Point(20, 20), AutoSize = true };
            cmbKullaniciSec = new ComboBox { Location = new Point(100, 18), Width = 180, DropDownStyle = ComboBoxStyle.DropDownList };

            Label l2 = new Label { Text = "Tarif:", Location = new Point(300, 20), AutoSize = true };
            ComboBox cmbTarifPuan = new ComboBox { Location = new Point(360, 18), Width = 180, DropDownStyle = ComboBoxStyle.DropDownList };

            Label l3 = new Label { Text = "Puan (1-5):", Location = new Point(560, 20), AutoSize = true };
            cmbPuan = new ComboBox { Location = new Point(660, 18), Width = 80, DropDownStyle = ComboBoxStyle.DropDownList };
            cmbPuan.Items.AddRange(new object[] { 1, 2, 3, 4, 5 });

            Button btnPuanVer = new Button { Text = "TARİFİ DEĞERLENDİR", Location = new Point(100, 55), Size = new Size(640, 35), BackColor = Color.Indigo, ForeColor = Color.White };
            btnPuanVer.Click += (s, e) => {
                if (cmbKullaniciSec.SelectedItem is Kullanici kul && cmbTarifPuan.SelectedItem is Tarif tar && cmbPuan.SelectedItem != null)
                {
                    Degerlendirme yeniKayit = kul.tarif_degerlendir(tar, (int)cmbPuan.SelectedItem);
                    degerlendirmeler.Add(yeniKayit);
                    TablolariGuncelle();
                    MessageBox.Show("Değerlendirme işleminiz başarıyla kaydedilmiştir.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            };

            dgvDegerlendirmeler = TabloOlustur();
            pnlDegerlendirme.Controls.AddRange(new Control[] { l1, cmbKullaniciSec, l2, cmbTarifPuan, l3, cmbPuan, btnPuanVer });
            sekmeDegerlendirme.Controls.Add(dgvDegerlendirmeler);
            sekmeDegerlendirme.Controls.Add(pnlDegerlendirme);

            sekmeler.TabPages.AddRange(new TabPage[] { sekmeTarif, sekmeMalzeme, sekmeDegerlendirme });
            this.Controls.Add(sekmeler);

            sekmeler.SelectedIndexChanged += (s, e) => {
                cmbTarifPuan.Items.Clear();
                foreach (var t in tarifler) cmbTarifPuan.Items.Add(t);
            };

            TablolariGuncelle();
        }

        private DataGridView TabloOlustur()
        {
            return new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Color.White,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AllowUserToAddRows = false,
                ReadOnly = true,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };
        }

        private void TablolariGuncelle()
        {
            dgvTarifler.DataSource = null; dgvTarifler.DataSource = tarifler.ToList();
            dgvDegerlendirmeler.DataSource = null; dgvDegerlendirmeler.DataSource = degerlendirmeler.ToList();

            cmbTarifSec.Items.Clear();
            cmbKullaniciSec.Items.Clear();

            foreach (var t in tarifler) cmbTarifSec.Items.Add(t);
            foreach (var k in kullanicilar) cmbKullaniciSec.Items.Add(k);
        }

        private void SeciliTarifinMalzemeleriniGetir(object sender, EventArgs e)
        {
            if (cmbTarifSec.SelectedItem is Tarif seciliTarif)
            {
                dgvMalzemeler.DataSource = null;
                dgvMalzemeler.DataSource = seciliTarif.malzemeler.ToList();
            }
        }

        private void TarifKaydet()
        {
            if (!string.IsNullOrWhiteSpace(txtTarifAd.Text) && int.TryParse(txtSure.Text, out int sure))
            {
                Tarif yeniTarif = new Tarif
                {
                    tarif_id = tarifSayac++,
                    tarif_adi = txtTarifAd.Text,
                    kategori = txtKategori.Text,
                    hazirlama_suresi = sure
                };

                yeniTarif.tarif_ekle(tarifler);

                txtTarifAd.Clear(); txtKategori.Clear(); txtSure.Clear();
                TablolariGuncelle();
                MessageBox.Show("Yeni tarif sisteme başarıyla eklenmiştir.", "İşlem Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Lütfen geçerli bir tarif adı ve sayısal bir hazırlama süresi giriniz.", "Hatalı Giriş", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void TarifGuncellemeIslemi()
        {
            if (dgvTarifler.SelectedRows.Count > 0)
            {
                var seciliTarif = dgvTarifler.SelectedRows[0].DataBoundItem as Tarif;
                if (seciliTarif != null && !string.IsNullOrWhiteSpace(txtTarifAd.Text) && int.TryParse(txtSure.Text, out int yeniSure))
                {
                    seciliTarif.tarif_guncelle(txtTarifAd.Text, txtKategori.Text, yeniSure);

                    txtTarifAd.Clear(); txtKategori.Clear(); txtSure.Clear();
                    TablolariGuncelle();
                    MessageBox.Show("Seçili tarif başarıyla güncellenmiştir.", "Güncelleme Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Güncelleme işlemi için geçerli bilgileri metin kutularına doldurunuz.", "Eksik Veri", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("Güncellenecek tarifi tablodan seçiniz.", "Seçim Yapılmadı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void MalzemeKaydet()
        {
            if (cmbTarifSec.SelectedItem is Tarif seciliTarif && !string.IsNullOrWhiteSpace(txtMalzemeAd.Text) && !string.IsNullOrWhiteSpace(txtMiktar.Text))
            {
                seciliTarif.malzemeler.Add(new Malzeme { malzeme_adi = txtMalzemeAd.Text, miktar = txtMiktar.Text });

                txtMalzemeAd.Clear(); txtMiktar.Clear();
                SeciliTarifinMalzemeleriniGetir(null, null);
                MessageBox.Show("Malzeme tarife başarıyla eklenmiştir.", "İşlem Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Lütfen geçerli bir tarif seçiniz ve malzeme bilgilerini eksiksiz doldurunuz.", "Eksik Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}