using ControleMedicamentos.ConsoleApp.Compartilhado;

namespace ControleMedicamentos.ConsoleApp.ModuloMedicamento
{
    internal class TelaCadastroMedicamento : TelaCadastro
    {
        public RepositorioMedicamento repositorioMedicamento = new RepositorioMedicamento();

        public void MenuMedicamento(ref bool sair)
        {
            bool repetir = false;
            do
            {
                CabecalhoEntidade("MEDICAMENTOS");
                Console.WriteLine("------------------------------------------------");

                repetir = false;
                string opcao = RecebeString("\n       1. Cadastrar um novo medicamento\n\t   2. Pesquisar medicamento\n\t     3. Visualizar estoque\n\t     4. Editar medicamento\n\t     5. Excluir medicamento\n\n\t R. Retornar ao menu principal\n\t\t   S. Sair\n------------------------------------------------\n\n Digite: ");
                switch (opcao)
                {
                    case "1": CadastrarItem(ref sair, ref repetir); break;
                    case "2": PesquisaMedicamento(ref sair, ref repetir); break;
                    case "3": VisualizarMedicamentos(ref sair, ref repetir); break;
                    case "4": EditarMedicamentos(ref sair, ref repetir); break;
                    case "5": ExcluirMedicamentos(ref sair, ref repetir); break;
                    case "S": sair = true; break;
                    case "R": break;
                    default: OpcaoInvalida(ref repetir); break;
                }
            }
            while (repetir);
        }
        protected override void CadastrarItem(ref bool sair, ref bool repetir)
        {
            CabecalhoEntidade("MEDICAMENTOS");
            Console.WriteLine("\t\t  Cadastrar\n------------------------------------------------");

            string nome = RecebeString("\n Informe o nome: ");
            int index = repositorioMedicamento.EsteItemExiste(nome);
            if (index != -1) MedicamentoJaExiste(index, ref sair, ref repetir);
            else
            {
                string descricao = RecebeString(" Informe a descrição: ");
                string fornecedor = RecebeString(" Informe o fornecedor: ");
                int quantidade = RecebeInt(" Informe a quantidade em estoque: ");
                int quantidadeCritica = RecebeInt(" Informe a quantidade mínima para este medicamento: ");
                var novoMedicamento = new Medicamento(nome, descricao, fornecedor, quantidade, quantidadeCritica);

                repositorioMedicamento.Cadastrar(novoMedicamento);
                RealizadoComSucesso("cadastrado");
            }
        }
        public void PesquisaMedicamento(ref bool sair, ref bool repetir)
        {
            CabecalhoEntidade("MEDICAMENTOS");
            int index = repositorioMedicamento.EsteItemExiste(RecebeString("\t\t    Pesquisar\n------------------------------------------------\n\n Informe o nome do medicamento: "));

            if (index == -1) ItemNaoCadastrado(ref sair, ref repetir, "medicamento", this);
            else 
            {
                Console.WriteLine($"\n Medicamento = {repositorioMedicamento.entidade[index].nome}\n Fornecedor = {repositorioMedicamento.entidade[index].fornecedor}");
                if (repositorioMedicamento.QuantidadeEstaCritica(index)) Notificação(ConsoleColor.Red, $" Quantidade em estoque = {repositorioMedicamento.entidade[index].quantidade}   Quantidade baixa!\n\n");
                else Console.WriteLine($" Quantidade em estoque = {repositorioMedicamento.entidade[index].quantidade}\n\n");
                ParaRetornarAoMenu(ref sair, ref repetir);
            }
        }
        public void VisualizarMedicamentos(ref bool sair, ref bool repetir)
        {
            CabecalhoEntidade("MEDICAMENTOS");
            Console.WriteLine("\t\t  Visualizar");

            if (repositorioMedicamento.NaoExistemItens()) AindaNaoExistemItens(ref sair, ref repetir, "medicamentos cadastrados");
            else
            {
                Visualizar();
                ParaRetornarAoMenu(ref sair, ref repetir);
            }
        }
        public void EditarMedicamentos(ref bool sair, ref bool repetir)
        {
            CabecalhoEntidade("MEDICAMENTOS");
            Console.WriteLine("\t\t    Editar");

            if (repositorioMedicamento.NaoExistemItens()) AindaNaoExistemItens(ref sair, ref repetir, "medicamentos cadastrados");
            else
            {
                Visualizar();
                int indexEditar = repositorioMedicamento.EsteItemExiste(RecebeString("\n Informe o nome do medicamento a editar: "));

                if (indexEditar == -1) ItemNaoCadastrado(ref sair, ref repetir, "medicamento", this);
                else NomeValidoParaEdicao(indexEditar, ref sair, ref repetir);
            }
        }
        public void ExcluirMedicamentos(ref bool sair, ref bool repetir)
        {
            CabecalhoEntidade("MEDICAMENTOS");
            Console.WriteLine("\t\t    Excluir");

            if (repositorioMedicamento.NaoExistemItens()) AindaNaoExistemItens(ref sair, ref repetir, "medicamentos cadastrados");
            else
            {
                Visualizar();
                int indexExcluir = repositorioMedicamento.EsteItemExiste(RecebeString("\n Informe o nome do medicamento a excluir: "));

                if (indexExcluir == -1) ItemNaoCadastrado(ref sair, ref repetir, "medicamento", this);
                else NomeValidoParaExclusao(indexExcluir);
            }
        }

