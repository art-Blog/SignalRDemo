using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.AspNet.SignalR.Client;

namespace SignalRDemoWinForm
{
    public partial class Form1 : Form
    {
        private const string SignalRurl = "http://localhost:22641/";
        private readonly HubConnection _conn;
        private readonly IHubProxy _chatHub;
        private readonly Dictionary<string, IHubProxy> _hubs;
        private readonly CurrectUser _currectUser;
        private int y = 25;

        public Form1()
        {
            InitializeComponent();
            _currectUser = GetUser();
            InitComboBox();

            _conn = new HubConnection(SignalRurl);
            // v1的測試hub
            _chatHub = _conn.CreateHubProxy("chatHub");
            _chatHub.On<string>("addMessage", (msg) =>
            {
                DoUiCallBack(() =>
                {
                    GenerateNewLabel(msg);
                });
            });

            // v2的測試Hub：應從DB中取得該人員參與的頻道，再加入Hub
            _hubs = GetUserHubs(_currectUser.Channel);

            foreach (var currectHub in _hubs)
            {
                currectHub.Value.On<string>("received", (msg) =>
                {
                    DoUiCallBack(() =>
                    {
                        GenerateNewLabel(msg);
                    });
                });
            }
        }

        private void InitComboBox()
        {
            //TODO:這邊應該要從資料庫中取得使用者應該要加入的Hub
            comboBox1.Items.Clear();
            comboBox1.Items.Add(new ComboboxItem { Text = "chathub" });
            comboBox1.Items.Add(new ComboboxItem { Text = "team1" });
            comboBox1.Items.Add(new ComboboxItem { Text = "leader" });
            comboBox1.Items.Add(new ComboboxItem { Text = "notice" });
            comboBox1.SelectedIndex = 0;
        }

        private CurrectUser GetUser()
        {
            //TODO:這邊應該要從資料庫中取得使用者的資料，包含應該要加入的Hub
            return new CurrectUser
            {
                Name = "張三",
                Channel = new List<ChannelInfo>
                {
                    new ChannelInfo {Name = "team1", Id = 0},
                    new ChannelInfo {Name = "leader", Id = 2},
                    new ChannelInfo {Name = "notice", Id = 3}
                }
            };
        }

        private Dictionary<string, IHubProxy> GetUserHubs(IEnumerable<ChannelInfo> channels)
        {
            var result = new Dictionary<string, IHubProxy>();
            foreach (var info in channels)
            {
                result.Add(info.Name, _conn.CreateHubProxy(info.Name));
            }

            // 這邊是為了 V1 的測試
            result.Add("chathub", _conn.CreateHubProxy("chathub"));

            return result;
        }

        private void GenerateNewLabel(string msg)
        {
            var lb = new Label
            {
                Location = new Point(15, y += 25),
                Text = msg,
                ForeColor = Color.Blue,
                Width = 200
            };
            this.Controls.Add(lb);
        }

        private void btnDisConnect_Click(object sender, EventArgs e)
        {
            try
            {
                _chatHub.Invoke("sendMessage", $"{_currectUser.Name}離開了聊天室");

                foreach (var currectHub in _hubs)
                {
                    currectHub.Value.Invoke("send", $"{_currectUser.Name}離開了聊天室");
                }

                new Task(() => { _conn.Stop(); }).Start();
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            _conn.Start().ContinueWith(task =>
            {
                if (task.IsFaulted) return;
                // V1 版本
                _chatHub.Invoke("sendMessage", $"{_currectUser.Name}加入了聊天室");

                // V2 版本
                foreach (var currectHub in _hubs)
                {
                    currectHub.Value.Invoke("send", $"{_currectUser.Name}加入了聊天室");
                }
            });
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            Chat($"{_currectUser.Name}：{textBox1.Text} at {DateTime.Now:f}");
            textBox1.Text = string.Empty;
        }

        private void Chat(string msg)
        {
            try
            {
                var nowChannel = comboBox1.Text;
                if (!_hubs.ContainsKey(nowChannel)) return;

                if (nowChannel == "chathub")
                {
                    // for v1 group hub test
                    _chatHub.Invoke("sendMessage", msg);
                }
                else
                {
                    // for v2 multi Hub test
                    _hubs[nowChannel].Invoke("send", $"[{nowChannel}]{msg}");
                }
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

        private class ComboboxItem
        {
            public string Text { get; set; }

            public override string ToString()
            {
                return Text;
            }
        }
    }
}