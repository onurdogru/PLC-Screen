// Decompiled with JetBrains decompiler
// Type: EsdTurnikesi.Ayarlar
// Assembly: EsdTurnikesi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C8099926-BBEB-495E-ADF6-36B4F5F75BE8
// Assembly location: C:\Users\serkan.baki\Desktop\esd-rar\ESD\Release\EsdTurnikesi.exe

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.IO.Ports;
using System.Runtime.CompilerServices;

namespace PLCScreen
{
    [CompilerGenerated]
    [GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "14.0.0.0")]
    internal sealed class Prog_Ayarlar : ApplicationSettingsBase
    {
        private static Prog_Ayarlar defaultInstance = (Prog_Ayarlar)SettingsBase.Synchronized((SettingsBase)new Prog_Ayarlar());

        private void SettingChangingEventHandler(object sender, SettingChangingEventArgs e)
        {
        }

        private void SettingsSavingEventHandler(object sender, CancelEventArgs e)
        {
        }

        public static Prog_Ayarlar Default
        {
            get
            {
                return Prog_Ayarlar.defaultInstance;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("")]
        public string iniDosyaYolu
        {
            get
            {
                return (string)this[nameof(iniDosyaYolu)];
            }
            set
            {
                this[nameof(iniDosyaYolu)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("")]
        public string iniKameraOkuDosyaYolu
        {
            get
            {
                return (string)this[nameof(iniKameraOkuDosyaYolu)];
            }
            set
            {
                this[nameof(iniKameraOkuDosyaYolu)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("")]
        public string iniKameraYazDosyaYolu
        {
            get
            {
                return (string)this[nameof(iniKameraYazDosyaYolu)];
            }
            set
            {
                this[nameof(iniKameraYazDosyaYolu)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("")]
        public string txtSQLDosyaYolu
        {
            get
            {
                return (string)this[nameof(txtSQLDosyaYolu)];
            }
            set
            {
                this[nameof(txtSQLDosyaYolu)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("")]
        public string txtReceteDosyaYolu
        {
            get
            {
                return (string)this[nameof(txtReceteDosyaYolu)];
            }
            set
            {
                this[nameof(txtReceteDosyaYolu)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("")]
        public string printerName
        {
            get
            {
                return (string)this[nameof(printerName)];
            }
            set
            {
                this[nameof(printerName)] = (object)value;
            }
        }


        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("1299")]
        public string adminSifre
        {
            get
            {
                return (string)this[nameof(adminSifre)];
            }
            set
            {
                this[nameof(adminSifre)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("1453")]
        public string kaliteSifre
        {
            get
            {
                return (string)this[nameof(kaliteSifre)];
            }
            set
            {
                this[nameof(kaliteSifre)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("")]
        public string projectName
        {
            get
            {
                return (string)this[nameof(projectName)];
            }
            set
            {
                this[nameof(projectName)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("C:\\Users\\serkan.baki\\Desktop\\MIND-BATCH-FILES\\")]
        public string vektordosyayolu
        {
            get
            {
                return (string)this[nameof(vektordosyayolu)];
            }
            set
            {
                this[nameof(vektordosyayolu)] = (object)value;
            }
        }


        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("C:\\Users\\serkan.baki\\Desktop\\MIND-BATCH-FILES\\")]
        public string vektorErrordosyayolu
        {
            get
            {
                return (string)this[nameof(vektorErrordosyayolu)];
            }
            set
            {
                this[nameof(vektorErrordosyayolu)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("Kitaplıklar\\Belgeler\\logpersonel.accdb")]
        public string PNGdosyayolu
        {
            get
            {
                return (string)this[nameof(PNGdosyayolu)];
            }
            set
            {
                this[nameof(PNGdosyayolu)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("10000")]
        public int timerAdmin
        {
            get
            {
                return (int)this[nameof(timerAdmin)];
            }
            set
            {
                this[nameof(timerAdmin)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("20")]
        public int programingTimeout
        {
            get
            {
                return (int)this[nameof(programingTimeout)];
            }
            set
            {
                this[nameof(programingTimeout)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("20")]
        public int motorTimeout
        {
            get
            {
                return (int)this[nameof(motorTimeout)];
            }
            set
            {
                this[nameof(motorTimeout)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("20")]
        public int outputWriteTimeout
        {
            get
            {
                return (int)this[nameof(outputWriteTimeout)];
            }
            set
            {
                this[nameof(outputWriteTimeout)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("20")]
        public int kameraTimeout
        {
            get
            {
                return (int)this[nameof(kameraTimeout)];
            }
            set
            {
                this[nameof(kameraTimeout)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("true")]
        public bool chBoxSerial1
        {
            get
            {
                return (bool)this[nameof(chBoxSerial1)];
            }
            set
            {
                this[nameof(chBoxSerial1)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("COM8")]
        public string SerialPort1Com
        {
            get
            {
                return (string)this[nameof(SerialPort1Com)];
            }
            set
            {
                this[nameof(SerialPort1Com)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("9600")]
        public int SerialPort1Baud
        {
            get
            {
                return (int)this[nameof(SerialPort1Baud)];
            }
            set
            {
                this[nameof(SerialPort1Baud)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("8")]
        public int SerialPort1dataBits
        {
            get
            {
                return (int)this[nameof(SerialPort1dataBits)];
            }
            set
            {
                this[nameof(SerialPort1dataBits)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("One")]
        public StopBits SerialPort1stopBit
        {
            get
            {
                return (StopBits)this[nameof(SerialPort1stopBit)];
            }
            set
            {
                this[nameof(SerialPort1stopBit)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("None")]
        public Parity SerialPort1Parity
        {
            get
            {
                return (Parity)this[nameof(SerialPort1Parity)];
            }
            set
            {
                this[nameof(SerialPort1Parity)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("1.0")]
        public string verNo
        {
            get
            {
                return (string)this[nameof(verNo)];
            }
            set
            {
                this[nameof(verNo)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("167000")]
        public string productNo
        {
            get
            {
                return (string)this[nameof(productNo)];
            }
            set
            {
                this[nameof(productNo)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("1")]
        public string barcodeNum
        {
            get
            {
                return (string)this[nameof(barcodeNum)];
            }
            set
            {
                this[nameof(barcodeNum)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("1")]
        public string step1Job
        {
            get
            {
                return (string)this[nameof(step1Job)];
            }
            set
            {
                this[nameof(step1Job)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("1")]
        public string step2Job
        {
            get
            {
                return (string)this[nameof(step2Job)];
            }
            set
            {
                this[nameof(step2Job)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("1")]
        public string step3Job
        {
            get
            {
                return (string)this[nameof(step3Job)];
            }
            set
            {
                this[nameof(step3Job)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("1")]
        public string step4Job
        {
            get
            {
                return (string)this[nameof(step4Job)];
            }
            set
            {
                this[nameof(step4Job)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("1")]
        public string step5Job
        {
            get
            {
                return (string)this[nameof(step5Job)];
            }
            set
            {
                this[nameof(step5Job)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("1")]
        public string step6Job
        {
            get
            {
                return (string)this[nameof(step6Job)];
            }
            set
            {
                this[nameof(step6Job)] = (object)value;
            }
        }


        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("1")]
        public string step7Job
        {
            get
            {
                return (string)this[nameof(step7Job)];
            }
            set
            {
                this[nameof(step7Job)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("1")]
        public string step8Job
        {
            get
            {
                return (string)this[nameof(step8Job)];
            }
            set
            {
                this[nameof(step8Job)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("1")]
        public string step9Job
        {
            get
            {
                return (string)this[nameof(step9Job)];
            }
            set
            {
                this[nameof(step9Job)] = (object)value;
            }
        }


        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("0")]
        public string barcode1
        {
            get
            {
                return (string)this[nameof(barcode1)];
            }
            set
            {
                this[nameof(barcode1)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("0")]
        public string barcode2
        {
            get
            {
                return (string)this[nameof(barcode2)];
            }
            set
            {
                this[nameof(barcode2)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("0")]
        public string barcode3
        {
            get
            {
                return (string)this[nameof(barcode3)];
            }
            set
            {
                this[nameof(barcode3)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("0")]
        public string barcode4
        {
            get
            {
                return (string)this[nameof(barcode4)];
            }
            set
            {
                this[nameof(barcode4)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("0")]
        public string barcode5
        {
            get
            {
                return (string)this[nameof(barcode5)];
            }
            set
            {
                this[nameof(barcode5)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("0")]
        public string barcode6
        {
            get
            {
                return (string)this[nameof(barcode6)];
            }
            set
            {
                this[nameof(barcode6)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("0")]
        public string barcode7
        {
            get
            {
                return (string)this[nameof(barcode7)];
            }
            set
            {
                this[nameof(barcode7)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("0")]
        public string barcode8
        {
            get
            {
                return (string)this[nameof(barcode8)];
            }
            set
            {
                this[nameof(barcode8)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("0")]
        public string barcode9
        {
            get
            {
                return (string)this[nameof(barcode9)];
            }
            set
            {
                this[nameof(barcode9)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("0")]
        public string Sbarcode1
        {
            get
            {
                return (string)this[nameof(Sbarcode1)];
            }
            set
            {
                this[nameof(Sbarcode1)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("0")]
        public string Sbarcode2
        {
            get
            {
                return (string)this[nameof(Sbarcode2)];
            }
            set
            {
                this[nameof(Sbarcode2)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("0")]
        public string Sbarcode3
        {
            get
            {
                return (string)this[nameof(Sbarcode3)];
            }
            set
            {
                this[nameof(Sbarcode3)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("0")]
        public string Sbarcode4
        {
            get
            {
                return (string)this[nameof(Sbarcode4)];
            }
            set
            {
                this[nameof(Sbarcode4)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("0")]
        public string Sbarcode5
        {
            get
            {
                return (string)this[nameof(Sbarcode5)];
            }
            set
            {
                this[nameof(Sbarcode5)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("0")]
        public string Sbarcode6
        {
            get
            {
                return (string)this[nameof(Sbarcode6)];
            }
            set
            {
                this[nameof(Sbarcode6)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("0")]
        public string Sbarcode7
        {
            get
            {
                return (string)this[nameof(Sbarcode7)];
            }
            set
            {
                this[nameof(Sbarcode7)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("0")]
        public string Sbarcode8
        {
            get
            {
                return (string)this[nameof(Sbarcode8)];
            }
            set
            {
                this[nameof(Sbarcode8)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("0")]
        public string Sbarcode9
        {
            get
            {
                return (string)this[nameof(Sbarcode9)];
            }
            set
            {
                this[nameof(Sbarcode9)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("2,170,85")]
        public string feedback1
        {
            get
            {
                return (string)this[nameof(feedback1)];
            }
            set
            {
                this[nameof(feedback1)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("true")]
        public bool chBoxSuccess
        {
            get
            {
                return (bool)this[nameof(chBoxSuccess)];
            }
            set
            {
                this[nameof(chBoxSuccess)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("true")]
        public bool chBoxError1
        {
            get
            {
                return (bool)this[nameof(chBoxError1)];
            }
            set
            {
                this[nameof(chBoxError1)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("true")]
        public bool chBoxError2
        {
            get
            {
                return (bool)this[nameof(chBoxError2)];
            }
            set
            {
                this[nameof(chBoxError2)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("true")]
        public bool chBoxError3
        {
            get
            {
                return (bool)this[nameof(chBoxError3)];
            }
            set
            {
                this[nameof(chBoxError3)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("")]
        public string successBatch
        {
            get
            {
                return (string)this[nameof(successBatch)];
            }
            set
            {
                this[nameof(successBatch)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("")]
        public string error1Batch
        {
            get
            {
                return (string)this[nameof(error1Batch)];
            }
            set
            {
                this[nameof(error1Batch)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("")]
        public string error2Batch
        {
            get
            {
                return (string)this[nameof(error2Batch)];
            }
            set
            {
                this[nameof(error2Batch)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("")]
        public string error3Batch
        {
            get
            {
                return (string)this[nameof(error3Batch)];
            }
            set
            {
                this[nameof(error3Batch)] = (object)value;
            }
        }
    }
}
