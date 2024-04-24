namespace ControleMedicamentos.ConsoleApp.ModuloRequisicao
{
    internal class RepositorioRequisicao
    {
        public Requisicao[] requisicao = new Requisicao[50];
        public int contador = 0;

        public void Cadastrar(string medicamento, string paciente, int posologia, DateTime dataValidade)
        {
            requisicao[contador] = new Requisicao(medicamento, paciente, posologia, dataValidade);
            contador++;
        }
        public int PesquisarIndex(string pesquisar)
        {
            int index = -1;
            for (int i = 0; i < requisicao.Length; i++) if (requisicao[i] != null) if (requisicao[i].paciente == pesquisar) index = i;
            return index;
        }
        public void Editar(string medicamento, string paciente, int posologia, DateTime dataValidade, int indexEditar)
        {
            requisicao[indexEditar] = new Requisicao(medicamento, paciente, posologia, dataValidade);
        }
        public void Excluir(int indexExcluir)
        {
            requisicao[indexExcluir] = null;
        }

        public bool NaoHaRequisicao()
        {
            for (int i = 0; i < requisicao.Length; i++) if (requisicao[i] != null) return false;
            return true;
        }
    }
}
