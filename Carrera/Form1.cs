using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Carrera
{
    public partial class Form1 : Form
    {
        int Ganador = 0;
        string NombreGAna = string.Empty;

        private TaskScheduler _scheduler;
        public Form1()
        {
            InitializeComponent();
            _scheduler = TaskScheduler.FromCurrentSynchronizationContext();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnIniciar_Click(object sender, EventArgs e)
        {
            new Task(() =>
            {
                Task[] tareas =
                {
                    Task.Factory.StartNew(() => IncreaseValue( Automovil_1, 1),
                    CancellationToken.None,
                    TaskCreationOptions.LongRunning, _scheduler),
                    Task.Factory.StartNew(() => IncreaseValue( Automovil_2, 1),
                    CancellationToken.None,
                    TaskCreationOptions.LongRunning, _scheduler),
                    Task.Factory.StartNew(() => IncreaseValue( Automovil_3, 1),
                    CancellationToken.None,
                    TaskCreationOptions.LongRunning, _scheduler)
                };
                Task.WaitAll(tareas);
            }).Start();
            Ganador = 0;
            MessageBox.Show("El ganador de la carrera es el: " + NombreGAna);
        }
        private void IncreaseValue(ProgressBar p, int c)
        {
            int a;
            Random aleatorio = new Random();
            for (int result = 0; result < 100;)
            {
                a = aleatorio.Next(1, 15);
                if ((a + result) <= 100)
                {

                    result = a + result;
                }

                Thread.Sleep(50);
                UpdateView(p, result);
            }
        }
       
        private void UpdateView(ProgressBar p, int result)
        {

            p.Value = result;
            if (p.Value == 100 && Ganador == 0)
            {
                Ganador = Ganador + 1;
                NombreGAna = p.Name;

            }



            Application.DoEvents();
        }


        private void btnSalir_Click(object sender, EventArgs e)
        {
            
               Application.Exit();
        }
    }
}
