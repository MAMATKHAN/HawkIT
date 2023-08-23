namespace HawkIT.Models
{
    public class SenderInfo
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Telegram { get; set; }
        public string Phone { get; set; }
        public string Message { get; set; }

        public string GetInfoToHtml()
        {
            string info = $@"<h2>Отправитель</h2>
                <h4>Имя - {Name}</h4>
                <h4>Email - {Email}</h4>
                <h4>Номер - телефона {Phone}</h4>
                <h4>Телеграмм - {Telegram}</h4> </br>
                <h2>Сообщение</h2>
                <p>{Message}</p>";

            return info;
        }
    }
}
