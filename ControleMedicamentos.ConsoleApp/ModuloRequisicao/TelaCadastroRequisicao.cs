using ControleMedicamentos.ConsoleApp.ModuloMedicamento;
using ControleMedicamentos.ConsoleApp.ModuloPaciente;
using Controlepacientes.ConsoleApp.ModuloPaciente;
namespace ControleMedicamentos.ConsoleApp.ModuloRequisicao
{
    internal class TelaCadastroRequisicao
    {
        TelaCadastroPaciente telaPaciente;
        TelaCadastroMedicamento telaMedicamento;
        RepositorioRequisicao repositorioRequisicao = new RepositorioRequisicao();
        public TelaCadastroRequisicao(TelaCadastroPaciente telaPaciente, TelaCadastroMedicamento telaMedicamento)
        {
            this.telaPaciente = telaPaciente;
            this.telaMedicamento = telaMedicamento;
        }

        public void MenuRequisicao(ref bool sair)
        {
            bool repetir = false;
            do
            {
                CabecalhoRequisicao();
                Console.WriteLine("------------------------------------------------");

                repetir = false;
                string opcao = RecebeString("\n        1. Cadastrar uma nova requisição\n\t   2. Visualizar requisições\n\t      3. Editar requisição\n\t     4. Excluir requisição\n         5. Dar baixa na requisição\n\n\t R. Retornar ao menu principal\n\t\t   S. Sair\n------------------------------------------------\n\n Digite: ");
                switch (opcao)
                {
                    case "1": CadastroRequisicao(ref sair, ref repetir); break;
                    case "2": VisualizarRequisicao(ref sair, ref repetir); break;
                    case "3": EditarRequisicao(ref sair, ref repetir); break;
                    case "4": ExcluirRequisicao(ref sair, ref repetir); break;
                    case "5": DarBaixaRequisicao(ref sair, ref repetir); break;
                    case "S": sair = true; break;
                    case "R": break;
                    default: OpcaoInvalida(ref opcao, ref sair, ref repetir); break;
                }
            }
            while (repetir);
        }
        public void CadastroRequisicao(ref bool sair, ref bool repetir)
        {
            CabecalhoRequisicao();
            Console.WriteLine("\t\t  Cadastrar\n------------------------------------------------");

            string paciente = RecebeString(" Informe o nome do paciente: ");
            int jaExiste = telaPaciente.repositorioPaciente.EstePacienteExiste(paciente);
            if (jaExiste == -1) PacienteNaoCadastrado(ref sair, ref repetir);
            else
            {
                string medicamento = RecebeString(" Informe o medicamento: ");
                jaExiste = telaMedicamento.repositorioMedicamento.EsteMedicamentoExiste(medicamento);
                if (jaExiste == -1) MedicamentoNaoCadastrado(ref sair, ref repetir);
                else
                {
                    int posologia = RecebeInt(" Informe a posologia: ");
                    DateTime dataValidade = RecebeData(" Informe a data de validade: ");

                    repositorioRequisicao.Cadastrar(medicamento, paciente, posologia, dataValidade);
                    RealizadoComSucesso("cadastrada");
                }
            }
        }
        public void VisualizarRequisicao(ref bool sair, ref bool repetir)
        {
            CabecalhoRequisicao();
            Console.WriteLine("\t\t  Visualizar");

            if (repositorioRequisicao.RepositorioVazio()) AindaNaoExistemRequisicoes(ref sair, ref repetir);
            else
            {
                ListarRequisicoes(repositorioRequisicao);
                ParaRetornarAoMenu(ref sair, ref repetir);
            }
        }
        public void EditarRequisicao(ref bool sair, ref bool repetir)
        {
            CabecalhoRequisicao();
            Console.WriteLine("\t\t    Editar");

            if (repositorioRequisicao.RepositorioVazio()) AindaNaoExistemRequisicoes(ref sair, ref repetir);
            else
            {
                ListarRequisicoes(repositorioRequisicao);
                int indexEditar = repositorioRequisicao.ValidarId(RecebeString("\n Informe o ID que deseja editar: "));

                if (indexEditar == -1) IdNaoValido(ref sair, ref repetir);
                else IdValidoParaEdicao(repositorioRequisicao, indexEditar, ref sair, ref repetir);
            }
        }
        public void ExcluirRequisicao(ref bool sair, ref bool repetir)
        {
            CabecalhoRequisicao();
            Console.WriteLine("\t\t    Excluir");

            if (repositorioRequisicao.RepositorioVazio()) AindaNaoExistemRequisicoes(ref sair, ref repetir);
            else
            {
                ListarRequisicoes(repositorioRequisicao);
                int indexExcluir = repositorioRequisicao.ValidarId(RecebeString("\n Informe o ID que deseja excluir: "));

                if (indexExcluir == -1) IdNaoValido(ref sair, ref repetir);
                else IdValidoParaExclusao(repositorioRequisicao, indexExcluir);
            }
        }
        public void DarBaixaRequisicao(ref bool sair, ref bool repetir)
        {
            CabecalhoRequisicao();
            Console.WriteLine("\t\t   Dar Baixa");

            if (repositorioRequisicao.RepositorioVazio()) AindaNaoExistemRequisicoes(ref sair, ref repetir);
            else
            {
                ListarRequisicoes(repositorioRequisicao);
                int indexDarBaixa = repositorioRequisicao.ValidarId(RecebeString("\n Informe o ID que deseja dar baixa: "));

                if (indexDarBaixa == -1) IdNaoValido(ref sair, ref repetir);
                else IdValidoParaMovimentacao(telaMedicamento, indexDarBaixa, repositorioRequisicao);
            }
        }