        #region Métodos Auxiliares

        #region Cadastro
        public void MedicamentoJaExiste(int index, ref bool sair, ref bool repetir)
        {
            Notificação(ConsoleColor.Red, " Este medicamento já existe!\n");
            string opcao = RecebeString("\n 1. para retornar ao menu\n 2. para atualizar a quantidade\n S. para sair\n\n Digite: ");
            switch (opcao)
            {
                case "1": break;
                case "2": repositorioMedicamento.AtualizarQnt(RecebeInt("\n Informe a nova quantidade: "), index); break;
                case "S": sair = true; break;
                default: OpcaoInvalida(ref repetir); break;
            }
        }
        #endregion

        #region Visualizar
        public void CabecalhoVisualizar() => Notificação(ConsoleColor.Blue, "\n------------------------------------------------\n Nome\t| Descrição\t| Fornecedor\t| Qnt\n------------------------------------------------\n");
        public void Visualizar()
        {
            CabecalhoVisualizar();

            for (int i = 0; i < repositorioMedicamento.entidade.Length; i++) if (repositorioMedicamento.entidade[i] != null)
                {
                    Console.Write($" {repositorioMedicamento.entidade[i].nome}\t| {repositorioMedicamento.entidade[i].descricao}\t\t| {repositorioMedicamento.entidade[i].fornecedor}\t\t| ");
                    if (repositorioMedicamento.QuantidadeEstaCritica(i))
                    {
                        Notificação(ConsoleColor.Red, $"{repositorioMedicamento.entidade[i].quantidade}\n");
                        Console.Write("------------------------------------------------\n");
                    }
                    else Console.Write($"{repositorioMedicamento.entidade[i].quantidade}\n------------------------------------------------\n");
                }
        }
        #endregion

        #region Edição
        public void NomeValidoParaEdicao(int indexEditar, ref bool sair, ref bool repetir)
        {
            var objetoAuxiliar = repositorioMedicamento.entidade[indexEditar];

            MenuParaEdicao(ref sair, ref repetir, objetoAuxiliar, out bool editado);
            if (!sair && !repetir)
            {
                var editarMedicamento = new Medicamento(objetoAuxiliar.nome, objetoAuxiliar.descricao, objetoAuxiliar.fornecedor, objetoAuxiliar.quantidade, objetoAuxiliar.quantidadeCritica);
                repositorioMedicamento.Editar(editarMedicamento, indexEditar);
                if (editado) RealizadoComSucesso("editado");
            }
        }
        public void MenuParaEdicao(ref bool sair, ref bool repetir, Entidades objetoAuxiliar, out bool editado)
        {
            Console.Clear();
            CabecalhoEntidade("MEDICAMENTOS");
            VisualizarParaEdicao(objetoAuxiliar);
            editado = true;
            string opcao = RecebeString(" Ótimo! O que deseja Editar?\n 1. nome\n 2. descricao\n 3. fornecedor\n 4. quantidade em estoque\n 5. quantidade mínima \n\n R. para retornar\n S. para sair\n\n Digite: ");
            switch (opcao)
            {
                case "1":
                    string nome = RecebeString("\n Informe o novo nome: ");
                    int index = repositorioMedicamento.EsteItemExiste(nome);
                    if (index == -1) objetoAuxiliar.nome = nome;
                    else
                    {
                        MedicamentoJaExiste(index, ref sair, ref repetir);
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
        }
        public void VisualizarParaEdicao(Entidades objetoAuxiliar)
        {
            Console.WriteLine("\t\t    Editar");
            CabecalhoVisualizar();

            Console.Write($" {objetoAuxiliar.nome}\t| {objetoAuxiliar.descricao}\t\t| {objetoAuxiliar.fornecedor}\t\t| ");
            if (objetoAuxiliar.quantidade <= objetoAuxiliar.quantidadeCritica) Notificação(ConsoleColor.Red, $"{objetoAuxiliar.quantidade}\n------------------------------------------------\n\n");
            else Console.Write($"{objetoAuxiliar.quantidade}\n------------------------------------------------\n\n");
        }
        #endregion

        #region Exclusão
        public void NomeValidoParaExclusao(int indexExcluir)
        {
            repositorioMedicamento.Excluir(indexExcluir);
            RealizadoComSucesso("excluido");
        }
        #endregion
        #endregion
    }
}
