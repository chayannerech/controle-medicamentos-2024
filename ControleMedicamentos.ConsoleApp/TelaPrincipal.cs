using ControleMedicamentos.ConsoleApp.Compartilhado;
using ControleMedicamentos.ConsoleApp.ModuloMedicamento;
using ControleMedicamentos.ConsoleApp.ModuloPaciente;
using ControleMedicamentos.ConsoleApp.ModuloRequisicao;
using Controlepacientes.ConsoleApp.ModuloPaciente;
namespace ControleMedicamentos.ConsoleApp
{
    internal class TelaPrincipal
    {
        static TelaCadastroMedicamento telaMedicamento = new TelaCadastroMedicamento(new RepositorioMedicamento(), "medicamento", "medicamentos");
        static TelaCadastroPaciente telaPaciente = new TelaCadastroPaciente(new RepositorioPaciente(), "paciente", "pacientes");
        TelaCadastroRequisicao telaRequisicao = new TelaCadastroRequisicao(telaPaciente, telaMedicamento, new RepositorioRequisicao(), "requisição", "requisições");

        public void MenuPrincipal(ref bool sair)
        {
            Console.Clear();
            Console.WriteLine("------------------------------------------------\n  Controle de Medicamentos dos Postos de Saúde\n------------------------------------------------\n");

            string opcao = telaMedicamento.RecebeString("\t    1. Gerir medicamentos\n\t      2. Gerir paciente\n\t     3. Gerir requisição\n\t 4. Dar baixa em requisição\n\n\t\t   S. Sair\n------------------------------------------------\n\n Digite: ");
            switch (opcao)
            {
                case "1": telaMedicamento.MenuEntidade(ref sair); break;
                case "2": telaPaciente.MenuEntidade(ref sair); break;
                case "3": telaRequisicao.MenuEntidade(ref sair); break;
                case "4": telaRequisicao.DarBaixaRequisicao(ref sair, ref sair); break;
                case "S": sair = true; break;
                default: OpcaoInvalida(ref sair); break;
            }
        }

        public void OpcaoInvalida(ref bool repetir)
        {
            telaMedicamento.Notificação(ConsoleColor.Red, "\n        Opção inválida. Tente novamente ");
            Console.ReadKey(true);
        }
    }
}
