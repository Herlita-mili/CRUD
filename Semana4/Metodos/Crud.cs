using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Semana4.Metodos
{
    internal class Crud
    {
        private DataGridView dataGridView1;
        private ComboBox cmbCargo;
        private ComboBox cmbDistrito;
        private TextBox textNombre;
        private TextBox textApellido;
        private TextBox textDireccio;
        private TextBox textTelefono;
        private TextBox textEstado;
        private TextBox txtBuscador;

        public Crud(DataGridView dataGridView1, ComboBox cmbCargo, ComboBox cmbDistrito, TextBox textNombre,
            TextBox textApellido, TextBox textDireccio, TextBox textTelefono, TextBox textEstado, TextBox txtBuscador)
        {
            this.dataGridView1 = dataGridView1;
            this.cmbCargo = cmbCargo;
            this.cmbDistrito = cmbDistrito;
            this.textNombre = textNombre;
            this.textApellido = textApellido;
            this.textDireccio = textDireccio;
            this.textTelefono = textTelefono;
            this.textEstado = textEstado;
            this.txtBuscador = txtBuscador;
        }

        public void CargaEmpleados()
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.cn))
                {
                    cn.Open();
                    SqlCommand cmd = new SqlCommand("listarEmpleados", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataReader dr = cmd.ExecuteReader();
                    dataGridView1.Rows.Clear();
                    while (dr.Read())
                    {
                        dataGridView1.Rows.Add(dr.GetValue(0),
                                               dr.GetValue(1),
                                               dr.GetValue(2),
                                               dr.GetValue(3),
                                               dr.GetValue(4),
                                               dr.GetValue(5),
                                               dr.GetValue(6),
                                               dr.GetValue(7));
                    }
                    dr.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void CrearEmpleado(string nombre, string apellido, string direccion, string telefono, string estado, int idCargo, int idDistrito)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.cn))
                {
                    cn.Open();
                    SqlCommand cmd = new SqlCommand("insertarEmpleado", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@nombre", nombre);
                    cmd.Parameters.AddWithValue("@apellido", apellido);
                    cmd.Parameters.AddWithValue("@direccion", direccion);
                    cmd.Parameters.AddWithValue("@telefono", telefono);
                    cmd.Parameters.AddWithValue("@estado", estado);
                    cmd.Parameters.AddWithValue("@idCargo", idCargo);
                    cmd.Parameters.AddWithValue("@idDistrito", idDistrito);
                    cmd.ExecuteNonQuery();
                    CargaEmpleados();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public bool EditarEmpleado(int idEmpleado, string nombre, string apellido, string direccion, string telefono, string estado, int idCargo, int idDistrito)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.cn))
                {
                    cn.Open();
                    SqlCommand cmd = new SqlCommand("modificarEmpleado", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Agregar parámetros al procedimiento almacenado
                    cmd.Parameters.AddWithValue("@idEmpleado", idEmpleado);
                    cmd.Parameters.AddWithValue("@nombre", nombre);
                    cmd.Parameters.AddWithValue("@apellido", apellido);
                    cmd.Parameters.AddWithValue("@direccion", direccion);
                    cmd.Parameters.AddWithValue("@telefono", telefono);
                    cmd.Parameters.AddWithValue("@estado", estado);
                    cmd.Parameters.AddWithValue("@idCargo", idCargo);
                    cmd.Parameters.AddWithValue("@idDistrito", idDistrito);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        public void EliminarEmpleado(int idEmpleado)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.cn))
                {
                    cn.Open();
                    SqlCommand cmd = new SqlCommand("eliminarEmpleado", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idEmpleado", idEmpleado);
                    cmd.ExecuteNonQuery();
                    CargaEmpleados();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }


}


