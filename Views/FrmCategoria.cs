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
    public partial class FrmCategoria : Form
    {
        private bool IsNuevo = false;

        private bool IsEditar = false;

        public FrmCategoria()
        {
            InitializeComponent();
            this.ttMensajes.SetToolTip(this.txtNombre, "Ingrese el Nombre de la Categoria");
        }

        //Mostrar Mensaje de Confirmacion
        private void MensajeOk(string mensaje)
        {
            MessageBox.Show(mensaje, "Sistema de Ventas", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        //Mostrar Mensaje de Error
        private void MensajeError(string mensaje)
        {
            MessageBox.Show(mensaje, "Sistema de Ventas", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        //Limpiar todos los controles del formulario
        private void Limpiar()
        {
            this.txtNombre.Text = string.Empty;
            txtDescripcion.Clear();
            txtIdcategoria.Text = "";
        }

        //Habilitar los controles del formulario
        private void Habilitar(bool valor)
        {
            this.txtNombre.ReadOnly = !valor;
            this.txtDescripcion.ReadOnly = !valor;
            this.txtIdcategoria.ReadOnly = !valor;
        }

        //Habilitar los botones
        private void Botones()
        {
            if (this.IsNuevo || this.IsEditar) //Alt + 124
            {
                this.Habilitar(true);
                this.btnNuevo.Enabled = false;
                this.btnGuardar.Enabled = true;
                this.btnEditar.Enabled = false;
                this.btnCancelar.Enabled = true;
            }
            else
            {
                this.Habilitar(false);
                this.btnNuevo.Enabled = true;
                this.btnGuardar.Enabled = false;
                this.btnEditar.Enabled = true;
                this.btnCancelar.Enabled = false;
            }

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
        private void BuscarNombre()
        {
            using (dbventasEntities2 contexto = new dbventasEntities2())
            {
                this.dataListado.DataSource = contexto.spbuscar_categoria(txtBuscar.Text);
                this.OcultarColumnas();
                lblTotal.Text = "Total de Regristro:" + Convert.ToString(dataListado.Rows.Count); 
            }
        }
        

        private void FrmCategoria_Load(object sender, EventArgs e)
        {
            this.Mostrar();
            this.Habilitar(false);
            this.Botones();
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            this.IsNuevo = true;
            this.IsEditar = false;
            this.Botones();
            this.Limpiar();
            this.Habilitar(true);
            this.txtNombre.Focus();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.txtNombre.Text == string.Empty)
                {
                    MensajeError("Falta ingresar algunos datos, serán remarcados");
                    errorProvider1.SetError(txtNombre, "Ingrese un Nombre");
                    errorProvider1.SetError(txtDescripcion, "Ingrese una Descripción");
                }
                else
                {
                    if (this.IsNuevo)
                    {
                        insertar();
                    }
                    else
                    {
                        editar();
                    }
                    if(this.IsNuevo == true)
                    {
                        this.MensajeOk("Se Inserto de forma correcta el registro");
                    }
                    else
                    {
                        this.MensajeOk("Se Actualizó de forma correcta el registro");
                    }

                    this.IsNuevo = false;
                    this.IsEditar = false;
                    this.Botones();
                    this.Limpiar();
                    this.Mostrar();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace);
            }
        }
        private void insertar()
        {
            string nombre = txtNombre.Text.Trim().ToUpper();
            string descripcion = txtDescripcion.Text.Trim();

            using (dbventasEntities2 contexto = new dbventasEntities2())
            {
                categoria c = new categoria
                {
                    nombre = nombre,
                    descripcion = descripcion
                };
                contexto.categoria.Add(c);
                contexto.SaveChanges();
                Mostrar();
            }
             
        }

        private void editar()
        {
            string nombre = txtNombre.Text.Trim().ToUpper();
            string descripcion = txtDescripcion.Text.Trim();

            int id = Convert.ToInt32(this.txtIdcategoria.Text);
            using (dbventasEntities2 contexto = new dbventasEntities2())
            {
                categoria c = contexto.categoria.FirstOrDefault(x => x.idcategoria == id);
                c.nombre = nombre;
                c.descripcion = descripcion;
                contexto.SaveChanges();
                Mostrar();
            }
        }
        private void eliminar()
        {
            
            using (dbventasEntities2 contexto = new dbventasEntities2())
            {
                categoria c = contexto.categoria.FirstOrDefault();
                contexto.categoria.Remove(c);
                contexto.SaveChanges();
                Mostrar();
            }
        }

        private void dataListado_DoubleClick(object sender, EventArgs e)
        {
            this.txtIdcategoria.Text = Convert.ToString(dataListado.CurrentRow.Cells["idcategoria"].Value);
            this.txtNombre.Text = Convert.ToString(dataListado.CurrentRow.Cells["nombre"].Value);
            this.txtDescripcion.Text = Convert.ToString(dataListado.CurrentRow.Cells["descripcion"].Value);

            this.tabControl1.SelectedIndex = 1;
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (!this.txtIdcategoria.Text.Equals(""))
            {
                this.IsEditar = true;
                this.Botones();
                this.Habilitar(true);
            }
            else
            {
                this.MensajeError("Debe de seleccionar primero el registro a Modificar");
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.IsNuevo = false;
            this.IsEditar = false;
            this.Botones();
            this.Limpiar();
            this.Habilitar(false);
        }

        private void chkEliminar_CheckedChanged(object sender, EventArgs e)
        {
            if (chkEliminar.Checked)
            {
                this.dataListado.Columns[0].Visible = true;
            }
            else
            {
                this.dataListado.Columns[0].Visible = false;
            }
        }

        private void dataListado_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataListado.Columns["Eliminar"].Index)
            {
                DataGridViewCheckBoxCell chkEliminar = (DataGridViewCheckBoxCell)dataListado.Rows[e.RowIndex].Cells["Eliminar"];
                chkEliminar.Value = !Convert.ToBoolean(chkEliminar.Value);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult Opcion;
                Opcion = MessageBox.Show("Realmente Desea Eliminar Los Registro", "Sistema de Venta", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

                if(Opcion == DialogResult.OK)
                {
                    foreach (DataGridViewRow row in dataListado.Rows)
                    {
                        if (Convert.ToBoolean(row.Cells[0].Value))
                        {
                            if(chkEliminar.Checked == true)
                            {
                                eliminar();
                            }
                            else
                            {
                                this.MensajeOk("Por seleccione un registro para eliminar");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace);
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            BuscarNombre();
        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            BuscarNombre();
        }
    }
}