        #region Métodos Auxiliares
        #region Gerais
        public void CabecalhoRequisicao()
        {
            Console.Clear();
            Console.WriteLine("------------------------------------------------\n  Controle de Medicamentos dos Postos de Saúde\n------------------------------------------------");
            Console.WriteLine("\t     GESTÃO DE REQUISIÇÕES");
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
            string opcao = RecebeString("\n     'Enter' para voltar ao Menu Principal\n    'R' para voltar ao Menu de Requisições\n\t       'S' para Sair\n\n\t\t   Digite: ");
            if (opcao == "") ;
            else if (opcao == "R") repetir = true;
            else if (opcao == "S") sair = true;
            else OpcaoInvalida(ref opcao, ref sair, ref repetir);
        }
        #endregion

        #region Cadastro
        public string RecebeString(string texto)
        {
            Console.Write(texto);
            return Console.ReadLine().ToUpper();
        }
        public int RecebeInt(string texto)
        {
            Console.Write(texto);
            char[] valorEmChar = Console.ReadLine().ToCharArray();
            string posologia = ""; //O intuito é testar caracter por caracter e depois concatenar numa string, pra converter pra int

            for (int i = 0; i < valorEmChar.Length; i++) if (Convert.ToInt32(valorEmChar[i]) >= 48 && Convert.ToInt32(valorEmChar[i]) <= 57) posologia += valorEmChar[i];
            if (posologia.Length != valorEmChar.Length) NaoEhNumero(ref posologia, texto);

            return Convert.ToInt32(posologia);
        }
        public DateTime RecebeData(string texto)
        {
            string data = RecebeString(" Informe a data: ");
            char[] dataValidade = data.ToCharArray();
            if (ValidaTabulacao(dataValidade) || ValidaDias(dataValidade) || ValidaMeses(dataValidade) || ValidaAnos(dataValidade))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n Data inválida! Tente novamente");
                Console.ResetColor();

                data = Convert.ToString(RecebeData(texto));
            }
            return Convert.ToDateTime(data);
        }

