using CinemaMOON.Data;
using CinemaMOON.Models;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace CinemaMOON.ViewModels
{
	public class OrderStatusFilterItem : ViewModelBase
	{
		public string DisplayNameKey { get; set; }
		public string FilterValue { get; set; }
		public string DisplayName => Application.Current.TryFindResource(DisplayNameKey) as string ?? DisplayNameKey;
		public override string ToString() => DisplayName;
	}

	public class AdminOrdersViewModel : ViewModelBase
	{
		private readonly AppDbContext _dbContext;
		private ObservableCollection<Order> _allOrders;
		private Order _selectedOrder;

		private ObservableCollection<OrderStatusFilterItem> _orderStatusFilters;
		private OrderStatusFilterItem _selectedOrderStatusFilter;

		public ObservableCollection<Order> AllOrders
		{
			get => _allOrders;
			set => SetProperty(ref _allOrders, value);
		}

		public Order SelectedOrder
		{
			get => _selectedOrder;
			set
			{
				if (SetProperty(ref _selectedOrder, value))
				{
					CancelOrderCommand.NotifyCanExecuteChanged();
					DeleteOrderCommand.NotifyCanExecuteChanged();
					SetViewedCommand.NotifyCanExecuteChanged();
				}
			}
		}

		public ObservableCollection<OrderStatusFilterItem> OrderStatusFilters
		{
			get => _orderStatusFilters;
			set => SetProperty(ref _orderStatusFilters, value);
		}

		public OrderStatusFilterItem SelectedOrderStatusFilter
		{
			get => _selectedOrderStatusFilter;
			set
			{
				if (SetProperty(ref _selectedOrderStatusFilter, value))
				{
					_ = LoadOrdersCommand.ExecuteAsync(null);
				}
			}
		}

		public IAsyncRelayCommand LoadOrdersCommand { get; }
		public IAsyncRelayCommand CancelOrderCommand { get; }
		public IAsyncRelayCommand DeleteOrderCommand { get; }
		public IAsyncRelayCommand SetViewedCommand { get; }
		public ICommand GoBackCommand { get; }

		public AdminOrdersViewModel(AppDbContext dbContext)
		{
			_dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
			AllOrders = new ObservableCollection<Order>();

			InitializeFilters();

			LoadOrdersCommand = new AsyncRelayCommand(LoadAllOrdersAsync);
			CancelOrderCommand = new AsyncRelayCommand(ExecuteCancelOrderAsync, CanExecuteCancelOrderAction);
			DeleteOrderCommand = new AsyncRelayCommand(ExecuteDeleteOrderAsync, CanExecuteDeleteOrderAction);
			SetViewedCommand = new AsyncRelayCommand(ExecuteSetViewedAsync, CanExecuteSetViewedAction);
			GoBackCommand = new RelayCommand<Page>(ExecuteGoBack);
		}

		private void InitializeFilters()
		{
			OrderStatusFilters = new ObservableCollection<OrderStatusFilterItem>
			{
                new OrderStatusFilterItem { DisplayNameKey = "AdminOrdersPage_Filter_All", FilterValue = null },
                new OrderStatusFilterItem { DisplayNameKey = "OrderStatus_Booked", FilterValue = "OrderStatus_Booked" },
				new OrderStatusFilterItem { DisplayNameKey = "OrderStatus_Canceled", FilterValue = "OrderStatus_Canceled" },
				new OrderStatusFilterItem { DisplayNameKey = "OrderStatus_Viewed", FilterValue = "OrderStatus_Viewed" }
			};
			_selectedOrderStatusFilter = OrderStatusFilters.FirstOrDefault(f => f.FilterValue == null);
		}

		private bool CanExecuteDeleteOrderAction()
		{
			return SelectedOrder != null;
		}

		private bool CanExecuteCancelOrderAction()
		{
			if (SelectedOrder == null) return false;

			return SelectedOrder.OrderStatus == "OrderStatus_Booked";
		}

		private bool CanExecuteSetViewedAction()
		{
			if (SelectedOrder == null) return false;

			return SelectedOrder.OrderStatus == "OrderStatus_Booked";
		}

		private async Task LoadAllOrdersAsync()
		{
			try
			{
				AllOrders.Clear();
				SelectedOrder = null;

				var query = _dbContext.Orders
					.Include(o => o.User)
					.Include(o => o.Schedule).ThenInclude(s => s.Movie)
					.Include(o => o.Schedule).ThenInclude(s => s.Hall)
					.AsQueryable();

				if (SelectedOrderStatusFilter != null && !string.IsNullOrEmpty(SelectedOrderStatusFilter.FilterValue))
				{
					query = query.Where(o => o.OrderStatus == SelectedOrderStatusFilter.FilterValue);
				}

				var ordersFromDb = await query
					.OrderByDescending(o => o.BookingTimestamp)
					.ToListAsync();

				foreach (var order in ordersFromDb)
				{
					AllOrders.Add(order);
				}
			}
			catch (Exception ex)
			{
				ShowMessageFormat("AdminOrdersPage_Error_LoadingOrders", "AdminPanel_Title_Error", MessageBoxImage.Error, ex.Message);
			}
			finally
			{
				CancelOrderCommand.NotifyCanExecuteChanged();
				DeleteOrderCommand.NotifyCanExecuteChanged();
				SetViewedCommand.NotifyCanExecuteChanged();
			}
		}

		private async Task ExecuteCancelOrderAsync()
		{
			if (!CanExecuteCancelOrderAction())
			{
				if (SelectedOrder == null)
					ShowMessage("AdminOrdersPage_Error_NoOrderSelected", "AdminPanel_Title_Warning", MessageBoxImage.Warning);
				else
				{
					string currentStatusKey = SelectedOrder.OrderStatus;
					string localizedCurrentStatus = GetResourceString(currentStatusKey) ?? currentStatusKey;
					ShowMessageFormat("AccountPage_Error_CannotCancelStatus", "AdminPanel_Title_Info", MessageBoxImage.Information, localizedCurrentStatus);
				}
				return;
			}

			var orderToModify = SelectedOrder;
			string question = string.Format(GetResourceString("AdminOrdersPage_Confirm_CancelOrder") ?? "Отменить заказ ID {0}?", orderToModify.Id);
			string title = GetResourceString("AdminOrdersPage_Confirm_CancelOrderTitle") ?? "Отмена заказа";

			if (MessageBox.Show(question, title, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
			{
				try
				{
					var orderInDb = await _dbContext.Orders.FindAsync(orderToModify.Id);
					if (orderInDb != null)
					{
						orderInDb.OrderStatus = "OrderStatus_Canceled";
						await _dbContext.SaveChangesAsync();
						ShowMessageFormat("AdminOrdersPage_Success_OrderCanceled", "AdminPanel_Title_Success", MessageBoxImage.Information, orderInDb.Id);
						await LoadAllOrdersAsync();
					}
					else
					{
						ShowMessage("AdminOrdersPage_Error_OrderNotFoundInDb", "AdminPanel_Title_Error", MessageBoxImage.Error);
					}
				}
				catch (Exception ex)
				{
					ShowMessageFormat("AdminOrdersPage_Error_CancelOrderFailed", "AdminPanel_Title_Error", MessageBoxImage.Error, orderToModify.Id, ex.Message);
				}
			}
		}

		private async Task ExecuteDeleteOrderAsync()
		{
			if (!CanExecuteDeleteOrderAction())
			{
				ShowMessage("AdminOrdersPage_Error_NoOrderSelected", "AdminPanel_Title_Warning", MessageBoxImage.Warning);
				return;
			}

			var orderToDelete = SelectedOrder;
			string question = string.Format(GetResourceString("AdminOrdersPage_Confirm_DeleteOrder") ?? "Удалить заказ ID {0}?", orderToDelete.Id);
			string title = GetResourceString("AdminOrdersPage_Confirm_DeleteOrderTitle") ?? "Удаление заказа";

			if (MessageBox.Show(question, title, MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
			{
				try
				{
					var orderInDb = await _dbContext.Orders.FindAsync(orderToDelete.Id);
					if (orderInDb != null)
					{
						_dbContext.Orders.Remove(orderInDb);
						await _dbContext.SaveChangesAsync();
						ShowMessageFormat("AdminOrdersPage_Success_OrderDeleted", "AdminPanel_Title_Success", MessageBoxImage.Information, orderInDb.Id);
						AllOrders.Remove(orderToDelete);
						SelectedOrder = null;
					}
					else
					{
						ShowMessage("AdminOrdersPage_Error_OrderNotFoundInDb", "AdminPanel_Title_Error", MessageBoxImage.Error);
						if (AllOrders.Contains(orderToDelete)) AllOrders.Remove(orderToDelete);
						SelectedOrder = null;
					}
				}
				catch (Exception ex)
				{
					ShowMessageFormat("AdminOrdersPage_Error_DeleteOrderFailed", "AdminPanel_Title_Error", MessageBoxImage.Error, orderToDelete.Id, ex.Message);
				}
			}
		}

		private async Task ExecuteSetViewedAsync()
		{
			if (!CanExecuteSetViewedAction())
			{
				if (SelectedOrder == null)
					ShowMessage("AdminOrdersPage_Error_NoOrderSelected", "AdminPanel_Title_Warning", MessageBoxImage.Warning);
				else
				{
					string currentStatusKey = SelectedOrder.OrderStatus;
					string localizedCurrentStatus = GetResourceString(currentStatusKey) ?? currentStatusKey;
					ShowMessageFormat("AccountPage_Error_CannotCancelStatus", "AdminPanel_Title_Info", MessageBoxImage.Information, localizedCurrentStatus);
				}
				return;
			}

			var orderToModify = SelectedOrder;
			string question = string.Format(GetResourceString("AdminOrdersPage_Confirm_SetViewed") ?? "Пометить заказ ID {0} как просмотрено?", orderToModify.Id);
			string title = GetResourceString("AdminOrdersPage_Confirm_SetViewedTitle") ?? "Статус 'Просмотрено'";

			if (MessageBox.Show(question, title, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
			{
				try
				{
					var orderInDb = await _dbContext.Orders.FindAsync(orderToModify.Id);
					if (orderInDb != null)
					{
						orderInDb.OrderStatus = "OrderStatus_Viewed";
						await _dbContext.SaveChangesAsync();
						ShowMessageFormat("AdminOrdersPage_Success_OrderSetViewed", "AdminPanel_Title_Success", MessageBoxImage.Information, orderInDb.Id);

						await LoadAllOrdersAsync();
					}
					else
					{
						ShowMessage("AdminOrdersPage_Error_OrderNotFoundInDb", "AdminPanel_Title_Error", MessageBoxImage.Error);
					}
				}
				catch (Exception ex)
				{
					ShowMessageFormat("AdminOrdersPage_Error_SetViewedFailed", "AdminPanel_Title_Error", MessageBoxImage.Error, orderToModify.Id, ex.Message);
				}
			}
		}

		private void ExecuteGoBack(Page currentPage)
		{
			if (currentPage?.NavigationService?.CanGoBack == true)
			{
				currentPage.NavigationService.GoBack();
			}
		}

		private string GetResourceString(string key) => Application.Current.TryFindResource(key) as string ?? $"[{key}]";

		private void ShowMessage(string messageKey, string titleKey, MessageBoxImage icon)
		{
			MessageBox.Show(GetResourceString(messageKey), GetResourceString(titleKey), MessageBoxButton.OK, icon);
		}

		private void ShowMessageFormat(string messageKey, string titleKey, MessageBoxImage icon, params object[] args)
		{
			string messageFormat = GetResourceString(messageKey);
			string message = string.Format(messageFormat, args);
			MessageBox.Show(message, GetResourceString(titleKey), MessageBoxButton.OK, icon);
		}

		public void UpdateLocalization()
		{
			var currentFilterValue = SelectedOrderStatusFilter?.FilterValue;
			InitializeFilters();
			SelectedOrderStatusFilter = OrderStatusFilters.FirstOrDefault(f => f.FilterValue == currentFilterValue)
									 ?? OrderStatusFilters.First();
			LoadOrdersCommand.NotifyCanExecuteChanged();
			CancelOrderCommand.NotifyCanExecuteChanged();
			DeleteOrderCommand.NotifyCanExecuteChanged();
			SetViewedCommand.NotifyCanExecuteChanged(); 
			_ = LoadAllOrdersAsync();
		}
	}
}