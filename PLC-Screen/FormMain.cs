using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using PLCScreen.Printer;
using System.Printing;
using System.Net;
using System.Net.Mail;
using JCS;
using System.Data.OleDb;
using OfficeOpenXml;
using System.IO;
using System.Runtime;
using OfficeOpenXml.Table;
using System.Globalization;

namespace PLCScreen
{
    public partial class FormMain : Form
    {
        public ProgAyarForm ProgAyarFrm;
        public Sifre SifreFrm;

        string customMessageBoxTitle;
        int adminTimerCounter = 0;
        int loopTimerCounter = 0;

        public int yetki;

        string cameraState = "";
        string oldCameraState = "";
        string ethernetState = "";
        string oldEthernetState = "";
        string objectOK = "";
        string objectNOK = "";
        string processDone = "";
        string snapShot1 = "";
        string snapShot2 = "";
        string snapShot3 = "";
        string snapShot4 = "";
        string snapShot5 = "";
        string snapShot6 = "";
        string snapShot7 = "";
        string snapShot8 = "";
        string snapShot9 = "";
        string snapShot10 = "";
        string snapShot11 = "";
        string snapShot12 = "";

        int cameraTimerCounter = 0;
        bool cameraTimeoutState = false;
        int cameraProcessCounter = 0;
        bool processControlState = true;
        int cameraStateErrorCounter = 0;
        int ethernetStateErrorCounter = 0;

        bool onceWrite = false;
        public string receteName = "";
        private Thread threadLoopManager = null;
        bool newObject = false;
        bool oldNewObject = false;
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
        public FormMain()
        {
            this.ProgAyarFrm = new ProgAyarForm();
            this.ProgAyarFrm.MainFrm = this;
            this.SifreFrm = new Sifre();
            this.SifreFrm.MainFrm = this;
            InitializeComponent();
        }

