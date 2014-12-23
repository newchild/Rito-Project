using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.IO;
using Newtonsoft.Json;

namespace RITO_PLOX
{
    public partial class Ritoconnector : Form
    {
        public Ritoconnector()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void Ritoconnector_Load(object sender, EventArgs e)
        {
            GetInfoButton.Visible = false;
            Matches.Visible = false;

        }

        private void ConnectButton_Click(object sender, EventArgs e)
        {
            string connecturl = "https://euw.api.pvp.net/api/lol/euw/v1.4/summoner/by-name/" + UsernameBox.Text + "?api_key=" + getAPIKey();
            System.Net.HttpWebRequest Connector =  (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(connecturl);
            Connector.Method = System.Net.WebRequestMethods.Http.Get;
            Connector.Accept = "application/json";
            GetInfoButton.Visible = true;
            Matches.Visible = true;
            string text;
            var Response = Connector.GetResponse();
            using (var sr = new StreamReader(Response.GetResponseStream()))
            {
                text = sr.ReadToEnd();
            }
            Champion user;
            user = (Champion)JsonConvert.DeserializeObject(text, typeof(Champion));
            MessageBox.Show(user.Summoners.Name);
            
            
        }

        private string getAPIKey()
        {
            if (KeyBox.Text != "")
            {
                return KeyBox.Text;
            }
            else
            {
                return "64becc79-cc38-40e1-afdf-a92b95b4c836";
            }
        }
    }
}
