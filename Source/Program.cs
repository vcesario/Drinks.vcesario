using System.Text.Json;
using Spectre.Console;

using (var client = new HttpClient())
{
    string urlCategories = "https://thecocktaildb.com/api/json/v1/1/list.php?c=list";
    DrinkCategoryList categoryList;
    using (var stream = await client.GetStreamAsync(urlCategories))
    {
        categoryList = JsonSerializer.Deserialize<DrinkCategoryList>(stream) ?? new(new());
    }

    do
    {
        Console.Clear();

        var prompt = new SelectionPrompt<DrinkCategory>()
            .Title("Choose a drink category")
            .AddChoices(categoryList.drinks)
            .UseConverter(cat => cat.strCategory);
        prompt.AddChoice(new DrinkCategory("Exit"));
        var chosenCategory = AnsiConsole.Prompt(prompt);

        if (chosenCategory.strCategory.Equals("Exit"))
        {
            break;
        }

        await PromptDrink(chosenCategory.strCategory, client);
    }
    while (true);
}

async Task PromptDrink(string category, HttpClient client)
{
    string urlDrinkList = "https://thecocktaildb.com/api/json/v1/1/filter.php?c=";
    DrinkPreviewList drinkList;
    using (var stream = await client.GetStreamAsync(urlDrinkList + category))
    {
        drinkList = JsonSerializer.Deserialize<DrinkPreviewList>(stream) ?? new(new());
    }

    do
    {
        Console.Clear();
        AnsiConsole.MarkupLine($"Category: [indianred]{category}[/]");
        Console.WriteLine();

        var prompt2 = new SelectionPrompt<DrinkPreview>()
                .Title("Choose a drink")
                .AddChoices(drinkList.drinks)
                .UseConverter(drink => drink.strDrink);
        prompt2.AddChoice(new DrinkPreview("Return", "", "-1"));
        var chosenDrink = AnsiConsole.Prompt(prompt2);

        if (chosenDrink.idDrink.Equals("-1"))
        {
            break;
        }

        await ShowDetails(chosenDrink.idDrink, client);
    }
    while (true);
}

async Task ShowDetails(string drinkId, HttpClient client)
{
    Console.Clear();

    string urlDrinkDetails = "https://thecocktaildb.com/api/json/v1/1/lookup.php?i=";
    var stream = await client.GetStreamAsync(urlDrinkDetails + drinkId);

    Table table = new Table();
    table.AddColumns("", "");
    table.HideHeaders();

    using (var document = JsonDocument.Parse(stream))
    {
        foreach (var property in document.RootElement.GetProperty("drinks").EnumerateArray())
        {
            foreach (var prop in property.EnumerateObject())
            {
                if (!string.IsNullOrEmpty(prop.Value.GetString()))
                {
                    table.AddRow($"[dodgerblue1]{ConvertPropertyName(prop.Name)}[/]", $"[silver]{prop.Value}[/]");
                }
            }
        }
    }
    table.Border = TableBorder.Double;
    AnsiConsole.Write(table);
    Console.ReadLine();
}

string ConvertPropertyName(string jsonName)
{
    switch (jsonName)
    {
        case "idDrink": return "ID";
        case "strDrink": return "Name";
        case "strDrinkAlternate": return "Alt. name";
        case "strTags": return "Tags";
        case "strVideo": return "Video URL";
        case "strCategory": return "Category";
        case "strIBA": return "IBA";
        case "strAlcoholic": return "Alcoholic";
        case "strGlass": return "Glass type";
        case "strInstructions": return "How-to";
        case "strInstructionsES": return "How-to (ES)";
        case "strInstructionsDE": return "How-to (DE)";
        case "strInstructionsFR": return "How-to (FR)";
        case "strInstructionsIT": return "How-to (IT)";
        case "strInstructionsZH-HANS": return "How-to (ZH-HANS)";
        case "strInstructionsZH-HANT": return "How-to (ZH-HANT)";
        case "strDrinkThumb": return "Image URL";
        case "strIngredient1": return "Ingredient 1";
        case "strIngredient2": return "Ingredient 2";
        case "strIngredient3": return "Ingredient 3";
        case "strIngredient4": return "Ingredient 4";
        case "strIngredient5": return "Ingredient 5";
        case "strIngredient6": return "Ingredient 6";
        case "strIngredient7": return "Ingredient 7";
        case "strIngredient8": return "Ingredient 8";
        case "strIngredient9": return "Ingredient 9";
        case "strIngredient10": return "Ingredient 10";
        case "strIngredient11": return "Ingredient 11";
        case "strIngredient12": return "Ingredient 12";
        case "strIngredient13": return "Ingredient 13";
        case "strIngredient14": return "Ingredient 14";
        case "strIngredient15": return "Ingredient 15";
        case "strMeasure1": return "Measure 1";
        case "strMeasure2": return "Measure 2";
        case "strMeasure3": return "Measure 3";
        case "strMeasure4": return "Measure 4";
        case "strMeasure5": return "Measure 5";
        case "strMeasure6": return "Measure 6";
        case "strMeasure7": return "Measure 7";
        case "strMeasure8": return "Measure 8";
        case "strMeasure9": return "Measure 9";
        case "strMeasure10": return "Measure 10";
        case "strMeasure11": return "Measure 11";
        case "strMeasure12": return "Measure 12";
        case "strMeasure13": return "Measure 13";
        case "strMeasure14": return "Measure 14";
        case "strMeasure15": return "Measure 15";
        case "strImageSource": return "Image Source URL";
        case "strImageAttribution": return "Image Attr.";
        case "strCreativeCommonsConfirmed": return "Creative Commons?";
        case "dateModified": return "Last updated";
        default:
            return jsonName;
    }
}