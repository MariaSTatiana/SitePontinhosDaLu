using MySql.Data.MySqlClient;

namespace PontinhosDaLu.Models
{
    public class Conexao
    {
        private static string servidor = "localhost";
        private static string bancoDados = "loja_artesa";
        private static string usuario = "root";
        private static string senha = "Kakaroto12@";

        private string stringConexao =
$"server={servidor};database={bancoDados};user={usuario};password={senha}";

        public MySqlConnection ObterConexao()
        {

            return new MySqlConnection(stringConexao);
        }
    }
}