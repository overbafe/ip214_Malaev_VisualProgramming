public class Account
{
    private decimal _balance;
    private List<INotifyer> _notifyers;

    public Account()
    {
        _balance = 0;
        _notifyers = new List<INotifyer>();
    }

    public Account(decimal initialBalance)
    {
        _balance = initialBalance;
        _notifyers = new List<INotifyer>();
    }

    public void AddNotifyer(INotifyer notifyer)
    {
        _notifyers.Add(notifyer);
    }

    public void ChangeBalance(decimal value)
    {
        _balance += value;
        Notification();
    }

    public decimal Balance
    {
        get 
        { 
            return _balance; 
        }
    }

    private void Notification()
    {
        foreach (INotifyer notifyer in _notifyers)
        {
            notifyer.Notify(_balance);
        }
    }
}

public interface INotifyer
{
    void Notify(decimal balance);
}

public class SMSLowBalanceNotifyer : INotifyer
{
    private string _phone;
    private decimal _lowBalanceValue;

    public SMSLowBalanceNotifyer(string phone, decimal lowBalanceValue)
    {
        _phone = phone;
        _lowBalanceValue = lowBalanceValue;
    }

    public void Notify(decimal balance)
    {
        if (balance < _lowBalanceValue)
        {
            Console.WriteLine("Отправка СМС по номеру {0} - Низкий баланс: {1}", _phone, balance);
        }
    }
}

public class EMailBalanceChangedNotifyer : INotifyer
{
    private string _email;

    public EMailBalanceChangedNotifyer(string email)
    {
        _email = email;
    }

    public void Notify(decimal balance)
    {
        Console.WriteLine("Отправка письма на электронную почту по адресу {0} - Баланс состовляет: {1}", _email, balance);
    }
}

class Program
{
    static void Main(string[] args)
    {
        Account account = new Account(1000m);
        Account account1 = new Account(300m);

        SMSLowBalanceNotifyer smsNotifyer = new SMSLowBalanceNotifyer("777", 100m);
        EMailBalanceChangedNotifyer emailNotifyer = new EMailBalanceChangedNotifyer("bankclient0@gmail.com");

        account.AddNotifyer(smsNotifyer);
        account.AddNotifyer(emailNotifyer);

        Console.WriteLine("Баланс на вашем аккаунте: {0}", account.Balance);
        account.ChangeBalance(-100m);
        account.ChangeBalance(-200m);
        account.ChangeBalance(-300m);
        account.ChangeBalance(-301m);

        SMSLowBalanceNotifyer smsNotifyer1 = new SMSLowBalanceNotifyer("888", 100m);
        EMailBalanceChangedNotifyer emailNotifyer1 = new EMailBalanceChangedNotifyer("bankclient1@gmail.com");

        account1.AddNotifyer(smsNotifyer1);
        account1.AddNotifyer(emailNotifyer1);

        Console.WriteLine("Баланс на вашем аккаунте: {0}", account1.Balance);
        account1.ChangeBalance(-50m);
        account1.ChangeBalance(-20m);
        account1.ChangeBalance(-30m);
        account1.ChangeBalance(-101m);
    }
}
