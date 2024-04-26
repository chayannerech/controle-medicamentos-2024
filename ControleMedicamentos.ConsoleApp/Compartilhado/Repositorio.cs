using ControleMedicamentos.ConsoleApp.ModuloMedicamento;

namespace ControleMedicamentos.ConsoleApp.Compartilhado
{
    internal class Repositorio
    {
        public Entidades[] entidade = new Entidades[50];
        public Entidades[] entidadesMovimentadas = new Entidades[50];
        public int contador = 0, qntCritica, contadorMovimentadas = 0;

        public int EsteItemExiste(string pesquisar)
        {
            int index = -1;
            for (int i = 0; i < entidade.Length; i++) if (entidade[i] != null) if (entidade[i].nome == pesquisar) index = i;
            return index;
        }
        public void Excluir(int indexExcluir)
        {
            entidade[indexExcluir] = null;
        }
        public bool NaoExistemItens()
        {
            for (int i = 0; i < entidade.Length; i++) if (entidade[i] != null) return false;
            return true;
        }
        public void AtualizarQnt(int quantidade, int index) => entidade[index].quantidade = quantidade;
        public bool QuantidadeEstaCritica(int i) => entidade[i].quantidade <= entidade[i].quantidadeCritica;

    }
}