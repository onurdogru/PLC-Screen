// Decompiled with JetBrains decompiler
// Type: EsdTurnikesi.AyarForm
// Assembly: EsdTurnikesi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C8099926-BBEB-495E-ADF6-36B4F5F75BE8
// Assembly location: C:\Users\serkan.baki\Desktop\esd-rar\ESD\Release\EsdTurnikesi.exe

using System;
using System.ComponentModel;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Text;

namespace PLCScreen
{
    public class ProgAyarForm : Form
    {
        public FormMain MainFrm;
        private IContainer components;

        private Button btnKaydet;
        private TextBox txtKaliteSifre;
        private TextBox txtAdminSifre;
        private TextBox txtTimerAdmin;
        private TextBox projectName;
        private Label label29;
        private Button btnIDsec;
        private TextBox txtINIdosya;
        private Label label220;
        private Button btnOkuIni;
        private Button btnKaydetIni;
        private GroupBox groupBox2;
        private GroupBox groupBox6;
        private CheckBox sifreChange;
        private Label label90;
        private Label label91;
        private Label label92;
        private Label label93;
        private Label label10;
        private Button btnKameraOkuIDsec;
        private TextBox txtKameraOkuINIdosya;
        private Label label13;
        private Button btnKameraYazIDsec;
        private TextBox txtKameraYazINIdosya;
        private Label label18;
        private TextBox txtReceteDosyaYolu;
        private Button btnSQLOnOff;
        private JCS.ToggleSwitch toggleSwitch1;

        public ProgAyarForm()
        {
            this.InitializeComponent();
        }

        public class INIKaydet
        {
            [DllImport("kernel32")]
            private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

            [DllImport("kernel32")]
            private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

            public INIKaydet(string dosyaYolu)
            {
                DOSYAYOLU = dosyaYolu;
            }
            private string DOSYAYOLU = String.Empty;
            public string Varsayilan { get; set; }
            public string Oku(string bolum, string ayaradi)
            {
                Varsayilan = Varsayilan ?? string.Empty;
                StringBuilder StrBuild = new StringBuilder(256);
                GetPrivateProfileString(bolum, ayaradi, Varsayilan, StrBuild, 255, DOSYAYOLU);
                return StrBuild.ToString();
            }
            public long Yaz(string bolum, string ayaradi, string deger)
            {
                return WritePrivateProfileString(bolum, ayaradi, deger, DOSYAYOLU);
            }
        }

        private void AyarForm_Load(object sender, EventArgs e)
        {
            this.txtINIdosya.Text = Prog_Ayarlar.Default.iniDosyaYolu;
            this.txtKameraOkuINIdosya.Text = Prog_Ayarlar.Default.iniKameraOkuDosyaYolu;
            this.txtKameraYazINIdosya.Text = Prog_Ayarlar.Default.iniKameraYazDosyaYolu;
            this.projectName.Text = Prog_Ayarlar.Default.projectName;
            this.txtReceteDosyaYolu.Text = Prog_Ayarlar.Default.txtReceteDosyaYolu;
            this.txtAdminSifre.Text = Prog_Ayarlar.Default.adminSifre.ToString();
            this.txtKaliteSifre.Text = Prog_Ayarlar.Default.kaliteSifre.ToString();

            this.txtTimerAdmin.Text = Prog_Ayarlar.Default.timerAdmin.ToString();
        }


        private void btnKaydet_Click(object sender, EventArgs e)
        {
            try
            {
                Prog_Ayarlar.Default.iniDosyaYolu = this.txtINIdosya.Text;
                Prog_Ayarlar.Default.iniKameraOkuDosyaYolu = this.txtKameraOkuINIdosya.Text;
                Prog_Ayarlar.Default.iniKameraYazDosyaYolu = this.txtKameraYazINIdosya.Text;
                Prog_Ayarlar.Default.projectName = this.projectName.Text;
                Prog_Ayarlar.Default.txtReceteDosyaYolu = this.txtReceteDosyaYolu.Text;

                Prog_Ayarlar.Default.adminSifre = this.txtAdminSifre.Text;
                Prog_Ayarlar.Default.kaliteSifre = this.txtKaliteSifre.Text;

                Prog_Ayarlar.Default.timerAdmin = Convert.ToInt32(this.txtTimerAdmin.Text);

                Prog_Ayarlar.Default.Save();

                MessageBox.Show("Bütün Ayarlar Başarıyla Kaydedildi.");
                this.Close();

                Application.Restart();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ayarlar Kayıt Hatası: " + ex.Message);
            }
        }
        
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (this.sifreChange.Checked)
            {
                this.txtAdminSifre.Enabled = true;
                this.txtKaliteSifre.Enabled = true;
                this.txtAdminSifre.PasswordChar = char.MinValue;
                this.txtKaliteSifre.PasswordChar = char.MinValue;
            }
            else
            {
                this.txtAdminSifre.Enabled = false;
                this.txtKaliteSifre.Enabled = false;
                this.txtAdminSifre.PasswordChar = '*';
                this.txtKaliteSifre.PasswordChar = '*';
            }
        }

