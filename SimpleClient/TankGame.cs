using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace SimpleClient
{
    public partial class TankGame : Form
    {
        SimpleClient Client;

        //Game Info
        Dictionary<string, Point> clientGameTank = new Dictionary<string, Point>();
        Dictionary<string, string> tankSprite = new Dictionary<string, string>();
     
        public TankGame(object client)
        {
            InitializeComponent();
            Client = (SimpleClient)client;
        }

        private void TankGame_Load(object sender, EventArgs e)
        {
            
        }

        public void UpdateDictionaryInfo(Dictionary<string, Point> clientGameInfoDictionary)
        {
            clientGameTank = clientGameInfoDictionary;
        }

        public void UpdateSpriteInfo(Dictionary<string, string> spriteInfoDictionary)
        {
            tankSprite = spriteInfoDictionary;
        }

        private void TankGame_Paint(object sender, PaintEventArgs e)
        {
            for(int i = 0; i < clientGameTank.Count; i++ )
            {
                e.Graphics.DrawImage(new Bitmap("TankSprite.png"), clientGameTank[clientGameTank.ElementAt(i).Key].X, clientGameTank[clientGameTank.ElementAt(i).Key].Y);
            }
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.Invalidate();
        }

        public void ForcePaint()
        {
            this.Invalidate();
        }

   
        private void TankGame_KeyDown(object sender, KeyEventArgs e)
        {

            switch (e.KeyValue)
            {
                case 87: //W
                    Client.UDPClientSend(new GameMovePacket("Upwards"));
                    break;
                case 65://A
                    Client.UDPClientSend(new GameMovePacket("Left"));
                    break;
                case 68://D
                    Client.UDPClientSend(new GameMovePacket("Right"));
                    break;
                case 83: //S
                    Client.UDPClientSend(new GameMovePacket("Downwards"));
                    break;

            }
        }
    }
}
