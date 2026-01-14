using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using JualIn.App.Mobile.Presentation.Core.Extensions.SingleScope;
using JualIn.App.Mobile.Presentation.Core.ViewModels;
using JualIn.App.Mobile.Presentation.Infrastructure.Mapping;
using JualIn.App.Mobile.Presentation.Modules.Inventories.Abstractions;
using JualIn.App.Mobile.Presentation.Modules.Inventories.Views;
using JualIn.App.Mobile.Presentation.Resources.Strings;
using JualIn.App.Mobile.Presentation.Shared.Persistence;
using JualIn.Domain.Inventories.Entities;
using JualIn.SharedLib.Extensions;
using SingleScope.Maui.Dialogs.Models;
using SingleScope.Navigations.Maui.Models;

namespace JualIn.App.Mobile.Presentation.Modules.Inventories.ViewModels
{
    public partial class InventoryFormViewModel : BaseViewModel, IQueryAttributable
    {
        private readonly IMapper _mapper;
        private readonly IInventoryRepository _inventoryRepository;
        private readonly ICategoryResolver<Inventory> _categoryResolver;

        [ObservableProperty]
        private long _id;

        [ObservableProperty]
        private string _inventoryId = string.Empty;

        [ObservableProperty]
        private string _category = string.Empty;

        [ObservableProperty]
        private string _name = string.Empty;

        [ObservableProperty]
        private double _unitPrice = 0;

        [ObservableProperty]
        private string _stockUnit = Domain.Inventories.ValueObjects.StockUnit.Gram;

        [ObservableProperty]
        private int? _batchNumber;

        [ObservableProperty]
        private string? _description;

        [ObservableProperty]
        private byte[]? _image;

        [ObservableProperty]
        private string? _sku;

        [ObservableProperty]
        private double _stock = 0;

        [ObservableProperty]
        private double _stockThreshold = 0;

        [ObservableProperty]
        private long? _supplierId;

        [ObservableProperty]
        private string? _supplierName;

        [ObservableProperty]
        private string? _upc;

        [ObservableProperty]
        private DateTime? _expirationDate;

        [ObservableProperty]
        private DateTime _lastStockUpdate = DateTime.UtcNow;

        [ObservableProperty]
        private DateTime _purchaseDate = DateTime.UtcNow;

        [ObservableProperty]
        private IEnumerable<string> _stockUnits = [];

        public ObservableCollection<string> InventoryCategories { get; } = [];

        public InventoryFormViewModel(
            IMapper mapper,
            IInventoryRepository inventoryRepository
            )
        {
            _mapper = mapper;
            _inventoryRepository = inventoryRepository;

            RegisterInteractionCommand(nameof(IsUserInteraction), SaveCommand);
            RegisterInteractionCommand(nameof(IsUserInteraction), SelectImageCommand);

            MainThread.BeginInvokeOnMainThread(() =>
            {
                StockUnits = [.. Domain.Inventories.ValueObjects.StockUnit.List.Select(x => x.Value)];
            });
        }

        void IQueryAttributable.ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.TryGetValue("inventory", out var obj) && obj is InventoryViewModel vm)
            {
                if (!InventoryCategories.Contains(vm.Entity.Category))
                {
                    InventoryCategories.Add(vm.Entity.Category);
                }

                _mapper.MapTo(vm, this);
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
        private async Task SaveAsync()
        {
            using var interaction = StartScopedUserInteraction();

            try
            {
                var dto = _mapper.Map<InventoryFormViewModel, Inventory>(this);
                var fromDetailPage = Shell.Current.Navigation.NavigationStack.Any(x => x is InventoryFormPage);

                await _inventoryRepository.UpsertAsync(dto);
                await _inventoryRepository.SaveAsync();
                await _navigation.BackAsync(ShellNavigationParams.Create(
                    route: fromDetailPage ? "../" : string.Empty,
                    queries: ("refresh", true)));
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

        [RelayCommand]
        private async Task AddCategoryAsync()
        {
            try
            {
                var category = await _dialogService.ShowAsync(Prompt.Untitled(AppStrings.InventoryForm_Lbl_AddCategory_Body));
                if (category != string.Empty)
                {
                    InventoryCategories.Add(category);

                    await Toast.Make(AppStrings.InventoryForm_Msg_AddedNewCategory).Show();
                }
            }
            catch (Exception ex)
            {
                _reporting.ReportProblems(ex);
            }
        }

        private async Task FetchStoredCategoriesAsync()
        {
            string[] categories = await _categoryResolver.GetCategoriesAsync();

            InventoryCategories.AddRange([.. categories.Distinct()]);
        }
    }
}