using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Grocery.App.Views;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;
using System.Collections.ObjectModel;

namespace Grocery.App.ViewModels
{
    [QueryProperty(nameof(GroceryList), nameof(GroceryList))]
    public partial class GroceryListItemsViewModel : BaseViewModel
    {
        private readonly IGroceryListItemsService _groceryListItemsService;
        private readonly IProductService _productService;
        public ObservableCollection<GroceryListItem> MyGroceryListItems { get; set; } = [];
        public ObservableCollection<Product> AvailableProducts { get; set; } = [];

        [ObservableProperty]
        GroceryList groceryList = new(0, "None", DateOnly.MinValue, "", 0);

        public GroceryListItemsViewModel(IGroceryListItemsService groceryListItemsService, IProductService productService)
        {
            _groceryListItemsService = groceryListItemsService;
            _productService = productService;
            Load(groceryList.Id);
        }

        private void Load(int id)
        {
            MyGroceryListItems.Clear();
            foreach (var item in _groceryListItemsService.GetAllOnGroceryListId(id)) MyGroceryListItems.Add(item);
            GetAvailableProducts();
        }

        private void GetAvailableProducts()
        {
            // Lege lijst
            AvailableProducts.Clear();

            // producten ophalen
            var allProducts = _productService.GetAll();

            // door alle producten langs lopen
            foreach (var product in allProducts)
            {
                // controle of de producten op voorraad zijn
                if (product.Stock > 0)
                {
                    // Stap 5: Controle of de producten niet al op de lijst staan
                    bool isAlreadyOnList = MyGroceryListItems.Any(item => item.ProductId == product.Id);

                    if (!isAlreadyOnList)
                    {
                        // Stap toevoegen beschikbare producten
                        AvailableProducts.Add(product);
                    }
                }
            }
        }

        partial void OnGroceryListChanged(GroceryList value)
        {
            Load(value.Id);
        }

        [RelayCommand]
        public async Task ChangeColor()
        {
            Dictionary<string, object> paramater = new() { { nameof(GroceryList), GroceryList } };
            await Shell.Current.GoToAsync($"{nameof(ChangeColorView)}?Name={GroceryList.Name}", true, paramater);
        }
        [RelayCommand]
        public void AddProduct(Product product)
        {
            if (product == null || product.Id <= 0) //Controleer of het product bestaat en dat de Id > 0
                return;

            var newItem = new GroceryListItem(0, GroceryList.Id, product.Id, 1); // nieuwe GroceryListItem

            //nieuwe items hebben Id 0
            // koppeling aan gekozen product
            // koppeling aan huidige boodschappen lijst


            _groceryListItemsService.Add(newItem); // toevoegen van item via service

            product.Stock--; //vermindering van de voorraad van het product

            _productService.Update(product); // opslaan van het gewijzigde product

            OnGroceryListChanged(GroceryList); // bewerken van de lijsten
        }
    }
}
