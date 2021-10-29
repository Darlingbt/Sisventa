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
    public partial class FrmArticulo : Form
    {
        private bool IsNuevo = false;

        private bool IsEditar = false;

        private static FrmArticulo _Instancia;

        public static FrmArticulo GetInstancia()
        {
            if(_Instancia == null)
            {
                _Instancia = new FrmArticulo();
            }
            return _Instancia;
        }

        public void setCategoria(string idcategoria, string nombre)
        {
            this.txtidCategoria.Text = idcategoria;
            this.txtCategoria.Text = nombre;
        }

        public FrmArticulo()
        {
            InitializeComponent();
            this.ttMensajes.SetToolTip(this.txtNombre, "Ingrese el Nombre del Articulo");
            this.ttMensajes.SetToolTip(this.pxImagen, "Seleccione la Imagen del Articulo");
            this.ttMensajes.SetToolTip(this.txtCategoria, "Seleccione la Categoria del Articulo");
            this.ttMensajes.SetToolTip(this.cbIdpresentacion, "Seleccione la Presentación del Articulo");

            this.txtidCategoria.Visible = false;
            this.txtCategoria.ReadOnly = true;
            this.LlenaComboArticulo();
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
            txtCodigo.Clear();
            txtidarticulo.Text = "";
            this.txtNombre.Text = string.Empty;
            txtDescripcion.Clear();
            txtCategoria.Clear();
            txtidCategoria.Clear();
            pxImagen.Image = global::Sisventa.Properties.Resources.imagen_transparente;
        }

        //Habilitar los controles del formulario
        private void Habilitar(bool valor)
        {
            this.txtCodigo.ReadOnly = !valor;
            this.txtNombre.ReadOnly = !valor;
            this.txtDescripcion.ReadOnly = !valor;
            this.btnBuscarCategoria.Enabled = valor;
            this.cbIdpresentacion.Enabled = valor;
            this.btnCargar.Enabled = valor;
            this.btnLimpiar.Enabled = valor;
            this.txtidarticulo.ReadOnly = !valor;
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
            this.dataListado.Columns[6].Visible = false;
            this.dataListado.Columns[8].Visible = false;
            this.dataListado.Columns[10].Visible = false;
            this.dataListado.Columns[11].Visible = false;
            this.dataListado.Columns[12].Visible = false;
        }

        //Metodo mostrar columna
        private void Mostrar()
        {
            dbventasEntities2 contexto = new dbventasEntities2();
            dataListado.DataSource = contexto.articulo.ToList();
            lblTotal.Text = "Total de Regristo : " + Convert.ToString(dataListado.Rows.Count);
            this.OcultarColumnas();
        }

        //Metodo BuscarNombre   
        private void BuscarNombre()
        {

        }

        private void insertar()
        {
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            this.pxImagen.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);

            string Nombre = txtNombre.Text.Trim().ToUpper();
            string Descripcion = txtDescripcion.Text;
            string Codigo = txtCodigo.Text;
            byte[] imagen = ms.GetBuffer();
            int Idcategoria = Convert.ToInt32(txtidCategoria.Text);
            int Idpresentacion = Convert.ToInt32(cbIdpresentacion.SelectedValue);
            string categoria = txtCategoria.Text;
            string presetancion = Convert.ToString(cbIdpresentacion.Text);
      
            using (dbventasEntities2 contexto = new dbventasEntities2())
            {
                articulo c = new articulo
                {
                    codigo = Codigo,
                    nombre = Nombre,
                    descripcion = Descripcion,
                    imagen = imagen,
                    idcategoria = Idcategoria,
                    idpresentacion = Idpresentacion,
                    categoria = categoria,
                    presentacion = presetancion
                };
                contexto.articulo.Add(c);
                contexto.SaveChanges();
                Mostrar();
            }

        }

        private void editar()
        {
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            this.pxImagen.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);

            string Nombre = txtNombre.Text.Trim().ToUpper();
            string Descripcion = txtDescripcion.Text.Trim();
            string Codigo = txtCodigo.Text.Trim();
            byte[] imagen = ms.GetBuffer();
            int Idcategoria = Convert.ToInt32(txtidCategoria.Text.Trim());
            int Idpresentacion = Convert.ToInt32(cbIdpresentacion.SelectedValue);
            string categoria = txtCategoria.Text;
            string presetancion = Convert.ToString(cbIdpresentacion.Text);

            int id = Convert.ToInt32(txtidarticulo.Text);

            using (dbventasEntities2 contexto = new dbventasEntities2())
            {
                articulo c = contexto.articulo.FirstOrDefault(x => x.idarticulo == id);
                c.codigo = Codigo;
                c.nombre = Nombre;
                c.descripcion = Descripcion;
                c.imagen = imagen;
                c.idcategoria = Idcategoria;
                c.idpresentacion = Idpresentacion;
                c.categoria = categoria;
                c.presentacion = presetancion;

                contexto.SaveChanges();
                Mostrar();
            }
        }
        private void eliminar()
        {

            using (dbventasEntities2 contexto = new dbventasEntities2())
            {
                articulo c = contexto.articulo.FirstOrDefault();
                contexto.articulo.Remove(c);
                contexto.SaveChanges();
                Mostrar();
            }
        }

        //Metodo para llenar el combobox desde la base de datos
        private void LlenaComboArticulo()
        {
            dbventasEntities2 contexto = new dbventasEntities2();
            cbIdpresentacion.DataSource = contexto.presentacion.ToList();
            cbIdpresentacion.ValueMember = "idpresentacion";
            cbIdpresentacion.DisplayMember = "nombre";
        }

        private void btnCargar_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();

            DialogResult resultado = dialog.ShowDialog();

            if (resultado == DialogResult.OK)
            {
                this.pxImagen.SizeMode = PictureBoxSizeMode.StretchImage;
                this.pxImagen.Image = Image.FromFile(dialog.FileName);
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            this.pxImagen.SizeMode = PictureBoxSizeMode.StretchImage;
            this.pxImagen.Image = global::Sisventa.Properties.Resources.imagen_transparente;
        }

        private void FrmArticulo_Load(object sender, EventArgs e)
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
                if (this.txtNombre.Text == string.Empty || this.txtidCategoria.Text == string.Empty || this.txtCodigo.Text == string.Empty)
                {
                    MensajeError("Falta ingresar algunos datos, serán remarcados");
                    errorProvider1.SetError(txtNombre, "Ingrese un Valor");
                    errorProvider1.SetError(txtCategoria, "Ingrese un Valor");
                    errorProvider1.SetError(txtCodigo, "Ingrese un Valor");

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
            if (!this.txtidarticulo.Text.Equals(""))
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
            this.txtidarticulo.Text = Convert.ToString(dataListado.CurrentRow.Cells["idarticulo"].Value);
            this.txtCodigo.Text = Convert.ToString(dataListado.CurrentRow.Cells["codigo"].Value);
            this.txtNombre.Text = Convert.ToString(dataListado.CurrentRow.Cells["nombre"].Value);
            this.txtDescripcion.Text = Convert.ToString(dataListado.CurrentRow.Cells["descripcion"].Value);

            byte[] imagenBuffer = (byte[])this.dataListado.CurrentRow.Cells["imagen"].Value;
            System.IO.MemoryStream ms = new System.IO.MemoryStream(imagenBuffer);
            this.pxImagen.Image = Image.FromStream(ms);
            this.pxImagen.SizeMode = PictureBoxSizeMode.StretchImage;

            this.txtidCategoria.Text = Convert.ToString(dataListado.CurrentRow.Cells["idcategoria"].Value);
            this.txtCategoria.Text = Convert.ToString(dataListado.CurrentRow.Cells["categoria"].Value);
            this.cbIdpresentacion.SelectedValue = Convert.ToString(dataListado.CurrentRow.Cells["idpresentacion"].Value);
            this.cbIdpresentacion.Text = Convert.ToString(dataListado.CurrentRow.Cells["presentacion"].Value);



            this.tabControl1.SelectedIndex = 1;
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

        private void btnBuscarCategoria_Click(object sender, EventArgs e)
        {
            FrmVistaCategoria_Articulo form = new FrmVistaCategoria_Articulo();
            form.ShowDialog();
        }
    }
}
