using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.AspNet.SignalR.Client;
using Newtonsoft.Json;

namespace SignalRDemoWinForm
{
    public partial class Form1 : Form
    {
        private const string SignalRurl = "http://localhost:22640/";
        private readonly HubConnection _conn;
        private readonly IHubProxy _chatHub;

        public Form1()
        {
            InitializeComponent();
            _conn = new HubConnection(SignalRurl);
            _chatHub = _conn.CreateHubProxy("chatHub");
            _conn.Received += ConnectionReceived;
            _conn.Closed += ConnectionClosed;
        }

        private void ConnectionClosed()
        {
            MessageBox.Show(@"您已離開聊天室");
        }

        private int y = 25;

        private void ConnectionReceived(string obj)
        {
            dynamic json = JsonConvert.DeserializeObject(obj);
            if (json.M == "addMessage")
            {
                // 更新 WinForm 畫面
                DoUiCallBack(
                    () =>
                    {
                        // 在畫面上加一個 label 顯示文字訊息
                        var lb = new Label
                        {
                            Location = new Point(15, y += 25),
                            Text = json.A[0],
                            ForeColor = Color.Blue,
                            Width = 200
                        };
                        this.Controls.Add(lb);
                    }
                );
            }

            if (json.M == "showSomething")
            {
                var fakeInfo = $"{label1.Text}的資訊：XXXXXXXXXXXXXXXXX";
                _chatHub.Invoke("sendMessage", fakeInfo);

                MessageBox.Show(@"有人正在查詢你的資料喔");
            }
        }

        private void btnDisConnect_Click(object sender, EventArgs e)
        {
            try
            {
                _chatHub.Invoke("sendMessage", "小叮噹離開了聊天室").ContinueWith(
                    task =>
                    {
                        new Task(() => { _conn.Stop(); }).Start();
                    }
                );
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            MessageBox.Show(@"加入聊天室");

            _conn.Start().ContinueWith(task =>
            {
                if (task.IsFaulted) return;

                _chatHub.Invoke("sendMessage", "小叮噹加入了聊天室");
            });
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            var msg = $"{label1.Text}：{textBox1.Text}";
            textBox1.Text = string.Empty;

            Chat(msg);
        }

        private void Chat(string msg)
        {
            try
            {
                _chatHub.Invoke("sendMessage", msg);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void DoUiCallBack(UiCallBack cb)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new UiCallBack(
                    cb.Invoke
                ));
            }
        }

        private delegate void UiCallBack();
    }
}