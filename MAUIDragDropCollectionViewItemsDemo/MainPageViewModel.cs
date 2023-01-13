using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace MAUIDragDropCollectionViewItemsDemo
{
    public partial class MainPageViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<User> userList;

        [ObservableProperty]
        private string user;

        private User _itemBeingDragged;

        public MainPageViewModel()
        {
            GetUserList();
        }

        [RelayCommand]
        public void AddUser()
        {
            Application.Current.Dispatcher.Dispatch(() =>
            {
                if (!string.IsNullOrEmpty(User))
                {
                    UserList.Add(new User() { UserName = User });
                    User = string.Empty;
                }
            });
        }

        [RelayCommand]
        public void DeleteUser(User user)
        {
            Application.Current.Dispatcher.Dispatch(() =>
            {
                if (UserList.Contains(user))
                {
                    UserList.Remove(user);
                }
            });
        }

        [RelayCommand]
        public void ItemDragged(User user)
        {
            Debug.WriteLine($"ItemDragged : {user?.UserName}");

            user.IsBeingDragged = true;
            _itemBeingDragged = user;
        }

        [RelayCommand]
        public void ItemDragLeave(User user)
        {
            Debug.WriteLine($"ItemDragLeave : {user?.UserName}");

            user.IsBeingDraggedOver = false;
        }

        [RelayCommand]
        public void ItemDraggedOver(User user)
        {
            Debug.WriteLine($"ItemDraggedOver : {user?.UserName}");

            if (user == _itemBeingDragged)
            {
                user.IsBeingDragged = false;
            }
            user.IsBeingDraggedOver = user != _itemBeingDragged;
        }

        [RelayCommand]
        public void ItemDropped(User user)
        {
            try
            {
                var itemToMove = _itemBeingDragged;
                var itemToInsertBefore = user;

                if (itemToMove == null || itemToInsertBefore == null || itemToMove == itemToInsertBefore)
                    return;

                int insertAtIndex = UserList.IndexOf(itemToInsertBefore);

                if (insertAtIndex >= 0 && insertAtIndex < UserList.Count)
                {
                    UserList.Remove(itemToMove);
                    UserList.Insert(insertAtIndex, itemToMove);

                    itemToMove.IsBeingDragged = false;
                    itemToInsertBefore.IsBeingDraggedOver = false;
                }

                Debug.WriteLine($"ItemDropped: [{itemToMove?.UserName}] => [{itemToInsertBefore?.UserName}], target index = [{insertAtIndex}]");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void GetUserList()
        {
            UserList = new ObservableCollection<User>()
            {
                new User(){UserName = "User 1"},
                new User(){UserName = "User 2"},
                new User(){UserName = "User 3"},
                new User(){UserName = "User 4"},
                new User(){UserName = "User 5"},
                new User(){UserName = "User 6"},
            };
        }
    }

    public partial class User : ObservableObject
    {
        [ObservableProperty]
        private string userName;

        [ObservableProperty]
        private bool isBeingDragged;

        [ObservableProperty]
        private bool isBeingDraggedOver;
    }
}