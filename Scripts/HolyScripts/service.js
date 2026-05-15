app.service("OrderingSystem2Service", function ($http) {
    // for login, registration and store setup of user
    this.registerUser = function (userData) {
        var response = $http({
            url: "/UserAccounts/RegisterUser",
            method: "post",
            data: userData
        });
        return response;
    }
    this.registerVendor = function (vendorData) {
        var response = $http({
            url: "/UserAccounts/RegisterVendor",
            method: "post",
            data: vendorData,
            headers: { 'Content-Type': undefined },
            transformRequest: angular.identity
        });
        return response;
    }
    this.checkTempInfo = function (userInfo) {
        var response = $http({
            url: "/UserAccounts/checkUser",
            method: "post",
            data: userInfo
        });
        return response;
    }
    this.authUser = function (authInfo) {
        var response = $http({
            url: "/UserAccounts/authenticateUser",
            method: "post",
            data: authInfo
        });
        return response;
    }
    // for editing profile (admin and normal)
    this.updateUserInfo = function (data) {
        var response = $http({
            url: "/UserAccounts/updateUserInfo",
            method: "post",
            data: data
        });
        return response;
    }
    // for normal user
    this.getRestoInfo = function () {
        return $http.get("/NormalUser/getRestoInfo");
    }
    this.getUserInfo = function () {
        return $http.get("/UserAccounts/getUserInfo");
    }
    this.logOut = function () {
        var response = $http({
            url: "/NormalUser/LogOut",
            method: "post"
        });
        return response;
    }
    this.getStoreCategory = function () {
        return $http.get("/NormalUser/getStoreCategory");
    }
    this.getFilterStoreList = function (restoInfo) {
        var response = $http({
            url: "/NormalUser/getfilterStoreList",
            method: "post",
            data: {
                restoInfo: restoInfo
            }
        });
        return response;
    }
    this.getSearchResult = function (userSearchResto) {
        var response = $http({
            url: "/NormalUser/getSearchResult",
            method: "post",
            data: {
                userSearch: userSearchResto
            }
        });
        return response;
    }
    this.getRestoMenus = function (Vendor_ID) {
        var response = $http({
            url: "/NormalUser/getRestoMenus",
            method: "post",
            data: {
                Vendor_ID: Vendor_ID
            }
        });
        return response;
    }
    this.executeOrderService = function (userOrder) {
        var response = $http({
            url: "/NormalUser/executeOrder",
            method: "post",
            data: userOrder
        });
        return response;
    }
    // for admin
    // to view menu
    this.getAdminMenus = function () {
        return $http.get("/AdminUser/getAdminMenu");
    }
    // to add menu
    this.addMenuService = function (menuData) {
        var response = $http({
            url: "/AdminUser/addMenu",
            method: "post",
            data: menuData,
            headers: { 'Content-Type': undefined },
            transformRequest: angular.identity
        });
        return response;
    }
    // to del Menu
    this.deleteMenuService = function (Menu_Id) {
        var response = $http({
            url: "/AdminUser/delMenu",
            method: "post",
            data: {
                Menu_ID: Menu_Id
            }
        });
        return response;
    };
    // to view orders
    this.getAdminOrdersService = function () {
        return $http.get("/AdminUser/getAdminOrder");
    }
    // to delete order
    this.markOrderCompleteService = function (orderId) {
        var response = $http({
            url: "/AdminUser/markOrderComplete",
            method: "post",
            data: {
                orderId: orderId
            }
        });
        return response;
    };
    // to display card stats
    this.getVendorDashboardService = function () {
        return $http.get("/AdminUser/getVendorCardStats");
    };
    this.getDashboardChartsService = function () {
        return $http.get("/AdminUser/getDashboardCharts");
    };
});