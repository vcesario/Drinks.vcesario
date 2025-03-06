using System.Text.Json;

string url = "https://thecocktaildb.com/api/json/v1/1/list.php?c=list";

var client = new HttpClient();

Stream stream = await client.GetStreamAsync(url);
var categoryList = JsonSerializer.Deserialize<DrinkCategoryList>(stream) ?? new(new());
foreach (var category in categoryList.drinks)
{
    Console.WriteLine(category.strCategory);
}
Console.ReadLine();