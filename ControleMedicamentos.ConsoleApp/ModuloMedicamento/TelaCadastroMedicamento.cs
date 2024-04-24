namespace ControleMedicamentos.ConsoleApp.ModuloMedicamento
{
    internal class TelaCadastroMedicamento
    {
        RepositorioMedicamento repositorioMedicamento = new RepositorioMedicamento();

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
                    case "1": CadastroMedicamento(ref sair); break;
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
        public void CadastroMedicamento(ref bool sair)
        {
            CabecalhoMedicamentos();
            Console.WriteLine("\t\t  Cadastrar\n------------------------------------------------");

            string nome = RecebeString("\n Informe o nome: ");
            string descricao = RecebeString(" Informe a descrição: ");
            string fornecedor = RecebeString(" Informe o fornecedor: ");
            int quantidade = RecebeInt(" Informe a quantidade em estoque: ");
            int quantidadeCritica = RecebeInt(" Informe a quantidade mínima para este medicamento: ");
            repositorioMedicamento.Cadastrar(nome, descricao, fornecedor, quantidade, quantidadeCritica);
            RealizadoComSucesso("cadastrado");
        }
        public void PesquisaMedicamento(ref bool sair, ref bool repetir)
        {
            CabecalhoMedicamentos();
            string PesquisarIndex = RecebeString("\t\t    Pesquisar\n------------------------------------------------\n\n Informe o nome do medicamento: ");
            int index = repositorioMedicamento.PesquisarIndex(PesquisarIndex);

            if (index != -1) Pesquisa(repositorioMedicamento, index);
            else MedicamentoNaoExiste();

            ParaRetornarAoMenu(ref sair, ref repetir);
        }
        public void VisualizarMedicamentos(ref bool sair, ref bool repetir)
        {
            CabecalhoMedicamentos();
            Console.WriteLine("\t\t  Visualizar");

            if (repositorioMedicamento.EstoqueEstaVazio()) EstoqueVazio(ref sair, ref repetir);
            else
            {
                Visualizar(repositorioMedicamento);
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
                Visualizar(repositorioMedicamento);
                int indexEditar = repositorioMedicamento.PesquisarIndex(RecebeString("\n Informe o nome do medicamento a editar: "));

                if (indexEditar != -1) NomeValidoParaEdicao(repositorioMedicamento, indexEditar, ref sair, ref repetir);
                else
                {
                    MedicamentoNaoExiste();
                    ParaRetornarAoMenu(ref sair, ref repetir);
                }
            }
        }
        public void ExcluirMedicamentos(ref bool sair, ref bool repetir)
        {
            CabecalhoMedicamentos();
            Console.WriteLine("\t\t    Excluir");

            if (repositorioMedicamento.EstoqueEstaVazio()) EstoqueVazio(ref sair, ref repetir);
            else
            {
                Visualizar(repositorioMedicamento);
                int indexExcluir = repositorioMedicamento.PesquisarIndex(RecebeString("\n Informe o nome do medicamento a excluir: "));

                if (indexExcluir != -1)
                {
                    repositorioMedicamento.Excluir(indexExcluir);
                    RealizadoComSucesso("excluido");
                }
                else
                {
                    MedicamentoNaoExiste();
                    ParaRetornarAoMenu(ref sair, ref repetir);
                }
            }
        }

        //Auxiliar Menu
        public void CabecalhoMedicamentos()
        {
            Console.Clear();
            Console.WriteLine("------------------------------------------------\n  Controle de Medicamentos dos Postos de Saúde\n------------------------------------------------");
            Console.WriteLine("\t    GESTÃO DE MEDICAMENTOS");
        }
        public string RecebeString(string texto)
        {
            Console.Write(texto);
            Console.ForegroundColor = ConsoleColor.White;
            return Console.ReadLine().ToUpper();
        }
        public void OpcaoInvalida(ref string opcao, ref bool sair, ref bool repetir)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            opcao = RecebeString("\n Opção inválida. 'Enter' para tentar novamente ou 'S' para sair: ");

            if (opcao == "S") sair = true;
            else if (opcao != "") OpcaoInvalida(ref opcao, ref sair, ref repetir);
        }
        //Auxiliar Cadastro
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
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n Não é um número! Tente novamente");
            Console.ForegroundColor = ConsoleColor.White;

            quantidade = Convert.ToString(RecebeInt(texto));//Para garantir que, ao sair do loop, o método "RecebeInt" não vai puxar a "quantidade" original (nula)
        }
        public void RealizadoComSucesso(string texto)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n      Medicamento {texto} com sucesso!");
            Console.ForegroundColor = ConsoleColor.White;
            string opcao = RecebeString("     'Enter' para voltar ao menu principal ");
        }
        //Auxiliar Pesquisa
        public void Pesquisa(RepositorioMedicamento repositorioMedicamento, int index)
        {
            Console.WriteLine($"\n Medicamento = {repositorioMedicamento.medicamento[index].nome}\n Fornecedor = {repositorioMedicamento.medicamento[index].fornecedor}");
            if (repositorioMedicamento.QuantidadeEhCritica(index))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($" Quantidade em estoque = {repositorioMedicamento.medicamento[index].quantidade}   Quantidade baixa!\n\n");
                Console.ForegroundColor = ConsoleColor.White;
            }
            else Console.WriteLine($" Quantidade em estoque = {repositorioMedicamento.medicamento[index].quantidade}\n\n");
        }
        public void ParaRetornarAoMenu(ref bool sair, ref bool repetir)
        {
            string opcao = RecebeString("\n     'Enter' para voltar ao Menu Principal\n    'R' para voltar ao Menu de Medicamentos\n\t       'S' para Sair\n\n\t\t   Digite: ");
            if (opcao == "") ;
            else if (opcao == "R") repetir = true;
            else if (opcao == "S") sair = true;
            else OpcaoInvalida(ref opcao, ref sair, ref repetir);
        }
        public void MedicamentoNaoExiste()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n\n\t   Medicamento não existente!");
            Console.ForegroundColor = ConsoleColor.White;
        }
        //Auxiliar Visualizar
        public void EstoqueVazio(ref bool sair, ref bool repetir)
        {
            Console.WriteLine("------------------------------------------------\n");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("      Não existem medicamentos em estoque");
            Console.ForegroundColor = ConsoleColor.White;
            ParaRetornarAoMenu(ref sair, ref repetir);
        }
        public void Visualizar(RepositorioMedicamento repositorioMedicamento)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("\n------------------------------------------------\n Nome\t| Descrição\t| Fornecedor\t| Qnt\n------------------------------------------------");
            Console.ForegroundColor = ConsoleColor.White;

            for (int i = 0; i < repositorioMedicamento.medicamento.Length; i++)
            {
                if (repositorioMedicamento.medicamento[i] != null)
                {
                    Console.Write($" {repositorioMedicamento.medicamento[i].nome}\t| {repositorioMedicamento.medicamento[i].descricao}\t\t| {repositorioMedicamento.medicamento[i].fornecedor}\t\t| ");
                    if (repositorioMedicamento.QuantidadeEhCritica(i))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write($"{repositorioMedicamento.medicamento[i].quantidade}\n");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("------------------------------------------------\n");
                    }
                    else Console.Write($"{repositorioMedicamento.medicamento[i].quantidade}\n------------------------------------------------\n");
                }
            }
        }
        //Auxiliar Editar
        public void NomeValidoParaEdicao(RepositorioMedicamento repositorioMedicamento, int indexEditar, ref bool sair, ref bool repetir)
        {
            var objetoAuxiliar = repositorioMedicamento.medicamento[indexEditar];

            MenuParaEdicao(ref sair, ref repetir, objetoAuxiliar);
            if (!sair && !repetir)
            {
                repositorioMedicamento.Editar(objetoAuxiliar.nome, objetoAuxiliar.descricao, objetoAuxiliar.fornecedor, objetoAuxiliar.quantidade, objetoAuxiliar.quantidadeCritica, indexEditar);
                RealizadoComSucesso("editado");
            }
        }
        public void MenuParaEdicao(ref bool sair, ref bool repetir, Medicamento objetoAuxiliar)
        {
            Console.Clear();
            CabecalhoMedicamentos();
            VisualizarParaEdicao(objetoAuxiliar);

            string opcao = RecebeString(" Ótimo! O que deseja Editar?\n 1. nome\n 2. descricao\n 3. fornecedor\n 4. quantidade em estoque\n 5. quantidade mínima \n\n R. para retornar\n S. para sair\n\n Digite: ");
            switch (opcao)
            {
                case "1": objetoAuxiliar.nome = RecebeString("\n Informe o novo nome: "); break;
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
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("\n------------------------------------------------\n Nome\t| Descrição\t| Fornecedor\t| Qnt\n------------------------------------------------");
            Console.ForegroundColor = ConsoleColor.White;

            Console.Write($" {objetoAuxiliar.nome}\t| {objetoAuxiliar.descricao}\t\t| {objetoAuxiliar.fornecedor}\t\t| ");
            if (objetoAuxiliar.quantidade <= objetoAuxiliar.quantidadeCritica)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write($"{objetoAuxiliar.quantidade}\n");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("------------------------------------------------\n");
            }
            else Console.Write($"{objetoAuxiliar.quantidade}\n------------------------------------------------\n");

            Console.WriteLine();
        }
    }
}
