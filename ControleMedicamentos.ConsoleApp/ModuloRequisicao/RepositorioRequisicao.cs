namespace ControleMedicamentos.ConsoleApp.ModuloRequisicao
{
    internal class RepositorioRequisicao
    {
        public Requisicao[] requisicao = new Requisicao[50];
        public Requisicao[] requisicoesMovimentadas = new Requisicao[50];
        public int contador = 0, contadorMovimentadas = 0;

        public void Cadastrar(string medicamento, string paciente, int posologia, DateTime dataValidade)
        {
            requisicao[contador] = new Requisicao(medicamento, paciente, posologia, dataValidade, contador + 1);
            contador++;
        }
        public int ValidarId(string id)
        {
            int index = -1;
            for (int i = 0; i < requisicao.Length; i++) if (requisicao[i] != null) if (requisicao[i].id == Convert.ToInt32(id)) index = i;
            return index;
        }
        public void Editar(string medicamento, string paciente, int posologia, DateTime dataValidade, int indexEditar)
        {
            requisicao[indexEditar] = new Requisicao(medicamento, paciente, posologia, dataValidade, 2);
        }
        public void Excluir(int indexExcluir)
        {
            requisicao[indexExcluir] = null;
        }
        public void DarBaixa(int indexDarBaixa)
        {
            requisicoesMovimentadas[contadorMovimentadas] = requisicao[indexDarBaixa];
            contadorMovimentadas++;
        }

        public bool RepositorioVazio()
        {
            for (int i = 0; i < requisicao.Length; i++) if (requisicao[i] != null) return false;
            return true;
        }
    }
}
