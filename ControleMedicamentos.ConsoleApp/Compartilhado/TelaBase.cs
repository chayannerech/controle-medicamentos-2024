using ControleMedicamentos.ConsoleApp.ModuloMedicamento;
using ControleMedicamentos.ConsoleApp.ModuloRequisicao;
using Controlepacientes.ConsoleApp.ModuloPaciente;
namespace ControleMedicamentos.ConsoleApp.Compartilhado
{
    abstract class TelaBase
    {
        public TelaCadastroPaciente telaPaciente;
        public TelaCadastroMedicamento telaMedicamento;
        public RepositorioBase repositorio;
        public string tipoEntidade, tipoEntidadePlural;

        public void MenuEntidade(ref bool sair)
        {
            bool repetir = false;
            do
            {
                CabecalhoEntidade(tipoEntidadePlural.ToUpper());
                Console.WriteLine("------------------------------------------------");

                repetir = false;
                string opcao = RecebeString($"\n\t    1. Cadastrar {tipoEntidade}\n\t   2. Visualizar {tipoEntidadePlural.ToLower()}\n\t      3. Editar {tipoEntidade}\n\t     4. Excluir {tipoEntidade}\n\n\t R. Retornar ao menu principal\n\t\t   S. Sair\n------------------------------------------------\n\n Digite: ");
                switch (opcao)
                {
                    case "1": CadastrarItem(ref sair, ref repetir); break;
                    case "2": VisualizarItens(ref sair, ref repetir); break;
                    case "3": EditarItem(ref sair, ref repetir, telaMedicamento, telaPaciente); break;
                    case "4": ExcluirItem(ref sair, ref repetir); break;
                    case "S": sair = true; break;
                    case "R": break;
                    default: OpcaoInvalida(ref repetir); break;
                }
            }
            while (repetir);
        }

        protected abstract void CadastrarItem(ref bool sair, ref bool repetir);
        public void VisualizarItens(ref bool sair, ref bool repetir)
        {
            CabecalhoEntidade(tipoEntidadePlural.ToUpper());
            Console.WriteLine("\t\t  Visualizar");

            if (repositorio.NaoExistemItens()) AindaNaoExistemItens(ref sair, ref repetir);
            else
            {
                CabecalhoVisualizar();
                for (int i = 0; i < repositorio.entidade.Length; i++) if (repositorio.entidade[i] != null) ListaItensParaVisualizar(i);
                ParaRetornarAoMenu(ref sair, ref repetir);
            }
        }
        public void EditarItem(ref bool sair, ref bool repetir, TelaBase telaMedicamento, TelaBase telaPaciente)
        {
            CabecalhoEntidade(tipoEntidadePlural.ToUpper());
            Console.WriteLine("\t\t    Editar");

            if (repositorio.NaoExistemItens()) AindaNaoExistemItens(ref sair, ref repetir);
            else
            {
                CabecalhoVisualizar();
                for (int i = 0; i < repositorio.entidade.Length; i++) if (repositorio.entidade[i] != null) ListaItensParaVisualizar(i);
                int indexEditar = repositorio.EsteItemExiste(RecebeString("\n Informe o ID que deseja editar: "));

                if (indexEditar == -1) ItemNaoCadastrado(ref sair, ref repetir);
                else MenuEdicao(ref sair, ref repetir, telaMedicamento, telaPaciente, indexEditar);
            }
        }
        public void ExcluirItem(ref bool sair, ref bool repetir)
        {
            CabecalhoEntidade(tipoEntidadePlural.ToUpper());
            Console.WriteLine("\t\t    Excluir");

            if (repositorio.NaoExistemItens()) AindaNaoExistemItens(ref sair, ref repetir);
            else
            {
                CabecalhoVisualizar();
                for (int i = 0; i < repositorio.entidade.Length; i++) if (repositorio.entidade[i] != null) ListaItensParaVisualizar(i);
                int indexExcluir = repositorio.EsteItemExiste(RecebeString("\n Informe o ID que deseja excluir: "));

                if (indexExcluir == -1) ItemNaoCadastrado(ref sair, ref repetir);
                else IdValidoParaExclusao(indexExcluir);
            }
        }

