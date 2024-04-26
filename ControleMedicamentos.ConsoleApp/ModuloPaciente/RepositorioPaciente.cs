using ControleMedicamentos.ConsoleApp.Compartilhado;
namespace ControleMedicamentos.ConsoleApp.ModuloPaciente
{
    internal class RepositorioPaciente : Repositorio
    {
        public void Cadastrar(string nome, string cpf, string endereco, string cartaoSUS)
        {
            entidade[contador] = new Paciente(nome, cpf, endereco, cartaoSUS);
            contador++;
        }
        public void Editar(string nome, string cpf, string endereco, string cartaoSUS, int indexEditar) => entidade[indexEditar] = new Paciente(nome, cpf, endereco, cartaoSUS);
    }
}
