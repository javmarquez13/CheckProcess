using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using PalletLinkDLL;

namespace CheckProcess
{
    public class MultiTaskingMES
    {
        public static async Task SendStepToMES_15(string[] SerialNumbers, string StepToSend)
        {
            string _result = string.Empty;
            string horaInicial = DateTime.Now.ToString("MM/dd/yyyy h:mm:ss tt");
            string horaFinal = DateTime.Now.ToString("MM/dd/yyyy h:mm:ss tt");

            foreach(string SerialNumber in SerialNumbers) 
            {
                string archivoMES = string.Format("S{0}\r\nC{1}\r\nN{2}\r\nOoperador\r\np{3}\r\nP{4}\r\nTP\r\n[{5}\r\n]{6}\r\n", SerialNumber, "DEXCOM", "WM-AQST200-09", 12, StepToSend, horaInicial, horaFinal);

                _result = new PalletLinkDLL.PalletLinkSN().fnSendToMES(archivoMES, SerialNumber);
              
            }

            MessageBox.Show("Ya termine_15");
        }


        public static async Task SendStepToMES_30(string[] SerialNumbers, string StepToSend)
        {
            string _result = string.Empty;
            string horaInicial = DateTime.Now.ToString("MM/dd/yyyy h:mm:ss tt");
            string horaFinal = DateTime.Now.ToString("MM/dd/yyyy h:mm:ss tt");

            foreach (string SerialNumber in SerialNumbers)
            {
                string archivoMES = string.Format("S{0}\r\nC{1}\r\nN{2}\r\nOoperador\r\np{3}\r\nP{4}\r\nTP\r\n[{5}\r\n]{6}\r\n", SerialNumber, "DEXCOM", "WM-AQST200-09", 12, StepToSend, horaInicial, horaFinal);

                _result = new PalletLinkDLL.PalletLinkSN().fnSendToMES(archivoMES, SerialNumber);
            }

            MessageBox.Show("Ya termine_30");
        }


        public static async Task SendStepToMES_45(string[] SerialNumbers, string StepToSend)
        {
            string _result = string.Empty;
            string horaInicial = DateTime.Now.ToString("MM/dd/yyyy h:mm:ss tt");
            string horaFinal = DateTime.Now.ToString("MM/dd/yyyy h:mm:ss tt");

            foreach (string SerialNumber in SerialNumbers)
            {
                string archivoMES = string.Format("S{0}\r\nC{1}\r\nN{2}\r\nOoperador\r\np{3}\r\nP{4}\r\nTP\r\n[{5}\r\n]{6}\r\n", SerialNumber, "DEXCOM", "WM-AQST200-09", 12, StepToSend, horaInicial, horaFinal);

                _result = new PalletLinkDLL.PalletLinkSN().fnSendToMES(archivoMES, SerialNumber);
              
            }

            MessageBox.Show("Ya termine_45");
        }


        public static async Task SendStepToMES_60(string[] SerialNumbers, string StepToSend)
        {
            string _result = string.Empty;
            string horaInicial = DateTime.Now.ToString("MM/dd/yyyy h:mm:ss tt");
            string horaFinal = DateTime.Now.ToString("MM/dd/yyyy h:mm:ss tt");

            foreach (string SerialNumber in SerialNumbers)
            {
                string archivoMES = string.Format("S{0}\r\nC{1}\r\nN{2}\r\nOoperador\r\np{3}\r\nP{4}\r\nTP\r\n[{5}\r\n]{6}\r\n", SerialNumber, "DEXCOM", "WM-AQST200-09", 12, StepToSend, horaInicial, horaFinal);

                _result = new PalletLinkDLL.PalletLinkSN().fnSendToMES(archivoMES, SerialNumber);
                
            }

            MessageBox.Show("Ya termine_60");
        }


        public static async Task SendStepToMES_75(string[] SerialNumbers, string StepToSend)
        {
            string _result = string.Empty;
            string horaInicial = DateTime.Now.ToString("MM/dd/yyyy h:mm:ss tt");
            string horaFinal = DateTime.Now.ToString("MM/dd/yyyy h:mm:ss tt");

            foreach (string SerialNumber in SerialNumbers)
            {
                string archivoMES = string.Format("S{0}\r\nC{1}\r\nN{2}\r\nOoperador\r\np{3}\r\nP{4}\r\nTP\r\n[{5}\r\n]{6}\r\n", SerialNumber, "DEXCOM", "WM-AQST200-09", 12, StepToSend, horaInicial, horaFinal);



                _result = new PalletLinkDLL.PalletLinkSN().fnSendToMES(archivoMES, SerialNumber);
             
            }

            MessageBox.Show("Ya termine_75");
        }


        public static async Task SendStepToMES_90(string[] SerialNumbers, string StepToSend)
        {
            string _result = string.Empty;
            string horaInicial = DateTime.Now.ToString("MM/dd/yyyy h:mm:ss tt");
            string horaFinal = DateTime.Now.ToString("MM/dd/yyyy h:mm:ss tt");

            foreach (string SerialNumber in SerialNumbers)
            {
                string archivoMES = string.Format("S{0}\r\nC{1}\r\nN{2}\r\nOoperador\r\np{3}\r\nP{4}\r\nTP\r\n[{5}\r\n]{6}\r\n", SerialNumber, "DEXCOM", "WM-AQST200-09", 12, StepToSend, horaInicial, horaFinal);


               
                _result = new PalletLinkDLL.PalletLinkSN().fnSendToMES(archivoMES, SerialNumber);
            }

            MessageBox.Show("Ya termine_90");
        }

    }
}