        #region Métodos Auxiliares
        #region Gerais
        public void CabecalhoEntidade(string texto)
        {
            Console.Clear();
            Console.WriteLine("------------------------------------------------\n  Controle de Medicamentos dos Postos de Saúde\n------------------------------------------------");
            Console.WriteLine($"\t     GESTÃO DE {texto}");
        }
        public string RecebeString(string texto)
        {
            Console.Write(texto);
            return Console.ReadLine().ToUpper();
        }
        public int RecebeInt(string texto)
        {
            Console.Write(texto);
            string quantidade = "";

            if (InputVazio(out string valorRecebido)) NaoEhNumero(ref valorRecebido, texto);

            char[] valorEmChar = valorRecebido.ToCharArray();
            for (int i = 0; i < valorEmChar.Length; i++) if (Convert.ToInt32(valorEmChar[i]) >= 48 && Convert.ToInt32(valorEmChar[i]) <= 57) quantidade += valorEmChar[i];
            if (quantidade.Length != valorEmChar.Length) NaoEhNumero(ref quantidade, texto);

            return Convert.ToInt32(quantidade);
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
        public void OpcaoInvalida(ref bool repetir)
        {
            Notificação(ConsoleColor.Red, "\n        Opção inválida. Tente novamente ");
            Console.ReadKey(true);
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
            string opcao = RecebeString($"\n     'Enter' para voltar ao menu principal\n    'R' para voltar ao menu de {tipoEntidade}\n\t       'S' para Sair\n\n\t\t   Digite: ");
            if (opcao == "") ;
            else if (opcao == "R") repetir = true;
            else if (opcao == "S") sair = true;
            else OpcaoInvalida(ref repetir);
        }
        #endregion
        #region Validação itens
        public void ItemNaoCadastrado(ref bool sair, ref bool repetir)
        {
            Notificação(ConsoleColor.Red, $"\n       Este item ainda não foi cadastrado!\n");
            string opcao = RecebeString($"\n 1. Cadastrar item\n R. Retornar\n S. Sair\n\n Digite: ");
            switch (opcao)
            {
                case "1": CadastrarItem(ref sair, ref repetir); break;
                case "R": repetir = true; break;
                case "S": sair = true; break;
                default: OpcaoInvalida(ref repetir); break;
            }
        }
        public void AindaNaoExistemItens(ref bool sair, ref bool repetir)
        {
            Console.WriteLine("------------------------------------------------\n");
            Notificação(ConsoleColor.Red, $"  Ainda não existem {tipoEntidadePlural} cadastrados(as)\n");
            ParaRetornarAoMenu(ref sair, ref repetir);
        }
        public void ItemJaExiste(ref bool sair, ref bool repetir)
        {
            Notificação(ConsoleColor.Red, $" Este {tipoEntidade} já existe!\n");
            ParaRetornarAoMenu(ref sair, ref repetir);
        }
        #endregion
        #region Validação int
        public bool InputVazio(out string valorRecebido)
        {
            valorRecebido = Console.ReadLine();
            return valorRecebido == "";
        }
        public void NaoEhNumero(ref string quantidade, string texto)
        {
            Notificação(ConsoleColor.Red, "\n Não é um número! Tente novamente\n");
            quantidade = Convert.ToString(RecebeInt(texto));//Para garantir que, ao sair do loop, o método "RecebeInt" não vai puxar a "quantidade" original (nula)
        }
        #endregion
        #region Validação data
        public bool ValidaTabulacao(char[] dataValidade) => dataValidade.Length != 10 || dataValidade[2] != '/' || dataValidade[5] != '/';
        public bool ValidaDias(char[] dataValidade) => (dataValidade[0] != '0' && dataValidade[0] != '1' && dataValidade[0] != '2' && dataValidade[0] != '3') || (dataValidade[0] == '3' && dataValidade[1] != '0');
        public bool ValidaMeses(char[] dataValidade) => (dataValidade[3] != '0' && dataValidade[3] != '1') || (dataValidade[3] == '1' && dataValidade[4] != '0' && dataValidade[4] != '1' && dataValidade[4] != '2');
        public bool ValidaAnos(char[] dataValidade) => (dataValidade[6] != '2' || dataValidade[7] != '0' || (dataValidade[8] != '0' && dataValidade[8] != '1' && dataValidade[8] != '2') || (dataValidade[8] == '2' && dataValidade[9] != '0' && dataValidade[9] != '1' && dataValidade[9] != '2' && dataValidade[9] != '3' && dataValidade[9] != '4'));
        #endregion
        #region Visualização
        protected abstract void CabecalhoVisualizar();
        protected abstract void ListaItensParaVisualizar(int i);
        #endregion
        #region Edição
        protected abstract void MenuEdicao(ref bool sair, ref bool repetir, TelaBase telaMedicamento, TelaBase telaPaciente, int indexEditar);
        protected abstract void VisualizarParaEdicao(EntidadeBase objetoAuxiliar);
        #endregion
        #region Exclusão
        public void IdValidoParaExclusao(int indexExcluir)
        {
            repositorio.Excluir(indexExcluir);
            RealizadoComSucesso("excluido");
        }
        #endregion
        #endregion
    }
}
