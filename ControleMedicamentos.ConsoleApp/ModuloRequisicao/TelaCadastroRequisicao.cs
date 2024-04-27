using ControleMedicamentos.ConsoleApp.Compartilhado;
using ControleMedicamentos.ConsoleApp.ModuloMedicamento;
using ControleMedicamentos.ConsoleApp.ModuloPaciente;
using Controlepacientes.ConsoleApp.ModuloPaciente;
namespace ControleMedicamentos.ConsoleApp.ModuloRequisicao
{
    internal class TelaCadastroRequisicao : TelaCadastro
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
                CabecalhoEntidade("REQUISIÇÕES");
                Console.WriteLine("------------------------------------------------");

                repetir = false;
                string opcao = RecebeString("\n        1. Cadastrar uma nova requisição\n\t   2. Visualizar requisições\n\t      3. Editar requisição\n\t     4. Excluir requisição\n         5. Dar baixa na requisição\n\n\t R. Retornar ao menu principal\n\t\t   S. Sair\n------------------------------------------------\n\n Digite: ");
                switch (opcao)
                {
                    case "1": CadastrarItem(ref sair, ref repetir); break;
                    case "2": VisualizarRequisicao(ref sair, ref repetir); break;
                    case "3": EditarRequisicao(ref sair, ref repetir); break;
                    case "4": ExcluirRequisicao(ref sair, ref repetir); break;
                    case "5": DarBaixaRequisicao(ref sair, ref repetir); break;
                    case "S": sair = true; break;
                    case "R": break;
                    default: OpcaoInvalida(ref repetir); break;
                }
            }
            while (repetir);
        }
        protected override void CadastrarItem(ref bool sair, ref bool repetir)
        {
            CabecalhoEntidade("REQUISIÇÕES");
            Console.WriteLine("\t\t  Cadastrar\n------------------------------------------------");

            string paciente = RecebeString(" Informe o nome do paciente: ");
            int jaExiste = telaPaciente.repositorioPaciente.EsteItemExiste(paciente);
            if (jaExiste == -1) ItemNaoCadastrado(ref sair, ref repetir, "paciente", telaPaciente);
            else
            {
                string medicamento = RecebeString(" Informe o medicamento: ");
                jaExiste = telaMedicamento.repositorioMedicamento.EsteItemExiste(medicamento);
                if (jaExiste == -1) ItemNaoCadastrado(ref sair, ref repetir, "medicamento", telaMedicamento);
                else
                {
                    int posologia = RecebeInt(" Informe a posologia: ");
                    DateTime dataValidade = RecebeData(" Informe a data de validade: ");
                    var novaRequisicao = new Requisicao(medicamento, paciente, posologia, dataValidade, repositorioRequisicao.contador + 1);

                    repositorioRequisicao.Cadastrar(novaRequisicao);
                    RealizadoComSucesso("cadastrada");
                }
            }
        }
        public void VisualizarRequisicao(ref bool sair, ref bool repetir)
        {
            CabecalhoEntidade("REQUISIÇÕES");
            Console.WriteLine("\t\t  Visualizar");

            if (repositorioRequisicao.NaoExistemItens()) AindaNaoExistemItens(ref sair, ref repetir, "requisições cadastradas");
            else
            {
                Visualizar();
                ParaRetornarAoMenu(ref sair, ref repetir);
            }
        }
        public void EditarRequisicao(ref bool sair, ref bool repetir)
        {
            CabecalhoEntidade("REQUISIÇÕES");
            Console.WriteLine("\t\t    Editar");

            if (repositorioRequisicao.NaoExistemItens()) AindaNaoExistemItens(ref sair, ref repetir, "requisições cadastradas");
            else
            {
                Visualizar();
                int indexEditar = repositorioRequisicao.EsteItemExiste(RecebeString("\n Informe o ID que deseja editar: "));

                if (indexEditar == -1) IdNaoValido(ref sair, ref repetir);
                else IdValidoParaEdicao(indexEditar, ref sair, ref repetir);
            }
        }
        public void ExcluirRequisicao(ref bool sair, ref bool repetir)
        {
            CabecalhoEntidade("REQUISIÇÕES");
            Console.WriteLine("\t\t    Excluir");

            if (repositorioRequisicao.NaoExistemItens()) AindaNaoExistemItens(ref sair, ref repetir, "requisições cadastradas");
            else
            {
                Visualizar();
                int indexExcluir = repositorioRequisicao.EsteItemExiste(RecebeString("\n Informe o ID que deseja excluir: "));

                if (indexExcluir == -1) IdNaoValido(ref sair, ref repetir);
                else IdValidoParaExclusao(indexExcluir);
            }
        }
        public void DarBaixaRequisicao(ref bool sair, ref bool repetir)
        {
            CabecalhoEntidade("REQUISIÇÕES");
            Console.WriteLine("\t\t   Dar Baixa");

            if (repositorioRequisicao.NaoExistemItens()) AindaNaoExistemItens(ref sair, ref repetir, "requisições cadastradas");
            else
            {
                Visualizar();
                int indexDarBaixa = repositorioRequisicao.EsteItemExiste(RecebeString("\n Informe o ID que deseja dar baixa: "));

                if (indexDarBaixa == -1) IdNaoValido(ref sair, ref repetir);
                else IdValidoParaMovimentacao(indexDarBaixa);
            }
        }

        #region Métodos Auxiliares

        #region Cadastro
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
        //Data
        public bool ValidaTabulacao(char[] dataValidade) => dataValidade.Length != 10 || dataValidade[2] != '/' || dataValidade[5] != '/';
        public bool ValidaDias(char[] dataValidade) => (dataValidade[0] != '0' && dataValidade[0] != '1' && dataValidade[0] != '2' && dataValidade[0] != '3') || (dataValidade[0] == '3' && dataValidade[1] != '0');
        public bool ValidaMeses(char[] dataValidade) => (dataValidade[3] != '0' && dataValidade[3] != '1') || (dataValidade[3] == '1' && dataValidade[4] != '0' && dataValidade[4] != '1' && dataValidade[4] != '2');
        public bool ValidaAnos(char[] dataValidade) => (dataValidade[6] != '2' || dataValidade[7] != '0' || (dataValidade[8] != '0' && dataValidade[8] != '1' && dataValidade[8] != '2') || (dataValidade[8] == '2' && dataValidade[9] != '0' && dataValidade[9] != '1' && dataValidade[9] != '2' && dataValidade[9] != '3' && dataValidade[9] != '4'));
        #endregion

        #endregion

        #region Visualizar
        public void CabecalhoVisualizacao() => Notificação(ConsoleColor.Blue, "\n--------------------------------------------------------------------\n ID\t| Medicamento\t| Paciente\t| Posologia\t| Validade\n--------------------------------------------------------------------\n");
        public void Visualizar()
        {
            CabecalhoVisualizacao();
            for (int i = 0; i < repositorioRequisicao.entidade.Length; i++) if (repositorioRequisicao.entidade[i] != null)
                {
                    Console.Write($" {repositorioRequisicao.entidade[i].id}\t| {repositorioRequisicao.entidade[i].medicamento}\t\t| {repositorioRequisicao.entidade[i].paciente}\t\t| {repositorioRequisicao.entidade[i].posologia}\t\t| {repositorioRequisicao.entidade[i].dataValidade.ToString("d")}" +
                                  $"\n--------------------------------------------------------------------\n");
                }
        }
        #endregion

        #region Edição
        public void IdValidoParaEdicao(int indexEditar, ref bool sair, ref bool repetir)
        {
            var objetoAuxiliar = repositorioRequisicao.entidade[indexEditar];
            MenuParaEdicao(ref sair, ref repetir, objetoAuxiliar, telaMedicamento, telaPaciente);

            if (!sair && !repetir)
            {
                var editarRequisicao = new Requisicao(objetoAuxiliar.medicamento, objetoAuxiliar.paciente, objetoAuxiliar.posologia, objetoAuxiliar.dataValidade, 2);
                repositorioRequisicao.Editar(editarRequisicao, indexEditar);
                RealizadoComSucesso("editada");
            }
        }
        public void IdNaoValido(ref bool sair, ref bool repetir)
        {
            Notificação(ConsoleColor.Red, "\n\n\t   Requisição não existente!\n");
            ParaRetornarAoMenu(ref sair, ref repetir);
        }
        public void MenuParaEdicao(ref bool sair, ref bool repetir, Entidades objetoAuxiliar, TelaCadastroMedicamento telaMedicamento, TelaCadastroPaciente telaPaciente)
        {
            CabecalhoEntidade("REQUISIÇÕES");
            VisualizarParaEdicao(objetoAuxiliar);

            string opcao = RecebeString(" Ótimo! O que deseja Editar?\n 1. medicamento\n 2. paciente\n 3. posologia\n 4. data de validade\n\n R. para retornar\n S. para sair\n\n Digite: ");
            switch (opcao)
            {
                case "1": 
                    objetoAuxiliar.medicamento = RecebeString(" Informe o novo medicamento: ");
                    int jaExiste = telaMedicamento.repositorioMedicamento.EsteItemExiste(objetoAuxiliar.medicamento);
                    if (jaExiste == -1) ItemNaoCadastrado(ref sair, ref repetir, "medicamento", telaMedicamento);
                    break;
                case "2": 
                    objetoAuxiliar.paciente = RecebeString(" Informe o novo paciente: ");
                    jaExiste = telaPaciente.repositorioPaciente.EsteItemExiste(objetoAuxiliar.paciente);
                    if (jaExiste == -1) ItemNaoCadastrado(ref sair, ref repetir, "paciente", telaPaciente); 
                    break;
                case "3": objetoAuxiliar.posologia = RecebeInt("\n Informe o novo endereço: "); break;
                case "4": objetoAuxiliar.dataValidade = RecebeData("\n Informe o novo cartão SUS: "); break;
                case "R": repetir = true; break;
                case "S": sair = true; break;
                default: OpcaoInvalida(ref repetir); break;
            }
            if (opcao == "") repetir = true;
        }
        public void VisualizarParaEdicao(Entidades objetoAuxiliar)
        {
            Console.WriteLine("\t\t    Editar");
            CabecalhoVisualizacao();

            Console.Write($" {objetoAuxiliar.id}\t| {objetoAuxiliar.medicamento}\t\t| {objetoAuxiliar.paciente}\t\t| {objetoAuxiliar.posologia}\t\t| {objetoAuxiliar.dataValidade.ToString("d")}" +
                          $"\n--------------------------------------------------------------------\n\n");
        }
        #endregion

        #region Exclusão
        public void IdValidoParaExclusao(int indexExcluir)
        {
            repositorioRequisicao.Excluir(indexExcluir);
            RealizadoComSucesso("excluido");
        }
        #endregion

        #region DarBaixa
        public void IdValidoParaMovimentacao(int indexDarBaixa)
        {
            repositorioRequisicao.DarBaixa(telaMedicamento.repositorioMedicamento.entidade, indexDarBaixa);
            if (telaMedicamento.repositorioMedicamento.QuantidadeEstaCritica(indexDarBaixa)) Notificação(ConsoleColor.Red, "\n         ATENÇÃO! Nível de estoque baixo!\n");
            RealizadoComSucesso("movimentada");
        }
        #endregion
        #endregion
    }
}
