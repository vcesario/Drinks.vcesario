using System.Text.Json;
using Spectre.Console;

var client = new HttpClient();

string url_categories = "https://thecocktaildb.com/api/json/v1/1/list.php?c=list";
Stream stream = await client.GetStreamAsync(url_categories);
var categoryList = JsonSerializer.Deserialize<DrinkCategoryList>(stream) ?? new(new());
var prompt = new SelectionPrompt<DrinkCategory>()
    .Title("Choose a drink category")
    .AddChoices(categoryList.drinks)
    .UseConverter(cat => cat.strCategory);
prompt.AddChoice(new DrinkCategory("Exit"));
var chosenCategory = AnsiConsole.Prompt(prompt);
if (chosenCategory.strCategory.Equals("Exit"))
{
    return;
}

string url_drinkbycategory = "https://thecocktaildb.com/api/json/v1/1/filter.php?c=";
stream = await client.GetStreamAsync(url_drinkbycategory + chosenCategory.strCategory);
var drinkList = JsonSerializer.Deserialize<DrinkPreviewList>(stream) ?? new(new());
var prompt2 = new SelectionPrompt<DrinkPreview>()
        .Title("Choose a drink")
        .AddChoices(drinkList.drinks)
        .UseConverter(drink => drink.strDrink);
prompt2.AddChoice(new DrinkPreview("Return", "", "-1"));
var chosenDrink = AnsiConsole.Prompt(prompt2);
if (chosenDrink.strDrink.Equals("Return"))
{
    return;
}

string url_drinkdetails = "https://thecocktaildb.com/api/json/v1/1/lookup.php?i=";
stream = await client.GetStreamAsync(url_drinkdetails + chosenDrink.idDrink);

Console.WriteLine("Drink information...");
Console.ReadLine();