        #region Validações de Cadastro
        public void PacienteNaoCadastrado(ref bool sair, ref bool repetir)
        {
            Notificação(ConsoleColor.Red, "\n     Este paciente ainda não foi cadastrado!\n");
            string opcao = RecebeString("\n 1. Cadastrar paciente\n R. Retornar\n S. Sair\n\n Digite: ");
            switch (opcao)
            {
                case "1": telaPaciente.CadastroPaciente(ref sair, ref repetir); break;
                case "R": repetir = true; break;
                case "S": sair = true; break;
                default: OpcaoInvalida(ref opcao, ref sair, ref repetir); break;
            }
        }
        public void MedicamentoNaoCadastrado(ref bool sair, ref bool repetir)
        {
            Notificação(ConsoleColor.Red, "\n     Este medicamento ainda não foi cadastrado!\n");
            string opcao = RecebeString("\n 1. Cadastrar medicamento\n R. Retornar\n S. Sair\n\n Digite: ");
            switch (opcao)
            {
                case "1": telaMedicamento.CadastroMedicamento(ref sair, ref repetir); break;
                case "R": repetir = true; break;
                case "S": sair = true; break;
                default: OpcaoInvalida(ref opcao, ref sair, ref repetir); break;
            }
        }
        //Data
        public bool ValidaTabulacao(char[] dataValidade) => dataValidade.Length != 10 || dataValidade[2] != '/' || dataValidade[5] != '/';
        public bool ValidaDias(char[] dataValidade) => (dataValidade[0] != '0' && dataValidade[0] != '1' && dataValidade[0] != '2' && dataValidade[0] != '3') || (dataValidade[0] == '3' && dataValidade[1] != '0');
        public bool ValidaMeses(char[] dataValidade) => (dataValidade[3] != '0' && dataValidade[3] != '1') || (dataValidade[3] == '1' && dataValidade[4] != '0' && dataValidade[4] != '1' && dataValidade[4] != '2');
        public bool ValidaAnos(char[] dataValidade) => (dataValidade[6] != '2' || dataValidade[7] != '0' || (dataValidade[8] != '0' && dataValidade[8] != '1' && dataValidade[8] != '2') || (dataValidade[8] == '2' && dataValidade[9] != '0' && dataValidade[9] != '1' && dataValidade[9] != '2' && dataValidade[9] != '3' && dataValidade[9] != '4'));
        //Número
        public void NaoEhNumero(ref string quantidade, string texto)
        {
            Notificação(ConsoleColor.Red, "\n Não é um número! Tente novamente\n");
            quantidade = Convert.ToString(RecebeInt(texto));//Para garantir que, ao sair do loop, o método "RecebeInt" não vai puxar a "quantidade" original (nula)
        }
        #endregion

        #endregion

        #region Visualizar
        public void AindaNaoExistemRequisicoes(ref bool sair, ref bool repetir)
        {
            Console.WriteLine("------------------------------------------------\n");
            Notificação(ConsoleColor.Red, "      Não existem requisições cadastradas\n");
            ParaRetornarAoMenu(ref sair, ref repetir);
        }
        public void CabecalhoVisualizacao() => Notificação(ConsoleColor.Blue, "\n--------------------------------------------------------------------\n ID\t| Medicamento\t| Paciente\t| Posologia\t| Validade\n--------------------------------------------------------------------\n");
        public void ListarRequisicoes(RepositorioRequisicao repositorioRequisicao)
        {
            CabecalhoVisualizacao();
            for (int i = 0; i < repositorioRequisicao.requisicao.Length; i++) if (repositorioRequisicao.requisicao[i] != null)
                {
                    Console.Write($" {repositorioRequisicao.requisicao[i].id}\t| {repositorioRequisicao.requisicao[i].medicamento}\t\t| {repositorioRequisicao.requisicao[i].paciente}\t\t| {repositorioRequisicao.requisicao[i].posologia}\t\t| {repositorioRequisicao.requisicao[i].dataValidade.ToString("d")}" +
                                  $"\n--------------------------------------------------------------------\n");
                }
        }
        #endregion

