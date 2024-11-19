using System;
using System.IO;

class Product
{
    public string ProductID { get; set; }
    public string ProductName { get; set; }
    public string Manufacturer { get; set; }
    public double Price { get; set; }
    public string Description { get; set; }

    public void WriteToBinary(BinaryWriter writer)
    {
        writer.Write(ProductID);
        writer.Write(ProductName);
        writer.Write(Manufacturer);
        writer.Write(Price);
        writer.Write(Description);
    }

    public static Product ReadFromBinary(BinaryReader reader)
    {
        return new Product
        {
            ProductID = reader.ReadString(),
            ProductName = reader.ReadString(),
            Manufacturer = reader.ReadString(),
            Price = reader.ReadDouble(),
            Description = reader.ReadString()
        };
    }

    public override string ToString()
    {
        return $"{ProductID}, {ProductName}, {Manufacturer}, {Price:C}, {Description}";
    }
}

class Program
{
    static string filePath = "products.dat";

    static void Main()
    {
        while (true)
        {
            Console.WriteLine("\nChọn chức năng:");
            Console.WriteLine("1. Thêm sản phẩm");
            Console.WriteLine("2. Hiển thị sản phẩm");
            Console.WriteLine("3. Tìm kiếm sản phẩm");
            Console.WriteLine("4. Thoát");
            Console.Write("Lựa chọn: ");
            int choice = int.Parse(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    AddProduct();
                    break;
                case 2:
                    DisplayProducts();
                    break;
                case 3:
                    SearchProduct();
                    break;
                case 4:
                    return;
                default:
                    Console.WriteLine("Lựa chọn không hợp lệ. Vui lòng thử lại.");
                    break;
            }
        }
    }

    static void AddProduct()
    {
        Product product = new Product();

        Console.Write("Nhập mã sản phẩm: ");
        product.ProductID = Console.ReadLine();

        Console.Write("Nhập tên sản phẩm: ");
        product.ProductName = Console.ReadLine();

        Console.Write("Nhập hãng sản xuất: ");
        product.Manufacturer = Console.ReadLine();

        Console.Write("Nhập giá: ");
        product.Price = double.Parse(Console.ReadLine());

        Console.Write("Nhập mô tả: ");
        product.Description = Console.ReadLine();

        using (FileStream fs = new FileStream(filePath, FileMode.Append, FileAccess.Write))
        using (BinaryWriter writer = new BinaryWriter(fs))
        {
            product.WriteToBinary(writer);
        }

        Console.WriteLine("Thêm sản phẩm thành công!");
    }

    static void DisplayProducts()
    {
        if (!File.Exists(filePath))
        {
            Console.WriteLine("Không có sản phẩm nào.");
            return;
        }

        using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
        using (BinaryReader reader = new BinaryReader(fs))
        {
            Console.WriteLine("\nDanh sách sản phẩm:");
            while (fs.Position < fs.Length)
            {
                Product product = Product.ReadFromBinary(reader);
                Console.WriteLine(product);
            }
        }
    }

    static void SearchProduct()
    {
        Console.Write("Nhập mã sản phẩm cần tìm: ");
        string searchId = Console.ReadLine();

        bool found = false;

        if (File.Exists(filePath))
        {
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            using (BinaryReader reader = new BinaryReader(fs))
            {
                while (fs.Position < fs.Length)
                {
                    Product product = Product.ReadFromBinary(reader);
                    if (product.ProductID == searchId)
                    {
                        Console.WriteLine($"Sản phẩm tìm thấy: {product}");
                        found = true;
                        break;
                    }
                }
            }

            if (!found)
                Console.WriteLine("Không tìm thấy sản phẩm với mã đã nhập.");
        }
        else
        {
            Console.WriteLine("Không có sản phẩm nào để tìm kiếm.");
        }
    }
}
