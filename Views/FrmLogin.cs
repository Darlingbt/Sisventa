using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Sisventa.Model;

namespace Sisventa.Views
{
    public partial class FrmLogin : Form
    {
        public FrmLogin()
        {
            InitializeComponent();
            LblHora.Text = DateTime.Now.ToString();
        }

        private void FrmLogin_Load(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            LblHora.Text = DateTime.Now.ToString();
        }

        private void brnSalir_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnIngresar_Click(object sender, EventArgs e)
        {
            using (dbventasEntities2 contexto = new dbventasEntities2())
            {
               
                var ist = from d in contexto.trabajador
                          where d.usuario == txtUsuario.Text
                          && d.password == txtPassword.Text
                          select d;
                           
                if(ist.Count() > 0)
                {
                    FrmPrincipal frm = new FrmPrincipal();
                    frm.FormClosed += (s, arsg) => this.Close();
                    frm.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("El Usuario no Existe", "Sistema de Venta", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}