        private void btnKaydetIni_Click(object sender, EventArgs e)
        {
            if (txtINIdosya.Text != "")
            {
                INIKaydet ini = new INIKaydet(txtINIdosya.Text);  // @"\Ayarlar.ini"
                ini.Yaz("timerAdmin", "Metin Kutusu", Convert.ToString(txtTimerAdmin.Text));
               
                ini.Yaz("txtSQLOnOffDosya", "Metin Kutusu", Convert.ToString(txtReceteDosyaYolu.Text));
                ini.Yaz("projectName", "Metin Kutusu", Convert.ToString(projectName.Text));

                MessageBox.Show("Bütün Ayarlar Dosyaya Başarıyla Kaydedildi.");
            }
            else
            {
                MessageBox.Show("Dosya Yolu Boş Kalamaz");
            }
        }

        private void btnOkuIni_Click(object sender, EventArgs e)
        {
            if (txtINIdosya.Text != "")
            {
                try
                {
                    if (File.Exists(txtINIdosya.Text))
                    {
                        INIKaydet ini = new INIKaydet(txtINIdosya.Text);
                        txtTimerAdmin.Text = ini.Oku("timerAdmin", "Metin Kutusu");
                     
                        txtReceteDosyaYolu.Text = ini.Oku("txtSQLOnOffDosya", "Metin Kutusu");
                        projectName.Text = ini.Oku("projectName", "Metin Kutusu");
                      
                        MessageBox.Show("Bütün Ayarlar Dosyadan Başarıyla Okundu.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("ini Dosyası Hasarlı", ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Dosya Yolu Boş Kalamaz");
            }
        }

        private void btnIDsec_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "|*.ini";
            if (openFileDialog.ShowDialog() != DialogResult.OK)
                return;
            this.txtINIdosya.Text = openFileDialog.FileName;
        }

        private void btnKameraOkuIDsec_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "|*.ini";
            if (openFileDialog.ShowDialog() != DialogResult.OK)
                return;
            this.txtKameraOkuINIdosya.Text = openFileDialog.FileName;
        }

