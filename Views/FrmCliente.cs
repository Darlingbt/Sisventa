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
    public partial class FrmCliente : Form
    {
        private bool IsNuevo = false;
        private bool IsEditar = false;

        public FrmCliente()
        {
            InitializeComponent();
            this.ttMensajes.SetToolTip(this.txtNombre, "Ingrese el Nombre del Cliente");
            this.ttMensajes.SetToolTip(this.txtApellidos, "Ingrese los Apellidos del Cliente");
            this.ttMensajes.SetToolTip(this.txtDireccion, "Ingrese la Dirección del Cliente");
            this.ttMensajes.SetToolTip(this.txtNum_documento, "Ingrese el Numero de Documento del Cliente");

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
            txtNombre.Clear();
            txtApellidos.Clear();
            txtNum_documento.Clear();
            txtDireccion.Clear();
            txtTelefono.Clear();
            txtEmail.Clear();
            txtIdcliente.Clear();
        }

        //Habilitar los controles del formulario
        private void Habilitar(bool valor)
        {
            this.txtNombre.ReadOnly = !valor;
            this.txtApellidos.ReadOnly = !valor;
            this.txtDireccion.ReadOnly = !valor;
            this.cbSexo.Enabled = valor;
            this.cbTipo_documento.Enabled = valor;
            this.txtNum_documento.ReadOnly = !valor;
            this.txtTelefono.ReadOnly = !valor;
            this.txtEmail.ReadOnly = !valor;
            this.txtIdcliente.ReadOnly = !valor;
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
            dataListado.DataSource = contexto.cliente.ToList();
            lblTotal.Text = "Total de Regristo : " + Convert.ToString(dataListado.Rows.Count);
            this.OcultarColumnas();
        }

        //Metodo BuscarRazon_Social y Documento
        private void BuscarRazon_Social_Documento()
        {

        }

        private void insertar()
        {
            string nombre = txtNombre.Text.Trim().ToUpper();
            string apellidos = txtApellidos.Text.Trim().ToUpper();
            string sexo = cbSexo.Text;
            DateTime fecha_nacimiento = Convert.ToDateTime(dtFechaNac.Value);
            string tipo_documento = cbTipo_documento.Text;
            string numero_documento = txtNum_documento.Text;
            string direccion = txtDireccion.Text;
            string telefono = txtTelefono.Text;
            string email = txtEmail.Text;

            using (dbventasEntities2 contexto = new dbventasEntities2())
            {
                cliente c = new cliente
                {
                    nombre = nombre,
                    apellido = apellidos,
                    sexo = sexo,
                    fecha_nacimiento = fecha_nacimiento,
                    tipo_documento = tipo_documento,
                    num_documento = numero_documento,
                    direccion = direccion,
                    telefono = telefono,
                    email = email

                };
                contexto.cliente.Add(c);
                contexto.SaveChanges();
                Mostrar();
            }

        }

        private void editar()
        {
            string nombre = txtNombre.Text.Trim().ToUpper();
            string apellidos = txtApellidos.Text.Trim().ToUpper();
            string sexo = cbSexo.Text;
            DateTime fecha_nacimiento = Convert.ToDateTime(dtFechaNac.Value);
            string tipo_documento = cbTipo_documento.Text;
            string numero_documento = txtNum_documento.Text;
            string direccion = txtDireccion.Text;
            string telefono = txtTelefono.Text;
            string email = txtEmail.Text;

            int id = Convert.ToInt32(this.txtIdcliente.Text);
            using (dbventasEntities2 contexto = new dbventasEntities2())
            {
                cliente c = contexto.cliente.FirstOrDefault(x => x.idcliente == id);
                c.nombre = nombre;
                c.apellido = apellidos;
                c.sexo = sexo;
                c.fecha_nacimiento = fecha_nacimiento;
                c.tipo_documento = tipo_documento;
                c.num_documento = numero_documento;
                c.direccion = direccion;
                c.telefono = telefono;
                c.email = email;
                contexto.SaveChanges();
                Mostrar();
            }
        }
        private void eliminar()
        {

            using (dbventasEntities2 contexto = new dbventasEntities2())
            {
                cliente c = contexto.cliente.FirstOrDefault();
                contexto.cliente.Remove(c);
                contexto.SaveChanges();
                Mostrar();
            }
        }
        private void FrmCliente_Load(object sender, EventArgs e)
        {
            this.Mostrar();
            this.Habilitar(false);
            this.Botones();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult Opcion;
                Opcion = MessageBox.Show("Realmente Desea Eliminar Los Registro", "Sistema de Venta", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

                if (Opcion == DialogResult.OK)
                {
                    foreach (DataGridViewRow row in dataListado.Rows)
                    {
                        if (Convert.ToBoolean(row.Cells[0].Value))
                        {
                            if (chkEliminar.Checked == true)
                            {
                                eliminar();
                            }
                            else
                            {
                                this.MensajeOk("Por favor seleccione un registro para eliminar");
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

        private void dataListado_DoubleClick(object sender, EventArgs e)
        {
            this.txtIdcliente.Text = Convert.ToString(dataListado.CurrentRow.Cells["idcliente"].Value);
            this.txtNombre.Text = Convert.ToString(dataListado.CurrentRow.Cells["nombre"].Value);
            this.txtApellidos.Text = Convert.ToString(dataListado.CurrentRow.Cells["apellido"].Value);
            this.cbSexo.Text = Convert.ToString(dataListado.CurrentRow.Cells["sexo"].Value);
            this.dtFechaNac.Value = Convert.ToDateTime(dataListado.CurrentRow.Cells["fecha_nacimiento"].Value);
            this.cbTipo_documento.Text = Convert.ToString(dataListado.CurrentRow.Cells["tipo_documento"].Value);
            this.txtNum_documento.Text = Convert.ToString(dataListado.CurrentRow.Cells["num_documento"].Value);
            this.txtDireccion.Text = Convert.ToString(dataListado.CurrentRow.Cells["direccion"].Value);
            this.txtTelefono.Text = Convert.ToString(dataListado.CurrentRow.Cells["telefono"].Value);
            this.txtEmail.Text = Convert.ToString(dataListado.CurrentRow.Cells["email"].Value);

            this.tabControl1.SelectedIndex = 1;
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
                if (this.txtNombre.Text == string.Empty || this.txtApellidos.Text == string.Empty || this.txtNum_documento.Text == string.Empty
                    || this.txtDireccion.Text == string.Empty)
                {
                    MensajeError("Falta ingresar algunos datos, serán remarcados");
                    errorProvider1.SetError(txtNombre, "Ingrese el Nombre del Cliente");
                    errorProvider1.SetError(txtApellidos, "Ingrese el Apellido del Cliente");
                    errorProvider1.SetError(txtNum_documento, "Ingrese El Numero de Documento del Cliente");
                    errorProvider1.SetError(txtDireccion, "Ingrese una Direccón");
                }
                else
                {
                    if (this.IsNuevo)
                    {
                        insertar();
                        errorProvider1.Clear();
                    }
                    else
                    {
                        editar();
                        errorProvider1.Clear();
                    }
                    if (this.IsNuevo == true)
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

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (!this.txtIdcliente.Text.Equals(""))
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
            this.Habilitar(false);
            this.Limpiar();
            this.txtIdcliente.Text = string.Empty;
        }
    }
}
