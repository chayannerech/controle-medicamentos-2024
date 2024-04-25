namespace ControleMedicamentos.ConsoleApp.ModuloPaciente
{
    internal class RepositorioPaciente
    {
        public Paciente[] pacientes = new Paciente[50];
        public int contador = 0;

        public void Cadastrar(string nome, string cpf, string endereco, string cartaoSUS)
        {
            pacientes[contador] = new Paciente(nome, cpf, endereco, cartaoSUS);
            contador++;
        }
        public int EstePacienteExiste(string pesquisarNome)
        {
            int index = -1;
            for (int i = 0; i < pacientes.Length; i++) if (pacientes[i] != null) if (pacientes[i].nome == pesquisarNome) index = i;
            return index;
        }
        public void Editar(string nome, string cpf, string endereco, string cartaoSUS, int indexEditar)
        {
            pacientes[indexEditar] = new Paciente(nome, cpf, endereco, cartaoSUS);
        }
        public void Excluir(int indexExcluir)
        {
            pacientes[indexExcluir] = null;
        }

        public bool NaoExistemPacientes()
        {
            for (int i = 0; i < pacientes.Length; i++) if (pacientes[i] != null) return false;
            return true;
        }
    }
}
