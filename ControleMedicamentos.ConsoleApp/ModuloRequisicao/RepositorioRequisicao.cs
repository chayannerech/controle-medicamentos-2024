using ControleMedicamentos.ConsoleApp.Compartilhado;
namespace ControleMedicamentos.ConsoleApp.ModuloRequisicao
{
    internal class RepositorioRequisicao : Repositorio
    {
        public void Cadastrar(string medicamento, string paciente, int posologia, DateTime dataValidade)
        {
            entidade[contador] = new Requisicao(medicamento, paciente, posologia, dataValidade, contador + 1);
            contador++;
        }
        public void Editar(string medicamento, string paciente, int posologia, DateTime dataValidade, int indexEditar) => entidade[indexEditar] = new Requisicao(medicamento, paciente, posologia, dataValidade, 2);
        public void DarBaixa(int indexDarBaixa)
        {
            entidadesMovimentadas[contadorMovimentadas] = entidade[indexDarBaixa];
            contadorMovimentadas++;
        }
        public int EsteIdExiste(string pesquisar)
        {
            int index = -1;
            for (int i = 0; i < entidade.Length; i++) if (entidade[i] != null) if (entidade[i].id == Convert.ToInt32(pesquisar)) index = i;
            return index;
        }
    }
}
