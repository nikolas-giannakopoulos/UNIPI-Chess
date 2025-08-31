using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace skaki
{
    public class Pawn
    {
        public PictureBox image;
        public String eidos;
        public bool eaten = false;
        public bool upgraded = false;
        public Pawn(PictureBox image, String eidos)
        {
            this.image = image;
            this.eidos = eidos;
            this.image.Tag = this;
        }
    }

}
