using System;
using System.IO;
using Newtonsoft.Json;

public class TransferConfig
{
    public int Threshold { get; set; }
    public int LowFee { get; set; }
    public int HighFee { get; set; }
}

public class ConfirmationConfig
{
    public string En { get; set; }
    public string Id { get; set; }
}

public class BankTransferConfig
{
    public string Lang { get; set; } = "en"; 
    public TransferConfig Transfer { get; set; } = new TransferConfig
    {
        Threshold = 25000000, 
        LowFee = 6500, 
        HighFee = 15000 
    };
    public string[] Methods { get; set; } = new string[] { "RTO (real-time)", "SKN", "RTGS", "BI FAST" }; 
    public ConfirmationConfig Confirmation { get; set; } = new ConfirmationConfig
    {
        En = "yes", 
        Id = "ya" 
    };

    public BankTransferConfig(string filePath)
    {
        LoadConfig(filePath);
    }

    private void LoadConfig(string filePath)
    {
        if (!File.Exists(filePath))
        {
            Console.WriteLine("File konfigurasi tidak ditemukan, menggunakan nilai default.");
            return;
        }

        var json = File.ReadAllText(filePath);

        var config = JsonConvert.DeserializeObject<BankTransferConfig>(json);

        Lang = config.Lang ?? Lang;
        Transfer = config.Transfer ?? Transfer;
        Methods = config.Methods ?? Methods;
        Confirmation = config.Confirmation ?? Confirmation;
    }
}

class Program
{
    static void Main()
    {
        var config = new BankTransferConfig("bank_transfer_config.json");

        if (config.Lang == "en")
        {
            Console.WriteLine("Please insert the amount of money to transfer:");
        }
        else if (config.Lang == "id")
        {
            Console.WriteLine("Masukkan jumlah uang yang akan di-transfer:");
        }

        int amount = int.Parse(Console.ReadLine());

        int transferFee;
        if (amount <= config.Transfer.Threshold)
        {
            transferFee = config.Transfer.LowFee;
        }
        else
        {
            transferFee = config.Transfer.HighFee;
        }

        int totalAmount = amount + transferFee;

        if (config.Lang == "en")
        {
            Console.WriteLine($"Transfer fee = {transferFee}");
            Console.WriteLine($"Total amount = {totalAmount}");
        }
        else if (config.Lang == "id")
        {
            Console.WriteLine($"Biaya transfer = {transferFee}");
            Console.WriteLine($"Total biaya = {totalAmount}");
        }

        if (config.Lang == "en")
        {
            Console.WriteLine("Select transfer method:");
        }
        else if (config.Lang == "id")
        {
            Console.WriteLine("Pilih metode transfer:");
        }

        for (int i = 0; i < config.Methods.Length; i++)
        {
            Console.WriteLine($"{i + 1}. {config.Methods[i]}");
        }

        if (config.Lang == "en")
        {
            Console.WriteLine($"Please type \"{config.Confirmation.En}\" to confirm the transaction:");
        }
        else if (config.Lang == "id")
        {
            Console.WriteLine($"Ketik \"{config.Confirmation.Id}\" untuk mengkonfirmasi transaksi:");
        }

        string confirmationInput = Console.ReadLine();

        if ((config.Lang == "en" && confirmationInput == config.Confirmation.En) ||
            (config.Lang == "id" && confirmationInput == config.Confirmation.Id))
        {
            if (config.Lang == "en")
            {
                Console.WriteLine("The transfer is completed.");
            }
            else if (config.Lang == "id")
            {
                Console.WriteLine("Proses transfer berhasil.");
            }
        }
        else
        {
            if (config.Lang == "en")
            {
                Console.WriteLine("Transfer is cancelled.");
            }
            else if (config.Lang == "id")
            {
                Console.WriteLine("Transfer dibatalkan.");
            }
        }
    }
}