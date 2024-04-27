namespace ControleMedicamentos.ConsoleApp.Compartilhado
{
    abstract class TelaCadastro
    {
        protected abstract void CadastrarItem(ref bool sair, ref bool repetir);

        public void ItemNaoCadastrado(ref bool sair, ref bool repetir, string texto, TelaCadastro telaCadastro)
        {
            Notificação(ConsoleColor.Red, $"\n     Este {texto} ainda não foi cadastrado!\n");
            string opcao = RecebeString($"\n 1. Cadastrar {texto}\n R. Retornar\n S. Sair\n\n Digite: ");
            switch (opcao)
            {
                case "1": telaCadastro.CadastrarItem(ref sair, ref repetir); break;
                case "R": repetir = true; break;
                case "S": sair = true; break;
                default: OpcaoInvalida(ref repetir); break;
            }
        }

        public string RecebeString(string texto)
        {
            Console.Write(texto);
            return Console.ReadLine().ToUpper();
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
            string opcao = RecebeString("\n     'Enter' para voltar ao Menu Principal\n    'R' para voltar ao Menu de Medicamentos\n\t       'S' para Sair\n\n\t\t   Digite: ");
            if (opcao == "");
            else if (opcao == "R") repetir = true;
            else if (opcao == "S") sair = true;
            else OpcaoInvalida(ref repetir);
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

        public void CabecalhoEntidade(string texto)
        {
            Console.Clear();
            Console.WriteLine("------------------------------------------------\n  Controle de Medicamentos dos Postos de Saúde\n------------------------------------------------");
            Console.WriteLine($"\t     GESTÃO DE {texto}");
        }
        public void AindaNaoExistemItens(ref bool sair, ref bool repetir, string texto)
        {
            Console.WriteLine("------------------------------------------------\n");
            Notificação(ConsoleColor.Red, $"      Não existem {texto}\n");
            ParaRetornarAoMenu(ref sair, ref repetir);
        }
    }
}
