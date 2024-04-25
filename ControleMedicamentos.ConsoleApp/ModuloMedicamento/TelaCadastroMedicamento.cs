namespace ControleMedicamentos.ConsoleApp.ModuloMedicamento
{
    internal class TelaCadastroMedicamento
    {
        public RepositorioMedicamento repositorioMedicamento = new RepositorioMedicamento();

        public void MenuMedicamento(ref bool sair)
        {
            bool repetir = false;
            do
            {
                CabecalhoMedicamentos();
                Console.WriteLine("------------------------------------------------");
                
                repetir = false;
                string opcao = RecebeString("\n       1. Cadastrar um novo medicamento\n\t   2. Pesquisar medicamento\n\t     3. Visualizar estoque\n\t     4. Editar medicamento\n\t     5. Excluir medicamento\n\n\t R. Retornar ao menu principal\n\t\t   S. Sair\n------------------------------------------------\n\n Digite: ");
                switch (opcao)
                {
                    case "1": CadastroMedicamento(ref sair, ref repetir); break;
                    case "2": PesquisaMedicamento(ref sair, ref repetir); break;
                    case "3": VisualizarMedicamentos(ref sair, ref repetir); break;
                    case "4": EditarMedicamentos(ref sair, ref repetir); break;
                    case "5": ExcluirMedicamentos(ref sair, ref repetir); break;
                    case "S": sair = true; break;
                    case "R": break;
                    default: OpcaoInvalida(ref opcao, ref sair, ref repetir); break;
                }
            }
            while (repetir);
        }
        public void CadastroMedicamento(ref bool sair, ref bool repetir)
        {
            CabecalhoMedicamentos();
            Console.WriteLine("\t\t  Cadastrar\n------------------------------------------------");

            string nome = RecebeString("\n Informe o nome: ");
            int index = repositorioMedicamento.EsteMedicamentoExiste(nome);
            if (index != -1) MedicamentoJaExiste(index, ref sair, ref repetir);
            else
            {
                string descricao = RecebeString(" Informe a descrição: ");
                string fornecedor = RecebeString(" Informe o fornecedor: ");
                int quantidade = RecebeInt(" Informe a quantidade em estoque: ");
                int quantidadeCritica = RecebeInt(" Informe a quantidade mínima para este medicamento: ");
                repositorioMedicamento.Cadastrar(nome, descricao, fornecedor, quantidade, quantidadeCritica);
                RealizadoComSucesso("cadastrado");
            }
        }
        public void PesquisaMedicamento(ref bool sair, ref bool repetir)
        {
            CabecalhoMedicamentos();
            int index = repositorioMedicamento.EsteMedicamentoExiste(RecebeString("\t\t    Pesquisar\n------------------------------------------------\n\n Informe o nome do medicamento: "));

            if (index == -1) MedicamentoNaoCadastrado(ref sair, ref repetir);
            else 
            {
                Console.WriteLine($"\n Medicamento = {repositorioMedicamento.medicamento[index].nome}\n Fornecedor = {repositorioMedicamento.medicamento[index].fornecedor}");
                if (repositorioMedicamento.QuantidadeEstaCritica(index)) Notificação(ConsoleColor.Red, $" Quantidade em estoque = {repositorioMedicamento.medicamento[index].quantidade}   Quantidade baixa!\n\n");
                else Console.WriteLine($" Quantidade em estoque = {repositorioMedicamento.medicamento[index].quantidade}\n\n");
                ParaRetornarAoMenu(ref sair, ref repetir);
            }
        }
        public void VisualizarMedicamentos(ref bool sair, ref bool repetir)
        {
            CabecalhoMedicamentos();
            Console.WriteLine("\t\t  Visualizar");

            if (repositorioMedicamento.EstoqueEstaVazio()) EstoqueVazio(ref sair, ref repetir);
            else
            {
                Visualizar();
                ParaRetornarAoMenu(ref sair, ref repetir);
            }
        }
        public void EditarMedicamentos(ref bool sair, ref bool repetir)
        {
            CabecalhoMedicamentos();
            Console.WriteLine("\t\t    Editar");

            if (repositorioMedicamento.EstoqueEstaVazio()) EstoqueVazio(ref sair, ref repetir);
            else
            {
                Visualizar();
                int indexEditar = repositorioMedicamento.EsteMedicamentoExiste(RecebeString("\n Informe o nome do medicamento a editar: "));

                if (indexEditar == -1) MedicamentoNaoCadastrado(ref sair, ref repetir);
                else NomeValidoParaEdicao(indexEditar, ref sair, ref repetir);
            }
        }
        public void ExcluirMedicamentos(ref bool sair, ref bool repetir)
        {
            CabecalhoMedicamentos();
            Console.WriteLine("\t\t    Excluir");

            if (repositorioMedicamento.EstoqueEstaVazio()) EstoqueVazio(ref sair, ref repetir);
            else
            {
                Visualizar();
                int indexExcluir = repositorioMedicamento.EsteMedicamentoExiste(RecebeString("\n Informe o nome do medicamento a excluir: "));

                if (indexExcluir == -1) MedicamentoNaoCadastrado(ref sair, ref repetir);
                else NomeValidoParaExclusao(indexExcluir);
            }
        }

        #region Métodos Auxiliares
        #region Gerais
        public void CabecalhoMedicamentos()
        {
            Console.Clear();
            Console.WriteLine("------------------------------------------------\n  Controle de Medicamentos dos Postos de Saúde\n------------------------------------------------");
            Console.WriteLine("\t    GESTÃO DE MEDICAMENTOS");
        }
        public string RecebeString(string texto)
        {
            Console.Write(texto);
            return Console.ReadLine().ToUpper();
        }
        public void OpcaoInvalida(ref string opcao, ref bool sair, ref bool repetir)
        {
            Notificação(ConsoleColor.Red, "\n        Opção inválida. Tente novamente ");
            Console.ReadLine();
            repetir = true;
        }
        public void Notificação(ConsoleColor cor, string texto)
        {
            Console.ForegroundColor = cor;
            Console.Write(texto);
            Console.ResetColor();
        }
        public void RealizadoComSucesso(string texto)
        {
            Notificação(ConsoleColor.Green, $"\n\n       Requisição {texto} com sucesso!\n");
            RecebeString("     'Enter' para voltar ao menu principal \n                       ");
        }
        public void ParaRetornarAoMenu(ref bool sair, ref bool repetir)
        {
            string opcao = RecebeString("\n     'Enter' para voltar ao Menu Principal\n    'R' para voltar ao Menu de Medicamentos\n\t       'S' para Sair\n\n\t\t   Digite: ");
            if (opcao == "") ;
            else if (opcao == "R") repetir = true;
            else if (opcao == "S") sair = true;
            else OpcaoInvalida(ref opcao, ref sair, ref repetir);
        }
        #endregion

        #region Cadastro
        public int RecebeInt(string texto)
        {
            Console.Write(texto);
            char[] valorEmChar = Console.ReadLine().ToCharArray();
            string quantidade = ""; //O intuito é testar caracter por caracter e depois concatenar numa string, pra converter pra int

            for (int i = 0; i < valorEmChar.Length; i++) if (Convert.ToInt32(valorEmChar[i]) >= 48 && Convert.ToInt32(valorEmChar[i]) <= 57) quantidade += valorEmChar[i];
            if (quantidade.Length != valorEmChar.Length) NaoEhNumero(ref quantidade, texto);

            return Convert.ToInt32(quantidade);
        }
        public void NaoEhNumero(ref string quantidade, string texto)
        {
            Notificação(ConsoleColor.Red, "\n Não é um número! Tente novamente\n");
            quantidade = Convert.ToString(RecebeInt(texto));//Para garantir que, ao sair do loop, o método "RecebeInt" não vai puxar a "quantidade" original (nula)
        }
        public void MedicamentoJaExiste(int index, ref bool sair, ref bool repetir)
        {
            Notificação(ConsoleColor.Red, " Este medicamento já existe!\n");
            string opcao = RecebeString("\n 1. para retornar ao menu\n 2. para atualizar a quantidade\n S. para sair\n\n Digite: ");
            switch (opcao)
            {
                case "1": break;
                case "2": repositorioMedicamento.AtualizarQnt(RecebeInt("\n Informe a nova quantidade: "), index); break;
                case "S": sair = true; break;
                default: OpcaoInvalida(ref opcao, ref sair, ref repetir); break;
            }
        }
        #endregion

        #region Pesquisa
        public void MedicamentoNaoCadastrado(ref bool sair, ref bool repetir)
        {
            Notificação(ConsoleColor.Red, "\n     Este medicamento ainda não foi cadastrado!\n");
            string opcao = RecebeString("\n 1. Cadastrar medicamento\n R. Retornar\n S. Sair\n\n Digite: ");
            switch (opcao)
            {
                case "1": CadastroMedicamento(ref sair, ref repetir); break;
                case "R": repetir = true; break;
                case "S": sair = true; break;
                default: OpcaoInvalida(ref opcao, ref sair, ref repetir); break;
            }
        }
        #endregion

        #region Visualizar
        public void EstoqueVazio(ref bool sair, ref bool repetir)
        {
            Console.WriteLine("------------------------------------------------\n");
            Notificação(ConsoleColor.Red, "      Não existem medicamentos em estoque\n");
            ParaRetornarAoMenu(ref sair, ref repetir);
        }
        public void CabecalhoVisualizar() => Notificação(ConsoleColor.Blue, "\n------------------------------------------------\n Nome\t| Descrição\t| Fornecedor\t| Qnt\n------------------------------------------------\n");
        public void Visualizar()
        {
            CabecalhoVisualizar();

            for (int i = 0; i < repositorioMedicamento.medicamento.Length; i++) if (repositorioMedicamento.medicamento[i] != null)
                {
                    Console.Write($" {repositorioMedicamento.medicamento[i].nome}\t| {repositorioMedicamento.medicamento[i].descricao}\t\t| {repositorioMedicamento.medicamento[i].fornecedor}\t\t| ");
                    if (repositorioMedicamento.QuantidadeEstaCritica(i))
                    {
                        Notificação(ConsoleColor.Red, $"{repositorioMedicamento.medicamento[i].quantidade}\n");
                        Console.Write("------------------------------------------------\n");
                    }
                    else Console.Write($"{repositorioMedicamento.medicamento[i].quantidade}\n------------------------------------------------\n");
                }
        }
        #endregion

        #region Edição
        public void NomeValidoParaEdicao(int indexEditar, ref bool sair, ref bool repetir)
        {
            var objetoAuxiliar = repositorioMedicamento.medicamento[indexEditar];

            MenuParaEdicao(ref sair, ref repetir, objetoAuxiliar, out bool editado);
            if (!sair && !repetir)
            {
                repositorioMedicamento.Editar(objetoAuxiliar.nome, objetoAuxiliar.descricao, objetoAuxiliar.fornecedor, objetoAuxiliar.quantidade, objetoAuxiliar.quantidadeCritica, indexEditar);
                if (editado) RealizadoComSucesso("editado");
            }
        }
        public void MenuParaEdicao(ref bool sair, ref bool repetir, Medicamento objetoAuxiliar, out bool editado)
        {
            Console.Clear();
            CabecalhoMedicamentos();
            VisualizarParaEdicao(objetoAuxiliar);
            editado = true;
            string opcao = RecebeString(" Ótimo! O que deseja Editar?\n 1. nome\n 2. descricao\n 3. fornecedor\n 4. quantidade em estoque\n 5. quantidade mínima \n\n R. para retornar\n S. para sair\n\n Digite: ");
            switch (opcao)
            {
                case "1":
                    string nome = RecebeString("\n Informe o novo nome: ");
                    int index = repositorioMedicamento.EsteMedicamentoExiste(nome);
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
                default: OpcaoInvalida(ref opcao, ref sair, ref repetir); break;
            }
            if (opcao == "") repetir = true;
        }
        public void VisualizarParaEdicao(Medicamento objetoAuxiliar)
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
