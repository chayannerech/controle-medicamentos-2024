using ControleMedicamentos.ConsoleApp.Compartilhado;
using ControleMedicamentos.ConsoleApp.ModuloPaciente;

namespace Controlepacientes.ConsoleApp.ModuloPaciente
{
    internal class TelaCadastroPaciente
    {
        public RepositorioPaciente repositorioPaciente = new RepositorioPaciente();

        public void MenuPaciente(ref bool sair)
        {
            bool repetir = false;
            do
            {
                CabecalhoPaciente();
                Console.WriteLine("------------------------------------------------");

                repetir = false;
                string opcao = RecebeString("\n        1. Cadastrar um novo paciente\n\t    2. Pesquisar paciente\n\t   3. Visualizar pacientes\n\t      4. Editar paciente\n\t     5. Excluir paciente\n\n\t R. Retornar ao menu principal\n\t\t   S. Sair\n------------------------------------------------\n\n Digite: ");
                switch (opcao)
                {
                    case "1": CadastroPaciente(ref sair, ref repetir); break;
                    case "2": PesquisaPaciente(ref sair, ref repetir); break;
                    case "3": VisualizarPaciente(ref sair, ref repetir); break;
                    case "4": EditarPaciente(ref sair, ref repetir); break;
                    case "5": ExcluirPaciente(ref sair, ref repetir); break;
                    case "S": sair = true; break;
                    case "R": break;
                    default: OpcaoInvalida(ref opcao, ref sair, ref repetir); break;
                }
            }
            while (repetir);
        }
        public void CadastroPaciente(ref bool sair, ref bool repetir)
        {
            CabecalhoPaciente();
            Console.WriteLine("\t\t  Cadastrar\n------------------------------------------------");

            string nome = RecebeString("\n Informe o nome: ");
            int index = repositorioPaciente.EsteItemExiste(nome);
            if (index != -1) PacienteJaExiste(ref sair, ref repetir);
            else
            {
                string cpf = RecebeString(" Informe o cpf: ");
                string endereco = RecebeString(" Informe o endereço: ");
                string cartaoSUS = RecebeString(" Informe o cartão SUS: ");
                var novoPaciente = new Paciente(nome, cpf, endereco, cartaoSUS);

                repositorioPaciente.Cadastrar(novoPaciente);
                RealizadoComSucesso("cadastrado");
            }
        }
        public void PesquisaPaciente(ref bool sair, ref bool repetir)
        {
            CabecalhoPaciente();
            int index = repositorioPaciente.EsteItemExiste(RecebeString("\t\t    Pesquisar\n------------------------------------------------\n\n Informe o nome do paciente: "));

            if (index == -1) PacienteNaoCadastrado(ref sair, ref repetir);
            else
            {
                Console.WriteLine($"\n paciente = {repositorioPaciente.entidade[index].nome}\n CPF = {repositorioPaciente.entidade[index].cpf}\n Endereço = {repositorioPaciente.entidade[index].endereco}\n Cartão SUS = {repositorioPaciente.entidade[index].cartaoSUS}\n");
                ParaRetornarAoMenu(ref sair, ref repetir);
            }
        }
        public void VisualizarPaciente(ref bool sair, ref bool repetir)
        {
            CabecalhoPaciente();
            Console.WriteLine("\t\t  Visualizar");

            if (repositorioPaciente.NaoExistemItens()) SemPacientes(ref sair, ref repetir);
            else
            {
                Visualizar();
                ParaRetornarAoMenu(ref sair, ref repetir);
            }
        }
        public void EditarPaciente(ref bool sair, ref bool repetir)
        {
            CabecalhoPaciente();
            Console.WriteLine("\t\t    Editar");

            if (repositorioPaciente.NaoExistemItens()) SemPacientes(ref sair, ref repetir);
            else
            {
                Visualizar();
                int indexEditar = repositorioPaciente.EsteItemExiste(RecebeString("\n Informe o nome do paciente a editar: "));
                
                if (indexEditar == -1) PacienteNaoCadastrado(ref sair, ref repetir);
                else NomeValidoParaEdicao(indexEditar, ref sair, ref repetir);
            }
        }
        public void ExcluirPaciente(ref bool sair, ref bool repetir)
        {
            CabecalhoPaciente();
            Console.WriteLine("\t\t    Excluir");

            if (repositorioPaciente.NaoExistemItens()) SemPacientes(ref sair, ref repetir);
            else
            {
                Visualizar();
                int indexExcluir = repositorioPaciente.EsteItemExiste(RecebeString("\n Informe o nome do paciente a excluir: "));

                if (indexExcluir == -1) PacienteNaoCadastrado(ref sair, ref repetir);
                else NomeValidoParaExclusao(indexExcluir);
            }
        }

