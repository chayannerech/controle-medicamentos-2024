using ControleMedicamentos.ConsoleApp.ModuloMedicamento;
using System.Security.Cryptography.X509Certificates;

namespace ControleMedicamentos.ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            TelaCadastroMedicamento telaMedicamento = new TelaCadastroMedicamento();
            bool sair = false;

            do Menu(ref sair, telaMedicamento);
            while (!sair);
        }

        static void Menu(ref bool sair, TelaCadastroMedicamento telaMedicamento)
        {
            Cabecalho();

            string opcao = RecebeString("\t    1. Gerir medicamentos\n\t     2. Gerir requisição\n\t      3. Gerir paciente\n\n\t\t   S. Sair\n------------------------------------------------\n\n Digite: ");
            switch (opcao)
            {
                case "1": telaMedicamento.MenuMedicamento(ref sair); break ;
                case "2": Console.WriteLine("requisição"); break;
                case "3": Console.WriteLine("paciente"); break;
                case "S": sair = true; break;
                default: OpcaoInvalida(opcao, ref sair); break;
            }
        }

        //Auxiliares
        static string RecebeString(string texto)
        {
            Console.Write(texto);
            Console.ForegroundColor = ConsoleColor.White;
            return Console.ReadLine().ToUpper();
        }
        static void OpcaoInvalida(string opcao, ref bool sair)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            opcao = RecebeString("\n Opção inválida. 'Enter' para tentar novamente ou 'S' para sair: ");
            if (opcao == "S") sair = true;
            else if (opcao != "") OpcaoInvalida(opcao, ref sair);
        }
        static void Cabecalho()
        {
            Console.Clear();
            Console.WriteLine("------------------------------------------------\n  Controle de Medicamentos dos Postos de Saúde\n------------------------------------------------\n");
        }
    }
}
