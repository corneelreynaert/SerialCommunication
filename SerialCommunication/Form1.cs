using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace SerialCommunication
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            timerOefening5.Interval = 1000;
            try
            {
                string[] portNames = SerialPort.GetPortNames().Distinct().ToArray();
                comboBoxPoort.Items.Clear();
                comboBoxPoort.Items.AddRange(portNames);
                if (comboBoxPoort.Items.Count > 0) comboBoxPoort.SelectedIndex = 0;

                comboBoxBaudrate.SelectedIndex = comboBoxBaudrate.Items.IndexOf("115200");
            }
            catch (Exception)
            { }
        }

        private void cboPoort_DropDown(object sender, EventArgs e)
        {
            try
            {
                string selected = (string)comboBoxPoort.SelectedItem;
                string[] portNames = SerialPort.GetPortNames().Distinct().ToArray();

                comboBoxPoort.Items.Clear();
                comboBoxPoort.Items.AddRange(portNames);

                comboBoxPoort.SelectedIndex = comboBoxPoort.Items.IndexOf(selected);
            }
            catch (Exception)
            {
                if (comboBoxPoort.Items.Count > 0) comboBoxPoort.SelectedIndex = 0;
            }
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            try
            {
                if (serialPortArduino.IsOpen)
                {
                    
                    // ik heb een verbinding -> de gebruiker wil deze verbreken
                    serialPortArduino.Close();
                    radioButtonVerbonden.Checked = false;
                    buttonConnect.Text = "Connect";
                    labelStatus.Text = "Status: Disconnected";

                }
                else
                {
                    // ik heb geen verbinding -> de gebruiker wil een verbinding maken
                    serialPortArduino.PortName = (string)comboBoxPoort.SelectedItem;
                    serialPortArduino.BaudRate = Int32.Parse((string)comboBoxBaudrate.SelectedItem);
                    serialPortArduino.DataBits = (int)numericUpDownDatabits.Value;

                    if (radioButtonParityEven.Checked) serialPortArduino.Parity = Parity.Even;
                    else if (radioButtonParityOdd.Checked) serialPortArduino.Parity = Parity.Odd;
                    else if (radioButtonParityNone.Checked) serialPortArduino.Parity = Parity.None;
                    else if (radioButtonParityMark.Checked) serialPortArduino.Parity = Parity.Mark;
                    else if (radioButtonParitySpace.Checked) serialPortArduino.Parity = Parity.Space;

                    if (radioButtonStopbitsNone.Checked) serialPortArduino.StopBits = StopBits.None;
                    else if (radioButtonStopbitsOne.Checked) serialPortArduino.StopBits = StopBits.One;
                    else if (radioButtonStopbitsOnePointFive.Checked) serialPortArduino.StopBits = StopBits.OnePointFive;
                    else if (radioButtonStopbitsTwo.Checked) serialPortArduino.StopBits = StopBits.Two;

                    if (radioButtonHandshakeNone.Checked) serialPortArduino.Handshake = Handshake.None;
                    else if (radioButtonHandshakeRTS.Checked) serialPortArduino.Handshake = Handshake.RequestToSend;
                    else if (radioButtonHandshakeRTSXonXoff.Checked) serialPortArduino.Handshake = Handshake.RequestToSendXOnXOff;
                    else if (radioButtonHandshakeXonXoff.Checked) serialPortArduino.Handshake = Handshake.XOnXOff;

                    serialPortArduino.RtsEnable = checkBoxRtsEnable.Checked;
                    serialPortArduino.DtrEnable = checkBoxDtrEnable.Checked;

                    serialPortArduino.Open();
                    serialPortArduino.ReadTimeout = 1000;
                    string commando = "ping";
                    serialPortArduino.WriteLine(commando);
                    string antwoord = serialPortArduino.ReadLine();
                    antwoord = antwoord.TrimEnd();
                    if (antwoord == "pong")
                    {
                        radioButtonVerbonden.Checked = true;
                        buttonConnect.Text = "Disconnect";
                        labelStatus.Text = "Status: Connected";
                    }
                    else
                    {
                        serialPortArduino.Close();
                        labelStatus.Text = "Error: verkeerd antwoord";
                    }

                }
            }
            catch (Exception exception)
            {
                labelStatus.Text = "Error: " + exception.Message;
                serialPortArduino.Close();
                radioButtonVerbonden.Checked = false;
                buttonConnect.Text = "Connect";

            }
        }

        private void checkBoxDigital2_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (serialPortArduino.IsOpen)
                {
                    string commando; //set d2 high/low
                    if (checkBoxDigital2.Checked) commando = "set d2 high";
                    else commando = "set d2 low";
                    serialPortArduino.WriteLine(commando);
                }
            }
            catch (Exception exception)
            {
                labelStatus.Text = "Error: " + exception.Message;
                serialPortArduino.Close();
                radioButtonVerbonden.Checked = false;
                buttonConnect.Text = "Connect";
            }

        }

        private void checkBoxDigital3_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (serialPortArduino.IsOpen)
                {
                    string commando; //set d3 high/low
                    if (checkBoxDigital3.Checked) commando = "set d3 high";
                    else commando = "set d3 low";
                    serialPortArduino.WriteLine(commando);
                }
            }
            catch (Exception exception)
            {
                labelStatus.Text = "Error: " + exception.Message;
                serialPortArduino.Close();
                radioButtonVerbonden.Checked = false;
                buttonConnect.Text = "Connect";
            }

        }

        private void checkBoxDigital4_CheckedChanged(object sender, EventArgs e)
        {

            try
            {
                if (serialPortArduino.IsOpen)
                {
                    string commando; //set d4 high/low
                    if (checkBoxDigital4.Checked) commando = "set d4 high";
                    else commando = "set d4 low";
                    serialPortArduino.WriteLine(commando);
                }
            }
            catch (Exception exception)
            {
                labelStatus.Text = "Error: " + exception.Message;
                serialPortArduino.Close();
                radioButtonVerbonden.Checked = false;
                buttonConnect.Text = "Connect";
            }
        }






        private void timerOefening5_Tick(object sender, EventArgs e)
        {
            try
            {
                if (!serialPortArduino.IsOpen)
                {
                    labelStatus.Text = "Error: geen verbinding met Arduino";
                    timerOefening5.Stop();
                    return;
                }

                int analogeWaardeA0 = LeesAnalogeWaarde("get a0", "a0");
                int analogeWaardeA1 = LeesAnalogeWaarde("get a1", "a1");

                double ricoGewenst = (45.0 - 5.0) / 1023.0;
                double offsetGewenst = 5.0;
                double gewensteTemperatuur = ricoGewenst * analogeWaardeA0 + offsetGewenst;

                double ricoHuidig = (500.0 - 0.0) / 1023.0;
                double offsetHuidig = 0.0;
                double huidigeTemperatuur = ricoHuidig * analogeWaardeA1 + offsetHuidig;

                labelGewensteTemp.Text = gewensteTemperatuur.ToString("0.0") + " °C";
                labelHuidigeTemp.Text = huidigeTemperatuur.ToString("0.0") + " °C";

                if (huidigeTemperatuur < gewensteTemperatuur)
                {
                    serialPortArduino.WriteLine("set d2 high");
                }
                else
                {
                    serialPortArduino.WriteLine("set d2 low");
                }

                labelStatus.Text = "Status: Connected";
            }
            catch (Exception exception)
            {
                labelStatus.Text = "Error oefening 5: " + exception.Message;

                // Niet meteen de verbinding sluiten.
                // Anders valt de Arduino-verbinding direct weg bij 1 fout antwoord.
            }
        }

        private int LeesAnalogeWaarde(string commando, string pinNaam)
        {
            serialPortArduino.WriteLine(commando);

            for (int i = 0; i < 5; i++)
            {
                string antwoord = serialPortArduino.ReadLine().Trim().ToLower();

                if (antwoord.StartsWith(pinNaam + ":"))
                {
                    string[] delen = antwoord.Split(':');
                    return Convert.ToInt32(delen[1].Trim());
                }
            }

            throw new Exception("Geen geldig antwoord voor " + pinNaam);
        }

        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab == tabPageOefening5)
            {
                timerOefening5.Start();
            }
            else
            {
                timerOefening5.Stop();
            }
        }


    }
}
