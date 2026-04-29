
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

public class Recipe
{
    public string Title { get; set; }
    public List<string> Ingredients { get; set; } = new();
    public string Instructions { get; set; }
}

public interface IRecipeBook
{
    void AddRecipe(Recipe recipe);
    void ListAllRecipes();
    Recipe FindRecipe(string title);
    List<Recipe> AllRecipes { get; }
}

public class RecipeBook : IRecipeBook
{
    private List<Recipe> recipes = new();
    public List<Recipe> AllRecipes => recipes;

    public void AddRecipe(Recipe recipe) => recipes.Add(recipe);

    public void ListAllRecipes()
    {
        Console.WriteLine("Recipes:");
        foreach (var r in recipes)
            Console.WriteLine($"- {r.Title}");
    }

    public Recipe FindRecipe(string title) =>
        recipes.FirstOrDefault(r => r.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
}

public static class RecipePersistence
{
    public static async Task SaveAsync(IRecipeBook book, string filename)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        using FileStream stream = File.Create($"./{filename}");
        await JsonSerializer.SerializeAsync(stream, book.AllRecipes, options);
    }

    public static async Task<RecipeBook> LoadAsync(string filename)
    {
        if (!File.Exists(filename)) return new RecipeBook();
        using FileStream stream = File.OpenRead($"./{filename}");
        var recipes = await JsonSerializer.DeserializeAsync<List<Recipe>>(stream); //transforma textul in obj
        var book = new RecipeBook();
        if (recipes != null)
            foreach (var r in recipes) book.AddRecipe(r);
        return book;
    }
}

class Program
{
    static async Task Main()
    {
        IRecipeBook myBook = await RecipePersistence.LoadAsync("recipes.json");

        while (true)
        {
            Console.Write("(add/list/find/exit):");
            var input = Console.ReadLine();
            if (input == null) continue;

            if (input.Equals("exit", StringComparison.OrdinalIgnoreCase))
            {
                await RecipePersistence.SaveAsync(myBook, "recipes.json");
                break;
            }
            else if (input.Equals("list", StringComparison.OrdinalIgnoreCase))
            {
                myBook.ListAllRecipes();
            }
            else if (input.StartsWith("find ", StringComparison.OrdinalIgnoreCase))
            {
                var title = input[5..];
                var recipe = myBook.FindRecipe(title);
                if (recipe != null)
                {
                    Console.WriteLine($"Title: {recipe.Title}");
                    Console.WriteLine("Ingredients:");
                    recipe.Ingredients.ForEach(i => Console.WriteLine($"- {i}"));
                    Console.WriteLine($"Instructions: {recipe.Instructions}");
                }
                else
                {
                    Console.WriteLine("Recipe not found.");
                }
            }
            else if (input.Equals("add", StringComparison.OrdinalIgnoreCase))
            {
                var recipe = new Recipe();
                Console.Write("Title: ");
                recipe.Title = Console.ReadLine();
                Console.WriteLine("Enter ingredients:");
                while (true)
                {
                    var ing = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(ing)) break;
                    recipe.Ingredients.Add(ing);
                }
                Console.Write("Instructions: ");
                recipe.Instructions = Console.ReadLine();
                myBook.AddRecipe(recipe);
            }
        }
    }
}
