namespace GrainInterfaces.Model;

// todo: Make into grain
public class Menu
{
    public MenuItem[] Items { get; set; } = new[]
    {
        new MenuItem("Kaffe", 10),
        new MenuItem("Kaffe Latte", 15),
        new MenuItem("Kaffe Mocca", 20),
    };
}