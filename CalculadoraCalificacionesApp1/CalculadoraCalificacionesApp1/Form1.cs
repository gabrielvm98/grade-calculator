using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CalculadoraCalificacionesApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            // Asignacion de Validacion al escribir en los cuadros de texto
            textBox1.TextChanged += TextBox_TextChanged;
            textBox2.TextChanged += TextBox_TextChanged;
            textBox3.TextChanged += TextBox_TextChanged;
            textBox4.TextChanged += TextBox_TextChanged;
            textBox5.TextChanged += TextBox_TextChanged;
        }

        // Evento del boton para limpiar los campos
        private void buttonLimpiar_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            labelPromedio.Text = "";
        }

        // Evento del boton para calcular el promedio
        private void buttonCalcular_Click(object sender, EventArgs e)
        {
            try
            {
                // Intenta convertir los valores de los cuadros de texto a decimales
                double calificacion1 = Convert.ToDouble(textBox1.Text);
                double calificacion2 = Convert.ToDouble(textBox2.Text);
                double calificacion3 = Convert.ToDouble(textBox3.Text);
                double calificacion4 = Convert.ToDouble(textBox4.Text);
                double calificacion5 = Convert.ToDouble(textBox5.Text);

                // Validar si las calificaciones son válidas
                if (!IsValidCalificacion(calificacion1) || !IsValidCalificacion(calificacion2) ||
                    !IsValidCalificacion(calificacion3) || !IsValidCalificacion(calificacion4) ||
                    !IsValidCalificacion(calificacion5))
                {
                    throw new ArgumentOutOfRangeException("Las calificaciones deben estar entre 0 y 100.");
                }

                // Crea una instancia de CalculadoraPromedio y agrega calificaciones
                CalculadoraPromedio calculadoraPromedio = new CalculadoraPromedio();
                calculadoraPromedio.AddCalificacion(calificacion1);
                calculadoraPromedio.AddCalificacion(calificacion2);
                calculadoraPromedio.AddCalificacion(calificacion3);
                calculadoraPromedio.AddCalificacion(calificacion4);
                calculadoraPromedio.AddCalificacion(calificacion5);

                // Calcula el promedio y lo muestra en el Label de promedio
                double promedio = calculadoraPromedio.CalcularPromedio();
                labelPromedio.Text = promedio.ToString("F2");


            }
            catch (FormatException)
            {
                MessageBox.Show("Ingrese calificaciones válidas en todos los campos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Evento del boton para generar el reporte
        private void buttonReporte_Click(object sender, EventArgs e)
        {

            // Verifica si el Label de promedio está vacío
            if (string.IsNullOrWhiteSpace(labelPromedio.Text))
            {
                MessageBox.Show("No se puede generar el reporte sin un promedio calculado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Genera el nombre del archivo con fecha y hora actual
            string nombreArchivo = $"reporte-{DateTime.Now:yyyyMMddHHmmss}.csv";
            string rutaArchivo = @"C:\Users\Usuario\source\repos\CalculadoraCalificacionesApp1\CalculadoraCalificacionesApp1\";

            // Genera el reporte en un archivo CSV
            using (StreamWriter writer = new StreamWriter(rutaArchivo + nombreArchivo))
            {
                writer.WriteLine("Calificaciones");
                writer.WriteLine($"{label2.Text},{textBox1.Text}");
                writer.WriteLine($"{label3.Text},{textBox2.Text}");
                writer.WriteLine($"{label4.Text},{textBox3.Text}");
                writer.WriteLine($"{label5.Text},{textBox4.Text}");
                writer.WriteLine($"{label6.Text},{textBox5.Text}");
                writer.WriteLine($"Promedio: {labelPromedio.Text}");
            }
            MessageBox.Show("Reporte generado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Funcion para Validacion del ingreso de calificaciones
        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            if (sender is TextBox textBox)
            {
                if (!string.IsNullOrWhiteSpace(textBox.Text) && !IsValidDecimal(textBox.Text))
                {
                    MessageBox.Show("Ingrese un valor decimal válido entre 0 y 100.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    textBox.Text = "";
                }
            }
        }

        // Funciones auxiliares para validar que lo ingresado sea un numero decimal válido entre 0 y 100
        private bool IsValidDecimal(string input)
        {
            if (double.TryParse(input, out double value))
            {
                return IsValidCalificacion(value);
            }
            return false;
        }

        private bool IsValidCalificacion(double calificacion)
        {
            return calificacion >= 0 && calificacion <= 100;
        }
    }


    // Clase implementada para el calculo del promedio
    class CalculadoraPromedio
    {
        private List<double> calificaciones { get; set; }

        public CalculadoraPromedio()
        {
            this.calificaciones = new List<double>();
        }

        public void AddCalificacion(double calificacion)
        {
            calificaciones.Add(calificacion);
        }

        public double CalcularPromedio()
        {
            double suma = 0;
            foreach (double calificacion in calificaciones)
            {
                suma += calificacion;
            }
            return calificaciones.Count > 0 ? suma / calificaciones.Count : 0;
        }
    }
}
