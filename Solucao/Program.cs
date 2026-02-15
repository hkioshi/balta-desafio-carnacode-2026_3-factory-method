// See https://aka.ms/new-console-template for more information
Console.WriteLine("=== Sistema de Notificações ===\n");


// Cliente 1 prefere Email
var emailManager = new EmailCreator();
emailManager.GerarNotificacaoConfirmacao("cliente@email.com", "12345");
Console.WriteLine();

var smsManager = new SmsCreator();
// Cliente 2 prefere SMS
smsManager.GerarNotificacaoConfirmacao("+5511999999999", "12346");
Console.WriteLine();

// Cliente 3 prefere Push
var pushManager = new PushCreator();
pushManager.GerarNotificacaoConfirmacao("device-token-abc123", "BR123456789");
Console.WriteLine();

// Cliente 4 prefere WhatsApp
var whatsappManager = new PushWhatsappCreator();
pushManager.GerarNotificacaoPagamento("+5511888888888", 150.00m);

//Product 
public interface INotification
{
    public void SendOrderConfirmation(string recipient, string orderNumber);
    public void SendShippingUpdate(string recipient, string trackingCode);
    public void SendPaymentReminder(string recipient, decimal amount);
    public void Send();
}
public class EmailNotification : INotification
{
    public string Recipient { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
    public bool IsHtml { get; set; }

    public void Send()
    {
        Console.WriteLine($"📧 Enviando Email para {Recipient}");
        Console.WriteLine($"   Assunto: {Subject}");
        Console.WriteLine($"   Mensagem: {Body}");
    }

    public void SendOrderConfirmation(string recipient, string orderNumber)
    {
        Recipient = recipient;
        Subject = "Confirmação de Pedido";
        Body = $"Seu pedido {orderNumber} foi confirmado!";
        IsHtml = true;
        Send();
    }

    public void SendPaymentReminder(string recipient, decimal amount)
    {
        Recipient = recipient;
        Subject = "Lembrete de Pagamento";
        Body = $"Você tem um pagamento pendente de R$ {amount:N2}";
        IsHtml = true;
        Send();
    }

    public void SendShippingUpdate(string recipient, string trackingCode)
    {
        Recipient = recipient;
        Subject = "Pedido Enviado";
        Body = $"Seu pedido foi enviado! Código de rastreamento: {trackingCode}";
        IsHtml = true;
        Send();
    }
}
public class SmsNotification : INotification
{
    public string PhoneNumber { get; set; }
    public string Message { get; set; }

    public void Send()
    {
        Console.WriteLine($"📱 Enviando SMS para {PhoneNumber}");
        Console.WriteLine($"   Mensagem: {Message}");
    }

    public void SendOrderConfirmation(string recipient, string orderNumber)
    {
        PhoneNumber = recipient;
        Message = $"Pedido {orderNumber} confirmado!";
        Send();
    }

    public void SendPaymentReminder(string recipient, decimal amount)
    {
        PhoneNumber = recipient;
        Message = $"Pagamento pendente: R$ {amount:N2}";
        Send();
    }

    public void SendShippingUpdate(string recipient, string trackingCode)
    {
        PhoneNumber = recipient;
        Message = $"Pedido enviado! Rastreamento: {trackingCode}";
        Send();
    }
}
public class PushNotification : INotification
{
    public string DeviceToken { get; set; }
    public string Title { get; set; }
    public string Message { get; set; }
    public int Badge { get; set; }

    public void Send()
    {
        Console.WriteLine($"🔔 Enviando Push para dispositivo {DeviceToken}");
        Console.WriteLine($"   Título: {Title}");
        Console.WriteLine($"   Mensagem: {Message}");
    }

    public void SendOrderConfirmation(string recipient, string orderNumber)
    {
        DeviceToken = recipient;
        Title = "Pedido Confirmado";
        Message = $"Pedido {orderNumber} confirmado!";
        Badge = 1;
        Send();
    }

    public void SendPaymentReminder(string recipient, decimal amount)
    {
        DeviceToken = recipient;
        Title = "Pedido Enviado";
        Message = $"Pagamento pendente: R$ {amount:N2}";
        Badge = 1;
        Send();
    }

    public void SendShippingUpdate(string recipient, string trackingCode)
    {
        DeviceToken = recipient;
        Title = "Pedido Enviado";
        Message = $"Rastreamento: {trackingCode}";
        Badge = 1;
        Send();
    }
}
public class WhatsAppNotification : INotification
{
    public string PhoneNumber { get; set; }
    public string Message { get; set; }
    public bool UseTemplate { get; set; }

    public void Send()
    {
        Console.WriteLine($"💬 Enviando WhatsApp para {PhoneNumber}");
        Console.WriteLine($"   Mensagem: {Message}");
        Console.WriteLine($"   Template: {UseTemplate}");
    }

    public void SendOrderConfirmation(string recipient, string orderNumber)
    {
        PhoneNumber = recipient;
        Message = $"✅ Seu pedido {orderNumber} foi confirmado!";
        UseTemplate = true;
        Send();
    }

    public void SendPaymentReminder(string recipient, decimal amount)
    {
        PhoneNumber = recipient;
        Message = $"Pagamento pendente: R$ {amount:N2}";
        UseTemplate = true;
        Send();
    }

    public void SendShippingUpdate(string recipient, string trackingCode)
    {
        PhoneNumber = recipient;
        Message = $"📦 Pedido enviado! Rastreamento: {trackingCode}";
        UseTemplate = true;
        Send();
    }
}

//Creator
public abstract class PagamentoCreator
{
    public abstract INotification CreateNotification();
    public void GerarNotificacaoConfirmacao(string recipient, string orderNumber)
    {
        var document = CreateNotification();
        document.SendOrderConfirmation(recipient, orderNumber);
    }

    public void GerarNotificacaoPagamento(string recipient, decimal amount)
    {
        var document = CreateNotification();
        document.SendPaymentReminder(recipient, amount);
    }

    public void GerarNotificacaoShipping(string recipient, string trackingCode)
    {
        var document = CreateNotification();
        document.SendShippingUpdate(recipient, trackingCode);
    }


}

//ConcreteCreator
public class EmailCreator : PagamentoCreator
{
    public override INotification CreateNotification()
    => new EmailNotification();
}
public class SmsCreator : PagamentoCreator
{
    public override INotification CreateNotification()
    => new SmsNotification();

}
public class PushCreator : PagamentoCreator
{
    public override INotification CreateNotification()
    => new PushNotification();

}
public class PushWhatsappCreator : PagamentoCreator
{
    public override INotification CreateNotification()
    => new WhatsAppNotification();

}