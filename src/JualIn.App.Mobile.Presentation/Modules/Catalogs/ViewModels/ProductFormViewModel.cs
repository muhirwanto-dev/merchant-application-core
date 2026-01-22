using System.Collections.ObjectModel;
using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using JualIn.App.Mobile.Presentation.Core.Extensions.SingleScope;
using JualIn.App.Mobile.Presentation.Core.ViewModels;
using JualIn.App.Mobile.Presentation.Infrastructure.Mapping;
using JualIn.App.Mobile.Presentation.Modules.Catalogs.Abstractions;
using JualIn.App.Mobile.Presentation.Modules.Catalogs.ViewModels.Popups;
using JualIn.App.Mobile.Presentation.Modules.Catalogs.Views;
using JualIn.App.Mobile.Presentation.Modules.Inventories.ViewModels;
using JualIn.App.Mobile.Presentation.Modules.Inventories.Views;
using JualIn.App.Mobile.Presentation.Resources.Strings;
using JualIn.App.Mobile.Presentation.Shared.Persistence;
using JualIn.App.Mobile.Presentation.Shared.ViewModels;
using JualIn.Domain.Catalogs.Entities;
using JualIn.Domain.Catalogs.ValueObjects;
using JualIn.Domain.Inventories.Entities;
using JualIn.SharedLib.Extensions;
using SingleScope.Maui.Dialogs.Models;
using SingleScope.Navigations.Maui.Models;
using SingleScope.Persistence.Abstraction;

namespace JualIn.App.Mobile.Presentation.Modules.Catalogs.ViewModels
{
    public partial class ProductFormViewModel : BaseViewModel, IQueryAttributable
    {
        private readonly IMapper _mapper;
        private readonly IPopupService _popupService;
        private readonly IProductRepository _productRepository;
        private readonly ICategoryResolver<Product> _categoryResolver;
        private readonly IReadRepository<Inventory> _inventoryRepository;

        [ObservableProperty]
        private long _id = 0;

        [ObservableProperty]
        private string _name = string.Empty;

