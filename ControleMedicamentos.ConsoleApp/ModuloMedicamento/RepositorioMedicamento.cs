namespace ControleMedicamentos.ConsoleApp.ModuloMedicamento
{
    internal class RepositorioMedicamento
    {
        public Medicamento[] medicamento = new Medicamento[50];
        public int contador = 0, qntCritica;

        public void Cadastrar(string nome, string descricao, string fornecedor, int quantidade, int quantidadeCritica)
        {
            medicamento[contador] = new Medicamento(nome, descricao, fornecedor, quantidade, quantidadeCritica);
            contador++;
        }
        public int PesquisarIndex(string pesquisar)
        {
            int index = -1;
            for (int i = 0; i < medicamento.Length; i++) if (medicamento[i] != null) if (medicamento[i].nome == pesquisar) index = i;
            return index;
        }
        public void Editar(string nome, string descricao, string fornecedor, int quantidade, int quantidadeCritica, int indexEditar)
        {
            medicamento[indexEditar] = new Medicamento(nome, descricao, fornecedor, quantidade, quantidadeCritica);
        }
        public void Excluir(int indexExcluir)
        {
            medicamento[indexExcluir] = null;
        }
        public void DarBaixa(int indexDarBaixa, string darBaixa, int quantidade)
        {
            for (int i = 0; i < medicamento.Length; i++) if (medicamento[i] != null) if (medicamento[i].nome == darBaixa)
                    {
                        medicamento[i].quantidade -= quantidade;
                    }
        }

        public void AtualizarQnt(int quantidade, int index)
        {
            medicamento[index].quantidade = quantidade;
        }
        public int MedicamentoJaExiste(string nome)
        {
            int jaExiste = -1;
            for (int i = 0; i < medicamento.Length; i++) if (medicamento[i] != null) if (medicamento[i].nome == nome) jaExiste = i;
            return jaExiste;
        }
        public bool EstoqueEstaVazio()
        {
            for (int i = 0; i < medicamento.Length; i++) if (medicamento[i] != null) return false;
            return true;
        }
        public bool QuantidadeEhCritica(int i) => medicamento[i].quantidade <= medicamento[i].quantidadeCritica;
    }
}
