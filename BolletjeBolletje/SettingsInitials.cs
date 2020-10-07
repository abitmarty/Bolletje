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

        public SettingsInitials(String p1Name, String p2Name)
        {
            this.setP1Name(p1Name);
            this.setP2Name(p2Name);
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
    }
}
