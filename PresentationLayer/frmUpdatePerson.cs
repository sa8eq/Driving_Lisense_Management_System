using BussinesLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PresentationLayer
{
    public partial class frmUpdatePerson : Form
    {
        private clsPersonBussinesLayer _Person = new clsPersonBussinesLayer();
        public frmUpdatePerson()
        {
            InitializeComponent();
        }
        public frmUpdatePerson(int PersonID)
        {
            InitializeComponent();
            _Person._ID = PersonID;
        }
        public frmUpdatePerson(clsPersonBussinesLayer Person)
        {
            InitializeComponent();
            _Person = Person;
        }
        private void _LoadInfo()
        {
            lblPersonID.Text = _Person._ID.ToString();
            ctrlPerson1.LoadPersonData(_Person._ID);
        }
        private void frmUpdatePerson_Load(object sender, EventArgs e)
        {
            _LoadInfo();
        }
    }
}
