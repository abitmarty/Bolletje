using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BolletjeBolletje
{
    class SettingsInitials
    {
        private String p1Name;
        private String p2Name;
        private string p1Icon;
        private string p2Icon;
        private int tilesX;
        private int tilesY;

        private int minimalX = 2;
        private int minimalY = 2;

        public SettingsInitials(String p1Name, String p2Name, int tilesX, int tilesY, string p1Icon, string p2Icon)
        {
            this.setP1Name(p1Name);
            this.setP2Name(p2Name);
            this.setTilesX(tilesX);
            this.setTilesY(tilesY);

            this.setP1Icon(p1Icon);
            this.setP2Icon(p2Icon);
        }

        public String getP1Name()
        {
            return this.p1Name;
        }

        public void setP1Name(String p1Name)
        {
            this.p1Name = p1Name;
        }

        public String getP2Name()
        {
            return this.p2Name;
        }

        public void setP2Name(String p2Name)
        {
            this.p2Name = p2Name;
        }

        public int getTilesX()
        {
            return this.tilesX;
        }

        public void setTilesX(int tilesX)
        {
            if (tilesX < this.minimalX)
            {
                this.tilesX = this.minimalX;
            }
            else
            {
                this.tilesX = tilesX;
            }
        }

        public int getTilesY()
        {
            return this.tilesY;
        }

        public void setTilesY(int tilesY)
        {
            if (tilesY < this.tilesY)
            {
                this.tilesY = this.minimalY;
            }
            else
            {
                this.tilesY = tilesY;
            }
        }

        public String getP1Icon()
        {
            return this.p1Icon;
        }

        public void setP1Icon(String p1Icon)
        {
            this.p1Icon = p1Icon;
        }

        public String getP2Icon()
        {
            return this.p2Icon;
        }

        public void setP2Icon(String p2Icon)
        {
            this.p2Icon = p2Icon;
        }
    }
}
