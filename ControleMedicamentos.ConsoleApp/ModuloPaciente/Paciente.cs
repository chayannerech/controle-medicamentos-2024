using ControleMedicamentos.ConsoleApp.Compartilhado;
namespace ControleMedicamentos.ConsoleApp.ModuloPaciente
{
    internal class Paciente : Entidades
    {
        public Paciente(string nome, string cpf, string endereco, string cartaoSUS)
        {
            this.nome = nome;
            this.cpf = cpf;
            this.endereco = endereco;
            this.cartaoSUS = cartaoSUS;
        }
    }
}
