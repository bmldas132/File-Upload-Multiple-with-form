/// <reference path="C:\Leedhar\Works\TableSure\TableSure\TableSure.Web\Scripts/angularjs/map.js" />
var AddDealApp = angular.module('AddDealApp', ['fupApp']);

AddDealApp.controller('AddDealController', ['$scope', 'AddDealService', function ($scope, AddDealService) {


    //Add File start.....
    $scope.getTheFiles = function ($files) {

        $scope.imagesrc = [];

        for (var i = 0; i < $files.length; i++) {

            var reader = new FileReader();
            reader.fileName = $files[i].name;

            reader.onload = function (event) {

                var image = {};
                image.Name = event.target.fileName;
                image.Size = (event.total / 1024).toFixed(2);
                image.Src = event.target.result;
                $scope.imagesrc.push(image);
                $scope.$apply();
            }
            reader.readAsDataURL($files[i]);
        }

        $scope.Files = $files;

    };
    //Add File End...

    // Submit Forn data
    $scope.Submit = function () {
        

        //FILL FormData WITH FILE DETAILS.
        var data = new FormData();
        
        angular.forEach($scope.Files, function (value, key) {
            data.append(key, value);
        });

        data.append("DealModel", angular.toJson($scope.DealDetail));
        AddDealService.AddDeal(data).then(function (response) {
            alert("Added Successfully");
        }, function () {

        });
    };


}]);


AddDealApp.factory('AddDealService', ['$http', function ($http) {

    var fac = {};
    
    fac.AddDeal = function (data) {
        return $http.post("/api/Deal", data, {
            withCredentials: true,
            headers: { 'Content-Type': undefined },
            transformRequest: angular.identity
        })
    }

    return fac;
}])