        #region Edição
        public void IdValidoParaEdicao(RepositorioRequisicao repositorioRequisicao, int indexEditar, ref bool sair, ref bool repetir)
        {
            var objetoAuxiliar = repositorioRequisicao.requisicao[indexEditar];
            MenuParaEdicao(ref sair, ref repetir, objetoAuxiliar, telaMedicamento, telaPaciente);

            if (!sair && !repetir)
            {
                repositorioRequisicao.Editar(objetoAuxiliar.medicamento, objetoAuxiliar.paciente, objetoAuxiliar.posologia, objetoAuxiliar.dataValidade, indexEditar);
                RealizadoComSucesso("editada");
            }
        }
        public void IdNaoValido(ref bool sair, ref bool repetir)
        {
            Notificação(ConsoleColor.Red, "\n\n\t   Requisição não existente!\n");
            ParaRetornarAoMenu(ref sair, ref repetir);
        }
        public void MenuParaEdicao(ref bool sair, ref bool repetir, Requisicao objetoAuxiliar, TelaCadastroMedicamento telaMedicamento, TelaCadastroPaciente telaPaciente)
        {
            CabecalhoRequisicao();
            VisualizarParaEdicao(objetoAuxiliar);

            string opcao = RecebeString(" Ótimo! O que deseja Editar?\n 1. medicamento\n 2. paciente\n 3. endereço\n 4. cartão SUS\n\n R. para retornar\n S. para sair\n\n Digite: ");
            switch (opcao)
            {
                case "1": 
                    objetoAuxiliar.medicamento = RecebeString(" Informe o novo medicamento: ");
                    int jaExiste = telaMedicamento.repositorioMedicamento.EsteMedicamentoExiste(objetoAuxiliar.medicamento);
                    if (jaExiste == -1) MedicamentoNaoCadastrado(ref sair, ref repetir);
                    break;
                case "2": 
                    objetoAuxiliar.paciente = RecebeString(" Informe o novo paciente: ");
                    jaExiste = telaPaciente.repositorioPaciente.EstePacienteExiste(objetoAuxiliar.paciente);
                    if (jaExiste == -1) PacienteNaoCadastrado(ref sair, ref repetir); 
                    break;
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
            CabecalhoVisualizacao();

            Console.Write($" {objetoAuxiliar.id}\t| {objetoAuxiliar.medicamento}\t\t| {objetoAuxiliar.paciente}\t\t| {objetoAuxiliar.posologia}\t\t| {objetoAuxiliar.dataValidade.ToString("d")}" +
                          $"\n--------------------------------------------------------------------\n\n");
        }
        #endregion

        #region Exclusão
        public void IdValidoParaExclusao(RepositorioRequisicao repositorioRequisicao, int indexExcluir)
        {
            repositorioRequisicao.Excluir(indexExcluir);
            RealizadoComSucesso("excluido");
        }
        #endregion

        #region DarBaixa
        public void IdValidoParaMovimentacao(TelaCadastroMedicamento telaMedicamento, int indexDarBaixa, RepositorioRequisicao repositorioRequisicao)
        {
            telaMedicamento.repositorioMedicamento.DarBaixa(indexDarBaixa, repositorioRequisicao.requisicao[indexDarBaixa].medicamento, repositorioRequisicao.requisicao[indexDarBaixa].posologia);
            if (telaMedicamento.repositorioMedicamento.QuantidadeEstaCritica(indexDarBaixa)) Notificação(ConsoleColor.Red, "\n         ATENÇÃO! Nível de estoque baixo!\n");
            repositorioRequisicao.DarBaixa(indexDarBaixa);
            repositorioRequisicao.Excluir(indexDarBaixa);
            RealizadoComSucesso("movimentada");
        }
        #endregion
        #endregion
    }
}
