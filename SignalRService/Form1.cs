using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Owin.Hosting;

namespace SignalRService
{
    public partial class Form1 : Form
    {
        public IDisposable SignalR { get; set; }
        private const string ServerUri = "http://localhost:22641"; // SignalR服務地址

        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 啟動服務
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStart_Click(object sender, EventArgs e)
        {
            btnStart.Enabled = false;

            WriteToConsole("正在啟動服務...");
            Task.Run(() => { btnStart.Enabled = !StartServer(); }); // 異步啟動SignalR服務

            //Task.Run(() => StartServer()); // 異步啟動SignalR服務
            //Task.Run(() =>
            //{
            //    BtnStart.Enabled = !StartServer();
            //bool flag = StartServer();
            //BtnStart.Invoke(new Action<bool>((f) => BtnStart.Enabled = !f), flag);

            // }); // 異步啟動SignalR服務

            //BtnStart.Enabled = !StartServer();
        }

        /// <summary>
        /// 停止服務
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStop_Click(object sender, EventArgs e)
        {
            SignalR.Dispose();
            Close();
        }

        /// <summary>
        /// 啟動SignalR服務，將SignalR服務寄宿在WPF進程中
        /// </summary>
        private bool StartServer()
        {
            try
            {
                SignalR = WebApp.Start(ServerUri);  // 啟動SignalR服務
            }
            catch (Exception ex)
            {
                WriteToConsole(ex.Message);
                return false;
            }

            WriteToConsole("服務已經成功啟動，地址為：" + ServerUri);
            return true;
        }

        /// <summary>
        /// 將消息添加到消息列表中
        /// </summary>
        /// <param name="message"></param>
        public void WriteToConsole(string message)
        {
            if (TxtConsole.InvokeRequired)
            {
                TxtConsole.Invoke(new Action<string>((string msg) => TxtConsole.AppendText(message + Environment.NewLine)), message);
                return;
            }

            TxtConsole.AppendText(message + Environment.NewLine);
        }
    }
}