namespace web_api_managemen_user.Class
{
    public static class MessageValue
    {
        public static string MessageValueDelete(string message_error)
        {
            string response = "";

            if(message_error.Contains("An error occurred while updating the entries"))
            {
                response = "Gagal Menghapus ! (Data terhubung dengan table lain)";
            }
            else
            {
                response = message_error;
            }
            return response;
        }

        public static string MessageValueAdd(string message_error)
        {
            string response = "";

            if (message_error.Contains("An error occurred while updating the entries"))
            {
                response = "Gagal Menyimpan ! (Ada data tidak sesuai dengan table simpan)";
            }
            else
            {
                response = message_error;
            }

            return response;
        }

        public static string MessageUpdate(string message_error)
        {
            string response = "";

            if (message_error.Contains("cannot be tracked because another instance with the same key value for"))
            {
                response = "Gagal Menggubah ! ()";
            }
            else
            {
                response = message_error;
            }

            return response;
            
        }
    }
}
