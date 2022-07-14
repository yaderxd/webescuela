using Domain;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace uni
{
    public partial class Form1 : Form
    {
        private static int id = 0;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            GetAllAlumnosViewModel();
        }

        private void txtTelefono_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar >= 32 && e.KeyChar <= 47) || (e.KeyChar >= 58 && e.KeyChar <= 255))
            {
                MessageBox.Show("Solo números", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Handled = true;
                return;
            }
        }
        private void Limpiar()
        {
            txtNombre.Text = String.Empty;
            txtApellido.Text = String.Empty;
            txtDireccion.Text = String.Empty;
            txtTelefono.Text = String.Empty;
        }
        private void Vacios()
        {
            if (string.IsNullOrEmpty(txtNombre.Text) && string.IsNullOrEmpty(txtApellido.Text)
                && string.IsNullOrEmpty(txtDireccion.Text) && string.IsNullOrEmpty(txtTelefono.Text)
                )
            {
                MessageBox.Show("¡Llene todas los espacios requeridos!");
                return;
            }
        }
        private async void GetAllAlumnosViewModel()
        {
            using (var client = new HttpClient())
            {
                using (var response = await client.GetAsync("https://localhost:44314/api/Alumnos"))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var AlumnosJsonString = await response.Content.ReadAsStringAsync();
                        dgvEscuela.DataSource = JsonConvert.
                           DeserializeObject<List<universitarios>>(AlumnosJsonString)
                            .ToList();
                    }
                    else
                    {
                        MessageBox.Show("No se puede obtener el Estudiante: " + response.StatusCode);
                    }
                }
            }
        }
        private async void GetAlumnoById(int id)
        {
            using (var client = new HttpClient())
            {
                string URI = "https://localhost:44314/api/Alumnos/" + id.ToString();
                var response = await client.GetAsync(URI);
                if (response.IsSuccessStatusCode)
                {
                    var EstudianteJsonString = await response.Content.ReadAsStringAsync();
                    universitarios oEstudiante =
                        JsonConvert.DeserializeObject<universitarios>(EstudianteJsonString);
                    txtNombre.Text = oEstudiante.Nombre;
                    txtApellido.Text = oEstudiante.Apellidos;
                    txtDireccion.Text = oEstudiante.Direccion;
                    dtpFecha.Value = oEstudiante.FechaIngreso;
                    txtTelefono.Text = oEstudiante.Telefono.ToString();
                }
                else
                    MessageBox.Show("No se pudo obtener el Estudiante solicitado: " + response.StatusCode);
            }
        }
        private async void AddAlumno()
        {
                Vacios();
           
                universitarios oEstudiante = new universitarios();
                oEstudiante.Nombre = txtNombre.Text;
                oEstudiante.Apellidos = txtApellido.Text;
                oEstudiante.Direccion = txtDireccion.Text;
                oEstudiante.FechaIngreso = dtpFecha.Value;
                oEstudiante.Telefono = int.Parse(txtTelefono.Text);
                using (var client = new HttpClient())
                {
                    var serializedLibro = JsonConvert.SerializeObject(oEstudiante);
                    var content = new StringContent(serializedLibro, Encoding.UTF8, "application/json");
                    var result = await client.PostAsync("https://localhost:44314/api/Alumnos", content);
                    if (result.IsSuccessStatusCode)
                        MessageBox.Show($"El Estudiante se ha agregado correctamente");
                    else
                        MessageBox.Show("No se pudo agregar el Estudiante: " + result.Content.ReadAsStringAsync().Result);
                }
                Limpiar();
            GetAllAlumnosViewModel();
            
        }
        private async void UpdateAlumno(int id)
        {
            Vacios();
           
                universitarios oEstudiante = new universitarios();
                oEstudiante.IdEstudiante = id;
                oEstudiante.Nombre = txtNombre.Text;
                oEstudiante.Apellidos = txtApellido.Text;
                oEstudiante.Direccion = txtDireccion.Text;
                oEstudiante.FechaIngreso = dtpFecha.Value;
                oEstudiante.Telefono = int.Parse(txtTelefono.Text);

                using (var client = new HttpClient())
                {
                    var serializedEstudiante = JsonConvert.SerializeObject(oEstudiante);
                    var content = new StringContent(serializedEstudiante, Encoding.UTF8, "application/json");
                    var result = await client.PutAsync("https://localhost:44314/api/Alumnos/" + oEstudiante.IdEstudiante, content);
                    if (result.IsSuccessStatusCode)
                        MessageBox.Show("Estudiante se ha actualizado correctamente", "Estudiante Actualizado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else
                        MessageBox.Show("Error al actualizar el Estudiante: " + result.StatusCode);
                }
                Limpiar();
            GetAllAlumnosViewModel();
        }
        private async void DeleteAlumno(int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44314/api/Alumnos");
                var response =
                    await client.DeleteAsync(String.Format("{0}/{1}", "https://localhost:44314/api/Alumnos", id));
                if (response.IsSuccessStatusCode)
                    MessageBox.Show("El Estudiante eliminado con éxito");
                else
                    MessageBox.Show("No se pudo eliminar el Estudiante Solicitado: " + response.StatusCode);
            }
            Limpiar();
            GetAllAlumnosViewModel();
        }

        private void dgvEscuela_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            foreach (DataGridViewRow row in dgvEscuela.Rows)
            {
                if (row.Index == e.RowIndex)
                {
                    id = int.Parse(row.Cells[0].Value.ToString());
                    GetAlumnoById(id);
                }
            }
        }

        private void btnIngresar_Click(object sender, EventArgs e)
        {
            AddAlumno();
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            if (id != 0)
                UpdateAlumno(id);
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (id != 0)
                DeleteAlumno(id);
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            Limpiar();
        }
    }
}