        [ObservableProperty]
        private string _category = ProductCategory.Food.Value;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Margin))]
        private double _price;

        [ObservableProperty]
        private int _stock = 0;

        [ObservableProperty]
        private string? _description;

        [ObservableProperty]
        private byte[]? _image;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Margin))]
        private double _capitalPrice;

        public double Margin => Price - CapitalPrice;

        public bool HasAnyComponent => ProductComponents.Count > 0;

        public ObservableCollection<string> ProductCategories { get; } = new(ProductCategory.List.Select(x => x.Value));

        public ObservableCollection<ProductComponentViewModel> ProductComponents { get; } = [];

        public ProductFormViewModel(
            IMapper mapper,
            IPopupService popupService,
            IProductRepository productRepository,
            ICategoryResolver<Product> categoryResolver,
            IReadRepository<Inventory> inventoryRepository)
        {
            _mapper = mapper;
            _popupService = popupService;
            _productRepository = productRepository;
            _categoryResolver = categoryResolver;
            _inventoryRepository = inventoryRepository;

            ProductComponents.CollectionChanged += (_, _) => NotifyComponentChanges();

            RegisterInteractionCommand(nameof(IsUserInteraction), SelectImageCommand);
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.TryGetValue("product", out object? value) && value is ProductViewModel product)
            {
                if (!ProductCategories.Contains(product.Entity.Category))
                {
                    // fix application freeze when notify category changes
                    // SelectedItem="{Binding Category}", should exist in ItemsSource="{Binding ProductCategories}"
                    ProductCategories.Add(product.Entity.Category);
                }

                _mapper.MapTo(product, this);
            }
        }

        [RelayCommand]
        private async Task AppearingAsync()
        {
            try
            {
                await FetchStoredCategoriesAsync();
            }
            catch (Exception ex)
            {
                _reporting.ReportProblems(ex);
            }
        }

        [RelayCommand(CanExecute = nameof(CanUserInteraction))]
        private async Task SelectImageAsync()
        {
            using var interaction = StartScopedUserInteraction();

            try
            {
                var file = await MediaPicker.Default.CapturePhotoAsync();
                if (file != null)
                {
                    using var stream = await file.OpenReadAsync();
                    using var ms = new MemoryStream();

                    await stream.CopyToAsync(ms);

                    Image = ms.ToArray();
                }
            }
            catch (Exception ex)
            {
                _reporting.ReportProblems(ex);
            }
        }

        [RelayCommand(CanExecute = nameof(CanUserInteraction))]
        private async Task SaveAsync()
        {
            using var interaction = StartScopedUserInteraction();

            try
            {
                var dto = _mapper.Map<ProductFormViewModel, Product>(this);
                var fromDetailPage = Shell.Current.Navigation.NavigationStack.Any(x => x is ProductDetailPage);
                var query = new Dictionary<string, object>
                {
                    ["refresh"] = true,
                };

                var components = dto.Components;
                dto.Components = [];

                await _productRepository.UpsertAsync(dto);
                await _productRepository.SaveAsync(); // save to update Id

                await _productRepository.UpdateComponentsAsync(dto.Id, components);
                await _productRepository.SaveAsync();

                await _navigation.BackAsync(ShellNavigationParams.Create(
                    route: fromDetailPage ? "../" : string.Empty,
                    queries: ("refresh", true)));
            }
            catch (Exception ex)
            {
                _reporting.ReportProblems(ex);
            }
        }

        [RelayCommand]
        private async Task AddCategoryAsync()
        {
            try
            {
                var category = await _dialogService.ShowAsync(Prompt.Untitled(AppStrings.ProductForm_Lbl_AddCategory_Body));
                if (category != null)
                {
                    ProductCategories.Add(category);

                    await Toast.Make(AppStrings.ProductForm_Msg_AddedNewCategory).Show();
                }
            }
            catch (Exception ex)
            {
                _reporting.ReportProblems(ex);
            }
        }

        [RelayCommand]
        private async Task EditComponentAsync()
        {
            try
            {
                var inventoryTask = _inventoryRepository.GetAllAsync();

                await _loadingService.ShowForAsync(async ct => await inventoryTask);

                var result = await _popupService.ShowPopupAsync<ProductComponentSelectionPopupViewModel, List<ProductComponentViewModel>>(
                    Shell.Current,
                    options: new PopupOptions
                    {
                        CanBeDismissedByTappingOutsideOfPopup = true,
                        Shadow = null,
                        Shape = null
                    },
                    shellParameters: new Dictionary<string, object>
                    {
                        ["components"] = ProductComponents,
                        ["inventories"] = inventoryTask.Result.Select(x => new SelectableItemViewModel<InventoryViewModel>(_mapper.Map<Inventory, InventoryViewModel>(x))
                        {
                            IsSelected = ProductComponents.Any(y => y.Inventory.Id == x.Id)
                        }).ToList()
                    });

                // cancelled
                if (result.Result is not IEnumerable<ProductComponentViewModel> selected)
                {
                    return;
                }

                ProductComponents.Clear();
                ProductComponents.AddRange(selected);
            }
            catch (Exception ex)
            {
                _reporting.ReportProblems(ex);
            }
        }

        [RelayCommand(CanExecute = nameof(CanNavigate))]
        private async Task OpenInventoryDetailAsync(ProductComponentViewModel item)
        {
            using var _ = StartScopedNavigation();

            try
            {
                await _navigation.NavigateToAsync<InventoryDetailPage>(ShellNavigationParams.Create(
                    ("inventory", _mapper.Map<Inventory, InventoryViewModel>(item.Inventory))));
            }
            catch (Exception ex)
            {
                _reporting.ReportProblems(ex);
            }
        }

        [RelayCommand]
        private void QuantityPerUnitChanged()
        {
            try
            {
                NotifyComponentChanges();
            }
            catch (Exception ex)
            {
                _reporting.ReportProblems(ex);
            }
        }

        private async Task FetchStoredCategoriesAsync()
        {
            var categories = await _categoryResolver.GetCategoriesAsync();
            var except = ProductCategory.List.Select(x => x.Value).ToArray();

            foreach (var cat in categories.Distinct().Except(except))
            {
                ProductCategories.Add(cat);
            }
        }

        public void NotifyComponentChanges()
        {
            CapitalPrice = ProductComponents.Sum(x => x.Inventory.UnitPrice * x.QuantityPerUnit);

            OnPropertyChanged(nameof(HasAnyComponent));
        }
    }
}