        [DllImport("user32.dll")]
        public static extern byte ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string ClassName, string WindowName);

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            nxCompoletBoolWrite("connectionStart", false);
            nxCompoletBoolWrite("cameraState", false);
            nxCompoletBoolWrite("ethernetState", false);
            closePythonBatchFile();
            loopTimer.Stop();
            loopTimer.Enabled = false;
        }

        public void runPythonBatchFile()
        {
            Process.Start(@"C:\\Users\\USER\\Desktop\\pcb-component-inspection-master-Windows\\abra_kadabra_s.py");
            Process.Start(@"C:\\Users\\USER\\Desktop\\pcb-component-inspection-master-Windows\\abra_kadabra_r.py");
        }

        public void closePythonBatchFile()
        {
            Process.Start(@"C:\\Users\\USER\\Desktop\\pcb-component-inspection-master-Windows\\PYTHONEXIT.bat");
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            runPythonBatchFile();
            formMainInit();

            nxCompoletBoolWrite("connectionStart", true);
            if (nxCompoletBoolRead("connectionStart"))
            {
                lblStatusCom2.Text = "ON";
                lblStatusCom2.BackColor = Color.Green;
            }
            else
            {
                lblStatusCom2.Text = "OFF";
                lblStatusCom2.BackColor = Color.Red;
            }
            nxCompoletStringWrite("cycleTime", "0");
            Control.CheckForIllegalCrossThreadCalls = false;
            threadLoopManager = new Thread(loopFunc);
            threadLoopManager.Start();
            //   loopTimer.Start();
        }

        private void formMainInit()     //FORM INIT
        {
            this.customMessageBoxTitle = Prog_Ayarlar.Default.projectName;
            this.projectNameTxt.Text = customMessageBoxTitle;
            this.projectNameTxt_2.Text = customMessageBoxTitle;
            this.Text = customMessageBoxTitle;

            this.timerAdmin.Interval = Prog_Ayarlar.Default.timerAdmin;
            this.yetki = 0;
            this.yetkidegistir();

            autoSwitch.Style = JCS.ToggleSwitch.ToggleSwitchStyle.Fancy;  //IOS
            autoSwitch.Size = new Size(98, 42);
            autoSwitch.OnText = "ON";
            autoSwitch.OnForeColor = Color.White;
            autoSwitch.OffText = "OFF";
            autoSwitch.OffForeColor = Color.FromArgb(141, 123, 141);

            bakimSwitch.Style = JCS.ToggleSwitch.ToggleSwitchStyle.Fancy;  //IOS
            bakimSwitch.Size = new Size(98, 42);
            bakimSwitch.OnText = "ON";
            bakimSwitch.OnForeColor = Color.White;
            bakimSwitch.OffText = "OFF";
            bakimSwitch.OffForeColor = Color.FromArgb(141, 123, 141);

            acilStopSwitch.Style = JCS.ToggleSwitch.ToggleSwitchStyle.Fancy;  //IOS
            acilStopSwitch.Size = new Size(98, 42);
            acilStopSwitch.OnText = "ON";
            acilStopSwitch.OnForeColor = Color.White;
            acilStopSwitch.OffText = "OFF";
            acilStopSwitch.OffForeColor = Color.FromArgb(141, 123, 141);
            sliderLabel.Text = Convert.ToString(motorSliderValue.Value);

            nxCompoletStringWrite("motorValue", "30");
        }

        #region Events
        /****************************************** Compolet Fonksiyon *************************************************/
        public bool nxCompoletBoolWrite(string variable, bool value)  //NX WRITE
        {
            try
            {
                nxCompolet1.WriteVariable(variable, value);
                return true;
            }
            catch
            {
                otherConsoleAppendLine("nxCompolet Hatası" + "\nKonum : BoolWrite" + "\nvariable = " + variable + "\nstate = " + value, Color.Red);
                return false;
            }
        }

        public bool nxCompoletBoolRead(string variable)  //NX READ
        {
            try
            {
                return Convert.ToBoolean(nxCompolet1.ReadVariable(variable));
            }
            catch
            {
                otherConsoleAppendLine("nxCompolet Hatası" + "\nKonum : BoolRead" + "\nvariable = " + variable, Color.Red);
                return false;
            }
        }

        public string nxCompoletStringRead(string variable)  //NX STRING
        {
            try
            {
                return Convert.ToString(nxCompolet1.ReadVariable(variable));
            }
            catch
            {
                otherConsoleAppendLine("nxCompolet Hatası" + "\nKonum : StringRead" + "\nvariable = " + variable, Color.Red);
                return "error";
            }
        }

        public double nxCompoletDoubleRead(string variable)  //NX STRING
        {
            try
            {
                return Convert.ToDouble(nxCompolet1.ReadVariable(variable));
            }
            catch
            {
                otherConsoleAppendLine("nxCompolet Hatası" + "\nKonum : DoubleRead" + "\nvariable = " + variable, Color.Red);
                return 0;
            }
        }

        public bool nxCompoletStringWrite(string variable, string value)  //NX STRING
        {
            try
            {
                nxCompolet1.WriteVariable(variable, value);
                return true;
            }
            catch
            {
                otherConsoleAppendLine("nxCompolet Hatası" + "\nKonum : StringWrite" + "\nvariable = " + variable, Color.Red);
                return false;
            }
        }

        /****************************************** LOOP *************************************************/
        private void loopTimer_Tick(object sender, EventArgs e)  //LOOP TIMER
        {
            loopFunc();
        }

        int loopCounter = 0;
        public void loopFunc()   //LOOP
        {
            for (;;)
            {
                if (threadLoopManager != null)
                {
                    loopCounter++;
                    if (loopCounter == 10)
                    {
                        loopCounter = 0;
                    }
                    loopCounterT.Invoke(new Action(delegate ()
                    {
                        loopCounterT.Text = Convert.ToString(loopCounter);
                    }));
                    try
                    {
                        cameraStation();
                        emergencyLoop();
                        loopPLCScreen();
                        systemReadyLoop();
                    }
                    catch (Exception ex)
                    {
                        otherConsoleAppendLine("loopFunc Hatası" + ex.Message, Color.Red);
                    }
                    System.Threading.Thread.Sleep(200);
                }
            }
        }

        public void loopPLCScreen()
        {
            try
            {
                if (nxCompoletBoolRead("emergencyStop"))
                {
                    if (this.IsHandleCreated)
                    {
                        acilStopSwitch.Invoke(new Action(delegate ()
                        {
                            acilStopSwitch.Checked = true;
                        }));
                    }
                    if (this.IsHandleCreated)
                    {
                        bakimSwitch.Invoke(new Action(delegate ()
                        {
                            bakimSwitch.Checked = false;
                        }));
                    }
                    if (this.IsHandleCreated)
                    {
                        autoSwitch.Invoke(new Action(delegate ()
                        {
                            autoSwitch.Checked = false;
                        }));
                    }
                }

                else
                {
                    if (this.IsHandleCreated)
                    {
                        acilStopSwitch.Invoke(new Action(delegate ()
                        {
                            acilStopSwitch.Checked = false;
                        }));
                    }
                    if (nxCompoletBoolRead("bakimMod"))
                    {
                        if (this.IsHandleCreated)
                        {
                            bakimSwitch.Invoke(new Action(delegate ()
                            {
                                bakimSwitch.Checked = true;
                            }));
                        }
                    }
                    else
                    {
                        if (this.IsHandleCreated)
                        {
                            bakimSwitch.Invoke(new Action(delegate ()
                            {
                                bakimSwitch.Checked = false;
                            }));
                        }
                    }
                    if (nxCompoletBoolRead("autoMod"))
                    {
                        if (this.IsHandleCreated)
                        {
                            autoSwitch.Invoke(new Action(delegate ()
                            {
                               autoSwitch.Checked = true;
                            }));
                        }
                    }
                    else
                    {
                        if (this.IsHandleCreated)
                        {
                           autoSwitch.Invoke(new Action(delegate ()
                            {
                               autoSwitch.Checked = false;
                            }));
                        }
                    }
                    try
                    {
                        if (this.IsHandleCreated)
                        {
                            wPos.Invoke(new Action(delegate ()
                            {
                                string wPoss = Convert.ToString(nxCompoletDoubleRead("wPos"));
                                this.wPos.Text = wPoss/*.Substring(0, 4)*/;
                            }));
                        }
                        if (this.IsHandleCreated)
                        {
                            zPos.Invoke(new Action(delegate ()
                            {
                                string zPoss = Convert.ToString(nxCompoletDoubleRead("zPos"));
                                this.zPos.Text = zPoss/*.Substring(0, 4)*/;
                            }));
                        }
                    }
                    catch (Exception ex)
                    {
                        otherConsoleAppendLine("Thread Fonksiyonu İç Loop Hatası" + ex.Message, Color.Red);
                    }
                }

                if (nxCompoletBoolRead("homeState"))
                {
                    homeBtn.Invoke(new Action(delegate ()
                    {
                        homeBtn.BackColor = Color.Green;
                        homeBtn.Text = "HOME PROCESSING";
                        bakimSwitch.Enabled = false;
                        autoSwitch.Enabled = false;
                        moveButton.Enabled = false;
                    }));
                }
                else
                {
                    homeBtn.Invoke(new Action(delegate ()
                    {
                        homeBtn.BackColor = Color.Red;
                        homeBtn.Text = "HOME";
                        bakimSwitch.Enabled = true;
                        autoSwitch.Enabled = true;
                        moveButton.Enabled = true;
                    }));
                }

                if (nxCompoletBoolRead("startButton"))
                {
                    startButton.Invoke(new Action(delegate ()
                    {
                        startButton.BackColor = Color.Green;
                    }));
                }
                else
                {
                    startButton.Invoke(new Action(delegate ()
                    {
                        startButton.BackColor = Color.Red;
                    }));
                }
            }
            catch (Exception ex)
            {
                otherConsoleAppendLine("loopPLCScreen Hatası" + ex.Message, Color.Red);
            }
        }

        private void systemReadyLoop()
        {
            if (!nxCompoletBoolRead("emergencyStop"))
            {
                emergencyStopReady.Invoke(new Action(delegate ()
                {
                    emergencyStopReady.Text = "ACİL STOP BASILI DEĞİL";
                    emergencyStopReady.BackColor = Color.Lime;
                }));
            }
            else
            {
                emergencyStopReady.Invoke(new Action(delegate ()
                {
                    emergencyStopReady.Text = "ACİL STOP BASILI";
                    emergencyStopReady.BackColor = Color.Red;
                }));
            }

            if (lblEthernetState.Text == "ON")
            {
                ethernetStateReady.Invoke(new Action(delegate ()
                {
                    ethernetStateReady.Text = "ETHERNET STATE OK";
                    ethernetStateReady.BackColor = Color.Lime;
                }));
            }
            else if (lblEthernetState.Text == "OFF")
            {
                ethernetStateReady.Invoke(new Action(delegate ()
                {
                    ethernetStateReady.Text = "ETHERNET STATE NOK";
                    ethernetStateReady.BackColor = Color.Red;
                }));
            }

            if (lblCameraState.Text == "ON")
            {
                cameraStateReady.Invoke(new Action(delegate ()
                {
                    cameraStateReady.Text = "CAMERA STATE OK";
                    cameraStateReady.BackColor = Color.Lime;
                }));
            }
            else if (lblCameraState.Text == "OFF")
            {
                cameraStateReady.Invoke(new Action(delegate ()
                {
                    cameraStateReady.Text = "CAMERA STATE NOK";
                    cameraStateReady.BackColor = Color.Red;
                }));
            }

            if (nxCompoletBoolRead("connectionStart"))
            {
                plcStateReady.Invoke(new Action(delegate ()
                {
                    plcStateReady.Text = "PLC SERVER HABERLEŞME OK";
                    plcStateReady.BackColor = Color.Lime;
                }));
            }
            else
            {
                plcStateReady.Invoke(new Action(delegate ()
                {
                    plcStateReady.Text = "PLC SERVER HABERLEŞME NOK";
                    plcStateReady.BackColor = Color.Red;
                }));
            }

            if (nxCompoletBoolRead("homeReadyState"))
            {
                homeStateReady.Invoke(new Action(delegate ()
                {
                    homeStateReady.Text = "HOME OK";
                    homeStateReady.BackColor = Color.Lime;
                }));
            }
            else
            {
                homeStateReady.Invoke(new Action(delegate ()
                {
                    homeStateReady.Text = "HOME NOK";
                    homeStateReady.BackColor = Color.Red;
                }));
            }

            if (autoSwitch.Checked == true)
            {
                autoStateReady.Invoke(new Action(delegate ()
                {
                    autoStateReady.Text = "OTOMATİK ÇALIŞMA AÇIK";
                    autoStateReady.BackColor = Color.Lime;
                }));
            }
            else
            {
                autoStateReady.Invoke(new Action(delegate ()
                {
                    autoStateReady.Text = "OTOMATİK ÇALIŞMA KAPALI";
                    autoStateReady.BackColor = Color.Red;
                }));
            }

            if (nxCompoletBoolRead("gripperState"))
            {
                gripperStateReady.Invoke(new Action(delegate ()
                {
                    gripperStateReady.Text = "GRIPPER KAPALI";
                    gripperStateReady.BackColor = Color.Lime;
                }));
            }
            else
            {
                gripperStateReady.Invoke(new Action(delegate ()
                {
                    gripperStateReady.Text = "GRIPPER AÇIK";
                    gripperStateReady.BackColor = Color.Red;
                }));
            }

            if (!nxCompoletBoolRead("frontDoorState"))
            {
                doorStateReady.Invoke(new Action(delegate ()
                {
                    doorStateReady.Text = "ÖN KAPI KAPALI";
                    doorStateReady.BackColor = Color.Lime;
                }));
            }
            else
            {
                doorStateReady.Invoke(new Action(delegate ()
                {
                    doorStateReady.Text = "ÖN KAPI AÇIK";
                    doorStateReady.BackColor = Color.Red;
                }));
            }
            try
            {
                string testState = Convert.ToString(nxCompoletDoubleRead("testState"));
                if (testState == "0")
                {

                    testProcess.Invoke(new Action(delegate ()
                    {
                        testProcess.Visible = false;
                    }));

                    testError.Invoke(new Action(delegate ()
                    {
                        testError.Visible = false;
                    }));

                    testSuccess.Invoke(new Action(delegate ()
                    {
                        testSuccess.Visible = false;
                    }));
                }
                else if (testState == "1")
                {
                    testProcess.Invoke(new Action(delegate ()
                    {
                        testProcess.Visible = true;
                    }));

                    testError.Invoke(new Action(delegate ()
                    {
                        testError.Visible = false;
                    }));

                    testSuccess.Invoke(new Action(delegate ()
                    {
                        testSuccess.Visible = false;
                    }));
                }
                else if (testState == "2")
                {
                    testProcess.Invoke(new Action(delegate ()
                    {
                        testProcess.Visible = false;
                    }));

                    testError.Invoke(new Action(delegate ()
                    {
                        testError.Visible = true;
                    }));

                    testSuccess.Invoke(new Action(delegate ()
                    {
                        testSuccess.Visible = false;
                    }));
                }
                else if (testState == "3")
                {
                    testProcess.Invoke(new Action(delegate ()
                    {
                        testProcess.Visible = false;
                    }));

                    testError.Invoke(new Action(delegate ()
                    {
                        testError.Visible = false;
                    }));

                    testSuccess.Invoke(new Action(delegate ()
                    {
                        testSuccess.Visible = true;
                    }));
                }

                string cycleTimee = Convert.ToString(nxCompoletDoubleRead("cycleTime"));
                txtCevrimSuresi.Invoke(new Action(delegate ()
                {
                    txtCevrimSuresi.Text = cycleTimee;
                }));
            }
            catch (Exception ex)
            {
                otherConsoleAppendLine("systemReadyLoop Hatası" + ex.Message, Color.Red);
            }
        }

        private void emergencyLoop()
        {
            try
            {
                if (nxCompoletBoolRead("emergencyStop"))
                {
                    if (onceWrite)
                    {
                        snapShot1 = "0";
                        cameraYaz(1);
                        snapShot2 = "0";
                        cameraYaz(2);
                        snapShot3 = "0";
                        cameraYaz(3);
                        snapShot4 = "0";
                        cameraYaz(4);
                        snapShot5 = "0";
                        cameraYaz(5);
                        snapShot6 = "0";
                        cameraYaz(6);
                        snapShot7 = "0";
                        cameraYaz(7);
                        snapShot8 = "0";
                        cameraYaz(8);
                        snapShot9 = "0";
                        cameraYaz(9);
                        snapShot10 = "0";
                        cameraYaz(10);
                        snapShot11 = "0";
                        cameraYaz(11);
                        snapShot12 = "0";
                        cameraYaz(12);
                        INIKaydet ini = new INIKaydet(Prog_Ayarlar.Default.iniKameraYazDosyaYolu);
                        ini.Yaz("panoId", "Metin Kutusu", "0");
                        ini.Yaz("emergencyStop", "Metin Kutusu", "1");
                        cameraState = "";
                        oldCameraState = "";
                        ethernetState = "";
                        oldEthernetState = "";
                        objectOK = "";
                        objectNOK = "";
                        processDone = "";
                        snapShot1 = "";
                        snapShot2 = "";
                        snapShot3 = "";
                        snapShot4 = "";
                        snapShot5 = "";
                        snapShot6 = "";
                        snapShot7 = "";
                        snapShot8 = "";
                        snapShot9 = "";
                        snapShot10 = "";
                        snapShot11 = "";
                        snapShot12 = "";
                        cameraTimerCounter = 0;
                        cameraTimeoutState = false;
                        cameraProcessCounter = 0;
                        processControlState = true;
                        onceWrite = false;
                    }
                }
                else
                {
                    onceWrite = true;
                    INIKaydet ini = new INIKaydet(Prog_Ayarlar.Default.iniKameraYazDosyaYolu);
                    ini.Yaz("emergencyStop", "Metin Kutusu", "0");
                }
            }
            catch (Exception ex)
            {
                otherConsoleAppendLine("emergencyLoop Hatası" + ex.Message, Color.Red);
            }
        }

        /****************************************** camera İstasyon *************************************************/
        public void cameraStation()  //FCT PLC-camera KOMPONENT
        {
            cameraOku();
            try
            {
                newObject = nxCompoletBoolRead("newObject");
                if(newObject != oldNewObject) //newObject PLC üzerinden cameraya Haber Ver.
                {
                    INIKaydet ini = new INIKaydet(Prog_Ayarlar.Default.iniKameraYazDosyaYolu);
                    if (newObject)
                    {
                        ini.Yaz("newObject", "Metin Kutusu", "1");
                    }
                    else
                    {
                        ini.Yaz("newObject", "Metin Kutusu", "0");
                    }
                    otherConsoleAppendLine("newObject: " + newObject, Color.Green);
                    oldNewObject = newObject;
                }
                if (nxCompoletBoolRead("snapShotOK[0]")) //snapShotOK PLC üzerinden cameraya Haber Ver.
                {
                    if (nxCompoletBoolWrite("snapShotOK[0]", false))
                    {
                        cameraTimeoutState = true;
                        snapShot1 = "1";
                        cameraYaz(1);
                    }
                }
                if (nxCompoletBoolRead("snapShotOK[1]")) //snapShotOK PLC üzerinden cameraya Haber Ver.
                {
                    if (nxCompoletBoolWrite("snapShotOK[1]", false))
                    {
                        cameraTimeoutState = true;
                        snapShot2 = "1";
                        cameraYaz(2);
                    }
                }
                if (nxCompoletBoolRead("snapShotOK[2]")) //snapShotOK PLC üzerinden cameraya Haber Ver.
                {
                    if (nxCompoletBoolWrite("snapShotOK[2]", false))
                    {
                        cameraTimeoutState = true;
                        snapShot3 = "1";
                        cameraYaz(3);
                    }
                }
                if (nxCompoletBoolRead("snapShotOK[3]")) //snapShotOK PLC üzerinden cameraya Haber Ver.
                {
                    if (nxCompoletBoolWrite("snapShotOK[3]", false))
                    {
                        cameraTimeoutState = true;
                        snapShot4 = "1";
                        cameraYaz(4);
                    }
                }
                if (nxCompoletBoolRead("snapShotOK[4]")) //snapShotOK PLC üzerinden cameraya Haber Ver.
                {
                    if (nxCompoletBoolWrite("snapShotOK[4]", false))
                    {
                        cameraTimeoutState = true;
                        snapShot5 = "1";
                        cameraYaz(5);
                    }
                }
                if (nxCompoletBoolRead("snapShotOK[5]")) //snapShotOK PLC üzerinden cameraya Haber Ver.
                {
                    if (nxCompoletBoolWrite("snapShotOK[5]", false))
                    {
                        cameraTimeoutState = true;
                        snapShot6 = "1";
                        cameraYaz(6);
                    }
                }
                if (nxCompoletBoolRead("snapShotOK[6]")) //snapShotOK PLC üzerinden cameraya Haber Ver.
                {
                    if (nxCompoletBoolWrite("snapShotOK[6]", false))
                    {
                        cameraTimeoutState = true;
                        snapShot7 = "1";
                        cameraYaz(7);
                    }
                }
                if (nxCompoletBoolRead("snapShotOK[7]")) //snapShotOK PLC üzerinden cameraya Haber Ver.
                {
                    if (nxCompoletBoolWrite("snapShotOK[7]", false))
                    {
                        cameraTimeoutState = true;
                        snapShot8 = "1";
                        cameraYaz(8);
                    }
                }
                if (nxCompoletBoolRead("snapShotOK[8]")) //snapShotOK PLC üzerinden cameraya Haber Ver.
                {
                    if (nxCompoletBoolWrite("snapShotOK[8]", false))
                    {
                        cameraTimeoutState = true;
                        snapShot9 = "1";
                        cameraYaz(9);
                    }
                }
                if (nxCompoletBoolRead("snapShotOK[9]")) //snapShotOK PLC üzerinden cameraya Haber Ver.
                {
                    if (nxCompoletBoolWrite("snapShotOK[9]", false))
                    {
                        cameraTimeoutState = true;
                        snapShot10 = "1";
                        cameraYaz(10);
                    }
                }
                if (nxCompoletBoolRead("snapShotOK[10]")) //snapShotOK PLC üzerinden cameraya Haber Ver.
                {
                    if (nxCompoletBoolWrite("snapShotOK[10]", false))
                    {
                        cameraTimeoutState = true;
                        snapShot11 = "1";
                        cameraYaz(11);
                    }
                }
                if (nxCompoletBoolRead("snapShotOK[11]")) //snapShotOK PLC üzerinden cameraya Haber Ver.
                {
                    if (nxCompoletBoolWrite("snapShotOK[11]", false))
                    {
                        cameraTimeoutState = true;
                        snapShot12 = "1";
                        cameraYaz(12);
                    }
                }
            }
            catch (Exception ex)
            {
                otherConsoleAppendLine("FCTcameraIstasyon(snapShotOK): " + ex.Message, Color.Red);
            }

            try
            {
                if (nxCompoletBoolRead("snapShotNOK"))  //snapShotNOK PLC üzerinden cameraya Haber Ver.
                {
                    snapShot1 = "0";
                    snapShot2 = "0";
                    snapShot3 = "0";
                    snapShot4 = "0";
                    snapShot5 = "0";
                    snapShot6 = "0";
                    snapShot7 = "0";
                    snapShot8 = "0";
                    snapShot9 = "0";
                    snapShot10 = "0";
                    snapShot11 = "0";
                    snapShot12 = "0";
                    for(int i = 0; i<=12; i++)  //Sıfırla
                    {
                        cameraYaz(i);
                    }
                     
                   // cameraProcessCounter++;
                    INIKaydet ini = new INIKaydet(Prog_Ayarlar.Default.iniKameraOkuDosyaYolu);
                    processDone = ini.Oku("processDone", "Metin Kutusu");
                    if (processDone == "0")  //1sn sonra
                    {
                        if (nxCompoletBoolRead("objectOK"))
                        {
                            nxCompoletBoolWrite("objectOK", false);
                        }
                        if (nxCompoletBoolRead("objectNOK"))
                        {
                            nxCompoletBoolWrite("objectNOK", false);
                        }
                        if (nxCompoletBoolRead("processDone"))
                        {
                            nxCompoletBoolWrite("processDone", false);
                        }
                        nxCompoletBoolWrite("snapShotNOK", false);
                        processControlState = true;
                        cameraProcessCounter = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                otherConsoleAppendLine("FCTcameraIstasyon(snapShotNOK): " + ex.Message, Color.Red);
            }

            if (cameraTimeoutState == true)  //PLC-camera-TIMEOUT
            {
                cameraTimerCounter++;
                if (cameraTimerCounter > Prog_Ayarlar.Default.kameraTimeout * 10)
                {
                    cameraComponentError();
                }
            }
        }

        private void cameraYaz(int state)  //PLC'DEN camera İÇİN TEXT'E VERİ YAZ
        {
            try
            {
                if (Prog_Ayarlar.Default.iniKameraYazDosyaYolu != "")
                {
                    INIKaydet ini = new INIKaydet(Prog_Ayarlar.Default.iniKameraYazDosyaYolu);
                    if (state == 1)
                    {
                        ini.Yaz("snapShot1", "Metin Kutusu", Convert.ToString(snapShot1));
                        snapShot1 = ini.Oku("snapShot1", "Metin Kutusu");
                        otherConsoleAppendLine("snapShot1 = " + snapShot1, Color.Green);
                    }
                    else if (state == 2)
                    {
                        ini.Yaz("snapShot2", "Metin Kutusu", Convert.ToString(snapShot2));
                        snapShot2 = ini.Oku("snapShot2", "Metin Kutusu");
                        otherConsoleAppendLine("snapShot2 = " + snapShot2, Color.Green);
                    }
                    else if (state == 3)
                    {
                        ini.Yaz("snapShot3", "Metin Kutusu", Convert.ToString(snapShot3));
                        snapShot3 = ini.Oku("snapShot3", "Metin Kutusu");
                        otherConsoleAppendLine("snapShot3 = " + snapShot3, Color.Green);
                    }
                    else if (state == 4)
                    {
                        ini.Yaz("snapShot4", "Metin Kutusu", Convert.ToString(snapShot4));
                        snapShot4 = ini.Oku("snapShot4", "Metin Kutusu");
                        otherConsoleAppendLine("snapShot4 = " + snapShot4, Color.Green);
                    }
                    else if (state == 5)
                    {
                        ini.Yaz("snapShot5", "Metin Kutusu", Convert.ToString(snapShot5));
                        snapShot5 = ini.Oku("snapShot5", "Metin Kutusu");
                        otherConsoleAppendLine("snapShot5 = " + snapShot5, Color.Green);
                    }
                    else if (state == 6)
                    {
                        ini.Yaz("snapShot6", "Metin Kutusu", Convert.ToString(snapShot6));
                        snapShot6 = ini.Oku("snapShot6", "Metin Kutusu");
                        otherConsoleAppendLine("snapShot6 = " + snapShot6, Color.Green);
                    }
                    else if (state == 7)
                    {
                        ini.Yaz("snapShot7", "Metin Kutusu", Convert.ToString(snapShot7));
                        snapShot7 = ini.Oku("snapShot7", "Metin Kutusu");
                        otherConsoleAppendLine("snapShot7 = " + snapShot7, Color.Green);
                    }
                    else if (state == 8)
                    {
                        ini.Yaz("snapShot8", "Metin Kutusu", Convert.ToString(snapShot8));
                        snapShot8 = ini.Oku("snapShot8", "Metin Kutusu");
                        otherConsoleAppendLine("snapShot8 = " + snapShot8, Color.Green);
                    }
                    else if (state == 9)
                    {
                        ini.Yaz("snapShot9", "Metin Kutusu", Convert.ToString(snapShot9));
                        snapShot9 = ini.Oku("snapShot9", "Metin Kutusu");
                        otherConsoleAppendLine("snapShot9 = " + snapShot9, Color.Green);
                    }
                    else if (state == 10)
                    {
                        ini.Yaz("snapShot10", "Metin Kutusu", Convert.ToString(snapShot10));
                        snapShot10 = ini.Oku("snapShot10", "Metin Kutusu");
                        otherConsoleAppendLine("snapShot10 = " + snapShot10, Color.Green);
                    }
                    else if (state == 11)
                    {
                        ini.Yaz("snapShot11", "Metin Kutusu", Convert.ToString(snapShot11));
                        snapShot11 = ini.Oku("snapShot11", "Metin Kutusu");
                        otherConsoleAppendLine("snapShot11 = " + snapShot11, Color.Green);
                    }
                    else if (state == 12)
                    {
                        ini.Yaz("snapShot12", "Metin Kutusu", Convert.ToString(snapShot12));
                        snapShot12 = ini.Oku("snapShot12", "Metin Kutusu");
                        otherConsoleAppendLine("snapShot12 = " + snapShot12, Color.Green);
                    }
                }
                else
                {
                    otherConsoleAppendLine("Dosya Yolu Boş Kalamaz", Color.Red);
                }
            }
            catch (Exception ex)
            {
                otherConsoleAppendLine("cameraYaz: " + ex.Message, Color.Red);
            }
        }

        private void cameraOku()  //camera'DAN PLC'YE VERİ GÖNDERMEK İÇİN TEXT OKU
        {
            try
            {
                if (Prog_Ayarlar.Default.iniKameraOkuDosyaYolu != "")
                {
                    if (File.Exists(Prog_Ayarlar.Default.iniKameraOkuDosyaYolu))
                    {
                        INIKaydet ini = new INIKaydet(Prog_Ayarlar.Default.iniKameraOkuDosyaYolu);

                        cameraState = ini.Oku("cameraState", "Metin Kutusu");
                        if (cameraState == "0")
                        {
                            cameraStateErrorCounter++;
                            if (cameraStateErrorCounter >= 15)
                            {
                                cameraStateErrorCounter = 0;
                                lblCameraState.Text = "OFF";
                                lblCameraState.BackColor = Color.Red;
                                nxCompoletBoolWrite("cameraState", false);
                            }
                        }
                        if (cameraState != oldCameraState)
                        {
                            if (cameraState == "1")
                            {
                                cameraStateErrorCounter = 0;
                                lblCameraState.Text = "ON";
                                lblCameraState.BackColor = Color.Green;
                                nxCompoletBoolWrite("cameraState", true);
                            }
                            else if (cameraState == "0")
                            {
                                lblCameraState.Text = "OFF";
                                lblCameraState.BackColor = Color.Red;
                                nxCompoletBoolWrite("cameraState", false);
                            }
                            else
                            {
                                //otherConsoleAppendLine("cameraState = BOŞ", Color.Red);
                            }
                            //otherConsoleAppendLine("cameraState = " + cameraState, Color.Green);
                            oldCameraState = cameraState;
                        }

                        ethernetState = ini.Oku("ethernetState", "Metin Kutusu");
                        if (ethernetState == "0")
                        {
                            ethernetStateErrorCounter++;
                            if (ethernetStateErrorCounter >= 15)
                            {
                                ethernetStateErrorCounter = 0;
                                lblEthernetState.Text = "OFF";
                                lblEthernetState.BackColor = Color.Red;
                                nxCompoletBoolWrite("ethernetState", false);
                            }
                        }
                        if (ethernetState != oldEthernetState)
                        {
                            if (ethernetState == "1")
                            {
                                ethernetStateErrorCounter = 0;
                                lblEthernetState.Text = "ON";
                                lblEthernetState.BackColor = Color.Green;
                                nxCompoletBoolWrite("ethernetState", true);
                            }
                            else if (ethernetState == "0")
                            {
                                lblEthernetState.Text = "OFF";
                                lblEthernetState.BackColor = Color.Red;
                                nxCompoletBoolWrite("ethernetState", false);
                            }
                            else
                            {
                                //otherConsoleAppendLine("ethernetState = BOŞ", Color.Red);
                            }
                            //otherConsoleAppendLine("ethernetState = " + ethernetState, Color.Green);
                            oldEthernetState = ethernetState;
                        }

                        processDone = ini.Oku("processDone", "Metin Kutusu");
                        if (processDone == "1" && processControlState == true)
                        {
                            if (processDone == "1")
                            {
                                nxCompoletBoolWrite("processDone", true);
                            }
                            else
                            {
                                nxCompoletBoolWrite("processDone", false);
                            }
                            otherConsoleAppendLine("processDone = " + processDone, Color.Green);
                            bool loopState = true;
                            while (loopState)
                            {
                                objectOK = ini.Oku("objectOK", "Metin Kutusu");
                                if (objectOK == "1")
                                {
                                    loopState = false;
                                    cameraComponentSuccess();
                                    nxCompoletBoolWrite("objectOK", true);
                                }
                                else if (objectOK == "0")
                                {
                                    loopState = false;
                                    nxCompoletBoolWrite("objectOK", false);
                                }
                            }
                            otherConsoleAppendLine("objectOK = " + objectOK, Color.Green);
                            loopState = true;
                            while (loopState)
                            {
                                objectNOK = ini.Oku("objectNOK", "Metin Kutusu");
                                if (objectNOK == "1")
                                {
                                    loopState = false;
                                    cameraComponentError();
                                    nxCompoletBoolWrite("objectNOK", true);
                                }
                                else if (objectNOK == "0")
                                {
                                    loopState = false;
                                    nxCompoletBoolWrite("objectNOK", false);
                                }
                            }
                            otherConsoleAppendLine("objectNOK = " + objectNOK, Color.Green);
                            processControlState = false;
                        }
                    }
                }
                else
                {
                    otherConsoleAppendLine("Dosya Yolu Boş Kalamaz", Color.Red);
                }
            }
            catch (Exception ex)
            {
                otherConsoleAppendLine("cameraOku: " + ex.Message, Color.Red);
            }
        }

        public void cameraComponentSuccess()  //camera-PLC KOMPONENT BAŞARILI
        {
            tbOtherState.Invoke(new Action(delegate ()
            {
                tbOtherState.BackColor = Color.Green;
                tbOtherState.Text = "Camera Object BAŞARILI";
            }));
            cameraRestart();
        }

        public void cameraComponentError()   //camera-PLC KOMPONENT BAŞARISIZ
        {
            tbOtherState.Invoke(new Action(delegate ()
            {
                tbOtherState.BackColor = Color.Red;
                tbOtherState.Text = "Camera Object BAŞARISIZ";
            }));
            cameraRestart();
        }

        public void cameraRestart()   //camera-PLC KOMPONENT RESTART
        {
            cameraTimeoutState = false;
            cameraTimerCounter = 0;
            cameraProcessCounter = 0;
            processControlState = true;
        }

        /********************************************** DATA GÖNDERME ************************************************/
        private void btnSend_Click(object sender, EventArgs e)
        {
            plcSend();
            cameraSend();
        }

        private void plcSend()
        {
            nxCompoletBoolWrite("photoSend", true);
            nxCompoletStringWrite("photoNum", photoNumCb.Text);
            string[] photo1 = new string[2];
            string[] photo2 = new string[2];
            string[] photo3 = new string[2];
            string[] photo4 = new string[2];
            string[] photo5 = new string[2];
            string[] photo6 = new string[2];
            string[] photo7 = new string[2];
            string[] photo8 = new string[2];
            string[] photo9 = new string[2];
            string[] photo10 = new string[2];
            string[] photo11 = new string[2];
            string[] photo12 = new string[2];

            photo1[0] = tbwPos1.Text;
            photo2[0] = tbwPos2.Text;
            photo3[0] = tbwPos3.Text;
            photo4[0] = tbwPos4.Text;
            photo5[0] = tbwPos5.Text;
            photo6[0] = tbwPos6.Text;
            photo7[0] = tbwPos7.Text;
            photo8[0] = tbwPos8.Text;
            photo9[0] = tbwPos9.Text;
            photo10[0] = tbwPos10.Text;
            photo11[0] = tbwPos11.Text;
            photo12[0] = tbwPos12.Text;

            photo1[1] = tbzPos1.Text;
            photo2[1] = tbzPos2.Text;
            photo3[1] = tbzPos3.Text;
            photo4[1] = tbzPos4.Text;
            photo5[1] = tbzPos5.Text;
            photo6[1] = tbzPos6.Text;
            photo7[1] = tbzPos7.Text;
            photo8[1] = tbzPos8.Text;
            photo9[1] = tbzPos9.Text;
            photo10[1] = tbzPos10.Text;
            photo11[1] = tbzPos11.Text;
            photo12[1] = tbzPos12.Text;

            if (photoNumCb.Text != "")
            {
                for (int j = 0; j < 2; j++)
                {
                    if (Convert.ToInt32(photoNumCb.Text) > 0)
                        nxCompoletStringWrite("photo1[" + j + "]", photo1[j]);
                    if (Convert.ToInt32(photoNumCb.Text) > 1)
                        nxCompoletStringWrite("photo2[" + j + "]", photo2[j]);
                    if (Convert.ToInt32(photoNumCb.Text) > 2)
                        nxCompoletStringWrite("photo3[" + j + "]", photo3[j]);
                    if (Convert.ToInt32(photoNumCb.Text) > 3)
                        nxCompoletStringWrite("photo4[" + j + "]", photo4[j]);
                    if (Convert.ToInt32(photoNumCb.Text) > 4)
                        nxCompoletStringWrite("photo5[" + j + "]", photo5[j]);
                    if (Convert.ToInt32(photoNumCb.Text) > 5)
                        nxCompoletStringWrite("photo6[" + j + "]", photo6[j]);
                    if (Convert.ToInt32(photoNumCb.Text) > 6)
                        nxCompoletStringWrite("photo7[" + j + "]", photo7[j]);
                    if (Convert.ToInt32(photoNumCb.Text) > 7)
                        nxCompoletStringWrite("photo8[" + j + "]", photo8[j]);
                    if (Convert.ToInt32(photoNumCb.Text) > 8)
                        nxCompoletStringWrite("photo9[" + j + "]", photo9[j]);
                    if (Convert.ToInt32(photoNumCb.Text) > 9)
                        nxCompoletStringWrite("photo10[" + j + "]", photo10[j]);
                    if (Convert.ToInt32(photoNumCb.Text) > 10)
                        nxCompoletStringWrite("photo11[" + j + "]", photo11[j]);
                    if (Convert.ToInt32(photoNumCb.Text) > 11)
                        nxCompoletStringWrite("photo12[" + j + "]", photo12[j]);
                }

                nxCompoletBoolWrite("photoSend", false);
            }
            else
            {
                MessageBox.Show("Lütfen Foto Sayısını Giriniz.");
            }
        }

        private void cameraSend()
        {
            try
            {
                if (Prog_Ayarlar.Default.iniKameraYazDosyaYolu != "")
                {
                    INIKaydet ini = new INIKaydet(Prog_Ayarlar.Default.iniKameraYazDosyaYolu);
                    ini.Yaz("panoId", "Metin Kutusu", Convert.ToString(receteName));
                }
                else
                {
                    otherConsoleAppendLine("Dosya Yolu Boş Kalamaz", Color.Red);
                }
            }
            catch (Exception ex)
            {
                otherConsoleAppendLine("cameraSend: " + ex.Message, Color.Red);
            }
        }

        /********************************************** Ortak Tüm Ana İşlemlerin Yönlendirilmesi************************************************/
        public void yetkidegistir()
        {
            if (this.yetki == 0)
            {
                this.btnCikis.Enabled = false;
                this.btnAyar.Enabled = false;
                btnCikis.BackColor = Color.Gray;
                btnAyar.BackColor = Color.Gray;
            }
            if (this.yetki == 1)
            {
                this.btnCikis.Enabled = true;
                this.btnAyar.Enabled = true;
                btnCikis.BackColor = Color.Red;
                btnAyar.BackColor = Color.Red;
                timerAdmin.Start();
            }
            if (this.yetki == 2)
            {
                this.btnCikis.Enabled = true;
                btnCikis.BackColor = Color.Red;
                timerAdmin.Start();
            }
        }

        private void btnAyar_Click(object sender, EventArgs e)
        {
            int num = (int)this.ProgAyarFrm.ShowDialog();
        }

        private void btnCikis_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData != Keys.L)
                return;
            if (this.yetki != 0)
            {
                timerAdmin.Stop();
                this.yetki = 0;
                this.yetkidegistir();
            }
            else
            {
                int num = (int)this.SifreFrm.ShowDialog();
                textBox1.Clear();
            }
        }

        private void timerAdmin_Tick(object sender, EventArgs e)
        {
            adminTimerCounter++;
            if (adminTimerCounter == 1)
            {
                adminTimerCounter = 0;
                timerAdmin.Stop();
                this.yetki = 0;
                this.yetkidegistir();
            }
        }

        /********************************************** PLC BUTTON ************************************************/
        private void homeBtn_Click(object sender, EventArgs e)
        {
            homeBtnClick();
        }

        private void motorSliderValue_Scroll(object sender, ScrollEventArgs e)
        {
            if (bakimSwitch.Checked == true && acilStopSwitch.Checked == false)
            {
                sliderLabel.Text = Convert.ToString(motorSliderValue.Value);
                nxCompoletStringWrite("motorValue", Convert.ToString(motorSliderValue.Value));
            }
        }

        private void moveButton_MouseDown(object sender, MouseEventArgs e)
        {
            if (bakimSwitch.Checked == true && acilStopSwitch.Checked == false)
            {
                moveButton.BackColor = Color.Green;
                moveButton.Text = "MOVE ON";
                if (wForward.Checked)
                {
                    nxCompoletBoolWrite("wForward", true);
                }
                else if (zForward.Checked)
                {
                    nxCompoletBoolWrite("zForward", true);
                }
                else if (wBackward.Checked)
                {
                    nxCompoletBoolWrite("wBackward", true);
                }
                else if (zBackward.Checked)
                {
                    nxCompoletBoolWrite("zBackward", true);
                }
            }
        }

        private void moveButton_MouseUp(object sender, MouseEventArgs e)
        {
            if (bakimSwitch.Checked == true && acilStopSwitch.Checked == false)
            {
                moveButton.BackColor = Color.Red;
                moveButton.Text = "MOVE OFF";
                if (wForward.Checked)
                {
                    nxCompoletBoolWrite("wForward", false);
                }
                else if (zForward.Checked)
                {
                    nxCompoletBoolWrite("zForward", false);
                }
                else if (wBackward.Checked)
                {
                    nxCompoletBoolWrite("wBackward", false);
                }
                else if (zBackward.Checked)
                {
                    nxCompoletBoolWrite("zBackward", false);
                }
            }
        }

        private void homeBtn_2_Click(object sender, EventArgs e)
        {
            homeBtnClick();
        }

        public void homeBtnClick()
        {
            if ((bakimSwitch.Checked == false && acilStopSwitch.Checked == false && autoSwitch.Checked == false))
            {
                nxCompoletBoolWrite("homeState", true);
            }
        }

        private void bakimSwitch_CheckedChanged(object sender, EventArgs e)
        {
            if (bakimSwitch.Checked == true)
            {
                otherConsoleAppendLine("True", Color.Orange);
                if (nxCompoletBoolWrite("bakimMod", true))
                {
                    bakimSwitch.Checked = true;
                }
            }
            else
            {
                if (nxCompoletBoolWrite("bakimMod", false))
                {
                    bakimSwitch.Checked = false;
                }
            }
        }

        private void autoSwitch_CheckedChanged(object sender, EventArgs e)
        {
            if (autoSwitch.Checked == true)
            {
                if (nxCompoletBoolWrite("autoMod", true))
                {
                    autoSwitch.Checked = true;
                }
            }
            else
            {
                if (nxCompoletBoolWrite("autoMod", false))
                {
                    autoSwitch.Checked = false;
                }
            }
        }

        private void wSendBtn_Click(object sender, EventArgs e)
        {
            if (Convert.ToDouble(wJog.Text) > 50 || Convert.ToDouble(wJog.Text) < 0.001)
            {
                MessageBox.Show("wJog Değer Aralığı 50'den büyük 0<001'den Küçük Olamaz");
            }
            else
            {
                nxCompoletStringWrite("wJog", wJog.Text);
            }
        }

        private void zSendBtn_Click(object sender, EventArgs e)
        {
            if (Convert.ToDouble(zJog.Text) > 50 || Convert.ToDouble(zJog.Text) < 0.001)
            {
                MessageBox.Show("zJog Değer Aralığı 50'den büyük 0<001'den Küçük Olamaz");
            }
            else
            {
                nxCompoletStringWrite("zJog", zJog.Text);
            }
        }

        /********************************************** EXCEL YÖNETİMİ ************************************************/
        private void btnList_Click(object sender, EventArgs e)
        {
            Listele();
        }

        public void Listele()
        {
            if (txtReceteName.Text != "")
            {
                try
                {
                    if (File.Exists(txtReceteName.Text))
                    {
                        INIKaydet ini = new INIKaydet(txtReceteName.Text);
                        photoNumCb.Text = ini.Oku("photoNum", "Metin Kutusu");

                        tbwPos1.Text = ini.Oku("wPos1", "Metin Kutusu");
                        tbwPos2.Text = ini.Oku("wPos2", "Metin Kutusu");
                        tbwPos3.Text = ini.Oku("wPos3", "Metin Kutusu");
                        tbwPos4.Text = ini.Oku("wPos4", "Metin Kutusu");
                        tbwPos5.Text = ini.Oku("wPos5", "Metin Kutusu");
                        tbwPos6.Text = ini.Oku("wPos6", "Metin Kutusu");
                        tbwPos7.Text = ini.Oku("wPos7", "Metin Kutusu");
                        tbwPos8.Text = ini.Oku("wPos8", "Metin Kutusu");
                        tbwPos9.Text = ini.Oku("wPos9", "Metin Kutusu");
                        tbwPos10.Text = ini.Oku("wPos10", "Metin Kutusu");
                        tbwPos11.Text = ini.Oku("wPos11", "Metin Kutusu");
                        tbwPos12.Text = ini.Oku("wPos12", "Metin Kutusu");
                        tbzPos1.Text = ini.Oku("zPos1", "Metin Kutusu");
                        tbzPos2.Text = ini.Oku("zPos2", "Metin Kutusu");
                        tbzPos3.Text = ini.Oku("zPos3", "Metin Kutusu");
                        tbzPos4.Text = ini.Oku("zPos4", "Metin Kutusu");
                        tbzPos5.Text = ini.Oku("zPos5", "Metin Kutusu");
                        tbzPos6.Text = ini.Oku("zPos6", "Metin Kutusu");
                        tbzPos7.Text = ini.Oku("zPos7", "Metin Kutusu");
                        tbzPos8.Text = ini.Oku("zPos8", "Metin Kutusu");
                        tbzPos9.Text = ini.Oku("zPos9", "Metin Kutusu");
                        tbzPos10.Text = ini.Oku("zPos10", "Metin Kutusu");
                        tbzPos11.Text = ini.Oku("zPos11", "Metin Kutusu");
                        tbzPos12.Text = ini.Oku("zPos12", "Metin Kutusu");
                        photoNumCb.Text = ini.Oku("photoNum", "Metin Kutusu");

                        otherConsoleAppendLine("Listelendi.", Color.Green);
                    }
                }
                catch (Exception hata)
                {
                    otherConsoleAppendLine("Listele Hata." + hata.Message, Color.Red);
                }
            }
            else
            {
                otherConsoleAppendLine("Dosya Yolu Boş Kalamaz.", Color.Red);
            }
        }

        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            Guncelle();
            Listele();
        }

        public void Guncelle()
        {
            if (txtReceteName.Text != "")
            {
                if (Convert.ToDouble(tbwPos1.Text) > 200 || Convert.ToDouble(tbwPos2.Text) > 200 || Convert.ToDouble(tbwPos3.Text) > 200
                    || Convert.ToDouble(tbwPos4.Text) > 200 || Convert.ToDouble(tbwPos5.Text) > 200 || Convert.ToDouble(tbwPos6.Text) > 200
                    || Convert.ToDouble(tbwPos7.Text) > 200 || Convert.ToDouble(tbwPos8.Text) > 200 || Convert.ToDouble(tbwPos9.Text) > 200
                    || Convert.ToDouble(tbwPos10.Text) > 200 || Convert.ToDouble(tbwPos11.Text) > 200 || Convert.ToDouble(tbwPos12.Text) > 200)
                {
                    MessageBox.Show("WPos Değer Aralığı 200'den büyük olamaz.");
                    otherConsoleAppendLine("WPos Değer Aralığı 200'den büyük olamaz.", Color.Red);
                }
                else if (Convert.ToDouble(tbwPos1.Text) < -20 || Convert.ToDouble(tbwPos2.Text) < -20 || Convert.ToDouble(tbwPos3.Text) < -20
                     || Convert.ToDouble(tbwPos4.Text) < -20 || Convert.ToDouble(tbwPos5.Text) < -20 || Convert.ToDouble(tbwPos6.Text) < -20
                     || Convert.ToDouble(tbwPos7.Text) < -20 || Convert.ToDouble(tbwPos8.Text) < -20 || Convert.ToDouble(tbwPos9.Text) < -20
                     || Convert.ToDouble(tbwPos10.Text) < -20 || Convert.ToDouble(tbwPos11.Text) < -20 || Convert.ToDouble(tbwPos12.Text) < -20)
                {
                    MessageBox.Show("WPos Değer Aralığı -20'den küçük olamaz.");
                    otherConsoleAppendLine("WPos Değer Aralığı -20'den küçük olamaz.", Color.Red);
                }
                else if (Convert.ToDouble(tbzPos1.Text) > 640 || Convert.ToDouble(tbzPos2.Text) > 640 || Convert.ToDouble(tbzPos3.Text) > 640
                    || Convert.ToDouble(tbzPos4.Text) > 640 || Convert.ToDouble(tbzPos5.Text) > 640 || Convert.ToDouble(tbzPos6.Text) > 640
                    || Convert.ToDouble(tbzPos7.Text) > 640 || Convert.ToDouble(tbzPos8.Text) > 640 || Convert.ToDouble(tbzPos9.Text) > 640
                    || Convert.ToDouble(tbzPos10.Text) > 640 || Convert.ToDouble(tbzPos11.Text) > 640 || Convert.ToDouble(tbzPos12.Text) > 640)
                {
                    MessageBox.Show("ZPos Değer Aralığı 640'dan büyük olamaz.");
                    otherConsoleAppendLine("ZPos Değer Aralığı 640'dan büyük olamaz.", Color.Red);
                }
                else if (Convert.ToDouble(tbzPos1.Text) < 0 || Convert.ToDouble(tbzPos2.Text) < 0 || Convert.ToDouble(tbzPos3.Text) < 0
                     || Convert.ToDouble(tbzPos4.Text) < 0 || Convert.ToDouble(tbzPos5.Text) < 0 || Convert.ToDouble(tbzPos6.Text) < 0
                     || Convert.ToDouble(tbzPos7.Text) < 0 || Convert.ToDouble(tbzPos8.Text) < 0 || Convert.ToDouble(tbzPos9.Text) < 0
                     || Convert.ToDouble(tbzPos10.Text) < 0 || Convert.ToDouble(tbzPos11.Text) < 0 || Convert.ToDouble(tbzPos12.Text) < 0)
                {
                    MessageBox.Show("ZPos Değer Aralığı 0'dan küçük olamaz.");
                    otherConsoleAppendLine("ZPos Değer Aralığı 0'dan küçük olamaz.", Color.Red);
                }
                else
                {
                    try
                    {
                        INIKaydet ini = new INIKaydet(txtReceteName.Text);
                        ini.Yaz("photoNum", "Metin Kutusu", Convert.ToString(photoNumCb.Text));

                        ini.Yaz("wPos1", "Metin Kutusu", Convert.ToString(tbwPos1.Text));
                        ini.Yaz("wPos2", "Metin Kutusu", Convert.ToString(tbwPos2.Text));
                        ini.Yaz("wPos3", "Metin Kutusu", Convert.ToString(tbwPos3.Text));
                        ini.Yaz("wPos4", "Metin Kutusu", Convert.ToString(tbwPos4.Text));
                        ini.Yaz("wPos5", "Metin Kutusu", Convert.ToString(tbwPos5.Text));
                        ini.Yaz("wPos6", "Metin Kutusu", Convert.ToString(tbwPos6.Text));
                        ini.Yaz("wPos7", "Metin Kutusu", Convert.ToString(tbwPos7.Text));
                        ini.Yaz("wPos8", "Metin Kutusu", Convert.ToString(tbwPos8.Text));
                        ini.Yaz("wPos9", "Metin Kutusu", Convert.ToString(tbwPos9.Text));
                        ini.Yaz("wPos10", "Metin Kutusu", Convert.ToString(tbwPos10.Text));
                        ini.Yaz("wPos11", "Metin Kutusu", Convert.ToString(tbwPos11.Text));
                        ini.Yaz("wPos12", "Metin Kutusu", Convert.ToString(tbwPos12.Text));

                        ini.Yaz("zPos1", "Metin Kutusu", Convert.ToString(tbzPos1.Text));
                        ini.Yaz("zPos2", "Metin Kutusu", Convert.ToString(tbzPos2.Text));
                        ini.Yaz("zPos3", "Metin Kutusu", Convert.ToString(tbzPos3.Text));
                        ini.Yaz("zPos4", "Metin Kutusu", Convert.ToString(tbzPos4.Text));
                        ini.Yaz("zPos5", "Metin Kutusu", Convert.ToString(tbzPos5.Text));
                        ini.Yaz("zPos6", "Metin Kutusu", Convert.ToString(tbzPos6.Text));
                        ini.Yaz("zPos7", "Metin Kutusu", Convert.ToString(tbzPos7.Text));
                        ini.Yaz("zPos8", "Metin Kutusu", Convert.ToString(tbzPos8.Text));
                        ini.Yaz("zPos9", "Metin Kutusu", Convert.ToString(tbzPos9.Text));
                        ini.Yaz("zPos10", "Metin Kutusu", Convert.ToString(tbzPos10.Text));
                        ini.Yaz("zPos11", "Metin Kutusu", Convert.ToString(tbzPos11.Text));
                        ini.Yaz("zPos12", "Metin Kutusu", Convert.ToString(tbzPos12.Text));
                        ini.Yaz("photoNum", "Metin Kutusu", Convert.ToString(photoNumCb.Text));

                        otherConsoleAppendLine("Güncellendi.", Color.Green);
                    }
                    catch (Exception hata)
                    {
                        otherConsoleAppendLine("Güncelle Hata." + hata.Message, Color.Red);
                    }
                }
            }
            else
            {
                otherConsoleAppendLine("Dosya Yolu Boş Kalamaz.", Color.Red);
            }
        }

        private void btnCreateExcel_Click(object sender, EventArgs e)
        {
            textCreate();
            Listele();
        }

        public void textCreate()
        {
            try
            {
                if (tbNewReceteName.Text != "")
                {
                    string path = Prog_Ayarlar.Default.txtReceteDosyaYolu + tbNewReceteName.Text + ".txt";                                                                                          //  string path = Prog_Ayarlar.Default.txtSQLOnOffDosyaYolu + tbNewReceteName.Text + ".xls";
                    StreamWriter FileWrite = new StreamWriter(path);
                    FileWrite.Close();
                    List<string> lines = new List<string>();
                    lines = File.ReadAllLines(Prog_Ayarlar.Default.txtReceteDosyaYolu + "kaynak.txt").ToList();
                    File.WriteAllLines(path, lines);
                    this.txtReceteName.Text = path;
                    txtReceteName.Text = path;
                }
                else
                {
                    otherConsoleAppendLine("Reçete İsmi Boş Kalamaz.", Color.Red);
                }
            }
            catch (Exception ex)
            {
                // ConsoleAppendLine("textCreate: " + ex.Message, Color.Red);
            }
        }

        private void btnExcelSec_Click(object sender, EventArgs e)
        {
            btnExcelSecClick();
        }

        private void btnExcelSec_2_Click(object sender, EventArgs e)
        {
            btnExcelSecClick();
        }

        public void btnExcelSecClick()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Database Dosyaları|*.txt|Tüm Dosyalar|*.*";
            if (openFileDialog.ShowDialog() != DialogResult.OK)
                return;
            this.txtReceteName.Text = openFileDialog.FileName;
            txtReceteName.Text = openFileDialog.FileName;
            Listele();
            tbNewReceteName.Text = "";

            receteName = txtReceteName.Text;
            String[] splitText1 = receteName.Split(new char[] { '\\', '\t' });
            receteName = splitText1[5];
            String[] splitText2 = receteName.Split(new char[] { '.', '\t' });
            receteName = splitText2[0];
            otherConsoleAppendLine(receteName, Color.Green);
            otherConsoleAppendLine(receteName, Color.Green);
            txtActiveRecete.Text = receteName;
        }

        private void photoNumCb_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbwPos1.Enabled = false;
            tbwPos2.Enabled = false;
            tbwPos3.Enabled = false;
            tbwPos4.Enabled = false;
            tbwPos5.Enabled = false;
            tbwPos6.Enabled = false;
            tbwPos7.Enabled = false;
            tbwPos8.Enabled = false;
            tbwPos9.Enabled = false;
            tbwPos10.Enabled = false;
            tbwPos11.Enabled = false;
            tbwPos12.Enabled = false;

            tbzPos1.Enabled = false;
            tbzPos2.Enabled = false;
            tbzPos3.Enabled = false;
            tbzPos4.Enabled = false;
            tbzPos5.Enabled = false;
            tbzPos6.Enabled = false;
            tbzPos7.Enabled = false;
            tbzPos8.Enabled = false;
            tbzPos9.Enabled = false;
            tbzPos10.Enabled = false;
            tbzPos11.Enabled = false;
            tbzPos12.Enabled = false;
            if (Convert.ToInt32(photoNumCb.Text) > 0)
            {
                tbwPos1.Enabled = true;
                tbzPos1.Enabled = true;
            }
            if (Convert.ToInt32(photoNumCb.Text) > 1)
            {
                tbwPos2.Enabled = true;
                tbzPos2.Enabled = true;
            }
            if (Convert.ToInt32(photoNumCb.Text) > 2)
            {
                tbwPos3.Enabled = true;
                tbzPos3.Enabled = true;
            }
            if (Convert.ToInt32(photoNumCb.Text) > 3)
            {
                tbwPos4.Enabled = true;
                tbzPos4.Enabled = true;
            }
            if (Convert.ToInt32(photoNumCb.Text) > 4)
            {
                tbwPos5.Enabled = true;
                tbzPos5.Enabled = true;
            }
            if (Convert.ToInt32(photoNumCb.Text) > 5)
            {
                tbwPos6.Enabled = true;
                tbzPos6.Enabled = true;
            }
            if (Convert.ToInt32(photoNumCb.Text) > 6)
            {
                tbwPos7.Enabled = true;
                tbzPos7.Enabled = true;
            }
            if (Convert.ToInt32(photoNumCb.Text) > 7)
            {
                tbwPos8.Enabled = true;
                tbzPos8.Enabled = true;
            }
            if (Convert.ToInt32(photoNumCb.Text) > 8)
            {
                tbwPos9.Enabled = true;
                tbzPos9.Enabled = true;
            }
            if (Convert.ToInt32(photoNumCb.Text) > 9)
            {
                tbwPos10.Enabled = true;
                tbzPos10.Enabled = true;
            }
            if (Convert.ToInt32(photoNumCb.Text) > 10)
            {
                tbwPos11.Enabled = true;
                tbzPos11.Enabled = true;
            }
            if (Convert.ToInt32(photoNumCb.Text) > 11)
            {
                tbwPos12.Enabled = true;
                tbzPos12.Enabled = true;
            }
        }

        /********************************************** Diğer Konsol-1 ************************************************/
        private void rtbConsoleOther_TextChanged(object sender, EventArgs e)
        {
            RichTextBox rtb2 = sender as RichTextBox;
            rtb2.SelectionStart = rtb2.Text.Length;
            rtb2.ScrollToCaret();
        }

        /*Kullanıcı Arayüzüne Temizlenir*/
        public void otherConsoleClean()
        {
            if (rtbConsoleOther.InvokeRequired)
            {
                rtbConsoleOther.Invoke(new Action(delegate ()
                {
                    rtbConsoleOther.Text = "";
                    rtbConsoleOther.Select(rtbConsoleOther.TextLength, 0);
                    rtbConsoleOther.SelectionColor = Color.White;
                }));
            }
            else
            {
                rtbConsoleOther.Text = "";
                rtbConsoleOther.Select(rtbConsoleOther.TextLength, 0);
                rtbConsoleOther.SelectionColor = Color.White;
            }
        }

        /*Kullanıcı Arayüzüne Yazı Yazılır*/
        public void otherConsoleAppendLine(string text, Color color)
        {
            if (rtbConsoleOther.InvokeRequired)
            {
                rtbConsoleOther.Invoke(new Action(delegate ()
                {
                    rtbConsoleOther.Select(rtbConsoleOther.TextLength, 0);
                    rtbConsoleOther.SelectionColor = color;
                    rtbConsoleOther.AppendText(text + Environment.NewLine);
                    rtbConsoleOther.Select(rtbConsoleOther.TextLength, 0);
                    rtbConsoleOther.SelectionColor = Color.White;
                }));
            }
            else
            {
                rtbConsoleOther.Select(rtbConsoleOther.TextLength, 0);
                rtbConsoleOther.SelectionColor = color;
                rtbConsoleOther.AppendText(text + Environment.NewLine);
                rtbConsoleOther.Select(rtbConsoleOther.TextLength, 0);
                rtbConsoleOther.SelectionColor = Color.White;
            }
            otherConsoleAppendLine_2(text, color);
        }

        /*Kullanıcı Arayüzünde Bir Satır Boşluk Bırakılır*/
        public void otherConsoleNewLine()
        {
            if (rtbConsoleOther.InvokeRequired)
            {
                rtbConsoleOther.Invoke(new Action(delegate () { rtbConsoleOther.AppendText(Environment.NewLine); }));
            }
            else
            {
                rtbConsoleOther.AppendText(Environment.NewLine);
            }
        }

        /********************************************** Diğer Konsol-2 ************************************************/
        private void rtbConsoleOther_2_TextChanged(object sender, EventArgs e)
        {
            RichTextBox rtb2 = sender as RichTextBox;
            rtb2.SelectionStart = rtb2.Text.Length;
            rtb2.ScrollToCaret();
        }

        public void otherConsoleClean_2()
        {
            if (rtbConsoleOther_2.InvokeRequired)
            {
                rtbConsoleOther_2.Invoke(new Action(delegate ()
                {
                    rtbConsoleOther_2.Text = "";
                    rtbConsoleOther_2.Select(rtbConsoleOther_2.TextLength, 0);
                    rtbConsoleOther_2.SelectionColor = Color.White;
                }));
            }
            else
            {
                rtbConsoleOther_2.Text = "";
                rtbConsoleOther_2.Select(rtbConsoleOther_2.TextLength, 0);
                rtbConsoleOther_2.SelectionColor = Color.White;
            }
        }

        /*Kullanıcı Arayüzüne Yazı Yazılır*/
        public void otherConsoleAppendLine_2(string text, Color color)
        {
            if (rtbConsoleOther_2.InvokeRequired)
            {
                rtbConsoleOther_2.Invoke(new Action(delegate ()
                {
                    rtbConsoleOther_2.Select(rtbConsoleOther_2.TextLength, 0);
                    rtbConsoleOther_2.SelectionColor = color;
                    rtbConsoleOther_2.AppendText(text + Environment.NewLine);
                    rtbConsoleOther_2.Select(rtbConsoleOther_2.TextLength, 0);
                    rtbConsoleOther_2.SelectionColor = Color.White;
                }));
            }
            else
            {
                rtbConsoleOther_2.Select(rtbConsoleOther_2.TextLength, 0);
                rtbConsoleOther_2.SelectionColor = color;
                rtbConsoleOther_2.AppendText(text + Environment.NewLine);
                rtbConsoleOther_2.Select(rtbConsoleOther_2.TextLength, 0);
                rtbConsoleOther_2.SelectionColor = Color.White;
            }
        }

        /*Kullanıcı Arayüzünde Bir Satır Boşluk Bırakılır*/
        public void otherConsoleNewLine_2()
        {
            if (rtbConsoleOther_2.InvokeRequired)
            {
                rtbConsoleOther_2.Invoke(new Action(delegate () { rtbConsoleOther_2.AppendText(Environment.NewLine); }));
            }
            else
            {
                rtbConsoleOther_2.AppendText(Environment.NewLine);
            }
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            if (nxCompoletBoolRead("startButton") == false)
            {
                otherConsoleAppendLine("startButton True", Color.Orange);
                nxCompoletBoolWrite("startButton", true);
            }
            else
            {
                otherConsoleAppendLine("startButton False", Color.Orange);
                nxCompoletBoolWrite("startButton", false);
            }
        }

        private void betterBtn_Click(object sender, EventArgs e)
        {
            tbwPos1.Text = "90";
            tbwPos2.Text = "90";
            tbwPos3.Text = "90";
            tbwPos4.Text = "90";
            tbwPos5.Text = "0";
            tbwPos6.Text = "0";
            tbwPos7.Text = "0";
            tbwPos8.Text = "0";
            tbwPos9.Text = "0";
            tbwPos10.Text = "0";
            tbwPos11.Text = "0";
            tbwPos12.Text = "0";

            tbzPos1.Text = "110";
            tbzPos2.Text = "290";
            tbzPos3.Text = "330";
            tbzPos4.Text = "515";
            tbzPos5.Text = "515";
            tbzPos6.Text = "330";
            tbzPos7.Text = "210";
            tbzPos8.Text = "110";
            tbzPos9.Text = "0";
            tbzPos10.Text = "0";
            tbzPos11.Text = "0";
            tbzPos12.Text = "0";

            photoNumCb.Text = "8";
            Guncelle();
        }

        private void ankastreBtn_Click(object sender, EventArgs e)
        {
            tbwPos1.Text = "0";
            tbwPos2.Text = "0";
            tbwPos3.Text = "0";
            tbwPos4.Text = "0";
            tbwPos5.Text = "0";
            tbwPos6.Text = "0";
            tbwPos7.Text = "0";
            tbwPos8.Text = "0";
            tbwPos9.Text = "0";
            tbwPos10.Text = "0";
            tbwPos11.Text = "0";
            tbwPos12.Text = "0";

            tbzPos1.Text = "0";
            tbzPos2.Text = "0";
            tbzPos3.Text = "0";
            tbzPos4.Text = "0";
            tbzPos5.Text = "515";
            tbzPos6.Text = "0";
            tbzPos7.Text = "320";
            tbzPos8.Text = "110";
            tbzPos9.Text = "0";
            tbzPos10.Text = "0";
            tbzPos11.Text = "0";
            tbzPos12.Text = "0";

            photoNumCb.Text = "8";
            Guncelle();
        }

		private void btn535_Click(object sender, EventArgs e)
		{
            tbwPos1.Text = "85";
            tbwPos2.Text = "85";
            tbwPos3.Text = "0";
            tbwPos4.Text = "85";
            tbwPos5.Text = "90";
            tbwPos6.Text = "0";
            tbwPos7.Text = "0";
            tbwPos8.Text = "90";
            tbwPos9.Text = "0";
            tbwPos10.Text = "0";
            tbwPos11.Text = "0";
            tbwPos12.Text = "0";

            tbzPos1.Text = "130";
            tbzPos2.Text = "332";
            tbzPos3.Text = "0";
            tbzPos4.Text = "520";
            tbzPos5.Text = "520";
            tbzPos6.Text = "0";
            tbzPos7.Text = "0";
            tbzPos8.Text = "130";
            tbzPos9.Text = "0";
            tbzPos10.Text = "0";
            tbzPos11.Text = "0";
            tbzPos12.Text = "0";

            photoNumCb.Text = "8";
            Guncelle();
        }

		private void btn536_Click(object sender, EventArgs e)
		{
            tbwPos1.Text = "0";
            tbwPos2.Text = "0";
            tbwPos3.Text = "0";
            tbwPos4.Text = "0";
            tbwPos5.Text = "85";
            tbwPos6.Text = "0";
            tbwPos7.Text = "85";
            tbwPos8.Text = "85";
            tbwPos9.Text = "0";
            tbwPos10.Text = "0";
            tbwPos11.Text = "0";
            tbwPos12.Text = "0";

            tbzPos1.Text = "0";
            tbzPos2.Text = "0";
            tbzPos3.Text = "0";
            tbzPos4.Text = "0";
            tbzPos5.Text = "520";
            tbzPos6.Text = "0";
            tbzPos7.Text = "332";
            tbzPos8.Text = "130";
            tbzPos9.Text = "0";
            tbzPos10.Text = "0";
            tbzPos11.Text = "0";
            tbzPos12.Text = "0";

            photoNumCb.Text = "8";
            Guncelle();
        }
	}
}
#endregion
