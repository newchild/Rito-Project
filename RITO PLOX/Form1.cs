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
        public SummonerDTO user;
        public WebResponse PreviousResponse;
        public WebResponse Response;
        public string path;
        public Ritoconnector()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void Ritoconnector_Load(object sender, EventArgs e)
        {
            this.Text = "SummonerIcon Dumper";
            Region.Text = "Region";
            comboBox1.Items.Add("EUW");
            comboBox1.Items.Add("NA");
            comboBox1.Items.Add("EUNE");
            comboBox1.Items.Add("BR");
            comboBox1.Items.Add("KR");
            comboBox1.Items.Add("LAN");
            comboBox1.Items.Add("LAS");
            comboBox1.Items.Add("OCE");
            comboBox1.Items.Add("TR");
            comboBox1.Items.Add("RU");
        }

        private void ConnectButton_Click(object sender, EventArgs e)
        {
            string region = comboBox1.SelectedItem.ToString().ToLower();
            string connecturl = "https://"+ region + ".api.pvp.net/api/lol/"+ region + "/v1.4/summoner/by-name/" + UsernameBox.Text + "?api_key=" + getAPIKey();
            System.Net.HttpWebRequest Connector =  (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(connecturl);
            Connector.Method = System.Net.WebRequestMethods.Http.Get;
            Connector.Accept = "application/json";
            
            string text;
            try
            {
                Response = Connector.GetResponse();
                PreviousResponse = Response;
            }
            catch (WebException Except)
            {
                if (Except.Status == WebExceptionStatus.ProtocolError)
                {
                        MessageBox.Show("Request to Server failed. Please retry later");
                        System.Threading.Thread.Sleep(1000);
                        goto here;
                }
                
                
                
            } 
           
         
            using (var sr = new StreamReader(Response.GetResponseStream()))
            {
                text = sr.ReadToEnd();
            }
            
            var cleanup = JsonConvert.DeserializeObject<Dictionary<string,object>>(text);
            text = cleanup[UsernameBox.Text.ToLower().Replace(" ",string.Empty)].ToString();
            user = (SummonerDTO)JsonConvert.DeserializeObject<SummonerDTO>(text);
            here:
            string Image = "http://ddragon.leagueoflegends.com/cdn/4.21.5/img/profileicon/" + user.ProfileIconId +".png";
            pictureBox1.ImageLocation = Image;
            
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

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void Region_Click(object sender, EventArgs e)
        {

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        
    }
}