        #region Métodos Auxiliares
        #region Gerais
        public void CabecalhoPaciente()
        {
            Console.Clear();
            Console.WriteLine("------------------------------------------------\n  Controle de Medicamentos dos Postos de Saúde\n------------------------------------------------");
            Console.WriteLine("\t      GESTÃO DE PACIENTES");
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
        public void PacienteJaExiste(ref bool sair, ref bool repetir)
        {
            Notificação(ConsoleColor.Red, " Este paciente já existe!\n");
            ParaRetornarAoMenu(ref sair, ref repetir);
        }
        #endregion

        #region Pesquisa
        public void PacienteNaoCadastrado(ref bool sair, ref bool repetir)
        {
            Notificação(ConsoleColor.Red, "\n     Este paciente ainda não foi cadastrado!\n");
            string opcao = RecebeString("\n 1. Cadastrar paciente\n R. Retornar\n S. Sair\n\n Digite: ");
            switch (opcao)
            {
                case "1": CadastroPaciente(ref sair, ref repetir); break;
                case "R": repetir = true; break;
                case "S": sair = true; break;
                default: OpcaoInvalida(ref opcao, ref sair, ref repetir); break;
            }
        }
        #endregion

        #region Visualizar
        public void SemPacientes(ref bool sair, ref bool repetir)
        {
            Console.WriteLine("------------------------------------------------\n");
            Notificação(ConsoleColor.Red, "      Não existem pacientes cadastrados\n");
            ParaRetornarAoMenu(ref sair, ref repetir);
        }
        public void CabecalhoVisualizar() => Notificação(ConsoleColor.Blue, "\n------------------------------------------------------\n Nome\t| CPF\t\t| Endereço\t| Cartão SUS\n------------------------------------------------------\n");
        public void Visualizar()
        {
            CabecalhoVisualizar();

            for (int i = 0; i < repositorioPaciente.entidade.Length; i++) if (repositorioPaciente.entidade[i] != null)
                {
                    Console.Write($" {repositorioPaciente.entidade[i].nome}\t| {repositorioPaciente.entidade[i].cpf}\t\t| {repositorioPaciente.entidade[i].endereco}\t\t| {repositorioPaciente.entidade[i].cartaoSUS}" +
                                  $"\n------------------------------------------------------\n");
                }
        }
        #endregion

        #region Edição
        public void NomeValidoParaEdicao(int indexEditar, ref bool sair, ref bool repetir)
        {
            var objetoAuxiliar = repositorioPaciente.entidade[indexEditar];

            MenuParaEdicao(ref sair, ref repetir, objetoAuxiliar, out bool editado);

            if (!sair && !repetir)
            {
                var editarPaciente = new Paciente(objetoAuxiliar.nome, objetoAuxiliar.cpf, objetoAuxiliar.endereco, objetoAuxiliar.cartaoSUS);
                repositorioPaciente.Editar(editarPaciente, indexEditar);
                if (editado) RealizadoComSucesso("editado");
            }
        }
        public void MenuParaEdicao(ref bool sair, ref bool repetir, Entidades objetoAuxiliar, out bool editado)
        {
            CabecalhoPaciente();
            VisualizarParaEdicao(objetoAuxiliar);
            editado = true;
            string opcao = RecebeString(" Ótimo! O que deseja Editar?\n 1. nome\n 2. CPF\n 3. endereço\n 4. cartão SUS\n\n R. para retornar\n S. para sair\n\n Digite: ");
            switch (opcao)
            {
                case "1":
                    string nome = RecebeString("\n Informe o novo nome: ");
                    if (repositorioPaciente.EsteItemExiste(nome) == -1) objetoAuxiliar.nome = nome;
                    else
                    {
                        PacienteJaExiste(ref sair, ref repetir);
                        editado = false;
                    }
                    break;
                case "2": objetoAuxiliar.cpf = RecebeString("\n Informe o novo CPF: "); break;
                case "3": objetoAuxiliar.endereco = RecebeString("\n Informe o novo endereço: "); break;
                case "4": objetoAuxiliar.cartaoSUS = RecebeString("\n Informe o novo cartão SUS: "); break;
                case "R": repetir = true; break;
                case "S": sair = true; break;
                default: OpcaoInvalida(ref opcao, ref sair, ref repetir); break;
            }
            if (opcao == "") repetir = true;
        }
        public void VisualizarParaEdicao(Entidades objetoAuxiliar)
        {
            Console.WriteLine("\t\t    Editar");
            CabecalhoVisualizar();
            Console.Write($" {objetoAuxiliar.nome}\t| {objetoAuxiliar.cpf}\t| {objetoAuxiliar.endereco}\t\t| {objetoAuxiliar.cartaoSUS}" +
                          $"\n------------------------------------------------\n\n");
        }
        #endregion

        #region Exclusão
        public void NomeValidoParaExclusao(int indexExcluir)
        {
            repositorioPaciente.Excluir(indexExcluir);
            RealizadoComSucesso("excluido");
        }
        #endregion
        #endregion
    }
}
