// MY MODELS MAPS SAKA CONTEXT NA TO WITH DB CONNECT
app.controller("OrderingSystem2Controller", function (OrderingSystem2Service, $scope, $window) {
    // for log in and registration of users (incl store setup)
    $scope.tempData = [];
    $scope.loginDisable = true;
    $scope.registerTwoWayDisable = true;
    $scope.regVendorDisable = true;
    $scope.fieldChecker = function (num) {
        switch (num) {
            case 1:
                if (!$scope.User_Username || !$scope.User_Password)
                {
                    $scope.loginDisable = true;
                }
                else
                {
                    $scope.loginDisable = false;
                }
                break;
            case 2:
                if (!$scope.User_Fullname || !$scope.User_Email || !$scope.User_Address || !$scope.User_Username || !$scope.User_Password)
                {
                    $scope.registerTwoWayDisable = true;
                }
                else
                {
                    $scope.registerTwoWayDisable = false;
                }
                break;
            case 3:
                if (!$scope.User_Storename || !$scope.User_Storeaddress || !$scope.User_Storepicture || !$scope.User_Storecategory)
                {
                    $scope.regVendorDisable = true;
                }
                else
                {
                    $scope.regVendorDisable = false;
                }
                break;
        }
    };
    $scope.emailValidator = function () {
        var emailRegx = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        if (emailRegx.test($scope.User_Email)) {
            $scope.emailAlert = false;
            $scope.fieldChecker(2);
        }
        else
        {
            $scope.emailAlert = true;
            $scope.fieldChecker(2);
        }
    }
    $scope.storeFileChoosen = function (element) {
        $scope.$apply(function () {
            if (element.files.length > 0) {
                $scope.User_Storepicture = element.files[0];
            } else {
                $scope.User_Storepicture = null;
            }
            $scope.fieldChecker(3);
        });
    };
    $scope.register = function (num) {
        switch (num) {
            case 1:
                if ($scope.isVendorChecked)
                {
                    $scope.tempData = {
                        User_Fullname: $scope.User_Fullname,
                        User_Email: $scope.User_Email,
                        User_Address: $scope.User_Address,
                        User_Username: $scope.User_Username,
                        User_Password: $scope.User_Password,
                        User_isVendor: true
                    }
                    Swal.fire({
                        title: 'Verify Information',
                        text: "here is the information you inputted. do you wish to continue to setup store",
                        html: "Fullname: " + $scope.User_Fullname + "<br>" +
                              "Email: " + $scope.User_Email + "<br>" + 
                              "Address: " + $scope.User_Address + "<br>" + 
                              "Username: " + $scope.User_Username + "<br>" + 
                              "Password: " + $scope.User_Password + "<br>" + 
                              "Are you a Vendor ?: " + ($scope.isVendorChecked ? 'yes' : 'no'),
                        icon: 'question',
                        showCancelButton: true,
                        confirmButtonText: 'Yes, continue',
                        cancelButtonText: 'No,Modiy Information'
                    }).then((result) => {
                        if (result.isConfirmed) {
                            var Service = OrderingSystem2Service.checkTempInfo($scope.tempData);
                            Service.then(function (response) {
                                if (response.data.success) {
                                    sessionStorage.setItem("tempInfo", JSON.stringify($scope.tempData));
                                    $scope.redirectFunc(1);
                                }
                                else {
                                    Swal.fire("Username and/or email already taken please try a different one");
                                }
                            });
                        }
                    });
                }
                else
                {
                    var normalUser = {
                        User_Fullname: $scope.User_Fullname,
                        User_Email: $scope.User_Email,
                        User_Address: $scope.User_Address,
                        User_Username: $scope.User_Username,
                        User_Password: $scope.User_Password,
                        User_isVendor: false
                    }
                    Swal.fire({
                        title: 'Verify Information',
                        text: "here is the information you inputted. do you to register as normal user ?",
                        html: "Fullname: " + $scope.User_Fullname + "<br>" +
                            "Email: " + $scope.User_Email + "<br>" +
                            "Address: " + $scope.User_Address + "<br>" +
                            "Username: " + $scope.User_Username + "<br>" +
                            "Password: " + $scope.User_Password + "<br>" +
                            "Are you a Vendor ?: " + ($scope.isVendorChecked ? 'yes' : 'no'),
                        icon: 'question',
                        showCancelButton: true,
                        confirmButtonText: 'Yes, finished register',
                        cancelButtonText: 'No, Modiy Information'
                    }).then((result) => {
                        if (result.isConfirmed) {
                            var Service = OrderingSystem2Service.registerUser(normalUser);
                            Service.then(function (response) {
                                if (response.data.success) {
                                    Swal.fire({
                                        title: 'User Successfully Registered as normal user',
                                        icon: 'success',
                                        confirmButtonText: 'OK'
                                    }).then((result) => {
                                        if (result.isConfirmed) {
                                            $scope.redirectFunc(2);
                                        }
                                    });
                                } else {
                                    Swal.fire("Failed to register: ", response.data.message);
                                }
                            });
                        }
                    });
                }
                break;
            case 2:
                var tempsData = sessionStorage.getItem("tempInfo");
                if (tempsData != null) {
                    $scope.tempData = JSON.parse(tempsData);
                }
                else {
                    Swal.fire("user data missing. please repeat the register process");
                    return;
                }
                var vendorData = new FormData();
                vendorData.append("User_Fullname", $scope.tempData.User_Fullname);
                vendorData.append("User_Email", $scope.tempData.User_Email);
                vendorData.append("User_Address", $scope.tempData.User_Address);
                vendorData.append("User_Username", $scope.tempData.User_Username);
                vendorData.append("User_Password", $scope.tempData.User_Password);
                vendorData.append("User_isVendor", $scope.tempData.User_isVendor);
                vendorData.append("Store_Category_Id", $scope.User_Storecategory);
                vendorData.append("Vendor_Storename", $scope.User_Storename);
                vendorData.append("Vendor_Store_Address", $scope.User_Storeaddress);
                vendorData.append("Vendor_Storepicture", $scope.User_Storepicture);
                $scope.category = [
                    { id: 1, name: "Fast Food" },
                    { id: 2, name: "Cafe" },
                    { id: 3, name: "Desserts" },
                    { id: 4, name: "Others" },
                    { id: 5, name: "Bakery" },
                    { id: 6, name: "Beverages" },
                    { id: 7, name: "Fine Dining" },
                ];
                var selectedCategory = $scope.category.find(x => x.id == $scope.User_Storecategory);
                var categoryName = selectedCategory.name;
                Swal.fire({
                    title: 'Verify Information',
                    text: "here is the information you inputted. do you to register as a Vendor ?",
                    html: "Store Name: " + $scope.User_Storename + "<br>" +
                        "Store Address: " + $scope.User_Storeaddress + "<br>" +
                        "Store Picture: " + $scope.User_Storepicture.name + "<br>" +
                        "Store Category: " + categoryName,
                    icon: 'question',
                    showCancelButton: true,
                    confirmButtonText: 'Yes, finished register',
                    cancelButtonText: 'No, Modiy Information'
                }).then((result) => {
                    if (result.isConfirmed) {
                        var Service = OrderingSystem2Service.registerVendor(vendorData);
                        Service.then(function (response) {
                            if (response.data.success) {
                                Swal.fire({
                                    title: 'User Successfully Registered as Vendor',
                                    icon: 'success',
                                    confirmButtonText: 'OK'
                                }).then((result) => {
                                    if (result.isConfirmed) {
                                        sessionStorage.removeItem("tempInfo");
                                        $scope.redirectFunc(2);
                                    }
                                });
                            }
                            else {
                                Swal.fire("Failed to register: ", response.data.message);
                            }
                        });
                    }
                });
                break;
        }
    };
    $scope.Login = function () {
        var authInfo = {
            User_Username: $scope.User_Username,
            User_Password: $scope.User_Password
        }
        var Service = OrderingSystem2Service.authUser(authInfo);
        Service.then(function (response) {
            if (response.data.success)
            {
                if (response.data.role == "Vendor")
                {
                    $scope.redirectFunc(6);
                }
                else
                {
                    $scope.redirectFunc(5);
                }
            }
            else
            {
                Swal.fire(response.data.message);
            }
        });
    }
    $scope.redirectFunc = function (num, Vendor_ID, User_ID) {
        switch (num) {
            // for user 
            case 1:
                $window.location.href = "/UserAccounts/StoreSetupPage";
                break;
            case 2:
                $window.location.replace("/UserAccounts/LoginPage");
                break;
            case 3:
                var userInfo = sessionStorage.getItem("tempInfo");
                if (userInfo != null) {
                    Swal.fire({
                        title: 'Are you sure?',
                        text: "You have unsaved registration data. Do you want to cancel registration?",
                        icon: 'warning',
                        showCancelButton: true,
                        confirmButtonText: 'Yes, cancel registration',
                        cancelButtonText: 'No, keep my data'
                    }).then((result) => {
                        if (result.isConfirmed) {
                            sessionStorage.removeItem("tempInfo");
                            $scope.redirectFunc(2);
                        }
                    });
                }
                else
                {
                    $scope.redirectFunc(2);
                }
                break;
            // for Normal Main Page
            case 4:
                $window.location.href = "/NormalUser/MenuPage?Vendor_ID=" + Vendor_ID;
                break;
            case 5:
                $window.location.replace("/NormalUser/NormalMainPage");
                break;
            case 6:
                $window.location.replace("/AdminUser/AdminMainPage");
                break;
            case 7:
                $window.location.href = "/AdminUser/AdminMenuViewPage";
                break;
            case 8:
                $window.location.href = "/AdminUser/AdminOrderViewPage";
                break;
            case 9:
                $window.location.href = "/NormalUser/NormalCartPage";
                break;
        }
    }
    // for user profile (normal and admin)
    $scope.isUpdating = false;
    $scope.getUserProfileInfos = function () {
        var Service = OrderingSystem2Service.getUserInfo();
        Service.then(function (response) {
            $scope.userProfInfo = response.data;
        });
    }
    $scope.editProfile = function () {
        $scope.isUpdating = true;
        $scope.tempInfo = angular.copy($scope.userProfInfo);
    }
    $scope.isNEmailInvalid = false;
    $scope.validEmail = function () {
        var emailRegx = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        if (emailRegx.test($scope.tempInfo.User_Email)) {
            $scope.isNEmailInvalid = false;
        } else {
            $scope.isNEmailInvalid = true;
        }
    }
    $scope.saveNewUserInfo = function () {
        var Service = OrderingSystem2Service.updateUserInfo($scope.tempInfo);
        Service.then(function (response) {
            if (response.data.success) {
                $scope.userProfInfo = angular.copy($scope.tempInfo);
                $scope.isUpdating = false;
                Swal.fire("Notice", "Updated Successfully");
            } else {
                Swal.fire("Notice", response.data.message);
            }
        });
    }
    // for the normal user main page
    $scope.getRestoInfo = function () {
        var Service = OrderingSystem2Service.getRestoInfo();
        Service.then(function (returnRestoInfo) {
            $scope.restoInfos = returnRestoInfo.data;
        });
    }
    $scope.getUserInfo = function () {
        var Service = OrderingSystem2Service.getUserInfo();
        Service.then(function (returnUserInfo) {
            $scope.userInfo = returnUserInfo.data;
        });
    }
    $scope.logOut = function () {
        Swal.fire({
            title: 'Notice',
            text: "Do you want to Log Out",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonText: 'Yes',
            cancelButtonText: 'No'
        }).then((result) => {
            if (result.isConfirmed) {
                var Service = OrderingSystem2Service.logOut();
                Service.then(function (response) {
                    if (response.data.success)
                    {
                        $scope.redirectFunc(2);
                    }
                    else
                    {
                        Swal.fire("Failed to logout from your account");
                    }
                });
            }
        });
    }
    $scope.getStoreCategory = function () {
        var Service = OrderingSystem2Service.getStoreCategory();
        Service.then(function (returnCategoryInfo) {
            $scope.categories = returnCategoryInfo.data;
        });
    }
    $scope.selectedCategory = 0;
    $scope.filterStoreList = function () {
        var Service = OrderingSystem2Service.getFilterStoreList($scope.selectedCategory);
        Service.then(function (response) {
            $scope.restoInfos = response.data;
        });
    }
    $scope.searchBtn = function () {
        var userSearchResto = $scope.searchBar;
        var Service = OrderingSystem2Service.getSearchResult(userSearchResto);
        Service.then(function (response) {
            $scope.restoInfos = response.data;
        });
    }
    $scope.getRestoMenus = function (Vendor_ID) {
        var Service = OrderingSystem2Service.getRestoMenus(Vendor_ID);
        Service.then(function (response) {
            $scope.restoMenuInfo = response.data;
        });
    }
    // for the add to cart
    $scope.cartItems = JSON.parse(sessionStorage.getItem("cart")) || [];
    $scope.addtoOrder = function (menu) {
        var existingItem = $scope.cartItems.find(x => x.Menu_ID === menu.Menu_ID);
        if (existingItem) {
            existingItem.Quantity += 1;
            Swal.fire("Notice", "Another item added to your cart");
        } else {

            $scope.cartItems.push({
                Menu_ID: menu.Menu_ID,
                Menu_Name: menu.Menu_Name,
                Menu_Price: menu.Menu_Price,
                Menu_Image: menu.Menu_Image,
                Quantity: 1,
                Vendor_ID: $scope.restoMenuInfo.Vendor.Vendors.Vendor_ID
            });
            Swal.fire("Success", "Item added to your cart");
        }
        $scope.saveOrder();
    };
    $scope.deleteOrder = function (menuId) {
        $scope.cartItems = $scope.cartItems.filter(x => x.Menu_ID !== menuId);
        $scope.saveOrder();
    };
    $scope.saveOrder = function () {
        sessionStorage.setItem("cart", JSON.stringify($scope.cartItems));
    }
    $scope.getOverallCost = function () {
        let total = 0;
        if (!$scope.cartItems) {
            return 0;
        }
        $scope.cartItems.forEach(item => {
            total += (item.Menu_Price * item.Quantity);
        });
        return total;
    };
    $scope.executeOrder = function () {
        var getOrder = sessionStorage.getItem("cart");
        if (getOrder != null) {
            $scope.cartItems = JSON.parse(getOrder);
        } else {
            Swal.fire("Notice", "Your Cart Is empty");
            return;
        }
        if (!$scope.selectedPayment) {
            Swal.fire("Notice", "Please select payment method");
            return;
        }
        var grouped = [];

        $scope.cartItems.forEach(item => {

            var existing = grouped.find(g => g.Vendor_ID === item.Vendor_ID);

            if (!existing) {
                existing = {
                    Vendor_ID: item.Vendor_ID,
                    Items: []
                };
                grouped.push(existing);
            }

            existing.Items.push(item);
        });

        var userOrder = {
            groupedOrders: grouped,
            paymentMethodId: $scope.selectedPayment,
            totalAmount: $scope.getOverallCost()
        };
        var Service = OrderingSystem2Service.executeOrderService(userOrder);
        Service.then(function (response) {
            if (response.data.success) {
                Swal.fire("Notice", "Order has been placed succesfully");
                $scope.cartItems = [];
                sessionStorage.removeItem("cart");
            } else {
                Swal.fire("Notice", "Failed to execute order " + response.data.message);
            }
        })
    }
    // for admin
    // to show menu
    $scope.getAdminMenus = function () {
        var Service = OrderingSystem2Service.getAdminMenus();
        Service.then(function (response) {
            $scope.adminMenu = response.data;
        });
    }
    // to add menu
    $scope.menuImageFile = null;
    $scope.menuImageChanged = function (element) {
        $scope.$apply(function () {
            $scope.menuImageFile = element.files[0];
        });
    }
    $scope.addMenu = function () {
        var menuData = new FormData();
        menuData.append("Menu_Name", $scope.Menu_Name);
        menuData.append("Menu_Description", $scope.Menu_Description);
        menuData.append("Menu_Price", $scope.Menu_Price);
        menuData.append("Menu_Status", $scope.Menu_Status);
        menuData.append("Menu_Image", $scope.menuImageFile);
        var Service = OrderingSystem2Service.addMenuService(menuData);
        Service.then(function (response) {
            if (response.data.success) {
                Swal.fire("Notice", "Menu Added Successfully");
                $scope.getAdminMenus();
            } else {
                Swal.fire("Notice", response.data.message);
            }
        })
    }
    $scope.delMenu = function (Menu_Id) {
        var Service = OrderingSystem2Service.deleteMenuService(Menu_Id);
        Service.then(function (response) {
            if (response.data.success) {
                Swal.fire("Deleted", "Menu item deleted successfully", "success");
                $scope.getAdminMenus(); 
            } else {
                Swal.fire("Error", "Failed to delete item", "error");
            }
        });
    };
    // to view orders
    $scope.getAdminOrders = function () {
        $scope.adminOrders = [];
        var Service = OrderingSystem2Service.getAdminOrdersService();
        Service.then(function (response) {
            $scope.adminOrders = response.data;
        });
    }
    // delete order
    $scope.markAsComplete = function (orderId) {
        Swal.fire({
            title: "Mark as complete?",
            text: "This will remove the order.",
            icon: "warning",
            showCancelButton: true,
            confirmButtonText: "Yes, complete it"
        }).then(function (result) {
            if (result.isConfirmed) {
                var service = OrderingSystem2Service.markOrderCompleteService(orderId);
                service.then(function (response) {
                    if (response.data.success) {
                        Swal.fire("Success", "Order marked as complete");
                        $scope.adminOrders = $scope.adminOrders.filter(
                            o => o.Orders.Order_ID !== orderId
                        );
                    } else {
                        Swal.fire("Error", response.data.message);
                    }
                });
            }
        });
    };
    // for admin charts
    // for cards
    $scope.getDashboardCardsStats = function () {
        var service = OrderingSystem2Service.getVendorDashboardService();
        service.then(function (response) {
            $scope.cardStats = response.data;
        });
    };
    // charts (just samples. to be editted)
    $scope.getDashboardChartStats = function () {
        var service = OrderingSystem2Service.getDashboardChartsService();
        service.then(function (response) {
            $scope.labels = response.data.pieLabels;
            $scope.data = response.data.pieData;

            $scope.labels2 = response.data.barLabels;
            $scope.data2 = [response.data.barData];
            $scope.series2 = ["Revenue"];

            $scope.labels3 = response.data.lineLabels;
            $scope.data3 = [response.data.lineData];
            $scope.series3 = ["Revenue"];
        });

    };
});
/*
* https://docs.angularjs.org/api/ng/input/input%5Bradio%5D
* sources of using form data
* https://www.youtube.com/watch?v=N5skN6ulO2g
* https://stackoverflow.com/questions/38603031/how-to-send-an-image-from-frontend-to-back-end
*/