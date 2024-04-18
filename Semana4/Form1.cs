using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Semana4.Metodos;


namespace Semana4
{
    public partial class Form1 : Form
    {
        private Crud crud;

        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            crud = new Crud(dataGridView1, cmbCargo, cmbDistrito, textNombre, textApellido, textDireccio, textTelefono, textEstado, txtBuscador);
            cargarCargos();
            cargarDistritos();
            crud.CargaEmpleados();
        }

        private void cargarCargos()
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.cn))
                {
                    cn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT idCargo, NomCargo FROM CARGO", cn);
                    SqlDataReader dr = cmd.ExecuteReader();

                    List<KeyValuePair<int, string>> cargos = new List<KeyValuePair<int, string>>();

                    while (dr.Read())
                    {
                        cargos.Add(new KeyValuePair<int, string>(dr.GetInt32(0), dr.GetString(1)));
                    }

                    cmbCargo.DataSource = cargos;
                    cmbCargo.DisplayMember = "Value";
                    cmbCargo.ValueMember = "Key";
                }
                cmbCargo.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void cargarDistritos()
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.cn))
                {
                    cn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT idDistrito, NomDistrito FROM DISTRITO", cn);
                    SqlDataReader dr = cmd.ExecuteReader();

                    List<KeyValuePair<int, string>> distritos = new List<KeyValuePair<int, string>>();

                    while (dr.Read())
                    {
                        distritos.Add(new KeyValuePair<int, string>(dr.GetInt32(0), dr.GetString(1)));
                    }

                    cmbDistrito.DataSource = distritos;
                    cmbDistrito.DisplayMember = "Value";
                    cmbDistrito.ValueMember = "Key";
                }
                cmbDistrito.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void btnAgregar_Click(object sender, EventArgs e)
        {
            string nombre = textNombre.Text;
            string apellido = textApellido.Text;
            string direccion = textDireccio.Text;
            string telefono = textTelefono.Text;
            string estado = textEstado.Text;
            int idCargo = 0;
            if (cmbCargo.SelectedItem != null)
            {
                idCargo = Convert.ToInt32(cmbCargo.SelectedValue);
            }

            int idDistrito = 0;
            if (cmbDistrito.SelectedItem != null)
            {
                idDistrito = Convert.ToInt32(cmbDistrito.SelectedValue);
            }

            crud.CrearEmpleado(nombre, apellido, direccion, telefono, estado, idCargo, idDistrito);
            LimpiarCampos();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                if (int.TryParse(dataGridView1.SelectedRows[0].Cells[0].Value?.ToString(), out int idEmpleado) && idEmpleado > 0)
                {
                    DialogResult result = MessageBox.Show("¿Está seguro de que desea eliminar este empleado?", "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        crud.EliminarEmpleado(idEmpleado);
                    }
                }
                else
                {
                    MessageBox.Show("El ID del empleado seleccionado no es válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Por favor, seleccione un empleado para eliminar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnGuardarCambios_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                if (filaSeleccionada != null)
                {
                    string nombre = textNombre.Text.Trim();
                    string apellido = textApellido.Text.Trim();
                    string direccion = textDireccio.Text.Trim();
                    string telefono = textTelefono.Text.Trim();
                    string estado = textEstado.Text.Trim();
                    int idCargo = 0;
                    int idDistrito = 0;
                    if (cmbCargo.SelectedItem != null && cmbDistrito.SelectedItem != null)
                    {
                        idCargo = Convert.ToInt32(cmbCargo.SelectedValue);
                        idDistrito = Convert.ToInt32(cmbDistrito.SelectedValue);
                        int idEmpleado = Convert.ToInt32(filaSeleccionada.Cells[0].Value);
                        bool edicionExitosa = crud.EditarEmpleado(idEmpleado, nombre, apellido, direccion, telefono, estado, idCargo, idDistrito);

                        if (edicionExitosa)
                        {
                            LimpiarCampos();
                            crud.CargaEmpleados();
                        }
                        else
                        {
                            MessageBox.Show("Error al editar el empleado. Por favor, intente nuevamente.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Por favor, seleccione un cargo y un distrito válidos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Por favor, seleccione un empleado para editar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Por favor, seleccione un empleado para editar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void LimpiarCampos()
        {
            textNombre.Text = "";
            textApellido.Text = "";
            textDireccio.Text = "";
            textTelefono.Text = "";
            textEstado.Text = "";
            cmbCargo.SelectedIndex = -1;
            cmbDistrito.SelectedIndex = -1;
        }

        private DataGridViewRow filaSeleccionada;
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                filaSeleccionada = dataGridView1.Rows[e.RowIndex];
            }
        }
        private void MostrarDetallesEmpleado(bool mostrar)
        {
            textNombre.Visible = mostrar;
            textApellido.Visible = mostrar;
            textDireccio.Visible = mostrar;
            textTelefono.Visible = mostrar;
            textEstado.Visible = mostrar;
            cmbCargo.Visible = mostrar;
            cmbDistrito.Visible = mostrar;
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            string nombreABuscar = txtBuscador.Text.Trim();

            if (!string.IsNullOrEmpty(nombreABuscar))
            {
                foreach (DataGridViewRow fila in dataGridView1.Rows)
                {
                    if (fila != null && fila.Cells["Column2"].Value != null)
                    {
                        string nombreEnFila = fila.Cells["Column2"].Value.ToString();
                        if (nombreEnFila.Contains(nombreABuscar))
                        {
                            fila.Visible = true;
                        }
                        else
                        {
                            fila.Visible = false;
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Por favor, ingrese un nombre para buscar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnMostrarTodo_Click(object sender, EventArgs e)
        {
            crud.CargaEmpleados();
        }

        private void btnEidtar_Click(object sender, EventArgs e)
        {
            if (filaSeleccionada != null)
            {

                textNombre.Text = filaSeleccionada.Cells["Column2"].Value.ToString();
                textApellido.Text = filaSeleccionada.Cells["Column3"].Value.ToString();
                textDireccio.Text = filaSeleccionada.Cells["Column4"].Value.ToString();
                textTelefono.Text = filaSeleccionada.Cells["Column5"].Value.ToString();
                textEstado.Text = filaSeleccionada.Cells["Column6"].Value.ToString();

                cmbCargo.SelectedIndex = cmbCargo.FindStringExact(filaSeleccionada.Cells["Column7"].FormattedValue.ToString());
                cmbDistrito.SelectedIndex = cmbDistrito.FindStringExact(filaSeleccionada.Cells["Column8"].FormattedValue.ToString());

                MostrarDetallesEmpleado(true);
            }
            else
            {
                MessageBox.Show("Por favor, seleccione un empleado antes de editar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}

