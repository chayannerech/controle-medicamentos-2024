using ControleMedicamentos.ConsoleApp.ModuloMedicamento;

namespace ControleMedicamentos.ConsoleApp.Compartilhado
{
    internal class Repositorio
    {
        public Entidades[] entidade = new Entidades[50];
        public Entidades[] entidadesMovimentadas = new Entidades[50];
        public int contador = 0, qntCritica, contadorMovimentadas = 0;

        public void Cadastrar(Entidades novaEntidade)
        {
            entidade[contador] = novaEntidade;
            contador++;
        }
        public void Editar(Entidades editarEntidade, int indexEditar) => entidade[indexEditar] = editarEntidade;
        public void Excluir(int indexExcluir) => entidade[indexExcluir] = null;
        public int EsteItemExiste(string pesquisar)
        {
            int index = -1;
            
            for (int i = 0; i < entidade.Length; i++) if (entidade[i] != null) if (entidade[i].id != null) if (entidade[i].id == Convert.ToInt32(pesquisar)) index = i;
            for (int i = 0; i < entidade.Length; i++) if (entidade[i] != null) if (entidade[i].nome != null) if (entidade[i].nome == pesquisar) index = i;

            return index;
        }
        public bool NaoExistemItens()
        {
            for (int i = 0; i < entidade.Length; i++) if (entidade[i] != null) return false;
            return true;
        }
        public void AtualizarQnt(int quantidade, int index) => entidade[index].quantidade = quantidade;
        public bool QuantidadeEstaCritica(int i) => entidade[i].quantidade <= entidade[i].quantidadeCritica;
        public void DarBaixa(Entidades[] medicamentoMovimentado, int indexDarBaixa)
        {
            entidadesMovimentadas[contadorMovimentadas] = entidade[indexDarBaixa];
            contadorMovimentadas++;

            for (int i = 0; i < medicamentoMovimentado.Length; i++) if (medicamentoMovimentado[i] != null) if (medicamentoMovimentado[i].nome == entidade[indexDarBaixa].medicamento) medicamentoMovimentado[i].quantidade -= entidade[indexDarBaixa].posologia;

            Excluir(indexDarBaixa);
        }
    }
}