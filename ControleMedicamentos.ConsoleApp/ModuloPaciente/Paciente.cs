using ControleMedicamentos.ConsoleApp.Compartilhado;
namespace ControleMedicamentos.ConsoleApp.ModuloPaciente
{
    internal class Paciente : EntidadeBase
    {
        public Paciente(string nome, string cpf, string endereco, string cartaoSUS, int id)
        {
            this.nome = nome;
            this.cpf = cpf;
            this.endereco = endereco;
            this.cartaoSUS = cartaoSUS;
            this.id = id;
        }
    }
}
