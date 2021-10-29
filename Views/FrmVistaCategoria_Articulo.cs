using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Sisventa.Model;

namespace Sisventa.Views
{
    public partial class FrmVistaCategoria_Articulo : Form
    {
        public FrmVistaCategoria_Articulo()
        {
            InitializeComponent();
        }

        //Metodo ocultar columna
        private void OcultarColumnas()
        {
            this.dataListado.Columns[0].Visible = false;
            this.dataListado.Columns[1].Visible = false;
        }

        //Metodo mostrar columna
        private void Mostrar()
        {
            dbventasEntities2 contexto = new dbventasEntities2();
            dataListado.DataSource = contexto.categoria.ToList();
            lblTotal.Text = "Total de Regristo : " + Convert.ToString(dataListado.Rows.Count);
            this.OcultarColumnas();
        }

        //Metodo BuscarNombre
        private async Task BuscarNombreAsync()
        {
            string valor = txtBuscar.Text;
            List<Model.categoria> fields = null;
            if (valor != string.Empty)
            {
                using (var db = new Model.dbventasEntities2())
                {
                    fields = await db.categoria.Where(o => o.descripcion.Contains(valor)
                   || o.nombre.Contains(valor)).ToListAsync();
                    dataListado.DataSource = null;
                    dataListado.DataSource = fields;
                }
            }
        }

        private void FrmVistaCategoria_Articulo_Load(object sender, EventArgs e)
        {
            this.Mostrar();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            this.BuscarNombreAsync();
        }

        private void dataListado_DoubleClick(object sender, EventArgs e)
        {
            FrmArticulo form = FrmArticulo.GetInstancia();
            string par1, par2;
            par1 = Convert.ToString(this.dataListado.CurrentRow.Cells["idcategoria"].Value);
            par2 = Convert.ToString(this.dataListado.CurrentRow.Cells["nombre"].Value);
            form.setCategoria(par1, par2);
            this.Hide();

        }
    }
}
