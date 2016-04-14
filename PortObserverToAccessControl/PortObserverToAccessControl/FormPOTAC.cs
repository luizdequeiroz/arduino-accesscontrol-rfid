using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PortObserverToAccessControl
{
    public partial class FormPOTAC : Form
    {
        public FormPOTAC()
        {
            InitializeComponent();
            Thread.Sleep(3000);
            new Thread(Observing).Start();
        }

        public void Observing()
        {
            while (true)
            {

            }
        }
    }
}
