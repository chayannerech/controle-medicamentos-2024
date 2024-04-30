using ControleMedicamentos.ConsoleApp.Compartilhado;
namespace ControleMedicamentos.ConsoleApp.ModuloMedicamento
{
    internal class TelaCadastroMedicamento : TelaBase
    {
        public TelaCadastroMedicamento(RepositorioBase repositorio, string nome, string NOME)
        {
            this.repositorio = repositorio;
            tipoEntidade = nome;
            tipoEntidadePlural = NOME;
        }

        protected override void CadastrarItem(ref bool sair, ref bool repetir)
        {
            CabecalhoEntidade("MEDICAMENTOS");
            Console.WriteLine("\t\t  Cadastrar\n------------------------------------------------");

            string nome = RecebeString("\n Informe o nome: ");
            int index = repositorio.EsteItemExiste(nome);
            if (index != -1) ItemJaExiste(ref sair, ref repetir);
            else
            {
                string descricao = RecebeString(" Informe a descrição: ");
                string fornecedor = RecebeString(" Informe o fornecedor: ");
                int quantidade = RecebeInt(" Informe a quantidade em estoque: ");
                int quantidadeCritica = RecebeInt(" Informe a quantidade mínima para este medicamento: ");
                var novoMedicamento = new Medicamento(nome, descricao, fornecedor, quantidade, quantidadeCritica, repositorio.contador + 1);

                repositorio.Cadastrar(novoMedicamento);
                RealizadoComSucesso("cadastrado");
            }
        }

        #region Métodos Auxiliares
        #region Visualizar
        protected override void CabecalhoVisualizar() => Notificação(ConsoleColor.Blue, "\n-----------------------------------------------------\n ID\t| Nome\t| Descrição\t| Fornecedor\t| Qnt\n-----------------------------------------------------\n");
        protected override void ListaItensParaVisualizar(int i)
        {
            Console.Write($" {repositorio.entidade[i].id}\t| {repositorio.entidade[i].nome}\t| {repositorio.entidade[i].descricao}\t\t| {repositorio.entidade[i].fornecedor}\t\t| ");
            if (repositorio.QuantidadeEstaCritica(i))
            {
                Notificação(ConsoleColor.Red, $"{repositorio.entidade[i].quantidade}\n");
                Console.Write("-----------------------------------------------------\n");
            }
            else Console.Write($"{repositorio.entidade[i].quantidade}\n-----------------------------------------------------\n");
        }
        #endregion
        #region Edição
        protected override void MenuEdicao(ref bool sair, ref bool repetir, TelaBase telaMedicamento, TelaBase telaPaciente, int indexEditar)
        {
            CabecalhoEntidade("MEDICAMENTOS");
            var objetoAuxiliar = repositorio.entidade[indexEditar]; bool editado = true;
            VisualizarParaEdicao(objetoAuxiliar);

            string opcao = RecebeString(" Ótimo! O que deseja Editar?\n 1. nome\n 2. descricao\n 3. fornecedor\n 4. quantidade em estoque\n 5. quantidade mínima \n\n R. para retornar\n S. para sair\n\n Digite: ");
            switch (opcao)
            {
                case "1":
                    string nome = RecebeString("\n Informe o novo nome: ");
                    int index = repositorio.EsteItemExiste(nome);
                    if (index == -1) objetoAuxiliar.nome = nome;
                    else
                    {
                        ItemJaExiste(ref sair, ref repetir);
                        editado = false;
                    }
                    break;
                case "2": objetoAuxiliar.descricao = RecebeString("\n Informe a nova descrição: "); break;
                case "3": objetoAuxiliar.fornecedor = RecebeString("\n Informe o novo fornecedor: "); break;
                case "4": objetoAuxiliar.quantidade = RecebeInt("\n Informe a nova quantidade: "); break;
                case "5": objetoAuxiliar.quantidadeCritica = RecebeInt("\n Informe a nova quantidade mínima: "); break;
                case "R": repetir = true; break;
                case "S": sair = true; break;
                default: OpcaoInvalida(ref repetir); break;
            }
            if (opcao == "") repetir = true;
            if (!sair && !repetir)
            {
                var editarMedicamento = new Medicamento(objetoAuxiliar.nome, objetoAuxiliar.descricao, objetoAuxiliar.fornecedor, objetoAuxiliar.quantidade, objetoAuxiliar.quantidadeCritica, objetoAuxiliar.id);
                repositorio.Editar(editarMedicamento, indexEditar);
                if (editado) RealizadoComSucesso("editado");
            }
        }
        protected override void VisualizarParaEdicao(EntidadeBase objetoAuxiliar)
        {
            Console.WriteLine("\t\t    Editar");
            CabecalhoVisualizar();
            Console.Write($" {objetoAuxiliar.id}\t| {objetoAuxiliar.nome}\t| {objetoAuxiliar.descricao}\t\t| {objetoAuxiliar.fornecedor}\t\t| ");

            if (objetoAuxiliar.quantidade <= objetoAuxiliar.quantidadeCritica) { Notificação(ConsoleColor.Red, $"{objetoAuxiliar.quantidade}\n"); Console.WriteLine("-----------------------------------------------------\n\n"); }
            else Console.Write($"{objetoAuxiliar.quantidade}\n-----------------------------------------------------\n\n");
        }
        #endregion
        #endregion
    }
}
