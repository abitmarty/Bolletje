using System;

namespace BolletjeBolletje
{
    class SettingsInitials
    {
        // Set values
        private String p1Name;
        private String p2Name;
        private string p1Icon;
        private string p2Icon;
        private int tilesX;
        private int tilesY;
        private int tileSizeX;
        private int tileSizeY;

        // Minmal values
        // These come back in the setters
        // If the values are too small these values will take place
        private int minimalX = 2;
        private int minimalY = 2;
        private int minimalTileSizeX = 51;
        private int minimalTileSizeY = 51;

        public SettingsInitials(String p1Name, String p2Name, int tilesX, int tilesY, string p1Icon, string p2Icon, int tileSizeX, int tileSizeY)
        {
            this.setP1Name(p1Name);
            this.setP2Name(p2Name);
            this.setTilesX(tilesX);
            this.setTilesY(tilesY);
            this.setP1Icon(p1Icon);
            this.setP2Icon(p2Icon);

            this.setTileSizeX(tileSizeX);
            this.setTileSizeY(tileSizeY);

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
            if (tilesY < this.minimalY)
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

        public int getTileSizeX()
        {
            return this.tileSizeX;
        }

        public void setTileSizeX(int tileSizeX)
        {
            if (tileSizeX < this.minimalTileSizeX)
            {
                this.tileSizeX = this.minimalTileSizeX;
            }
            else
            {
                this.tileSizeX = tileSizeX;
            }
        }
        public int getTileSizeY()
        {
            return this.tileSizeY;
        }

        public void setTileSizeY(int tileSizeY)
        {
            if (tileSizeY < this.minimalTileSizeY)
            {
                this.tileSizeY = this.minimalTileSizeY;
            }
            else
            {
                this.tileSizeY = tileSizeY;
            }
        }
    }
}
