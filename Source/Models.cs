public record class DrinkCategoryList(List<DrinkCategory> drinks);
public record class DrinkCategory(string strCategory);

public record class DrinkPreviewList(List<DrinkPreview> drinks);
public record class DrinkPreview(string strDrink, string strDrinkThumb, string idDrink);