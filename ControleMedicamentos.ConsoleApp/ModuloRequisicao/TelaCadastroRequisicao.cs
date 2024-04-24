namespace ControleMedicamentos.ConsoleApp.ModuloRequisicao
{
    internal class TelaCadastroRequisicao
    {
        RepositorioRequisicao repositorioRequisicao = new RepositorioRequisicao();

        public void MenuRequisicao(ref bool sair)
        {
            bool repetir = false;
            do
            {
                CabecalhoRequisicao();
                Console.WriteLine("------------------------------------------------");

                repetir = false;
                string opcao = RecebeString("\n        1. Cadastrar uma nova requisição\n\t   2. Visualizar requisiçãos\n\t      3. Editar requisição\n\t     4. Excluir requisição\n\n\t R. Retornar ao menu principal\n\t\t   S. Sair\n------------------------------------------------\n\n Digite: ");
                switch (opcao)
                {
                    case "1": CadastroRequisicao(ref sair); break;
                    case "2": VisualizarRequisicao(ref sair, ref repetir); break;
                    case "3": EditarRequisicao(ref sair, ref repetir); break;
                    case "4": ExcluirRequisicao(ref sair, ref repetir); break;
                    case "S": sair = true; break;
                    case "R": break;
                    default: OpcaoInvalida(ref opcao, ref sair, ref repetir); break;
                }
            }
            while (repetir);
        }
        public void CadastroRequisicao(ref bool sair)
        {
            CabecalhoRequisicao();
            Console.WriteLine("\t\t  Cadastrar\n------------------------------------------------");

            string medicamento = RecebeString("\n Informe o medicamento: ");
            string paciente = RecebeString(" Informe o paciente: ");
            int posologia = RecebeInt(" Informe a posologia: ");
            DateTime dataValidade = Convert.ToDateTime(RecebeString(" Informe a data de validade: "));
            repositorioRequisicao.Cadastrar(medicamento, paciente, posologia, dataValidade);
            RealizadoComSucesso("cadastrado");
        }
        public void VisualizarRequisicao(ref bool sair, ref bool repetir)
        {
            CabecalhoRequisicao();
            Console.WriteLine("\t\t  Visualizar");

            if (repositorioRequisicao.NaoHaRequisicao()) SemRequisicao(ref sair, ref repetir);
            else
            {
                Visualizar(repositorioRequisicao);
                ParaRetornarAoMenu(ref sair, ref repetir);
            }
        }
        public void EditarRequisicao(ref bool sair, ref bool repetir)
        {
            CabecalhoRequisicao();
            Console.WriteLine("\t\t    Editar");

            if (repositorioRequisicao.NaoHaRequisicao()) SemRequisicao(ref sair, ref repetir);
            else
            {
                Visualizar(repositorioRequisicao);
                int indexEditar = repositorioRequisicao.PesquisarIndex(RecebeString("\n Informe o medicamento a editar: "));

                if (indexEditar != -1) NomeValidoParaEdicao(repositorioRequisicao, indexEditar, ref sair, ref repetir);
                else
                {
                    RequisicaoNaoExiste();
                    ParaRetornarAoMenu(ref sair, ref repetir);
                }
            }
        }
        public void ExcluirRequisicao(ref bool sair, ref bool repetir)
        {
            CabecalhoRequisicao();
            Console.WriteLine("\t\t    Excluir");

            if (repositorioRequisicao.NaoHaRequisicao()) SemRequisicao(ref sair, ref repetir);
            else
            {
                Visualizar(repositorioRequisicao);
                int indexExcluir = repositorioRequisicao.PesquisarIndex(RecebeString("\n Informe o medicamento a excluir: "));

                if (indexExcluir != -1)
                {
                    repositorioRequisicao.Excluir(indexExcluir);
                    RealizadoComSucesso("excluido");
                }
                else
                {
                    RequisicaoNaoExiste();
                    ParaRetornarAoMenu(ref sair, ref repetir);
                }
            }
        }

        //Auxiliar Menu
        public void CabecalhoRequisicao()
        {
            Console.Clear();
            Console.WriteLine("------------------------------------------------\n  Controle de Medicamentos dos Postos de Saúde\n------------------------------------------------");
            Console.WriteLine("\t     GESTÃO DE REQUISIÇÕES");
        }
        public string RecebeString(string texto)
        {
            Console.Write(texto);
            Console.ResetColor();
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
            string posologia = ""; //O intuito é testar caracter por caracter e depois concatenar numa string, pra converter pra int

            for (int i = 0; i < valorEmChar.Length; i++) if (Convert.ToInt32(valorEmChar[i]) >= 48 && Convert.ToInt32(valorEmChar[i]) <= 57) posologia += valorEmChar[i];
            if (posologia.Length != valorEmChar.Length) NaoEhNumero(ref posologia, texto);

            return Convert.ToInt32(posologia);
        }
        public void NaoEhNumero(ref string quantidade, string texto)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n Não é um número! Tente novamente");
            Console.ResetColor();

            quantidade = Convert.ToString(RecebeInt(texto));//Para garantir que, ao sair do loop, o método "RecebeInt" não vai puxar a "quantidade" original (nula)
        }
        public void RealizadoComSucesso(string texto)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n       Requisição {texto} com sucesso!");
            Console.ResetColor();
            string opcao = RecebeString("     'Enter' para voltar ao menu principal ");
        }
        //Auxiliar Pesquisa
        public void ParaRetornarAoMenu(ref bool sair, ref bool repetir)
        {
            string opcao = RecebeString("\n     'Enter' para voltar ao Menu Principal\n    'R' para voltar ao Menu de requisições\n\t       'S' para Sair\n\n\t\t   Digite: ");
            if (opcao == "") ;
            else if (opcao == "R") repetir = true;
            else if (opcao == "S") sair = true;
            else OpcaoInvalida(ref opcao, ref sair, ref repetir);
        }
        public void RequisicaoNaoExiste()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n\n\t   Requisição não existente!");
            Console.ResetColor();
        }
        //Auxiliar Visualizar
        public void SemRequisicao(ref bool sair, ref bool repetir)
        {
            Console.WriteLine("------------------------------------------------\n");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("      Não existem requisições cadastradas");
            Console.ResetColor();
            ParaRetornarAoMenu(ref sair, ref repetir);
        }
        public void Visualizar(RepositorioRequisicao repositorioRequisicao)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("\n------------------------------------------------------------\n Medicamento\t| Paciente\t| Posologia\t| Validade\n------------------------------------------------------------");
            Console.ResetColor();

            for (int i = 0; i < repositorioRequisicao.requisicao.Length; i++)
            {
                if (repositorioRequisicao.requisicao[i] != null) Console.Write($" {repositorioRequisicao.requisicao[i].medicamento}\t\t| {repositorioRequisicao.requisicao[i].paciente}\t\t| {repositorioRequisicao.requisicao[i].posologia}\t\t| {repositorioRequisicao.requisicao[i].dataValidade.ToString("d")}\n------------------------------------------------------------\n");
            }
        }
        //Auxiliar Editar
        public void NomeValidoParaEdicao(RepositorioRequisicao repositorioRequisicao, int indexEditar, ref bool sair, ref bool repetir)
        {
            var objetoAuxiliar = repositorioRequisicao.requisicao[indexEditar];

            MenuParaEdicao(ref sair, ref repetir, objetoAuxiliar);
            if (!sair && !repetir)
            {
                repositorioRequisicao.Editar(objetoAuxiliar.medicamento, objetoAuxiliar.paciente, objetoAuxiliar.posologia, objetoAuxiliar.dataValidade, indexEditar);
                RealizadoComSucesso("editado");
            }
        }
        public void MenuParaEdicao(ref bool sair, ref bool repetir, Requisicao objetoAuxiliar)
        {
            Console.Clear();
            CabecalhoRequisicao();
            VisualizarParaEdicao(objetoAuxiliar);

            string opcao = RecebeString(" Ótimo! O que deseja Editar?\n 1. medicamento\n 2. paciente\n 3. endereço\n 4. cartão SUS\n\n R. para retornar\n S. para sair\n\n Digite: ");
            switch (opcao)
            {
                case "1": objetoAuxiliar.medicamento = RecebeString("\n Informe o novo medicamento: "); break;
                case "2": objetoAuxiliar.paciente = RecebeString("\n Informe o novo paciente: "); break;
                case "3": objetoAuxiliar.posologia = RecebeInt("\n Informe o novo endereço: "); break;
                case "4": objetoAuxiliar.dataValidade = Convert.ToDateTime(RecebeString("\n Informe o novo cartão SUS: ")); break;
                case "R": repetir = true; break;
                case "S": sair = true; break;
                default: OpcaoInvalida(ref opcao, ref sair, ref repetir); break;
            }
            if (opcao == "") repetir = true;
        }
        public void VisualizarParaEdicao(Requisicao objetoAuxiliar)
        {
            Console.WriteLine("\t\t    Editar");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("\n------------------------------------------------------\n Nome\t| paciente\t| Posologia\t| Cartão SUS\n------------------------------------------------------");
            Console.ResetColor();

            Console.Write($" {objetoAuxiliar.medicamento}\t| {objetoAuxiliar.paciente}\t\t| {objetoAuxiliar.posologia}\t\t| {objetoAuxiliar.dataValidade.ToString("d")}\n------------------------------------------------------\n\n");
        }
    }
}
