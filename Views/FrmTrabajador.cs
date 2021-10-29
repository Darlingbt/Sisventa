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
    public partial class FrmTrabajador : Form
    {
        private bool IsNuevo = false;
        private bool IsEditar = false;

        public FrmTrabajador()
        {
            InitializeComponent();
            ttMensajes.SetToolTip(txtNombre, "Ingrese el Nombre del Trabajador");
            ttMensajes.SetToolTip(txtApellidos, "Ingrese los Apellidos del Trabajador");
            ttMensajes.SetToolTip(txtUsuario, "Ingrese el Usuario del Trabajador");
            ttMensajes.SetToolTip(txtPassword, "Ingrese el Password del Trabajador");
            ttMensajes.SetToolTip(cbAcceso, "Seleccione el Nivel de Acceso del Trabajador");
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
            txtUsuario.Clear();
            txtPassword.Clear();
            txtIdtrabajador.Clear();

        }

        //Habilitar los controles del formulario
        private void Habilitar(bool valor)
        {
            this.txtNombre.ReadOnly = !valor;
            this.txtApellidos.ReadOnly = !valor;
            this.txtDireccion.ReadOnly = !valor;
            this.cbSexo.Enabled = valor;
            this.cbAcceso.Enabled = valor;
            this.txtNum_documento.ReadOnly = !valor;
            this.txtTelefono.ReadOnly = !valor;
            this.txtEmail.ReadOnly = !valor;
            this.txtUsuario.ReadOnly = !valor;
            this.txtPassword.ReadOnly = !valor;
            this.txtIdtrabajador.ReadOnly = !valor;
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
            dataListado.DataSource = contexto.trabajador.ToList();
            lblTotal.Text = "Total de Regristo : " + Convert.ToString(dataListado.Rows.Count);
            this.OcultarColumnas();
        }

        //Metodo BuscarRazon_Social y Documento
        private void BuscarRazon_Social_Documento()
        {

        }

        private void insertar()
        {
            string nombres = txtNombre.Text.Trim().ToUpper();
            string apellidos = txtApellidos.Text.Trim().ToUpper();
            string sexo = cbSexo.Text;
            DateTime fecha_nac = Convert.ToDateTime(dtFechaNac.Value);
            string acceso = cbAcceso.Text;
            string numero_documento = txtNum_documento.Text;
            string direccion = txtDireccion.Text;
            string telefono = txtTelefono.Text;
            string email = txtEmail.Text;
            string usuario = txtUsuario.Text;
            string password = txtPassword.Text;

            using (dbventasEntities2 contexto = new dbventasEntities2())
            {
                trabajador c = new trabajador
                {
                    nombres = nombres,
                    apellidos = apellidos,
                    sexo = sexo,
                    fecha_nac = fecha_nac,
                    num_documento = numero_documento,
                    direccion = direccion,
                    telefono = telefono,
                    email = email,
                    acceso = acceso,
                    usuario = usuario,
                    password = password
                };
                contexto.trabajador.Add(c);
                contexto.SaveChanges();
                Mostrar();
            }

        }

        private void editar()
        {
            string nombres = txtNombre.Text.Trim().ToUpper();
            string apellidos = txtApellidos.Text.Trim().ToUpper();
            string sexo = cbSexo.Text;
            DateTime fecha_nac = Convert.ToDateTime(dtFechaNac.Value);
            string acceso = cbAcceso.Text;
            string numero_documento = txtNum_documento.Text;
            string direccion = txtDireccion.Text;
            string telefono = txtTelefono.Text;
            string email = txtEmail.Text;
            string usuario = txtUsuario.Text;
            string password = txtPassword.Text;

            int id = Convert.ToInt32(this.txtIdtrabajador.Text);
            using (dbventasEntities2 contexto = new dbventasEntities2())
            {
                trabajador c = contexto.trabajador.FirstOrDefault(x => x.idtrabajador == id);
                c.nombres = nombres;
                c.apellidos = apellidos;
                c.sexo = sexo;
                c.fecha_nac = fecha_nac;
                c.num_documento = numero_documento;
                c.direccion = direccion;
                c.telefono = telefono;
                c.email = email;
                c.acceso = acceso;
                c.usuario = usuario;
                c.password = password;
                contexto.SaveChanges();
                Mostrar();
            }
        }
        private void eliminar()
        {

            using (dbventasEntities2 contexto = new dbventasEntities2())
            {
                trabajador c = contexto.trabajador.FirstOrDefault();
                contexto.trabajador.Remove(c);
                contexto.SaveChanges();
                Mostrar();
            }
        }

        private void FrmTrabajador_Load(object sender, EventArgs e)
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
            this.txtIdtrabajador.Text = Convert.ToString(dataListado.CurrentRow.Cells["idtrabajador"].Value);
            this.txtNombre.Text = Convert.ToString(dataListado.CurrentRow.Cells["nombres"].Value);
            this.txtApellidos.Text = Convert.ToString(dataListado.CurrentRow.Cells["apellidos"].Value);
            this.cbSexo.Text = Convert.ToString(dataListado.CurrentRow.Cells["sexo"].Value);
            this.dtFechaNac.Value = Convert.ToDateTime(dataListado.CurrentRow.Cells["fecha_nac"].Value);
            this.cbAcceso.Text = Convert.ToString(dataListado.CurrentRow.Cells["acceso"].Value);
            this.txtNum_documento.Text = Convert.ToString(dataListado.CurrentRow.Cells["num_documento"].Value);
            this.txtDireccion.Text = Convert.ToString(dataListado.CurrentRow.Cells["direccion"].Value);
            this.txtTelefono.Text = Convert.ToString(dataListado.CurrentRow.Cells["telefono"].Value);
            this.txtEmail.Text = Convert.ToString(dataListado.CurrentRow.Cells["email"].Value);
            this.txtUsuario.Text = Convert.ToString(dataListado.CurrentRow.Cells["usuario"].Value);
            this.txtPassword.Text = Convert.ToString(dataListado.CurrentRow.Cells["password"].Value);

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

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.IsNuevo = false;
            this.IsEditar = false;
            this.Botones();
            this.Limpiar();
            this.Habilitar(false);
            this.txtIdtrabajador.Text = string.Empty;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.txtNombre.Text == string.Empty || this.txtApellidos.Text == string.Empty || this.txtNum_documento.Text == string.Empty
                    || this.txtDireccion.Text == string.Empty || this.txtUsuario.Text == string.Empty || this.txtPassword.Text == string.Empty)
                {
                    MensajeError("Falta ingresar algunos datos, serán remarcados");
                    errorProvider1.SetError(txtNombre, "Ingrese el Nombre del Trabajador");
                    errorProvider1.SetError(txtApellidos, "Ingrese el Apellido del Trabajador");
                    errorProvider1.SetError(txtNum_documento, "Ingrese El Numero de Documento del Trabajador");
                    errorProvider1.SetError(txtDireccion, "Ingrese una Direccón");
                    errorProvider1.SetError(txtUsuario, "Ingrese el Usuario del Trabajador");
                    errorProvider1.SetError(txtPassword, "Ingrese el Password del Trabajador");

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
            if (!this.txtIdtrabajador.Text.Equals(""))
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
    }
}