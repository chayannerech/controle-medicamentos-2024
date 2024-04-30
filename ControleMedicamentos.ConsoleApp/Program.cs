using ControleMedicamentos.ConsoleApp.Compartilhado;
using ControleMedicamentos.ConsoleApp.ModuloMedicamento;
using ControleMedicamentos.ConsoleApp.ModuloPaciente;
using ControleMedicamentos.ConsoleApp.ModuloRequisicao;
using Controlepacientes.ConsoleApp.ModuloPaciente;
using System.Security.Cryptography.X509Certificates;
namespace ControleMedicamentos.ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            TelaPrincipal tela = new TelaPrincipal();
            bool sair = false;            
            do tela.MenuPrincipal(ref sair);
            while (!sair);
        }
    }
}