        private void btnKameraYazIDsec_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "|*.ini";
            if (openFileDialog.ShowDialog() != DialogResult.OK)
                return;
            this.txtKameraYazINIdosya.Text = openFileDialog.FileName;
        }

        private void btnSQLOnOff_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Database Dosyaları|*.txt|Tüm Dosyalar|*.*";
            if (openFileDialog.ShowDialog() != DialogResult.OK)
                return;
            this.txtReceteDosyaYolu.Text = openFileDialog.FileName;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
                this.components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProgAyarForm));
            this.btnKaydet = new System.Windows.Forms.Button();
            this.txtKaliteSifre = new System.Windows.Forms.TextBox();
            this.txtAdminSifre = new System.Windows.Forms.TextBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.sifreChange = new System.Windows.Forms.CheckBox();
            this.label90 = new System.Windows.Forms.Label();
            this.label91 = new System.Windows.Forms.Label();
            this.txtTimerAdmin = new System.Windows.Forms.TextBox();
            this.label92 = new System.Windows.Forms.Label();
            this.label93 = new System.Windows.Forms.Label();
            this.projectName = new System.Windows.Forms.TextBox();
            this.label29 = new System.Windows.Forms.Label();
            this.btnIDsec = new System.Windows.Forms.Button();
            this.txtINIdosya = new System.Windows.Forms.TextBox();
            this.label220 = new System.Windows.Forms.Label();
            this.btnOkuIni = new System.Windows.Forms.Button();
            this.btnKaydetIni = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label13 = new System.Windows.Forms.Label();
            this.btnKameraYazIDsec = new System.Windows.Forms.Button();
            this.txtKameraYazINIdosya = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.btnKameraOkuIDsec = new System.Windows.Forms.Button();
            this.txtKameraOkuINIdosya = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.txtReceteDosyaYolu = new System.Windows.Forms.TextBox();
            this.btnSQLOnOff = new System.Windows.Forms.Button();
            this.toggleSwitch1 = new JCS.ToggleSwitch();
            this.groupBox6.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnKaydet
            // 
            this.btnKaydet.BackColor = System.Drawing.Color.Aqua;
            this.btnKaydet.ForeColor = System.Drawing.Color.Black;
            this.btnKaydet.Location = new System.Drawing.Point(398, 177);
            this.btnKaydet.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnKaydet.Name = "btnKaydet";
            this.btnKaydet.Size = new System.Drawing.Size(203, 53);
            this.btnKaydet.TabIndex = 6;
            this.btnKaydet.Text = "Ayarları Kaydet";
            this.btnKaydet.UseVisualStyleBackColor = false;
            this.btnKaydet.Click += new System.EventHandler(this.btnKaydet_Click);
            // 
            // txtKaliteSifre
            // 
            this.txtKaliteSifre.Enabled = false;
            this.txtKaliteSifre.Location = new System.Drawing.Point(75, 82);
            this.txtKaliteSifre.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtKaliteSifre.Name = "txtKaliteSifre";
            this.txtKaliteSifre.PasswordChar = '*';
            this.txtKaliteSifre.Size = new System.Drawing.Size(89, 24);
            this.txtKaliteSifre.TabIndex = 0;
            // 
            // txtAdminSifre
            // 
            this.txtAdminSifre.Enabled = false;
            this.txtAdminSifre.Location = new System.Drawing.Point(75, 48);
            this.txtAdminSifre.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtAdminSifre.Name = "txtAdminSifre";
            this.txtAdminSifre.PasswordChar = '*';
            this.txtAdminSifre.Size = new System.Drawing.Size(89, 24);
            this.txtAdminSifre.TabIndex = 0;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.sifreChange);
            this.groupBox6.Controls.Add(this.label90);
            this.groupBox6.Controls.Add(this.txtAdminSifre);
            this.groupBox6.Controls.Add(this.txtKaliteSifre);
            this.groupBox6.Controls.Add(this.label91);
            this.groupBox6.Controls.Add(this.txtTimerAdmin);
            this.groupBox6.Controls.Add(this.label92);
            this.groupBox6.Controls.Add(this.label93);
            this.groupBox6.Location = new System.Drawing.Point(398, 12);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(203, 165);
            this.groupBox6.TabIndex = 11;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Şifre Ayarları:";
            // 
            // sifreChange
            // 
            this.sifreChange.AutoSize = true;
            this.sifreChange.Location = new System.Drawing.Point(9, 21);
            this.sifreChange.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.sifreChange.Name = "sifreChange";
            this.sifreChange.Size = new System.Drawing.Size(99, 21);
            this.sifreChange.TabIndex = 3;
            this.sifreChange.Text = "Şifre Değiştir";
            this.sifreChange.UseVisualStyleBackColor = true;
            this.sifreChange.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
            // 
            // label90
            // 
            this.label90.AutoSize = true;
            this.label90.Location = new System.Drawing.Point(-1, 55);
            this.label90.Name = "label90";
            this.label90.Size = new System.Drawing.Size(70, 17);
            this.label90.TabIndex = 1;
            this.label90.Text = "Adm. Şifre:";
            // 
            // label91
            // 
            this.label91.AutoSize = true;
            this.label91.Location = new System.Drawing.Point(-3, 85);
            this.label91.Name = "label91";
            this.label91.Size = new System.Drawing.Size(72, 17);
            this.label91.TabIndex = 1;
            this.label91.Text = "Kalite Şifre:";
            // 
            // txtTimerAdmin
            // 
            this.txtTimerAdmin.Location = new System.Drawing.Point(91, 115);
            this.txtTimerAdmin.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtTimerAdmin.Name = "txtTimerAdmin";
            this.txtTimerAdmin.Size = new System.Drawing.Size(68, 24);
            this.txtTimerAdmin.TabIndex = 25;
            // 
            // label92
            // 
            this.label92.AutoSize = true;
            this.label92.Location = new System.Drawing.Point(-1, 118);
            this.label92.Name = "label92";
            this.label92.Size = new System.Drawing.Size(61, 17);
            this.label92.TabIndex = 23;
            this.label92.Text = "T. Admin:";
            // 
            // label93
            // 
            this.label93.AutoSize = true;
            this.label93.Location = new System.Drawing.Point(165, 118);
            this.label93.Name = "label93";
            this.label93.Size = new System.Drawing.Size(25, 17);
            this.label93.TabIndex = 24;
            this.label93.Text = "mS";
            // 
            // projectName
            // 
            this.projectName.Location = new System.Drawing.Point(131, 91);
            this.projectName.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.projectName.Name = "projectName";
            this.projectName.Size = new System.Drawing.Size(167, 24);
            this.projectName.TabIndex = 62;
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Location = new System.Drawing.Point(36, 95);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(89, 17);
            this.label29.TabIndex = 61;
            this.label29.Text = "Project Name:";
            // 
            // btnIDsec
            // 
            this.btnIDsec.BackColor = System.Drawing.Color.Aqua;
            this.btnIDsec.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnIDsec.Location = new System.Drawing.Point(306, 23);
            this.btnIDsec.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnIDsec.Name = "btnIDsec";
            this.btnIDsec.Size = new System.Drawing.Size(65, 24);
            this.btnIDsec.TabIndex = 587;
            this.btnIDsec.Text = "Seç";
            this.btnIDsec.UseVisualStyleBackColor = false;
            this.btnIDsec.Click += new System.EventHandler(this.btnIDsec_Click);
            // 
            // txtINIdosya
            // 
            this.txtINIdosya.Location = new System.Drawing.Point(131, 22);
            this.txtINIdosya.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtINIdosya.Name = "txtINIdosya";
            this.txtINIdosya.Size = new System.Drawing.Size(167, 24);
            this.txtINIdosya.TabIndex = 586;
            // 
            // label220
            // 
            this.label220.AutoSize = true;
            this.label220.Location = new System.Drawing.Point(9, 22);
            this.label220.Name = "label220";
            this.label220.Size = new System.Drawing.Size(116, 17);
            this.label220.TabIndex = 585;
            this.label220.Text = "Ayarlar Dosya Yolu:";
            // 
            // btnOkuIni
            // 
            this.btnOkuIni.BackColor = System.Drawing.Color.Aqua;
            this.btnOkuIni.Location = new System.Drawing.Point(217, 55);
            this.btnOkuIni.Name = "btnOkuIni";
            this.btnOkuIni.Size = new System.Drawing.Size(80, 30);
            this.btnOkuIni.TabIndex = 584;
            this.btnOkuIni.Text = "Oku";
            this.btnOkuIni.UseVisualStyleBackColor = false;
            this.btnOkuIni.Click += new System.EventHandler(this.btnOkuIni_Click);
            // 
            // btnKaydetIni
            // 
            this.btnKaydetIni.BackColor = System.Drawing.Color.Aqua;
            this.btnKaydetIni.Location = new System.Drawing.Point(131, 55);
            this.btnKaydetIni.Name = "btnKaydetIni";
            this.btnKaydetIni.Size = new System.Drawing.Size(80, 30);
            this.btnKaydetIni.TabIndex = 583;
            this.btnKaydetIni.Text = "Kaydet";
            this.btnKaydetIni.UseVisualStyleBackColor = false;
            this.btnKaydetIni.Click += new System.EventHandler(this.btnKaydetIni_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label29);
            this.groupBox2.Controls.Add(this.projectName);
            this.groupBox2.Controls.Add(this.label18);
            this.groupBox2.Controls.Add(this.label13);
            this.groupBox2.Controls.Add(this.txtReceteDosyaYolu);
            this.groupBox2.Controls.Add(this.btnKameraYazIDsec);
            this.groupBox2.Controls.Add(this.btnSQLOnOff);
            this.groupBox2.Controls.Add(this.txtKameraYazINIdosya);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.btnKameraOkuIDsec);
            this.groupBox2.Controls.Add(this.txtKameraOkuINIdosya);
            this.groupBox2.Controls.Add(this.label220);
            this.groupBox2.Controls.Add(this.btnOkuIni);
            this.groupBox2.Controls.Add(this.btnIDsec);
            this.groupBox2.Controls.Add(this.btnKaydetIni);
            this.groupBox2.Controls.Add(this.txtINIdosya);
            this.groupBox2.Location = new System.Drawing.Point(3, 4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(389, 226);
            this.groupBox2.TabIndex = 588;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Ini Dosyası Ayarları:";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label13.Location = new System.Drawing.Point(10, 198);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(115, 13);
            this.label13.TabIndex = 591;
            this.label13.Text = "Kamera Dosya Yaz Yolu:";
            // 
            // btnKameraYazIDsec
            // 
            this.btnKameraYazIDsec.BackColor = System.Drawing.Color.Aqua;
            this.btnKameraYazIDsec.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnKameraYazIDsec.Location = new System.Drawing.Point(307, 194);
            this.btnKameraYazIDsec.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnKameraYazIDsec.Name = "btnKameraYazIDsec";
            this.btnKameraYazIDsec.Size = new System.Drawing.Size(65, 24);
            this.btnKameraYazIDsec.TabIndex = 593;
            this.btnKameraYazIDsec.Text = "Seç";
            this.btnKameraYazIDsec.UseVisualStyleBackColor = false;
            this.btnKameraYazIDsec.Click += new System.EventHandler(this.btnKameraYazIDsec_Click);
            // 
            // txtKameraYazINIdosya
            // 
            this.txtKameraYazINIdosya.Location = new System.Drawing.Point(131, 193);
            this.txtKameraYazINIdosya.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtKameraYazINIdosya.Name = "txtKameraYazINIdosya";
            this.txtKameraYazINIdosya.Size = new System.Drawing.Size(167, 24);
            this.txtKameraYazINIdosya.TabIndex = 592;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label10.Location = new System.Drawing.Point(9, 163);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(119, 13);
            this.label10.TabIndex = 588;
            this.label10.Text = "Kamera Dosya Oku Yolu:";
            // 
            // btnKameraOkuIDsec
            // 
            this.btnKameraOkuIDsec.BackColor = System.Drawing.Color.Aqua;
            this.btnKameraOkuIDsec.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnKameraOkuIDsec.Location = new System.Drawing.Point(306, 159);
            this.btnKameraOkuIDsec.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnKameraOkuIDsec.Name = "btnKameraOkuIDsec";
            this.btnKameraOkuIDsec.Size = new System.Drawing.Size(65, 24);
            this.btnKameraOkuIDsec.TabIndex = 590;
            this.btnKameraOkuIDsec.Text = "Seç";
            this.btnKameraOkuIDsec.UseVisualStyleBackColor = false;
            this.btnKameraOkuIDsec.Click += new System.EventHandler(this.btnKameraOkuIDsec_Click);
            // 
            // txtKameraOkuINIdosya
            // 
            this.txtKameraOkuINIdosya.Location = new System.Drawing.Point(131, 158);
            this.txtKameraOkuINIdosya.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtKameraOkuINIdosya.Name = "txtKameraOkuINIdosya";
            this.txtKameraOkuINIdosya.Size = new System.Drawing.Size(167, 24);
            this.txtKameraOkuINIdosya.TabIndex = 589;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label18.Location = new System.Drawing.Point(4, 129);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(124, 14);
            this.label18.TabIndex = 72;
            this.label18.Text = "Reçeteler Dosya Yolu:";
            // 
            // txtReceteDosyaYolu
            // 
            this.txtReceteDosyaYolu.Location = new System.Drawing.Point(131, 125);
            this.txtReceteDosyaYolu.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtReceteDosyaYolu.Name = "txtReceteDosyaYolu";
            this.txtReceteDosyaYolu.Size = new System.Drawing.Size(167, 24);
            this.txtReceteDosyaYolu.TabIndex = 73;
            // 
            // btnSQLOnOff
            // 
            this.btnSQLOnOff.BackColor = System.Drawing.Color.Aqua;
            this.btnSQLOnOff.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnSQLOnOff.Location = new System.Drawing.Point(306, 123);
            this.btnSQLOnOff.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnSQLOnOff.Name = "btnSQLOnOff";
            this.btnSQLOnOff.Size = new System.Drawing.Size(65, 24);
            this.btnSQLOnOff.TabIndex = 74;
            this.btnSQLOnOff.Text = "Seç";
            this.btnSQLOnOff.UseVisualStyleBackColor = false;
            this.btnSQLOnOff.Click += new System.EventHandler(this.btnSQLOnOff_Click);
            // 
            // toggleSwitch1
            // 
            this.toggleSwitch1.Location = new System.Drawing.Point(0, 0);
            this.toggleSwitch1.Name = "toggleSwitch1";
            this.toggleSwitch1.OffFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.toggleSwitch1.OnFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.toggleSwitch1.Size = new System.Drawing.Size(50, 19);
            this.toggleSwitch1.TabIndex = 0;
            // 
            // ProgAyarForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.ClientSize = new System.Drawing.Size(604, 234);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btnKaydet);
            this.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "ProgAyarForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Ayarlar";
            this.Load += new System.EventHandler(this.AyarForm_Load);
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }
    }
}
