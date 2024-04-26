using ControleMedicamentos.ConsoleApp.Compartilhado;
namespace ControleMedicamentos.ConsoleApp.ModuloMedicamento
{
    internal class RepositorioMedicamento : Repositorio
    {
        public void Cadastrar(string nome, string descricao, string fornecedor, int quantidade, int quantidadeCritica)
        {
            entidade[contador] = new Medicamento(nome, descricao, fornecedor, quantidade, quantidadeCritica);
            contador++;
        }
        public void Editar(string nome, string descricao, string fornecedor, int quantidade, int quantidadeCritica, int indexEditar)
        {
            entidade[indexEditar] = new Medicamento(nome, descricao, fornecedor, quantidade, quantidadeCritica);
        }
        public void DarBaixa(int indexDarBaixa, string darBaixa, int quantidade)
        {
            for (int i = 0; i < entidade.Length; i++) if (entidade[i] != null) if (entidade[i].nome == darBaixa) entidade[i].quantidade -= quantidade;
        }
    }
}
